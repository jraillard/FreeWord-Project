using EchoMe.Models;
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
                try
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

                            int i = 0;
                            int nbtime = 0;
                            foreach (string s in System.IO.File.ReadAllLines(fileName, enc).Skip(1)) //skip the FirstLine
                            {
                                //split
                                tempString = s.Split(';');

                                //Create Words
                                // [0] = french word / [1] = english word / [2] url word
                                //five first words are discovered => =1 (=2 mean discovered and written)
                                if(i<5) { nbtime = 1; }
                                else { nbtime = 0; }
                                WoGamWord woGamWordFR = new WoGamWord
                                {
                                    wd_value = tempString[0],
                                    wd_url = tempString[2],
                                    wd_cat = woGamCategoryFR.cat_id,
                                    wd_nbtime = nbtime
                                    
                                };

                                WoGamWord woGamWordEN = new WoGamWord
                                {
                                    wd_value = tempString[1],
                                    wd_url = tempString[2],
                                    wd_cat = woGamCategoryEN.cat_id,
                                    wd_nbtime = nbtime
                                };

                                woGameDb.WoGamWords.Add(woGamWordFR);
                                woGameDb.WoGamWords.Add(woGamWordEN);
                                woGameDb.SaveChanges();
                                i++;
                            }//foreach

                        }//endif

                    }//end foreach
                }
                catch(Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }                

                return "Done";
            }

            return "User already used";

        }

        public string LoginUser(string username, string password)
        {

            if (woGameDb.WoGamProfiles.Any(p => p.usr_name == username && p.usr_pwd == password))
            {
                return "Done";
            }
            // user doesnt exist

            return "Error";
        }

        public string GetGameLanguage(string username)
        {
            string lng = "";

            //warning don't forget "First()" , or it lng = SQL request
            lng = woGameDb.WoGamProfiles.Where(p => p.usr_name == username).Select(p => p.usr_gameLangage).First();//.ToString();
            if (lng == "English" || lng == "Français")
            {
                return lng;
            }

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
            return "Error";
        }

        public string GetCategories(string username, string language)
        {
            List<string> catList1 = new List<string>();
            List<string> catList2 = new List<string>();
            List<string> catUrlList = new List<string>();
            Dictionary<string, string> catList = new Dictionary<string, string>(); //better than Hashtable => .Net 2.0 (can use linq easier on it)
            string language2 = "";

           
            
            if (woGameDb.WoGamProfiles.Any(p => p.usr_name == username))
            {
                if (language == "Français")
                {
                    language2 = "English";
                }
                else
                {
                    language2 = "Français";
                }

                catList1 = woGameDb.WoGamCategories.Where(p => p.WoGamProfile.usr_name == username && p.cat_usr == p.WoGamProfile.usr_id && p.cat_langage == language).Select(p => p.cat_name).ToList();
                catList2 = woGameDb.WoGamCategories.Where(p => p.cat_usr == p.WoGamProfile.usr_id && p.cat_langage == language2).Select(p => p.cat_name).ToList();
                catUrlList = woGameDb.WoGamCategories.Where(p => p.cat_usr == p.WoGamProfile.usr_id && p.cat_langage == language).Select(p => p.cat_url).ToList();

                for (int i = 0; i < catList1.Count; i++)
                {
                    catList.Add(catList1[i], catList2[i] +"|"+ catUrlList[i]);
                }

                var jsonString = JsonConvert.SerializeObject(catList);

                return jsonString;
            }
            return "Error";
            
        }

        public string GetWordsInCategory(string username, string language, string category)
        {
            List<string> wordList = new List<string>();
            List<int> wordIncList = new List<int>();
            List<string> wordUrlList = new List<string>();
            Dictionary<string, int> wordsInCat = new Dictionary<string, int>(); //better than Hashtable => .Net 2.0 (can use linq easier on it)

            int catID;

            if (woGameDb.WoGamProfiles.Any(p => p.usr_name == username))
            {
                catID = woGameDb.WoGamCategories.Where(p => p.WoGamProfile.usr_name == username && p.cat_usr == p.WoGamProfile.usr_id && p.cat_langage == language && p.cat_name == category ).Select(p => p.cat_id).First(); //First convert in int
                //Debug.WriteLine(catID);
                wordList = woGameDb.WoGamWords.Where(p => p.wd_cat == catID && p.WoGamCategory.cat_langage == language).Select(p => p.wd_value).ToList();
                wordIncList = woGameDb.WoGamWords.Where(p => p.wd_cat == catID && p.WoGamCategory.cat_langage == language).Select(p => p.wd_nbtime).ToList();
                wordUrlList = woGameDb.WoGamWords.Where(p => p.wd_cat == catID && p.WoGamCategory.cat_langage == language).Select(p => p.wd_url).ToList();

                for (int i = 0; i < wordList.Count; i++)
                {
                    wordsInCat.Add(wordList[i] + "|" + wordUrlList[i], wordIncList[i] );
                }

                var jsonString = JsonConvert.SerializeObject(wordsInCat);         

                return jsonString;
            }           

            return "Error";
        }

        public string SendWordsDiscovered(string username, string language, string category, string wordsdiscovered)
        {
            int catID;
            /* //for test
            Dictionary<string, int> wordsdiscovered = new Dictionary<string, int>();
            wordsdiscovered.Add("framboise", 1);
            wordsdiscovered.Add("figue", 2);
            wordsdiscovered.Add("abricot", 1);
            wordsdiscovered.Add("avocat", 2);
            wordsdiscovered.Add("raisin", 3);
            wordsdiscovered.Add("banane", 5);
            */
            Dictionary<string, int> wordsDictionary = JsonConvert.DeserializeObject < Dictionary<string, int>>(wordsdiscovered);

            if (woGameDb.WoGamProfiles.Any(p => p.usr_name == username))
            {
                foreach (KeyValuePair<string, int> de in wordsDictionary)
                {
                    catID = woGameDb.WoGamCategories.Where(p => p.WoGamProfile.usr_name == username && p.cat_usr == p.WoGamProfile.usr_id && p.cat_langage == language && p.cat_name == category).Select(p => p.cat_id).First(); //First convert in int
                    woGameDb.WoGamWords.First(p => p.wd_cat == catID && p.WoGamCategory.cat_langage == language && p.wd_value == de.Key).wd_nbtime += 1; //discovered 1 more time
                    
                }

                woGameDb.SaveChanges();
                return "Done";

            }
                return "Error";
        }

    }
}
