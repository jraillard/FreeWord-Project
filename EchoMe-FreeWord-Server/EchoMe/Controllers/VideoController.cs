using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.WebPages;
using EchoMe.Filters;
using EchoMe.Models;
using Newtonsoft.Json;
using WebMatrix.WebData;

namespace EchoMe.Controllers
{
    public class VideoController : Controller
    {
        private readonly EchoDBEntities _echoDb = new EchoDBEntities();
        //
        // GET: /Video/

        [InitializeSimpleMembership]
        public ActionResult Index(string videoId)
        {
            int videoIdInt = Int32.Parse(videoId);
            ViewBag.VideoId = videoId;
            ViewBag.UserName = User.Identity.Name;
            ViewBag.UserId = WebSecurity.CurrentUserId;
            ViewBag.VideoCreatorId = _echoDb.Presentations.First(p => p.PresentationId == videoIdInt).UserId;
            return View();
        }

        [InitializeSimpleMembership]
        [HttpPost]
        public ActionResult OneVideo(string videoId)
        {
            int videoIdNumber = 0;
            bool isInteger = Int32.TryParse(videoId, out videoIdNumber);
            if (isInteger)
            {
                List<Presentation> presentationList = _echoDb.Presentations.Where(p => p.PresentationId == videoIdNumber).ToList();
                if (presentationList.Count == 1)
                {
                    return Json(presentationList[0]);
                }
                else
                {
                    return Json("error 101A");
                }
            }
            else
            {
                return Json("error 102A");
            }
        }

        [InitializeSimpleMembership]
        [HttpPost]
        public ActionResult UserVideos(string userId)
        {
            try
            {
                int userIdNumber = 0;
                bool isInteger = Int32.TryParse(userId, out userIdNumber);
                if (isInteger)
                {
                    List<Presentation> presentationList = _echoDb.Presentations.Where(p => p.UserId == userIdNumber).ToList();
                    return Json(presentationList);
                }
                return Json("error");
            }
            catch (Exception)
            {
                return Json("error");
            }

        }

        [InitializeSimpleMembership]
        [HttpPost]
        public ActionResult UserComments(string userId)
        {
            try
            {
                int userIdNumber = 0;
                bool isInteger = Int32.TryParse(userId, out userIdNumber);
                int thisUserId = WebSecurity.CurrentUserId;
                if (thisUserId == userIdNumber)
                {
                    if (isInteger)
                    {
                        List<Comment> commentList = _echoDb.Comments.Where(p => p.UsesrId == userIdNumber).ToList();
                        return Json(commentList);
                    }
                }
                return Json("error");
            }
            catch (Exception)
            {
                return Json("error");
            }

        }

        [InitializeSimpleMembership]
        [HttpPost]
        public ActionResult SearchVideos(string searchVideo)
        {
            try
            {
                List<Presentation> presentationList = _echoDb.Presentations.Where(p => p.Name.Contains(searchVideo)||p.Description.Contains(searchVideo)).ToList();
                return Json(presentationList);
            }
            catch (Exception)
            {
                return Json("error");
            }
        }

        [InitializeSimpleMembership]
        [HttpPost]
        public ActionResult SubmitComment(string videoId,string commentTime,string commentText, string isPrivate, string isAnonymous)
        {
            try
            {
                Comment newComment = new Comment
                {
                    PresentationId = Int32.Parse(videoId),
                    Time = commentTime,
                    Content = commentText,
                    UsesrId = WebSecurity.CurrentUserId,
                    UserName = User.Identity.Name,
                    IsPrivate = bool.Parse(isPrivate),
                    IsAnonymous = bool.Parse(isAnonymous)
                };
                _echoDb.Comments.Add(newComment);
                _echoDb.SaveChanges();
                int id = newComment.CommentId;
                return Json(id);
            }
            catch (Exception)
            {
                return Json("error");
            }
        }

