using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace net.tkglaser.imgsclr.Models
{
    public class CachedImage
    {
        public string ContentType { get; set; }
        public byte[] Content { get; set; }
    }
}