using EchoMe.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Newtonsoft.Json;
using System.Collections;

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


                        foreach (string s in System.IO.File.ReadAllLines(fileName, enc).Skip(1)) //skip the FirstLine
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

        public string LoginUser(string username, string password, string language)
        {
            if (woGameDb.WoGamProfiles.Any(p => p.usr_name == username && p.usr_pwd == password))
            {
                if(woGameDb.WoGamProfiles.Any(p => p.usr_name == username && p.usr_pwd == password && p.usr_gameLangage == language))
                {
                    return "GoToHomepage";
                }

                return "GoToLanguageToPlay";
            }
            // user doesnt exist
            return "Error";
        }

        public string SetGameLanguage(string username, string language)
        {
            if (woGameDb.WoGamProfiles.Any(p => p.usr_name == username))
            {
                woGameDb.WoGamProfiles
                    .First(p => p.usr_name == username)
                    .usr_gameLangage = language;
                woGameDb.SaveChanges();
                return "Done";
            }
            // user doesnt exist
            return "Error_2";
        }

        public string GetCategories(string username, string language)
        {
            List<string> catList = new List<string>();

            if (woGameDb.WoGamProfiles.Any(p => p.usr_name == username))
            {
                catList = woGameDb.WoGamCategories.Where(p => p.cat_usr == p.WoGamProfile.usr_id && p.cat_langage == language).Select(p => p.cat_name).ToList();
                var jsonString = JsonConvert.SerializeObject(catList);
                Debug.WriteLine(jsonString);
                catList = JsonConvert.DeserializeObject<List<string>>(jsonString); //catList == null => don't work

                return "Done";
            }
            return "Error";
        }

        public string GetWordsInCategory(string username, string language, string category)
        {
            List<string> wordList = new List<string>();
            List<int> wordIncList = new List<int>();
            Hashtable wordsInCat = new Hashtable();
            int catID;

            if (woGameDb.WoGamProfiles.Any(p => p.usr_name == username))
            {
                catID = woGameDb.WoGamCategories.Where(p => p.cat_usr == p.WoGamProfile.usr_id && p.cat_langage == language && p.cat_name == category ).Select(p => p.cat_id).First(); //First convert in int
                //Debug.WriteLine(catID);
                wordList = woGameDb.WoGamWords.Where(p => p.wd_cat == catID && p.WoGamCategory.cat_langage == language).Select(p => p.wd_value).ToList();
                wordIncList = woGameDb.WoGamWords.Where(p => p.wd_cat == catID && p.WoGamCategory.cat_langage == language).Select(p => p.wd_nbtime).ToList();

                for (int i = 0; i < wordList.Count; i++)
                {
                    wordsInCat.Add(wordList[i], wordIncList[i]);
                }

                var jsonString = JsonConvert.SerializeObject(wordsInCat);
                //Debug.WriteLine(jsonString);
                wordsInCat = JsonConvert.DeserializeObject<Hashtable>(jsonString);

                return "Done";
            }           

            return "Error";
        }

        public string SendWordsDiscovered(string username, string language, string category, Hashtable wordsdiscovered)
        {
            int catID;
            /*
            wordsdiscovered = new Hashtable();
            wordsdiscovered.Add("abricot", 1);
            wordsdiscovered.Add("avocat", 2);
            wordsdiscovered.Add("raisin", 3);
            wordsdiscovered.Add("banane", 5);
            */

            if (woGameDb.WoGamProfiles.Any(p => p.usr_name == username))
            {                
                foreach (DictionaryEntry de in wordsdiscovered)
                {
                    catID = woGameDb.WoGamCategories.Where(p => p.cat_usr == p.WoGamProfile.usr_id && p.cat_langage == language && p.cat_name == category).Select(p => p.cat_id).First(); //First convert in int
                    woGameDb.WoGamWords.First(p => p.wd_cat == catID && p.WoGamCategory.cat_langage == language && p.wd_value == (string)de.Key).wd_nbtime = (int)de.Value;
                    
                }

               // /WoGamUser/SendWordsDiscovered?username=juju&language=Français&category=Fruits&wordsdiscovered=
                woGameDb.SaveChanges();
                return "Done";

            }
                return "Error";
        }

    }
}