        [InitializeSimpleMembership]
        [HttpPost]
        public ActionResult LoadComment(string videoId)
        {
            try
            {
                int userId = WebSecurity.CurrentUserId;
                int presentationId = Int32.Parse(videoId);
                int presentationOwnerId = _echoDb.Presentations.First(p => p.PresentationId == presentationId).UserId;

                List<Comment> commentList =
                    _echoDb.Comments.Where(p => p.PresentationId == presentationId).ToList();

                if (userId != presentationOwnerId)
                {
                    commentList =
                    commentList.Where(
                        p => p.IsPrivate == false || p.UsesrId == userId).ToList();
                }

                List<CustomComment> customCommentList =
                    commentList.Select(
                        p => p.IsAnonymous
                            ? new CustomComment()
                            {
                                CommentId = p.CommentId,
                                Content = p.Content,
                                IsAnonymous = p.IsAnonymous,
                                IsPrivate = p.IsPrivate,
                                PresentationId = p.PresentationId,
                                Time = p.Time,
                                UserName = "Anonymous",
                                UsesrId = 0,
                                UpVote = p.IsPrivate ? -1 : _echoDb.UpVotes.Count(n => n.CommentId == p.CommentId),
                                UpVoteButton = (userId != p.UsesrId),
                                UpVoted = _echoDb.UpVotes.Any(n=>(n.CommentId==p.CommentId&&n.UserId==userId))
                            }
                            : new CustomComment()
                            {
                                CommentId = p.CommentId,
                                Content = p.Content,
                                IsAnonymous = p.IsAnonymous,
                                IsPrivate = p.IsPrivate,
                                PresentationId = p.PresentationId,
                                Time = p.Time,
                                UserName = p.UserName,
                                UsesrId = 0,
                                UpVote = p.IsPrivate ? -1 : _echoDb.UpVotes.Count(n => n.CommentId == p.CommentId),
                                UpVoteButton = userId != p.UsesrId,
                                UpVoted = _echoDb.UpVotes.Any(n => (n.CommentId == p.CommentId && n.UserId == userId))
                            })
                        .ToList();
                return Json(customCommentList);
            }
            catch (Exception)
            {
                return Json("error");
            }
        }

        //UserCommentRemove?commentId
        [InitializeSimpleMembership]
        [HttpPost]
        public ActionResult UserCommentRemove(string commentId)
        {
            try
            {
                int commentIdInt = Int32.Parse(commentId);
                int userId = WebSecurity.CurrentUserId;
                Comment removingComment = _echoDb.Comments.First(p => p.CommentId == commentIdInt && p.UsesrId == userId);
                List<UpVote> removingUpVotes =
                    _echoDb.UpVotes.Where(p => p.CommentId == removingComment.CommentId).ToList();
                foreach (UpVote removingUpVote in removingUpVotes)
                {
                    _echoDb.UpVotes.Remove(removingUpVote);
                }
                _echoDb.Comments.Remove(removingComment);
                _echoDb.SaveChanges();
                return Json("done");
            }
            catch (Exception)
            {
                return Json("error");
            }
        }

        //xhrUserCommentUpdate.open("POST", "/Video/UserCommentUpdate?commentId=" + thisCommentId  + "&commentContent=" +commentContent+ + "&commentPrivate="+commentPrivate+ + "&commentTime="+commentAnonymous);
        [InitializeSimpleMembership]
        [HttpPost]
        public ActionResult UserCommentUpdate(string commentId, string commentContent, string commentPrivate, string commentAnonymous)
        {
            try
            {
                int commentIdInt = Int32.Parse(commentId);
                int userId = WebSecurity.CurrentUserId;
                Comment updatingComment = _echoDb.Comments.First(p => p.CommentId == commentIdInt && p.UsesrId == userId);
                updatingComment.Content = commentContent;
                updatingComment.IsPrivate = bool.Parse(commentPrivate);
                updatingComment.IsAnonymous = bool.Parse(commentAnonymous);
                _echoDb.SaveChanges();
                return Json("done");
            }
            catch (Exception)
            {
                return Json("error");
            }
        }

        //xhrPlusUpvote.open("POST", "/Video/LoadComment?CommentId=" + commentId +"&UserId="+@ViewBag.UserId);
        [InitializeSimpleMembership]
        [HttpPost]
        public ActionResult PlusUpvote(string commentId, string userId)
        {
            try
            {
                int commentIdInt = Int32.Parse(commentId);
                int clientId = Int32.Parse(userId);
                int serverUserId = WebSecurity.CurrentUserId;
                if (clientId == serverUserId)
                {
                    UpVote upVote = new UpVote
                    {
                        CommentId = commentIdInt,
                        UserId = clientId
                    };
                    _echoDb.UpVotes.Add(upVote);
                    _echoDb.SaveChanges();
                    int upvoteNumber = _echoDb.UpVotes.Count(p => p.CommentId == commentIdInt);
                    return Json(upvoteNumber);
                }
                return Json("error");
            }
            catch (Exception)
            {
                return Json("error");
            }
        }

