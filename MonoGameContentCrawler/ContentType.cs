using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoGameContentCrawler
{
  class ContentType
  {
    protected string _extension;
    protected string _template;

    public ContentType(string extension, string template)
    {
      _extension = extension;
      _template = template;
    }

    public virtual string BuildContent(string rootPath, string filePath)
    {
      string content = "\r\n\r\n#begin " + filePath + "\r\n";
      content += _template;
      content += "/build:" + filePath;
      return content;
    }
  }
}
