﻿using EchoMe.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Newtonsoft.Json;


namespace EchoMe.Controllers
{
    public class WoGamUserController : Controller
    {
        // GET: /WoGamUser/
        private EchoDBEntities woGameDb = new EchoDBEntities();
        
        public string CreateUser(string username, string password)
        {
            if (!woGameDb.WoGamProfiles.Any(p => p.usr_name == username)) //if doesn't exist
            {
                string csvFilePath = Server.MapPath("~/WoGam_CSV_Files/");
                String[] tempString;                

                //create pofile
                WoGamProfile woGamProfile = new WoGamProfile
                {                    
                    usr_name = username,
                    usr_pwd = password
                };

                woGameDb.WoGamProfiles.Add(woGamProfile);
                woGameDb.SaveChanges();                

                
                //Add data
                foreach (string fileName in Directory.GetFiles(csvFilePath))
                {
                    if (Path.GetExtension(fileName) == ".csv")
                    {

                        //Create Category read first Line => using System.Linq 
                        Encoding enc = Encoding.GetEncoding("iso-8859-1"); //encoding for west EU letters
                        tempString = System.IO.File.ReadLines(fileName, enc).First().Split(';'); //filename contain the path
                        //string tempString2 = System.IO.File.ReadLines(fileName).First();
                        // [0] = french cat / [1] = english cat / [2] = url cat

                        Debug.WriteLine(tempString[0]);
                        WoGamCategory woGamCategoryFR = new WoGamCategory
                        {
                            cat_name = tempString[0],//catFR,
                            cat_reached = false,
                            cat_langage = "Français",
                            cat_url = tempString[2],
                            cat_usr = woGamProfile.usr_id
                        };

                        WoGamCategory woGamCategoryEN = new WoGamCategory
                        {
                            cat_name = tempString[1],//catEN,

                            cat_reached = false,
                            cat_langage = "English",
                            cat_url = tempString[2],
                            cat_usr = woGamProfile.usr_id
                        };

                        woGameDb.WoGamCategories.Add(woGamCategoryFR);
                        woGameDb.WoGamCategories.Add(woGamCategoryEN);
                        woGameDb.SaveChanges();

                        
                        foreach (string s in System.IO.File.ReadAllLines(fileName,enc).Skip(1)) //skip the FirstLine
                        {
                            //split
                            tempString = s.Split(';');
                            
                            //Create Words
                            // [0] = french word / [1] = english word / [2] url word
                            WoGamWord woGamWordFR = new WoGamWord
                            {
                                wd_value = tempString[0],
                                wd_url = tempString[2],
                                wd_cat = woGamCategoryFR.cat_id,
                                wd_nbtime = 0
                            };

                            WoGamWord woGamWordEN = new WoGamWord
                            {
                                wd_value = tempString[1],
                                wd_url = tempString[2],
                                wd_cat = woGamCategoryEN.cat_id,
                                wd_nbtime = 0
                            };

                            woGameDb.WoGamWords.Add(woGamWordFR);
                            woGameDb.WoGamWords.Add(woGamWordEN);
                            woGameDb.SaveChanges();
                        }//foreach
                        
                    }//endif

                }//end foreach
                
                return "Done";
            }

            return "error_3";
           
        }

        public string LoginUser(string username, string password)
        {
            if (woGameDb.WoGamProfiles.Any(p => p.usr_name == username && p.usr_pwd == password))
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


        public List<string> GetCategories(string username, string langage)
        {
            List<string> catList = new List<string>();

            if (woGameDb.WoGamProfiles.Any(p => p.usr_name == username))
            {
                catList = woGameDb.WoGamCategories.Where(p => p.cat_usr == p.WoGamProfile.usr_id && p.cat_langage == langage).Select(p => p.cat_name).ToList();
            }

             var jsonString= JsonConvert.SerializeObject(catList);      
            Debug.WriteLine( jsonString);
            catList = JsonConvert.DeserializeObject<List<string>>(jsonString); //catList == null => don't work
            
            return null;
        }

    }
}