        //xhrPlusUpvote.open("POST", "/Video/MinusComment?CommentId=" + commentId +"&UserId="+@ViewBag.UserId);
        [InitializeSimpleMembership]
        [HttpPost]
        public ActionResult MinusUpvote(string commentId, string userId)
        {
            try
            {
                int commentIdInt = Int32.Parse(commentId);
                int clientId = Int32.Parse(userId);
                int serverUserId = WebSecurity.CurrentUserId;
                if (clientId == serverUserId)
                {
                    UpVote upVote = _echoDb.UpVotes.First(p => p.CommentId == commentIdInt && p.UserId == clientId);
                    _echoDb.UpVotes.Remove(upVote);
                    _echoDb.SaveChanges();
                    int upvoteNumber = _echoDb.UpVotes.Count(p => p.CommentId == commentIdInt);
                    return Json(upvoteNumber);
                }
                return Json("error");
            }
            catch (Exception)
            {
                return Json("error");
            }
        }

        //"/Video/RateVideo?videoId=" + @ViewBag.VideoId + "&userId=" + @ViewBag.UserId + "&rate="+value
        [InitializeSimpleMembership]
        [HttpPost]
        public ActionResult RateVideo(string videoId, string userId, string rate, string videoCreatorId)
        {
            try
            {
                int videoCreatorIdInt = Int32.Parse(videoCreatorId);
                int videoInt = Int32.Parse(videoId);
                int clientId = Int32.Parse(userId);
                int serverUserId = WebSecurity.CurrentUserId;
                int rateInt = Int32.Parse(rate);
                if (clientId == serverUserId)
                {
                    if (_echoDb.RateVideos.Any(p => p.UserId == clientId && p.PresentationId == videoInt))
                    {
                        _echoDb.RateVideos.First(p => p.UserId == clientId && p.PresentationId == videoInt).Rate = rateInt;
                    }
                    else
                    {
                        _echoDb.RateVideos.Add(new RateVideo()
                        {
                            PresentationId = videoInt,
                            UserId = clientId,
                            Rate = rateInt,
                            VideoCreatorId = videoCreatorIdInt
                        });
                    }
                    _echoDb.SaveChanges();
                    return Json("done");
                }
                return Json("error");
            }
            catch (Exception)
            {
                return Json("error");
            }
        }

        //"/Video/GetRateVideo?videoId=" + @ViewBag.VideoId + "&userId=" + @ViewBag.UserId
        [InitializeSimpleMembership]
        [HttpPost]
        public ActionResult GetRateVideo(string videoId, string userId)
        {
            try
            {
                int videoInt = Int32.Parse(videoId);
                int clientId = Int32.Parse(userId);
                int serverUserId = WebSecurity.CurrentUserId;
                int rateInt = 0;
                if (clientId == serverUserId)
                {
                    if (_echoDb.RateVideos.Any(p => p.UserId == clientId && p.PresentationId == videoInt))
                    {
                        rateInt = _echoDb.RateVideos.First(p => p.UserId == clientId && p.PresentationId == videoInt).Rate;
                    }
                    if (!_echoDb.WatchedPresentations.Any(q => q.UserId == clientId && q.PresentationId == videoInt))
                    {
                        _echoDb.WatchedPresentations.Add(new WatchedPresentation()
                        {
                            UserId = clientId,
                            PresentationId = videoInt
                        });
                        _echoDb.SaveChanges();
                    }
                    return Json(rateInt);
                }
                return Json("error");
            }
            catch (Exception)
            {
                return Json("error");
            }
        }
    }

    public class CustomComment
    {
        public int CommentId { get; set; }
        public int PresentationId { get; set; }
        public int UsesrId { get; set; }
        public string UserName { get; set; }
        public string Time { get; set; }
        public string Content { get; set; }
        public bool IsPrivate { get; set; }
        public bool IsAnonymous { get; set; }
        public int UpVote { get; set; }
        public bool UpVoteButton { get; set; }
        public bool UpVoted { get; set; }
    }
}
