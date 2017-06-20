using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EchoMe.Models
{
    public class Leader
    {
        public string Username { get; set; }
        public int Videos { get; set; }
        public int Comments { get; set; }
        public int Likes { get; set; }
        public int Watched { get; set; }
        public double VideosRate { get; set; }
        public double Points { get; set; }
    }
}