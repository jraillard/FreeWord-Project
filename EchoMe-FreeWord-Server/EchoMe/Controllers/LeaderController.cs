using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Web.Security;
using EchoMe.Filters;
using EchoMe.Models;
using WebMatrix.WebData;

namespace EchoMe.Controllers
{
    public class LeaderController : Controller
    {
        private readonly EchoDBEntities _echoDb = new EchoDBEntities();
        private readonly int _oneVideoPoint = int.Parse(ConfigurationManager.AppSettings["VideoPoint"]);
        private readonly int _oneCommentPoint = int.Parse(ConfigurationManager.AppSettings["CommentPoint"]);
        private readonly int _oneLikePoint = int.Parse(ConfigurationManager.AppSettings["LikePoint"]);
        private readonly int _oneWatchedPoint = int.Parse(ConfigurationManager.AppSettings["WatchedPoint"]);
        private readonly int _oneRatingPoint = int.Parse(ConfigurationManager.AppSettings["RatingPoint"]);
        //
        // GET: /Leader/

        [InitializeSimpleMembership]
        public ActionResult Index()
        {
            List<Leader> leaderModel = new List<Leader>();
            try
            {
                if (!(User.IsInRole("Admin")||User.IsInRole("Student"))) return View(leaderModel);
                var roleProvider = (SimpleRoleProvider) Roles.Provider;
                leaderModel = roleProvider.GetUsersInRole("Student")
                    .Select(z => new {username = z, userid = WebSecurity.GetUserId(z)})
                    .Select(
                        p =>
                            new Leader()
                            {
                                Username = p.username,
                                Videos = _echoDb.Presentations.Count(n => n.UserId == p.userid),
                                Comments = _echoDb.Comments.Count(m => m.UsesrId == p.userid),
                                Likes =
                                    _echoDb.Comments.Count(z => z.UsesrId == p.userid) == 0
                                        ? 0
                                        : _echoDb.Comments.Where(o => o.UsesrId == p.userid)
                                            .Select(l => _echoDb.UpVotes.Count(k => k.CommentId == l.CommentId))
                                            .Sum(),
                                Watched = _echoDb.WatchedPresentations.Count(y=>y.UserId == p.userid),
                                VideosRate = _echoDb.RateVideos.Count(u=>u.VideoCreatorId==p.userid) == 0
                                        ? 0
                                        : _echoDb.RateVideos.Where(u=>u.VideoCreatorId==p.userid).Average(i=>i.Rate),
                                Points = 0
                            })
                    .Select(
                        v =>
                            new Leader()
                            {
                                Username = v.Username,
                                Videos = v.Videos,
                                Comments = v.Comments,
                                Likes = v.Likes,
                                Watched = v.Watched,
                                VideosRate = Math.Round(v.VideosRate,2),
                                Points =
                                    _oneVideoPoint * v.Videos + _oneCommentPoint * v.Comments + _oneLikePoint * v.Likes + _oneWatchedPoint * v.Watched + _oneRatingPoint * Math.Round(v.VideosRate, 2)
                            })
                    .OrderByDescending(w => w.Points)
                    .ToList();
                if (User.IsInRole("Admin")) { return View(leaderModel); }
                else { return View(leaderModel.Take(10).ToList()); }
                
            }
            catch (Exception)
            {
                return View(leaderModel.Take(0).ToList());
            }
        }

        [InitializeSimpleMembership]
        [HttpPost]
        public ActionResult SetPointValue(string videoPoint, string commentPoint, string likePoint, string watchedPoint, string ratingPoint)
        {
            try
            {
                if (!User.IsInRole("Admin")) return Json("error");
                Configuration webConfig = WebConfigurationManager.OpenWebConfiguration("~");
                webConfig.AppSettings.Settings["VideoPoint"].Value = videoPoint;
                webConfig.AppSettings.Settings["CommentPoint"].Value = commentPoint;
                webConfig.AppSettings.Settings["LikePoint"].Value = likePoint;
                webConfig.AppSettings.Settings["WatchedPoint"].Value = watchedPoint;
                webConfig.AppSettings.Settings["RatingPoint"].Value = ratingPoint;
                webConfig.Save();
                return Json("done");
            }
            catch (Exception)
            {

                return Json("error");
            }
        }


        [InitializeSimpleMembership]
        [HttpPost]
        public ActionResult ReadPointValue()
        {
            try
            {
                if (!User.IsInRole("Admin")) return Json("error");
                string videoPoint = ConfigurationManager.AppSettings["VideoPoint"];
                string commentPoint = ConfigurationManager.AppSettings["CommentPoint"];
                string likePoint = ConfigurationManager.AppSettings["LikePoint"];
                string watchedPoint = ConfigurationManager.AppSettings["WatchedPoint"];
                string ratingPoint = ConfigurationManager.AppSettings["RatingPoint"];
                return
                    Json(
                        new
                        {
                            VideoPoint = videoPoint,
                            CommentPoint = commentPoint,
                            LikePoint = likePoint,
                            WatchedPoint = watchedPoint,
                            RatingPoint = ratingPoint
                        });
            }
            catch (Exception)
            {

                return Json("error");
            }
        }
    }
}
