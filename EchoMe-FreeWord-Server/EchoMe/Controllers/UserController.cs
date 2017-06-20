using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EchoMe.Filters;
using EchoMe.Models;
using WebMatrix.WebData;

namespace EchoMe.Controllers
{
    public class UserController : Controller
    {
        private readonly EchoDBEntities _echoDb = new EchoDBEntities();
        private readonly int _oneVideoPoint = int.Parse(ConfigurationManager.AppSettings["VideoPoint"]);
        private readonly int _oneCommentPoint = int.Parse(ConfigurationManager.AppSettings["CommentPoint"]);
        private readonly int _oneLikePoint = int.Parse(ConfigurationManager.AppSettings["LikePoint"]);
        private readonly int _oneWatchedPoint = int.Parse(ConfigurationManager.AppSettings["WatchedPoint"]);
        private readonly int _oneRatingPoint = int.Parse(ConfigurationManager.AppSettings["RatingPoint"]);

        // GET: /User/
        [InitializeSimpleMembership]
        public ActionResult Index(string username)
        {
            if (username == User.Identity.Name)
            {
                int userId = WebSecurity.CurrentUserId;
                int uploadedVideosNumber = _echoDb.Presentations.Count(p => p.UserId==userId);
                List<Comment> madeComments = _echoDb.Comments.Where(p => p.UsesrId == userId).ToList();
                int madeCommentsNumber = madeComments.Count;
                int receivedLikeNumber =
                    madeComments.Select(p => _echoDb.UpVotes.Count(n => n.CommentId == p.CommentId)).Sum();
                ViewBag.UserId = userId.ToString();
                ViewBag.UserName = username;
                ViewBag.Videos = uploadedVideosNumber;
                ViewBag.Comments = madeCommentsNumber;
                ViewBag.Likes = receivedLikeNumber;
                ViewBag.OneVideoPoint = _oneVideoPoint;
                ViewBag.OneCommentPoint = _oneCommentPoint;
                ViewBag.OneLikePoint = _oneLikePoint;
                return View();
            }
            else
            {
                return Content("Item not found");
            }
        }

        //xhrUserPoints.open("POST", "/User/UserPoints?userId=" + @ViewBag.UserId);
        [InitializeSimpleMembership]
        [HttpPost]
        public ActionResult UserPoints(string userId)
        {
            try
            {
                int userIdServer = WebSecurity.CurrentUserId;
                int userIdClient = int.Parse(userId);
                if (userIdClient == userIdServer)
                {
                    int uploadedVideosNumber = _echoDb.Presentations.Count(p => p.UserId == userIdClient);
                    List<Comment> madeComments = _echoDb.Comments.Where(p => p.UsesrId == userIdClient).ToList();
                    int madeCommentsNumber = madeComments.Count;
                    int receivedLikeNumber =
                        madeComments.Select(p => _echoDb.UpVotes.Count(n => n.CommentId == p.CommentId)).Sum();
                    return
                        Json(
                            new
                            {
                                Videos = uploadedVideosNumber,
                                Comments = madeCommentsNumber,
                                Likes = receivedLikeNumber,
                                OneVideoPoint = _oneVideoPoint,
                                OneCommentPoint = _oneCommentPoint,
                                OneLikePoint = _oneLikePoint
                            });
                }
                return Json("error");
            }
            catch (Exception)
            {
                return Json("error");
            }
        }

    }
}
