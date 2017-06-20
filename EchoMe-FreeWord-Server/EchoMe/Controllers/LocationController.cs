using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EchoMe.Controllers
{
    public class LocationController : Controller
    {
        //
        // GET: /Location/

        public string Store(string user, string data)
        {
            try
            {
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(Server.MapPath("~") + @"\LocationService.txt", true))
                {
                    DateTime now = DateTime.Now.AddHours(10);
                    string time = string.Format("{0}:{1}:{2}:{3}:{4}:{5}", now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second);
                    file.WriteLine(user+"-"+data + "-" + time);
                }
            }
            catch (Exception)
            {
                return "problem";
            }
            return "ok";
        }

        public string LoadAll()
        {
            try
            {
                using (System.IO.StreamReader file =
            new System.IO.StreamReader(Server.MapPath("~") + @"\LocationService.txt", true))
                {
                    string allString = file.ReadToEnd();
                    List<string> listString = allString.Split(new string[] { "\r\n" }, StringSplitOptions.None).ToList(); ;
                    return string.Join("\r\n", listString);
                }
            }
            catch (Exception)
            {
                return "problem";
            }
        }

        public string Clear()
        {
            try
            {
                System.IO.File.WriteAllText(Server.MapPath("~") + @"\LocationService.txt", "");
            }
            catch (Exception)
            {
                return "problem";
            }
            return "ok";
        }
    }
}
