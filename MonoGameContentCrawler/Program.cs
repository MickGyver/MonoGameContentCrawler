using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;
using System.Globalization;
using System.Xml;

namespace MonoGameContentCrawler
{
  class Program
  {
    static private Dictionary<string, ContentType> _contentTypes = new Dictionary<string, ContentType>();

    static void Main(string[] args)
    {
      string appDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
      string directory = Environment.CurrentDirectory;
      string contentFile = "Content.mgcb";
      string contentFilePath = contentFilePath = Path.Combine(directory, contentFile);
      

      Console.WriteLine("*********************************");
      Console.WriteLine("* MonoGame Content Crawler v0.2 *");
      Console.WriteLine("*   by Mick @ GamePhase 2019    *");
      Console.WriteLine("*********************************");

      // Add content types
      string configPath = null;
      if (File.Exists(Path.Combine(appDirectory, "MonoGameContentCrawler.config")))
      {
        AddContentTypes(Path.Combine(appDirectory, "MonoGameContentCrawler.config"));
      }
      else
      {
        Console.WriteLine("Configuration file does not exist!");
        Environment.Exit(0);
      }

      // Is a specific content file supplied as argument?
      if (args.Length > 0)
      {
        if (args[0].EndsWith(".mgcb"))
        {
          if (File.Exists(args[0]))
          {
            contentFilePath = args[0];
            FileInfo fInfo = new FileInfo(contentFilePath);
            contentFile = fInfo.Name;
            directory = fInfo.DirectoryName;
          }
          else if (File.Exists(Path.Combine(directory, args[0])))
          {
            contentFile = args[0];
            contentFilePath = Path.Combine(directory, contentFile);
          }
          else
          {
            Console.WriteLine("'" + args[0] + "' does not exist!");
            Environment.Exit(0);
          }
        }
        else
        {
          Console.WriteLine("The argument supplied has to be a .mgcb content file!");
          Environment.Exit(0);
        }
      }
      else if (!File.Exists(contentFile))
      {
        Console.WriteLine("'" + contentFile + "' does not exist!");
        Environment.Exit(0);
      }

      Console.WriteLine("Root folder: " + directory);
      Console.WriteLine("Content file: " + contentFile);

      string content = File.ReadAllText(contentFilePath);

      Console.WriteLine("Adding resources to '" + contentFile + "'...");

      ProcessFolder(ref content, directory, "");

      File.WriteAllText(contentFilePath, content);

      Console.WriteLine("\nAll done!");
    }

    static void ProcessFolder(ref string content, string rootPath, string subPath)
    {
      string[] files = Directory.GetFiles(Path.Combine(rootPath, subPath));
      foreach (string file in files)
      {
        FileInfo fInfo = new FileInfo(file);
        CompareInfo compareInfo = CultureInfo.InvariantCulture.CompareInfo;
        string filePath = (subPath.Length > 0 ? subPath.Replace("\\", "/") + "/" : "") + fInfo.Name;
        if (compareInfo.IndexOf(content, "#begin " + filePath) < 0)
        {
          string result = addResource(rootPath, subPath, fInfo.Name);
          if (result.Length > 0)
          {
            content += result;
            Console.WriteLine(filePath + " [added]");
          }
          else
            Console.WriteLine(filePath + " [ignored]");
        }
        else
          Console.WriteLine(filePath + " [exists]");
      }

      string[] folders = Directory.GetDirectories(Path.Combine(rootPath, subPath));
      foreach (string folder in folders)
      {
        DirectoryInfo dInfo = new DirectoryInfo(folder);
        ProcessFolder(ref content, rootPath, Path.Combine(subPath, dInfo.Name));
      }
    }

    static bool checkValidExtension(string path)
    {
      if (path.EndsWith(".png") || path.EndsWith(".fx") || path.EndsWith(".wav") || path.EndsWith(".mp3") || path.EndsWith(".wma"))
        return true;
      else
        return false;
    }

    static string addResource(string rootPath, string subPath, string fileName)
    {
      FileInfo fInfo = new FileInfo(Path.Combine(rootPath, subPath, fileName));
      string content = "";
      string filePath = (subPath.Length > 0 ? subPath.Replace("\\", "/") + "/" : "") + fileName;

      string extension = fInfo.Extension.ToLower();
      if (_contentTypes.ContainsKey(extension))
      {
        content = _contentTypes[extension].BuildContent(rootPath, filePath);
      }
      
      /*switch (fInfo.Extension.ToLower())
      {
        case ".spritefont":
          bool localizedFont = false;
          XmlDocument xmlDoc = new XmlDocument();
          try
          {
            xmlDoc.Load(fInfo.FullName);
            XmlNode xmlNode = xmlDoc.SelectSingleNode("//Asset");
            XmlAttribute xmlAttribute = (XmlAttribute)xmlNode.Attributes.GetNamedItem("Type");
            if (xmlAttribute.InnerText.ToLower() == "graphics:localizedfontdescription")
              localizedFont = true;
          }
          catch (Exception ex)
          {
            Console.WriteLine(ex.ToString());
          }

          content += "\r\n\r\n#begin " + filePath + "\r\n";
          content += "/importer:FontDescriptionImporter\r\n";
          if (localizedFont)
            content += "/processor:LocalizedFontProcessor\r\n";
          else
            content += "/processor:FontDescriptionProcessor\r\n";
          content += "/processorParam:PremultiplyAlpha=True\r\n";
          content += "/processorParam:TextureFormat=Compressed\r\n";
          content += "/build:" + filePath;
          break;
      }*/

      return content;
    }

    private static bool AddContentTypes(string configPath)
    {
      if (configPath != null)
      {
       // Declare variables 
        string line;
        string lines = "";
        bool newType = true;
        bool typeAdded = true;
        string[] extensions = null;

        // Read config file, line by line
        System.IO.StreamReader configFile = new StreamReader(configPath);
        while ((line = configFile.ReadLine()) != null)
        {
          line = line.Trim();
          if (line.Length == 0)
          {
            if (!typeAdded)
            {
              AddContentType(extensions, lines);
              typeAdded = true;
            }
            newType = true;
          }
          else
          {
            if (newType)
            {
              extensions = line.Split(';');
              newType = false;
              lines = "";
              typeAdded = false;
            }
            else
            {
              lines += line + "\r\n";
            }
          }
        }  
        // Add last content type if it hasn't already been added
        if (!typeAdded)
          AddContentType(extensions, lines);

        return true;
      }
      else
        return false;
    }

    private static void AddContentType(string[] extensions, string template)
    {
      foreach (string extension in extensions)
      {
        ContentType type = new ContentType(extension,template);
        _contentTypes.Add(extension,type);
      }
    }

  }
}
