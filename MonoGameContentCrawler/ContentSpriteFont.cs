using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoGameContentCrawler
{
  class ContentSpriteFont : ContentType
  {
    public ContentSpriteFont(string extension, string template) : base(extension, template)
    {
    }
    
    public override string BuildContent(string rootPath, string filePath)
    {
      return "";
    }
  }
}
