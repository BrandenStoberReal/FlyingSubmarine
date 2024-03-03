# The Orobouros Framework
Orobouros is a C# framework for scraping the web. Many attempts to do this have been created in various languages, but a different approach is taken with Orobouros due to the patented OrobourosModule™ system that allows any person to write their own plugin for any website.

# Usage
This is a ``.NET 8 Core`` library and can be freely imported into any project with the same .NET version. Please keep in mind Linux support is entirely unknown at this time due to SQLite binaries, but it should be compatible in theory.

# Building
Bleeding edge builds can be found in the ``Actions`` tab pre-compiled. If you insist on compiling this yourself, all you need is ``.NET 8 Core``. I would not recommend taking advantage of the tests, as they require specific configurations I use in debugging.

# Development
Take a look at the TestModule project included in this repo to get a general idea on how to use this framework. XML annotations are also provided. At some point I will create a wiki with relevant information, but for now the core functionality takes priority. Obfuscated code is allowed (as in the framework won't refuse to execute it) but incredibly discouraged due to malware concerns. If you really feel the need to keep your source code hidden, just don't share your module.

# DMCA
This repository holds no responsibility on any modules programmers develop for this framework. No copywritten content is included in this repo and will never be. If someone has made a module for your website and you don't like it, I cannot help you. You must get in contact with them to resolve such matters. This also applies to potentially illicit/illegal content scraped with modules created by the community. 

# TODO:
- [x] Dynamic module loading
- [x] Raw HTTP support
- [x] Downloader service
- [x] Attribute scanning
- [x] Custom attributes
- [x] Module init method
- [x] Module supplementary methods
- [x] Module scrape method
- [ ] Module options
- [x] Module return data
- [x] Custom library support
- [x] SQLite support
- [x] Dynamic database support
- [ ] Website API support (separate from raw HTTP)
- [ ] Cross-module support
- [x] XML annotations
- [ ] Module security checks
- [x] Module sanity checks
- [x] Multiple modules for same website support
- [ ] Improved module error handling
- [x] SQlite module integration
- [ ] Public module downloader tool
- [ ] Better data sanitizing & JSON storage
- [ ] General framework configuration class
- [ ] Cross-language support (extremely advanced)
- [ ] Data language translation toolkit
- [ ] Overhaul download class & integrate better
- [ ] Logging overhaul
- [x] Module developer web toolkit
- [ ] Framework-level exception handling
- [ ] Bulk data downloading functions (stored in RAM)

# Credits
- Branden Stober - Main Project Lead
- ImSoupp - Reflection Help & Database Help
- CTAG - Database Help
