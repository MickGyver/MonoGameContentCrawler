### What is MonoGame Content Crawler?
MonoGame Content Crawler is a command line tool for conveniently adding assets to MonoGame projects. It recursively adds all supported files to a .mgcb content file.

### How to use?
Download the latest release and copy MonoGameContentCrawler.exe to the same folder as your Content.mgcb file. Simply run it to add all supported files in that folder (also in subfolders) to the Content.mgcb file. Files already existing in the content file will be ignored. You can use the build in content editor normally besides this tool.

### Advanced use
If you want to use another content file, just add the name or full path of the content file as an argument (optionally creating a batch file).

### Limitations
This tool is very limited at the moment, and is more a proof-of-concept. For basic use it works but much more can (and hopefully will) be done.
