using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.IO;

namespace MonoGameContentCrawler
{
  class ContentSpriteFont : ContentType
  {
    public ContentSpriteFont(string extension, string template) : base(extension, template)
    {
    }
    
    public override string BuildContent(string rootPath, string filePath)
    {
      FileInfo fInfo = new FileInfo(Path.Combine(rootPath, filePath));
      string content = "\r\n\r\n#begin " + filePath + "\r\n";
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
      catch (Exception)
      { }

      content += "/importer:FontDescriptionImporter\r\n";
      if (localizedFont)
        content += "/processor:LocalizedFontProcessor\r\n";
      else
        content += "/processor:FontDescriptionProcessor\r\n";
      content += "/processorParam:PremultiplyAlpha=True\r\n";
      content += "/processorParam:TextureFormat=Compressed\r\n";
      content += "/build:" + filePath;

      return content;
    }
  }
}
