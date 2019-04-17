### What is MonoGame Content Crawler?
MonoGame Content Crawler is a command line tool for conveniently adding assets to MonoGame projects. It recursively adds all supported files to a .mgcb content file.

### How to use?
Download the latest release and copy MonoGameContentCrawler.exe (and .config) to the same folder as your Content.mgcb file. Simply run it to add all supported files in that folder (also in subfolders) to the Content.mgcb file. Files already existing in the content file will be ignored. You can use the build in content editor normally besides this tool.

### Advanced use
If you want to use another content project file, just add the name or full path of the content file as an argument (optionally creating a batch file). You can have MonoGameContentCrawler.exe (and .config) somewhere in the PATH (Windows folder etc.).

### Supported assets
The MonoGameContentCrawler.config file has templates for all supported asset types, based on filename extensions. Add or remove templates as you wish. Files with extensions not in the config file will be ignored. The format of the extension file should be self explanatory but you need to have an empty line between templates. If a config file exists in the same folder as the content project file, that config file will be used instead of the config file in the application folder.

### Limitations
This tool is very limited at the moment, and is more a proof-of-concept. For basic use it works but much more can (and hopefully will) be done.
