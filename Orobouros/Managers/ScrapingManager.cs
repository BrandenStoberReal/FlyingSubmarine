﻿using Orobouros.Attributes;
using Orobouros.Bases;
using Orobouros.Tools.Containers;
using Orobouros.Tools.Web;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static Orobouros.UniAssemblyInfo;
using Module = Orobouros.Bases.Module;

namespace Orobouros.Managers
{
    public static class ScrapingManager
    {
        /// <summary>
        /// Token to cancel supplementary tasks. Tasks should be cancelled on application shutdown.
        /// </summary>
        public static CancellationTokenSource SupplementaryCancelToken { get; private set; }

        /// <summary>
        /// Thread assigned to supplementary methods.
        /// </summary>
        public static Thread SupplementaryThread { get; private set; }

        /// <summary> Initializes scraper modules & runs startup logic. ONLY CALL THIS ONCE UNLESS
        /// YOU KNOW WHAT YOU'RE DOING!</summary> <param name="modulesPath">Optional custom modules
        /// folder to load</param>
        public static void InitializeModules(string modulesPath = "./modules")
        {
            // Load assemblies
            ModuleManager.LoadAssemblies(modulesPath);

            // Start supplementary methods
            Thread thready = new Thread(() =>
            {
                // Run thread in background (obviously)
                Thread.CurrentThread.IsBackground = true;
                Thread.CurrentThread.Name = "Orobouros Supplementary Methods Worker";

                // Create new token source
                CancellationTokenSource CancellationTokenSource = new CancellationTokenSource();

                // Assign new token to parent class
                SupplementaryCancelToken = CancellationTokenSource;

                while (true)
                {
                    // Handle cancellations
                    if (SupplementaryCancelToken.Token.IsCancellationRequested)
                    {
                        break;
                    }

                    // Run all supplementary methods
                    foreach (Module mod in ModuleManager.Container.Modules)
                    {
                        foreach (MethodInfo method in mod.SupplementaryMethods)
                        {
                            ReflectionManager.InvokeReflectedMethod(method, mod.PsuedoClass);
                        }
                    }
                }
            });
            SupplementaryThread = thready;
            thready.Start();
        }

        /// <summary>
        /// Forcibly stops the supplementary modules thread. InitializeModules will need to be run
        /// again after this if the application will continue.
        /// </summary>
        public static void FlushSupplementaryMethods()
        {
            if (SupplementaryCancelToken != null)
            {
                SupplementaryCancelToken.Cancel();
            }
        }

        /// <summary>
        /// Performs a scrape request against all loaded modules.
        /// </summary>
        /// <param name="url">
        /// Specific URL to scrape. This should be a URL that points to a page with scrapable data.
        /// </param>
        /// <param name="contentToFetch">
        /// List of content to fetch from supported modules. Keep in mind the module may not support
        /// the data types you request.
        /// </param>
        /// <param name="numofinstances">
        /// (Optional) Number of instances to scrape. Defaults to 1 and is rarely changed.
        /// </param>
        /// <returns></returns>
        public static ModuleData? ScrapeURL(string url, List<ModuleContent> contentToFetch, int numofinstances = -1, List<Post> posts = null)
        {
            // Placeholder for discovered module
            ModuleContainer foundModules = new ModuleContainer();

            // Fetch base domain for compatibility checks
            Uri myUri = new Uri(url);

            string baseDomain = myUri.Host;

            // Find module with supported website
            foreach (Module mod in ModuleManager.Container.Modules)
            {
                foreach (string Website in mod.SupportedWebsites)
                {
                    if (Website.Contains(baseDomain))
                    {
                        foundModules.Modules.Add(mod);
                    }
                }
            }

            if (foundModules == null)
            {
                // Compatible module not found
                DebugManager.WriteToDebugLog($"No module found with URL \"{url}\"! Ensure you have a supported module installed.");
                return null;
            }
            else
            {
                // Check if module supports content we want
                foreach (ModuleContent content in contentToFetch)
                {
                    foreach (Module mod in foundModules.Modules)
                    {
                        if (!mod.SupportedContent.Contains(content))
                        {
                            foundModules.Modules.Remove(mod);
                        }
                    }
                }

                // Bad content was requested
                if (foundModules.Modules.Count == 0)
                {
                    DebugManager.WriteToDebugLog($"Content has been requested that the discovered module(s) do not support! Please ensure you have the correct module(s) installed.");
                    return null;
                }

                // Generate parameters to inject
                ScrapeParameters parms = new ScrapeParameters();
                parms.URL = url;
                parms.RequestedContent = parms.RequestedContent.Concat(contentToFetch).ToList();
                parms.ScrapeInstances = numofinstances;
                parms.Subposts = posts;

                if (foundModules.Modules.Count > 1)
                {
                    // Multiple supported modules found
                    DebugManager.WriteToDebugLog($"Multiple modules for the same website supporting the same content found. A random one will be selected. Please avoid this behavior in the future.");
                    Random rng = new Random();
                    int randInt = rng.Next(foundModules.Modules.Count);
                    return (ModuleData?)ReflectionManager.InvokeReflectedMethod(foundModules.Modules[randInt].ScrapeMethod, foundModules.Modules[randInt].PsuedoClass, new object[] { parms });
                }
                else
                {
                    // Only 1 module found, should be default behavior
                    return (ModuleData?)ReflectionManager.InvokeReflectedMethod(foundModules.Modules.FirstOrDefault().ScrapeMethod, foundModules.Modules.FirstOrDefault().PsuedoClass, new object[] { parms });
                }
            }
        }
    }
}