using net.tkglaser.imgsclr.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace net.tkglaser.imgsclr.Controllers
{
    public class ScalerController : Controller
    {
        private CachedImage GetImage(string uri)
        {
            if (HttpContext.Cache[uri] == null)
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                if ((response.StatusCode == HttpStatusCode.OK ||
                    response.StatusCode == HttpStatusCode.Moved ||
                    response.StatusCode == HttpStatusCode.Redirect) &&
                    response.ContentType.StartsWith("image", StringComparison.OrdinalIgnoreCase))
                {
                    using (Stream inputStream = response.GetResponseStream())
                    {
                        var bmp = Bitmap.FromStream(inputStream) as Bitmap;
                        var ct = response.ContentType;

                        var mem = new MemoryStream();
                        if (ct.ToLower() == "image/jpg")
                        {
                            bmp.Save(mem, ImageFormat.Jpeg);
                        }
                        else
                        {
                            bmp.Save(mem, ImageFormat.Png);
                            ct = "image/png";
                        }
                        
                        mem.Position = 0;

                        HttpContext.Cache[uri] = new CachedImage()
                        {
                            ContentType = ct,
                            Content = mem.ToArray()
                        };
                    }
                }
            }

            return HttpContext.Cache[uri] as CachedImage;
        }

        public ActionResult Scale(string dimensions, string u)
        {
            var image = GetImage(u);

            return File(image.Content, image.ContentType);
        }
    }
}
