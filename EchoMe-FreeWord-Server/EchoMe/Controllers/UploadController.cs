using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using EchoMe.Filters;
using EchoMe.Models;
using WebMatrix.WebData;

namespace EchoMe.Controllers
{
    public class UploadController : Controller
    {
        private readonly EchoDBEntities _echoDb = new EchoDBEntities();
        private string videoAddress = "~/Videos";

        [InitializeSimpleMembership]
        [HttpPost]
        public string MultiUpload(string id, string fileName)
        {
            string userFileName = WebSecurity.CurrentUserId + "_" + fileName;
            var chunkNumber = id;
            string tempPath = Server.MapPath(videoAddress+"/Temp");
            try
            {
                using (FileStream fs = System.IO.File.Create(Path.Combine(tempPath, userFileName+chunkNumber)))
                {
                    byte[] bytes = new byte[3757000];
                    int bytesRead;
                    while ((bytesRead = Request.InputStream.Read(bytes, 0, bytes.Length)) > 0)
                    {
                        fs.Write(bytes, 0, bytesRead);
                    }
                }
                return "done";
            }
            catch (Exception)
            {
                string[] filePaths = Directory.GetFiles(tempPath).Where(p => p.Contains(userFileName)).ToArray();
                foreach (string filePath in filePaths)
                {
                    System.IO.File.Delete(filePath);
                }
                return "error";
            }
        }

        [InitializeSimpleMembership]
        [HttpPost]
        public string UploadComplete(string fileName, string complete, string fileTitle, string fileDescription)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return "error";
            }
            string userFileName = WebSecurity.CurrentUserId + "_" + fileName;
            string tempPath = Server.MapPath(videoAddress + "/Temp");
            string videoPath = Server.MapPath(videoAddress);
            try
            {
                if (complete == "1")
                {
                    string[] filePaths = Directory.GetFiles(tempPath).Where(p => p.Contains(userFileName)).OrderBy(p => Int32.Parse(p.Replace(userFileName, "$").Split('$')[1])).ToArray();
                    foreach (string filePath in filePaths)
                    {
                        MergeFiles(Path.Combine(tempPath, userFileName), filePath);
                    }
                }
                System.IO.File.Move(Path.Combine(tempPath, userFileName), Path.Combine(videoPath, userFileName));

                Presentation presentation = new Presentation
                {
                    Name = userFileName,
                    Description = fileDescription,
                    Title = fileTitle==""?fileName:fileTitle,
                    UserId = WebSecurity.CurrentUserId
                };
                _echoDb.Presentations.Add(presentation);
                _echoDb.SaveChanges();
                return "done";
            }
            catch (Exception)
            {
                string[] filePaths = Directory.GetFiles(tempPath).Where(p => p.Contains(userFileName)).ToArray();
                foreach (string filePath in filePaths)
                {
                    if(System.IO.File.Exists(filePath))
                    System.IO.File.Delete(filePath);
                }
                if(System.IO.File.Exists(Path.Combine(videoPath, userFileName)))
                {
                    System.IO.File.Delete(Path.Combine(videoPath, userFileName));
                }
                return "error";
            }
        }

        private static void MergeFiles(string file1, string file2)
        {
            FileStream fs1 = null;
            FileStream fs2 = null;
            try
            {
                fs1 = System.IO.File.Open(file1, FileMode.Append);
                fs2 = System.IO.File.Open(file2, FileMode.Open);
                byte[] fs2Content = new byte[fs2.Length];
                fs2.Read(fs2Content, 0, (int) fs2.Length);
                fs1.Write(fs2Content, 0, (int) fs2.Length);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + " : " + ex.StackTrace);
            }
            finally
            {
                if (fs1 != null) fs1.Close();
                if (fs2 != null) fs2.Close();
                System.IO.File.Delete(file2);
            }
        }

        [InitializeSimpleMembership]
        [HttpPost]
        public string FileExist(string fileName)
        {
            string userFileName = WebSecurity.CurrentUserId + "_" + fileName;
            string videoPath = Server.MapPath(videoAddress);
            if (System.IO.File.Exists(Path.Combine(videoPath, userFileName)))
            {
                return "error";
            }
            return "done";
        }
    }
}
