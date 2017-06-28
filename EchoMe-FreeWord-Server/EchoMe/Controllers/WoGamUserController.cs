using EchoMe.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;


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
                string csvFilePath = "../WoGam_CSV_Files/";
                String[] tempString;

                //create pofile
                WoGamProfile woGamProfile = new WoGamProfile
                {
                    usr_name = username,
                    usr_pwd = password
                };

                woGameDb.WoGamProfiles.Add(woGamProfile);
                woGameDb.SaveChanges();

                //create Languages 
                WoGamLangage woGamLangageFR = new WoGamLangage
                {
                    lng_langage = "Français"
                };

                WoGamLangage woGamLangageEN = new WoGamLangage
                {
                    lng_langage = "English"
                };

                woGameDb.WoGamLangages.Add(woGamLangageFR);
                woGameDb.WoGamLangages.Add(woGamLangageEN);
                woGameDb.SaveChanges();

                

                //Bind profile&langage
                //=>to check if implemented here on in the database
                Console.WriteLine(woGamLangageFR.lng_id);
                Console.WriteLine(woGamLangageEN.lng_id);
                Console.WriteLine(woGamProfile.usr_id);
                WoGamChoice woGamChoice1 = new WoGamChoice
                {
                    chc_langage = woGamLangageFR.lng_id,
                    chc_usr = woGamProfile.usr_id,
                };

                WoGamChoice woGamChoice2 = new WoGamChoice
                {
                    chc_langage = woGamLangageEN.lng_id,
                    chc_usr = woGamProfile.usr_id,
                };

                woGameDb.WoGamChoices.Add(woGamChoice1);
                woGameDb.WoGamChoices.Add(woGamChoice2);
                woGameDb.SaveChanges();

                //Add data
                foreach (string fileName in Directory.GetFiles(csvFilePath))
                {
                    if (Path.GetExtension(fileName) == ".csv")
                    {
                        //testFileName
                        Console.WriteLine(fileName);

                        //Open the file 
                        System.IO.File.Open(csvFilePath + fileName, FileMode.Open);

                        //Create Category read first Line => using System.Linq 
                        tempString = System.IO.File.ReadLines(csvFilePath + fileName).First().Split('s');
                        // [0] = french cat / [1] = english cat / [2] = url cat
                        WoGamCategory woGamCategoryFR = new WoGamCategory
                        {
                            cat_name = tempString[0],
                            cat_reached = false,
                            cat_lng = woGamLangageFR.lng_id,
                            cat_url = tempString[2]

                        };

                        WoGamCategory woGamCategoryEN = new WoGamCategory
                        {
                            cat_name = tempString[1],
                            cat_reached = false,
                            cat_lng = woGamLangageEN.lng_id,
                            cat_url = tempString[2]

                        };

                        woGameDb.WoGamCategories.Add(woGamCategoryFR);
                        woGameDb.WoGamCategories.Add(woGamCategoryEN);
                        woGameDb.SaveChanges();

                        foreach (string s in System.IO.File.ReadAllLines(csvFilePath + fileName).Skip(1)) //skip the FirstLine
                        {
                            //split
                            tempString = s.Split('s');
                            
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
                        }

                    }

                }

            }

            return "";
        }

        public string CreateUserTest(string username, string password)
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

    }
}
