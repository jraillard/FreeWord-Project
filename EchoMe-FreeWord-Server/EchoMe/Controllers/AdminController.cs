using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using EchoMe.Filters;
using EchoMe.Models;
using Microsoft.AspNet.Identity;
using WebMatrix.WebData;

namespace EchoMe.Controllers
{
    public class AdminController : Controller
    {
        //
        // GET: /Admin/

        [InitializeSimpleMembership]
        public ActionResult Index()
        {
            List<UserList> userListModel = new List<UserList>();
            try
            {
                if (!User.IsInRole("Admin")) return View(userListModel);
                List<string> roleList = Roles.GetAllRoles().ToList();
                var simpleMembershipProvider = (SimpleMembershipProvider)Membership.Provider;
                foreach (string role in roleList)
                {
                    var roleProvider = (SimpleRoleProvider)Roles.Provider;
                    UserList userList = new UserList
                    {
                        Role = role,
                        UserFields = roleProvider.GetUsersInRole(role).ToList().Select(p=>new UserField(){Username = p, CreateDateTime = simpleMembershipProvider.GetCreateDate(p)}).OrderBy(p=>p.CreateDateTime).ToList()
                    };
                    userListModel.Add(userList);
                }
                return View(userListModel);
            }
            catch (Exception)
            {
                return View(userListModel);
            }
        }

    }
}
