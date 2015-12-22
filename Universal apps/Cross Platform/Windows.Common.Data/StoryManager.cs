using System;
using System.Net;
using System.Windows;
using System.Linq;
using OnlineVideos.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
using Common.Library;


namespace OnlineVideos.Data
{
    public static class StoryManager
    {
        public static string[] ReadFromDatabase(string ShowID, int index, int LanguageId)
        {
            string[] values = new string[2];
            try
            {
                int showid = int.Parse(ShowID);
                List<Stories> storieslist = new List<Stories>();
                storieslist = Task.Run(async () => await Constants.connection.Table<Stories>().Where(i => i.ShowID == showid && i.LanguageID == LanguageId).OrderBy(j => j.ID).ToListAsync()).Result;
                foreach (Stories retriverow in storieslist.Skip(index - 1).Take(1))
                {
                    values[0] = retriverow.Description;
                    values[1] = retriverow.Image;
                    break;
                }
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in ReadFromDatabase Method In StoryManger.cs file", ex);
            }
            return values;
        }
        public static int MaxRows(string ShowID, int LanguageId)
        {

            int showid = int.Parse(ShowID);
            List<Stories> storieslist = new List<Stories>();
            try
            {
                storieslist = Task.Run(async () => await Constants.connection.Table<Stories>().Where(i => i.ShowID == showid && i.LanguageID == LanguageId).OrderBy(j => j.ShowID).ToListAsync()).Result;
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in MaxRows Method In StoryManger.cs file", ex);
            }

            return storieslist.Count();
        }
        public static string Gettitle(string ShowID)
        {
            string Title = string.Empty;
            int showid = int.Parse(ShowID);
            try
            {
                Stories s = Task.Run(async () => await Constants.connection.Table<Stories>().Where(i => i.ShowID == showid).FirstOrDefaultAsync()).Result;
                Title = s.Title;
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in Gettitle Method In StoryManger.cs file", ex);
            }
            return Title;
        }
        public static string GetStoryIntro(string ShowID, int LanguageId)
        {
            string storyIntroduction = string.Empty;
            int storyIntroMaxLength = 0;
#if WINDOWS_PHONE_APP
            if (ResourceHelper.AppName == Apps.Vedic_Library.ToString())
            {
                storyIntroMaxLength = 145;
            }
            else
            {
                storyIntroMaxLength = 294;
            }

#else
                storyIntroMaxLength = 600;
#endif
            try
            {
                int showid = int.Parse(ShowID);
                List<Stories> ListStories = Task.Run(async () => await Constants.connection.Table<Stories>().Where(i => i.ShowID == showid && i.LanguageID == LanguageId).OrderBy(j => j.ID).ToListAsync()).Result;
                foreach (Stories paragraph in ListStories)
                {
                        if (!string.IsNullOrEmpty(paragraph.Description))
                            storyIntroduction += paragraph.Description;
                    
                        if (storyIntroduction.Length > storyIntroMaxLength)
                        {
                          string truncatedIntroduction = storyIntroduction.Substring(0, storyIntroMaxLength);
                           storyIntroduction = truncatedIntroduction.Substring(0, truncatedIntroduction.LastIndexOf(' '));
                        break;
                         
                        }
                    }
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in GetStoryIntro Method In StoryManger.cs file", ex);
            }
            return storyIntroduction;
        }

        //*** Code for Wp8

        public static string GetTitle(int showID)
        {
            string pageTitle = string.Empty;

            var paragraphs = Task.Run(async () => await Constants.connection.Table<Stories>().Where(i => i.ShowID == showID).ToListAsync()).Result;
            foreach (var paragraph in paragraphs)
            {
                pageTitle = paragraph.Title;
                break;
            }

            return pageTitle;
        }
		#if ANDROID
		public static string GetStoryIntroForAndroid(string ShowID, int LanguageId,int StorySize)
		{
			string storyIntroduction = string.Empty;
			int storyIntroMaxLength = StorySize;
			try {
				int showid = int.Parse (ShowID);
				List<Stories> ListStories = Task.Run (async () => await Constants.connection.Table<Stories> ().Where (i => i.ShowID == showid && i.LanguageID == LanguageId).OrderBy (j => j.ID).ToListAsync ()).Result;
				foreach (Stories paragraph in ListStories) {
					if (!string.IsNullOrEmpty (paragraph.Description))
						storyIntroduction += paragraph.Description;

					if (storyIntroduction.Length > storyIntroMaxLength) {
						string truncatedIntroduction = storyIntroduction.Substring (0, storyIntroMaxLength);
						storyIntroduction = truncatedIntroduction.Substring (0, truncatedIntroduction.LastIndexOf (' '));
						break;
					}
				}
			} catch (Exception ex) {
				Exceptions.SaveOrSendExceptions ("Exception in GetStoryIntro Method In StoryManger.cs file", ex);
			}
			return storyIntroduction;
		}
		#endif

  
    }
}
