using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EchoMe.Models;

namespace EchoMe.Controllers
{
    public class WoGamUserController : Controller
    {
        //
        // GET: /WoGamUser/
        private EchoDBEntities woGameDb = new EchoDBEntities();


        public string CreateUser(string username, string password)
        {
            if (!woGameDb.WoGamProfiles.Any(p => p.usr_name == username))
            {
                WoGamProfile woGamProfile = new WoGamProfile
                {
                    usr_name = username,
                    usr_pwd = password
                };
                woGameDb.WoGamProfiles.Add(woGamProfile);
                woGameDb.SaveChanges();
                return "Done";
            }
            // user already exist
            return "Error_0";
        }

        public string LoginUser(string username, string password)
        {
            if (woGameDb.WoGamProfiles.Any(p=>p.usr_name==username&&p.usr_pwd==password))
            {
                return "Done";
            }
            // user doesnt exist
            return "Error_1";
        }

        public string SetGameLanguage(string username, string password, string language)
        {
            if (woGameDb.WoGamProfiles.Any(p => p.usr_name == username && p.usr_pwd == password))
            {
                woGameDb.WoGamProfiles
                    .First(p => p.usr_name == username && p.usr_pwd == password)
                    .usr_gameLangage = language;
                woGameDb.SaveChanges();
                return "Done";
            }
            // user doesnt exist
            return "Error_2";
        }
        
        public string GetGameLanguage(string username, string password, string language)
        {
            if (woGameDb.WoGamProfiles.Any(p => p.usr_name == username && p.usr_pwd == password))
            {
                return "Done";
            }
            // user doesnt exist
            return "Error_3";
        }



    }
}
