using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EchoMe.Models;

namespace EchoMe.Controllers
{
    public class FreeSpeechController : Controller
    {
        private readonly EchoDBEntities _echoDb = new EchoDBEntities();
        //
        // GET: /FreeSpeech/

        public ActionResult Index()
        {
            return null;
        }

        [HttpPost]
        public ActionResult Load(string userId, string url)
        {
            try
            {
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(Server.MapPath("~") + @"\LogFreeSpeechLoad.txt", true))
                {
                    file.Write(url);
                }
                FREECOMMENT f = new FREECOMMENT();
                
                int UserId = int.Parse(userId);
                var allFreeComment = _echoDb.FREECOMMENTs.Where(p => p.FREEURL == url).Select(p=>new{p.ID,p.COMMENT,p.AUTHOR,p.FREEURL,LIKES=_echoDb.FREEVOTEs.Count(n=>n.CommentId==p.ID),LIKED=_echoDb.FREEVOTEs.Any(m=>m.UserId==UserId && m.CommentId==p.ID)}).OrderBy(v=>v.LIKES).ToList();
                return
                    Json(allFreeComment);
            }
            catch (Exception)
            {
                return Json("error");
            }
        }


        //xhrMakeComment.open("POST", "http://crowd-multilogue.com/FreeSpeech/Make?author=" + request.author + "&comment=" + request.comment + "&url=" + MyFreeURL, true);

        [HttpPost]
        public ActionResult Make(string author, string comment, string url)
        {
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(Server.MapPath("~") + @"\LogFreeSpeechLoad.txt", true))
            {
                file.Write(author+","+comment+","+url);
            }
            FREECOMMENT freecomment = new FREECOMMENT
            {
                AUTHOR = author,
                COMMENT = comment,
                FREEURL = url
            };

            try
            {
                _echoDb.FREECOMMENTs.Add(freecomment);
                _echoDb.SaveChanges();
                return Json("done");
            }
            catch (Exception exception)
            {
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(Server.MapPath("~") + @"\LogFreeSpeechLoad.txt", true))
                {
                    file.Write(exception.Message);
                }
                return Json("error");
            }
        }

        //http://crowd-multilogue.com/FreeSpeech/RegisterUsername?username
        [HttpPost]
        public ActionResult RegisterUsername(string username)
        {
            try
            {
                FREEUSERNAME user = new FREEUSERNAME { Username = username };
                if (_echoDb.FREEUSERNAMEs.Any(p => p.Username == username))
                {
                    return Json(new{status="error",moreInfo="It is taken, try another username!"});
                }
                _echoDb.FREEUSERNAMEs.Add(user);
                _echoDb.SaveChanges();
                return Json(new {status = "done" ,userId = user.Id});
            }
            catch (Exception exception)
            {
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(Server.MapPath("~") + @"\LogFreeSpeechLoad.txt", true))
                {
                    file.Write(exception.Message);
                }
                return Json(new { status = "error", moreInfo = "Server is busy, try later!" });
            }
        }

        //http://crowd-multilogue.com/FreeSpeech/LikeComment?userId=" + request.message.userId + "&commentId=" + request.message.commentId, true);
        [HttpPost]
        public ActionResult LikeComment(string userId, string commentId)
        {
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(Server.MapPath("~") + @"\LogFreeSpeechLoad.txt", true))
            {
                file.Write(userId+"-"+commentId);
            }
            try
            {
                FREEVOTE freevote = new FREEVOTE
                {
                    CommentId = int.Parse(commentId),
                    UserId = int.Parse(userId)
                };
                _echoDb.FREEVOTEs.Add(freevote);
                _echoDb.SaveChanges();
                return Json(new { status = "done" });
            }
            catch (Exception)
            {
                return Json(new { status = "error" });
            }
        }

        //http://crowd-multilogue.com/FreeSpeech/UnlikeComment?userId=" + request.message.userId + "&commentId=" + request.message.commentId, true);
        [HttpPost]
        public ActionResult UnlikeComment(string userId, string commentId)
        {
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(Server.MapPath("~") + @"\LogFreeSpeechLoad.txt", true))
            {
                file.Write(userId + "-" + commentId);
            }
            try
            {
                int CommentId = int.Parse(commentId);
                int UserId = int.Parse(userId);
                FREEVOTE freevote = _echoDb.FREEVOTEs.First(p => p.UserId == UserId && p.CommentId == CommentId);
                _echoDb.FREEVOTEs.Remove(freevote);
                _echoDb.SaveChanges();
                return Json(new { status = "done" });
            }
            catch (Exception)
            {
                return Json(new { status = "error" });
            }
        }
    }
}
