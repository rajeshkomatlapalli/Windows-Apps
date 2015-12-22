#define DEBUG_AGENT
using System;
using System.Net;
using System.Windows;
using System.Windows.Input;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Xml.Linq;
using System.Linq;
using comm =OnlineVideos.Entities;
using System.IO;
using com = OnlineVideos.Common;
using System.Threading;
using System.Globalization;
using System.Xml.Serialization;
using OnlineVideos.Entities;
using System.Reflection;
using System.Linq.Expressions;
using System.Collections;
using Common.Library;
using System.Threading.Tasks;
using OnlineVideos.Data;
using OnlineVideos.Library;
using lib = Common.Library;
#if ( WINDOWS_APP && NOTANDROID)|| (WINDOWS_PHONE_APP && NOTANDROID)
using Windows.Storage;
using Windows.Storage.Streams;
#endif
#if WINDOWS_APP
using System.Net.Http;
#endif

public static class MovieUpdates
{
    public static string ApplicationName = string.Empty;
    public static List<string> Downloadedmovieid = new List<string>();
    public static List<string> TableNames = new List<string>();

    public static void checkForShowUpdates()
    {
        try
        {
#if WINDOWS_APP
            if (SettingsHelper.getStringValue("DownloadCompleted") != "0")
#endif
#if WINDOWS_PHONE_APP || ANDROID
if (NetworkHelper.IsNetworkAvailableForDownloads() && SettingsHelper.getStringValue("DownloadCompleted") != "0")
#endif
            {
                ShowDownloadForBlogs show = new ShowDownloadForBlogs();
                show.StartDownload();

            }
            else
            {
                AppSettings.BackgroundAgentStatus = SyncAgentStatus.UpdateShows;
                if (OnlineVideos.Common.SyncAgentState.SyncAgent != null)
                {
                    ((OnlineVideos.Common.IDowloadCompleteCallback)(OnlineVideos.Common.SyncAgentState.SyncAgent)).OnStartEvent();
                }
            }
            Exceptions.UpdateAgentLog("checkForShowUpdates Completed In MovieUpdates ");
			#if ANDROID 
			Constants.Timer.Interval=10000;
			Constants.Timer.Start();
			#endif
        }
        catch (Exception ex)
        {
			#if ANDROID 
			Constants.Timer.Interval=10000;
			Constants.Timer.Start();
			#endif
            Exceptions.SaveOrSendExceptions("Exception in checkForShowUpdates Method In MovieUpdates.cs file", ex);
        }
    }
    public static object CheckShow(Type TableType, string FirstColoumnName, string FirstColoumnValue, string SecondColoumnName, string SecondColoumnValue, string ThirdColoumnName, string ThirdColoumnValue, object TableInstance)
    {
        try
        {
#if WINDOWS_PHONE_APP
            //var d1 = typeof(DataManager<>);
            //Type[] typeArgs = { TableType };
            //var makeme = d1.MakeGenericType(typeArgs);
            //object o = Activator.CreateInstance(makeme);
            //var queryableData = o.GetType().GetTypeInfo().GetDeclaredMethod("LoadData").Invoke(o, null);
#endif
            object RowExists = null;
            ParameterExpression pe = System.Linq.Expressions.Expression.Parameter(TableType, "i");
#if (WINDOWS_APP && NOTANDROID) || (WINDOWS_PHONE_APP && NOTANDROID)
            System.Linq.Expressions.Expression left = System.Linq.Expressions.Expression.Property(pe, TableType.GetRuntimeProperty(FirstColoumnName));
            System.Linq.Expressions.Expression right = System.Linq.Expressions.Expression.Constant(Convert.ChangeType(FirstColoumnValue, TableType.GetRuntimeProperty(FirstColoumnName).PropertyType, null));
            System.Linq.Expressions.Expression left1 = System.Linq.Expressions.Expression.Property(pe, TableType.GetRuntimeProperty(SecondColoumnName));
            System.Linq.Expressions.Expression right1 = System.Linq.Expressions.Expression.Constant(Convert.ChangeType(SecondColoumnValue, TableType.GetRuntimeProperty(SecondColoumnName).PropertyType, null));
            System.Linq.Expressions.Expression left2 = System.Linq.Expressions.Expression.Property(pe, TableType.GetRuntimeProperty(ThirdColoumnName));
            System.Linq.Expressions.Expression right2 = System.Linq.Expressions.Expression.Constant(Convert.ChangeType(ThirdColoumnValue, TableType.GetRuntimeProperty(ThirdColoumnName).PropertyType, null));
#endif
#if ANDROID
            System.Linq.Expressions.Expression left = System.Linq.Expressions.Expression.Property(pe, TableType.GetProperty(FirstColoumnName));
            System.Linq.Expressions.Expression right = System.Linq.Expressions.Expression.Constant(Convert.ChangeType(FirstColoumnValue, TableType.GetProperty(FirstColoumnName).PropertyType, null));
            System.Linq.Expressions.Expression left1 = System.Linq.Expressions.Expression.Property(pe, TableType.GetProperty(SecondColoumnName));
            System.Linq.Expressions.Expression right1 = System.Linq.Expressions.Expression.Constant(Convert.ChangeType(SecondColoumnValue, TableType.GetProperty(SecondColoumnName).PropertyType, null));
            System.Linq.Expressions.Expression left2 = System.Linq.Expressions.Expression.Property(pe, TableType.GetProperty(ThirdColoumnName));
            System.Linq.Expressions.Expression right2 = System.Linq.Expressions.Expression.Constant(Convert.ChangeType(ThirdColoumnValue, TableType.GetProperty(ThirdColoumnName).PropertyType, null));
#endif
            var comparison = System.Linq.Expressions.Expression.Equal(left, right);
            var comparison1 = System.Linq.Expressions.Expression.Equal(left1, right1);
            var comparison3 = System.Linq.Expressions.Expression.Equal(left2, right2);
            System.Linq.Expressions.Expression Combine = System.Linq.Expressions.Expression.And(comparison, comparison1);
            System.Linq.Expressions.Expression Combine1 = System.Linq.Expressions.Expression.And(Combine, comparison3);
            var delegateType = typeof(Func<,>).MakeGenericType(TableType, typeof(bool));
            var yourExpression = System.Linq.Expressions.Expression.Lambda(delegateType, Combine1, pe);
#if WINDOWS_PHONE_APP
            //Type listtype = typeof(List<>).MakeGenericType(TableType);
            //IList list = (IList)Activator.CreateInstance(listtype);
            //list = (IList)queryableData;
            //MethodCallExpression whereCallExpression = System.Linq.Expressions.Expression.Call(
            //       typeof(Queryable),
            //       "Where",
            //       new Type[] { list.AsQueryable().ElementType },
            //       list.AsQueryable().Expression,
            //      yourExpression);
            //var results = list.AsQueryable().Provider.CreateQuery(whereCallExpression);
            //foreach (var val in results)
            //{
            //    RowExists = val;
            //}
            var d1 = typeof(DataManager<>);
            Type[] typeArgs = { TableType };
            var makeme = d1.MakeGenericType(typeArgs);
            object o = Activator.CreateInstance(makeme);
            RowExists = o.GetType().GetTypeInfo().GetDeclaredMethod("Get").Invoke(o, new object[] { yourExpression });
            Exceptions.UpdateAgentLog("CheckShow Method Completed In MovieUpdates");
#else
            var d1 = typeof(DataManager<>);
            Type[] typeArgs = { TableType };
            var makeme = d1.MakeGenericType(typeArgs);
            object o = Activator.CreateInstance(makeme);
            RowExists = o.GetType().GetTypeInfo().GetDeclaredMethod("Get").Invoke(o, new object[] { yourExpression });
            Exceptions.UpdateAgentLog("CheckShow Method Completed In MovieUpdates");
#endif
#if WINDOWS_PHONE_APP
            //queryableData = null;
            GC.Collect();
            GC.WaitForPendingFinalizers();
#endif
            return RowExists;
        }
        catch (Exception ex)
        {
            Exceptions.SaveOrSendExceptions("Exception in CheckShow Method In MovieUpdates.cs file", ex);
            return null;
        }

    }
    public static object CheckShow(Type TableType, string FirstColoumnName, string FirstColoumnValue, string SecondColoumnName, string SecondColoumnValue, object TableInstance)
    {
        try
        {
#if WINDOWS_PHONE_APP
            //var d1 = typeof(DataManager<>);
            //Type[] typeArgs = { TableType };
            //var makeme = d1.MakeGenericType(typeArgs);
            //object o = Activator.CreateInstance(makeme);
            //var queryableData = o.GetType().GetTypeInfo().GetDeclaredMethod("LoadData").Invoke(o, null);
#endif
            object RowExists = null;
            ParameterExpression pe = System.Linq.Expressions.Expression.Parameter(TableType, "i");
#if (WINDOWS_APP && NOTANDROID) || (WINDOWS_PHONE_APP && NOTANDROID)
            System.Linq.Expressions.Expression left = System.Linq.Expressions.Expression.Property(pe, TableType.GetRuntimeProperty(FirstColoumnName));
            System.Linq.Expressions.Expression right = System.Linq.Expressions.Expression.Constant(Convert.ChangeType(FirstColoumnValue, TableType.GetRuntimeProperty(FirstColoumnName).PropertyType, null));
            System.Linq.Expressions.Expression left1 = System.Linq.Expressions.Expression.Property(pe, TableType.GetRuntimeProperty(SecondColoumnName));
            System.Linq.Expressions.Expression right1 = System.Linq.Expressions.Expression.Constant(Convert.ChangeType(SecondColoumnValue, TableType.GetRuntimeProperty(SecondColoumnName).PropertyType, null));
#endif
#if ANDROID
            System.Linq.Expressions.Expression left = System.Linq.Expressions.Expression.Property(pe, TableType.GetProperty(FirstColoumnName));
            System.Linq.Expressions.Expression right = System.Linq.Expressions.Expression.Constant(Convert.ChangeType(FirstColoumnValue, TableType.GetProperty(FirstColoumnName).PropertyType, null));
            System.Linq.Expressions.Expression left1 = System.Linq.Expressions.Expression.Property(pe, TableType.GetProperty(SecondColoumnName));
            System.Linq.Expressions.Expression right1 = System.Linq.Expressions.Expression.Constant(Convert.ChangeType(SecondColoumnValue, TableType.GetProperty(SecondColoumnName).PropertyType, null));
#endif
            var comparison = System.Linq.Expressions.Expression.Equal(left, right);
            var comparison1 = System.Linq.Expressions.Expression.Equal(left1, right1);
            System.Linq.Expressions.Expression Combine = System.Linq.Expressions.Expression.And(comparison, comparison1);
            var delegateType = typeof(Func<,>).MakeGenericType(TableType, typeof(bool));
            var yourExpression = System.Linq.Expressions.Expression.Lambda(delegateType, Combine, pe);
#if WINDOWS_PHONE_APP
            //Type listtype = typeof(List<>).MakeGenericType(TableType);
            //IList list = (IList)Activator.CreateInstance(listtype);
            //list = (IList)queryableData;
            //MethodCallExpression whereCallExpression = System.Linq.Expressions.Expression.Call(
            //       typeof(Queryable),
            //       "Where",
            //       new Type[] { list.AsQueryable().ElementType },
            //       list.AsQueryable().Expression,
            //      yourExpression);
            //var results = list.AsQueryable().Provider.CreateQuery(whereCallExpression);
            //foreach (var val in results)
            //{
            //    RowExists = val;
            //}
            var d1 = typeof(DataManager<>);
            Type[] typeArgs = { TableType };
            var makeme = d1.MakeGenericType(typeArgs);
            object o = Activator.CreateInstance(makeme);
            RowExists = o.GetType().GetTypeInfo().GetDeclaredMethod("Get").Invoke(o, new object[] { yourExpression });
            Exceptions.UpdateAgentLog("CheckShow Method Completed In MovieUpdates");
#else
            var d1 = typeof(DataManager<>);
            Type[] typeArgs = { TableType };
            var makeme = d1.MakeGenericType(typeArgs);
            object o = Activator.CreateInstance(makeme);
            RowExists = o.GetType().GetTypeInfo().GetDeclaredMethod("Get").Invoke(o, new object[] { yourExpression });
            Exceptions.UpdateAgentLog("CheckShow Method Completed In MovieUpdates");
#endif
#if WINDOWS_PHONE_APP
            //queryableData = null;
            GC.Collect();
            GC.WaitForPendingFinalizers();
#endif
            return RowExists;
        }
        catch (Exception ex)
        {
            Exceptions.SaveOrSendExceptions("Exception in CheckShow Method In MovieUpdates.cs file", ex);
            return null;
        }

    }
    public static object CheckShow(Type TableType, string ColoumnName, string ColoumnValue, object TableInstance)
    {
        try
        {
#if WINDOWS_PHONE_APP
            //var d1 = typeof(DataManager<>);
            //Type[] typeArgs = { TableType };
            //var makeme = d1.MakeGenericType(typeArgs);
            //object o = Activator.CreateInstance(makeme);
            //var queryableData = o.GetType().GetTypeInfo().GetDeclaredMethod("LoadData").Invoke(o, null);
#endif
            object RowExists = null;
            ParameterExpression pe = System.Linq.Expressions.Expression.Parameter(TableType, "i");
#if (WINDOWS_APP && NOTANDROID) || (WINDOWS_PHONE_APP && NOTANDROID)
            System.Linq.Expressions.Expression left = System.Linq.Expressions.Expression.Property(pe, TableType.GetRuntimeProperty(ColoumnName));
            System.Linq.Expressions.Expression right = System.Linq.Expressions.Expression.Constant(Convert.ChangeType(ColoumnValue, TableType.GetRuntimeProperty(ColoumnName).PropertyType, null));
#endif
#if ANDROID
            System.Linq.Expressions.Expression left = System.Linq.Expressions.Expression.Property(pe, TableType.GetProperty(ColoumnName));
            System.Linq.Expressions.Expression right = System.Linq.Expressions.Expression.Constant(Convert.ChangeType(ColoumnValue, TableType.GetProperty(ColoumnName).PropertyType, null));
#endif
            var comparison = System.Linq.Expressions.Expression.Equal(left, right);
            var delegateType = typeof(Func<,>).MakeGenericType(TableType, typeof(bool));
            var yourExpression = System.Linq.Expressions.Expression.Lambda(delegateType, comparison, pe);
#if WINDOWS_PHONE_APP

            var d1 = typeof(DataManager<>);
            Type[] typeArgs = { TableType };
            var makeme = d1.MakeGenericType(typeArgs);
            object o = Activator.CreateInstance(makeme);            
            RowExists = o.GetType().GetTypeInfo().GetDeclaredMethod("Get").Invoke(o, new object[] { yourExpression });
            Exceptions.UpdateAgentLog("CheckShow Method Completed In MovieUpdates");


            //Type listtype = typeof(List<>).MakeGenericType(TableType);
            //IList list = (IList)Activator.CreateInstance(listtype);
            //list = (IList)queryableData;
            //MethodCallExpression whereCallExpression = System.Linq.Expressions.Expression.Call(
            //       typeof(Queryable),
            //       "Where",
            //       new Type[] { list.AsQueryable().ElementType },
            //       list.AsQueryable().Expression,
            //      yourExpression);
            //var results = list.AsQueryable().Provider.CreateQuery(whereCallExpression);
            //foreach (var val in results)
            //{
            //    RowExists = val;
            //}            
#else
            var d1 = typeof(DataManager<>);
            Type[] typeArgs = { TableType };
            var makeme = d1.MakeGenericType(typeArgs);
            object o = Activator.CreateInstance(makeme);
            RowExists = o.GetType().GetTypeInfo().GetDeclaredMethod("Get").Invoke(o, new object[] { yourExpression });
            Exceptions.UpdateAgentLog("CheckShow Method Completed In MovieUpdates");
#endif
#if WINDOWS_PHONE_APP
            //queryableData = null;
            GC.Collect();
            GC.WaitForPendingFinalizers();
#endif
            return RowExists;
        }
        catch (Exception ex)
        {
            Exceptions.SaveOrSendExceptions("Exception in CheckShow Method In MovieUpdates.cs file", ex);
            return null;
        }
    }
    public  static void DownloadNextShowWithTopVideosAndAppCountCompleted(string xmlname)
    {
        try
        {
            AppSettings.RelatedAppCount = 11;
            if (AppSettings.LastUpdatedDate < AppSettings.LastPublishedDate)
            {
                InsertorUpdateTable(xmlname);
            }
            else
            {
                AppSettings.LastUpdatedDate = AppSettings.LastPublishedDate;
                Downloadedmovieid.Clear();

                if (Task.Run(async()=>  await Storage.FileExists("Movies.xml")).Result)
                    Storage.DeleteFile("Movies.xml");
                    AppSettings.BackgroundAgentStatus = SyncAgentStatus.UpDateLiveTile;
                if (SettingsHelper.Contains(ResourceHelper.ProjectName + "LartTileDate"))
                {
                    if (AppSettings.LartTileDate != null)
                    {
                        AppSettings.LartTileDate = AppSettings.LastUpdatedDate;
                    }
                }
                else
                {
                    AppSettings.LartTileDate = AppSettings.LastUpdatedDate; ;
                }
				#if ANDROID
				Constants.Timer.Interval=10000;
				Constants.Timer.Start();
				#endif
				Exceptions.UpdateAgentLog("DownloadNextShowWithTopVideosAndAppCountCompleted Completed In MovieUpdates");   
            }
           
        }
        catch (Exception ex)
        {
            Exceptions.SaveOrSendExceptions("Exception in DownloadNextShowWithTopVideosAndAppCountCompleted Method In MovieUpdates.cs file", ex);
        }
    }

	public static void RetriveConditionsForTables(List<string> TableNames, string Xmlname)
    {
        try
        {
            Dictionary<string, string> ConditionForWhereClause = new Dictionary<string, string>();
#if WINDOWS_PHONE_APP
            Assembly DataAssembly = Assembly.Load(new AssemblyName(Constants.NamespaceForData));
#endif

#if WINDOWS_APP
      Assembly DataAssembly = Assembly.Load(new AssemblyName("OnlineVideos.Entities"));
#endif
			string TableNamesForUpDateAgentLog=string.Empty;
			foreach (var TableName in TableNames)
            {

                Type TableType = DataAssembly.GetType(Constants.NamespaceForData+"."+TableName.ToString());
#if (WINDOWS_APP && NOTANDROID) || (WINDOWS_PHONE_APP && NOTANDROID)
				PropertyInfo[] ConditionProperty=TableType.GetRuntimeProperties().ToArray();
#endif
#if  ANDROID
				PropertyInfo[] ConditionProperty = TableType.GetProperties();
#endif

                foreach (PropertyInfo property in ConditionProperty)
                {
                    IEnumerable<Attribute> ConditionAttributes = property.GetCustomAttributes();
                    foreach (Attribute condition in ConditionAttributes)
                    {
                        if (condition is OnlineVideos.Entities.ConditionTypeAttribute)
                        {
                            OnlineVideos.Entities.ConditionTypeAttribute c = (OnlineVideos.Entities.ConditionTypeAttribute)condition;

                            if (c.Condition.ContainsValue(property.Name.ToString()))
                            {
                                foreach (KeyValuePair<string, string> pair in c.Condition)
                                {
                                    if (pair.Value.Equals(property.Name.ToString()))
                                        ConditionForWhereClause.Add(property.Name.ToString() + "-" + TableName, pair.Key);
                                }
                            }
                        }
                    }
                }
				TableNamesForUpDateAgentLog+=TableName +"  ";
            }
            if (Xmlname.Contains("Movies"))
            {
#if WINDOWS_APP
                AppSettings.ShowTableConditions = ConditionForWhereClause; 
#endif
#if WINDOWS_PHONE_APP
                AppSettings.ShowTableConditions_New = ConditionForWhereClause;
#endif
            }
            else
            {
#if WINDOWS_APP
                AppSettings.PeopleTableConditions = ConditionForWhereClause; 
#endif
#if WINDOWS_PHONE_APP
                AppSettings.PeopleTableConditions_New = ConditionForWhereClause;
#endif

            }
			//Exceptions.UpdateAgentLog("Conditions ForTables are "+ " " + TableNamesForUpDateAgentLog + " " +"In"+" " +Xmlname+ " " +" In RetriveConditionsForTables Method Completed In MovieUpdates.cs");
        }
        catch (Exception ex)
        {
            Exceptions.SaveOrSendExceptions("Exception in RetriveConditionsForTables Method In MovieUpdates.cs file", ex);
        }
    }
    public static void RetriveConditionsForTable(XDocument TableNames, string Xmlname)
    {
        try
        {
            Dictionary<string, string> ConditionForWhereClause = new Dictionary<string, string>();
            List<string> TableNamesList = new List<string>();
#if WINDOWS_PHONE_APP
            Assembly DataAssembly = Assembly.Load(new AssemblyName(Constants.NamespaceForData));
#endif
#if WINDOWS_APP
                     Assembly DataAssembly = Assembly.Load(new AssemblyName("OnlineVideos.Entities"));
#endif
            foreach (var TableName in TableNames.Root.Elements().OfType<XElement>().Select(x => x.Name).Distinct())
            {
                Type TableType = DataAssembly.GetType(Constants.NamespaceForData + "." + TableName.ToString());

                IEnumerable<Attribute> ConditionAttributes = TableType.GetTypeInfo().GetCustomAttributes();
                foreach (Attribute condition in ConditionAttributes)
                {
                    if (condition is OnlineVideos.Entities.ConditionTypeAttribute)
                    {
                        OnlineVideos.Entities.ConditionTypeAttribute c = (OnlineVideos.Entities.ConditionTypeAttribute)condition;

                        if (c.Condition.ContainsValue(TableType.Name.ToString()))
                        {
                            foreach (KeyValuePair<string, string> pair in c.Condition)
                            {                                
                                ConditionForWhereClause.Add(TableType.Name.ToString(), pair.Key);
                                TableNamesList.Add(TableType.Name.ToString());
                            }
                        }
                    }
                }
            }
            if (Xmlname.Contains("Movies"))
            {
                AppSettings.ShowTableCondition = ConditionForWhereClause;
                AppSettings.ShowTableNames = TableNamesList;
#if WINDOWS_PHONE_APP
                AppSettings.ShowTableCondition_New = ConditionForWhereClause;
                AppSettings.ShowTableNames_New = TableNamesList;
#endif
                AppSettings.ShowTableCondition = ConditionForWhereClause;
                AppSettings.ShowTableNames = TableNamesList;
            }
            else
            {
#if WINDOWS_PHONE_APP
                AppSettings.PeopleTableCondition_New = ConditionForWhereClause;
                AppSettings.PeopleTableNames_New = TableNamesList;
#endif
                AppSettings.PeopleTableCondition= ConditionForWhereClause;
                AppSettings.PeopleTableNames = TableNamesList;
                AppSettings.PeopleTableCondition = ConditionForWhereClause;
                
            }
			//Exceptions.UpdateAgentLog("Conditions For Tables From" +" "+Xmlname+" " +"In RetriveConditionsForTable Method Completed In MovieUpdates.cs ");
        }
        catch (Exception ex)
        {
            Exceptions.SaveOrSendExceptions("Exception in RetriveConditionsForTable Method In MovieUpdates.cs file", ex);
        }
    }
    public static string[] GetPath(lib.ShowImages value)
    {
        string[] path = new string[2];
        try
        {
            path[0] = ".jpg";
            if (value.ToString().Contains("FlickrPersonImageUrl"))
            {
                path[1] = "Images\\PersonImages\\";
            }
            if (value.ToString().Contains("Scale100"))
            {
                path[1] = "Images\\scale-100\\";
            }
            if (value.ToString().Contains("Scale140"))
            {
                path[1] = "Images\\scale-140\\";
            }
            if (value.ToString().Contains("Scale180"))
            {
                path[1] = "Images\\Scale-180\\";
            }
            if (value.ToString().Contains("ListImages"))
            {
                path[1] = "Images\\ListImages\\";
            }
            if (value.ToString().Contains("Tile30_30"))
            {
                path[1] = "Images\\TileImages\\30-30\\";
            }
            if (value.ToString().Contains("Tile150_150"))
            {
                path[1] = "Images\\TileImages\\150-150\\";
            }            
            if (value.ToString().Contains("Tile310_150"))
            {
                path[1] = "Images\\TileImages\\310-150\\";
            }
            if (value.ToString().Contains("FlickrStoryImageUrl"))
            {
                path[1] = "Images\\storyImages\\";
            }
            if (value.ToString().Contains("Tile310_150"))
            {
                path[1] = "Images\\TileImages\\310-150\\";
            }
            if (value.ToString().Contains("FlickrQuizImageUrl"))
            {
                path[1] = "Images\\QuestionsImage\\";
            }
            //string[] path = new string[2];
            //try
            //{
            //    FieldInfo fi = value.GetType().GetTypeInfo().GetDeclaredField(value.ToString());
            //    IEnumerable<Attribute> ConditionAttributes = fi.GetCustomAttributes();
            //    foreach (Attribute condition in ConditionAttributes)
            //    {
            //        if (condition is comm.ConditionTypeAttribute)
            //        {
            //            comm.ConditionTypeAttribute c = (comm.ConditionTypeAttribute)condition;
            //            foreach (KeyValuePair<string, string> pair in c.Condition)
            //            {
            //                path[0] = pair.Value;
            //                path[1] = pair.Key;
            //            }
            //        }
            //    }
            //Exceptions.UpdateAgentLog(value+"  " +"is Complrted In GetPath Method Completed In MovieUpdates.cs");
        }
        catch (Exception ex)
        {
            Exceptions.SaveOrSendExceptions("Exception in GetPath Method In MovieUpdates.cs file", ex);
        }
        return path;
    }

    private async static void InsertorUpdateTable(string XmlName)
    {
        try
        {
            string ConditionBeforeInsert = string.Empty;
            object rowExists = null;
            int LastShowId = 0;
            int FirstShowId = 0;
            //AppSettings.CheckedTableNames = new List<string>();
#if WINDOWS_PHONE_APP
            AppSettings.DeletedTableNames_New = new List<string>();
#endif
            AppSettings.DeletedTableNames = new List<string>();
            List<XElement> ListtoInsert = new List<XElement>();
# if (WINDOWS_APP && NOTANDROID)
            StorageFolder store = ApplicationData.Current.LocalFolder;
            StorageFile file = Task.Run(async () => await store.CreateFileAsync(XmlName, Windows.Storage.CreationCollisionOption.OpenIfExists)).Result;
            var f = Task.Run(async () => await file.OpenAsync(FileAccessMode.Read)).Result;
            IInputStream inputStream = f.GetInputStreamAt(0);
            DataReader dataReader = new DataReader(inputStream);
            uint numBytesLoaded = Task.Run(async () => await dataReader.LoadAsync((uint)f.Size)).Result;
            string xmlData = (dataReader.ReadString(numBytesLoaded)).ToString();
            System.IO.MemoryStream ms = new System.IO.MemoryStream(System.Text.Encoding.UTF8.GetBytes(xmlData));
            XDocument doc = XDocument.Load(ms);
            //await f.FlushAsync();
            f.Dispose();
            inputStream.Dispose();
            ms.Dispose();
#endif

# if ANDROID || WINDOWS_PHONE_APP

            StorageFolder store = ApplicationData.Current.LocalFolder;
            StorageFile file = Task.Run(async () => await store.CreateFileAsync(XmlName, Windows.Storage.CreationCollisionOption.OpenIfExists)).Result;
            var f = Task.Run(async () => await file.OpenAsync(FileAccessMode.Read)).Result;
            IInputStream inputStream = f.GetInputStreamAt(0);
            DataReader dataReader = new DataReader(inputStream);
            uint numBytesLoaded = Task.Run(async () => await dataReader.LoadAsync((uint)f.Size)).Result;
            string xmlData = (dataReader.ReadString(numBytesLoaded)).ToString();
            System.IO.MemoryStream ms = new System.IO.MemoryStream(System.Text.Encoding.UTF8.GetBytes(xmlData));
            XDocument doc = XDocument.Load(ms);
            //await f.FlushAsync();
            f.Dispose();
            inputStream.Dispose();
            ms.Dispose();

            string docty = doc.ToString();
#endif
            List<string> TableNames = new List<string>();
            Dictionary<string, string> TableCondition = new Dictionary<string, string>();
            Dictionary<string, string> TableConditions = new Dictionary<string, string>();
            if (AppSettings.IsNewVersionForTables == true)
            {
                if (XmlName.Contains("Movies"))
                {
                    RetriveConditionsForTable(doc, XmlName);
#if WINDOWS_APP
                    RetriveConditionsForTables(AppSettings.ShowTableNames, XmlName); 
#endif
#if WINDOWS_PHONE_APP
                    RetriveConditionsForTables(AppSettings.ShowTableNames_New, XmlName);
#endif
                    AppSettings.IsNewVersionForTables = false;
                }
                else
                {
                    RetriveConditionsForTable(doc, XmlName);
#if WINDOWS_APP
                    RetriveConditionsForTables(AppSettings.PeopleTableNames, XmlName); 
#endif
#if WINDOWS_PHONE_APP
                    RetriveConditionsForTables(AppSettings.PeopleTableNames_New, XmlName);
#endif
                }
            }

            if (XmlName.Contains("Movies"))
            {
#if WINDOWS_APP
                TableNames = AppSettings.ShowTableNames;
                TableConditions = AppSettings.ShowTableConditions;
                TableCondition = AppSettings.ShowTableCondition; 
#endif
#if WINDOWS_PHONE_APP
                TableNames = AppSettings.ShowTableNames_New;
                TableConditions = AppSettings.ShowTableConditions_New;
                TableCondition = AppSettings.ShowTableCondition_New;
#endif
            }
            else
            {
#if WINDOWS_APP
                TableNames = AppSettings.PeopleTableNames;
                TableConditions = AppSettings.PeopleTableConditions;
                TableCondition = AppSettings.PeopleTableCondition; 
#endif
#if WINDOWS_PHONE_APP
                 TableNames = AppSettings.PeopleTableNames_New;
                TableConditions = AppSettings.PeopleTableConditions_New;
                TableCondition = AppSettings.PeopleTableCondition_New;
#endif
            }
            var query = from q in doc.Descendants(Constants.ParentTagForDownloadedXml) select q;
            string[] coloumnNames = new string[4];
            string[] Tablenames = new string[1];

            foreach (KeyValuePair<string, string> pair in TableCondition)
            {
                if (pair.Value.Equals("PrimaryCondition"))
                    Tablenames[0] = pair.Key;
            }
            foreach (KeyValuePair<string, string> pair in TableConditions)
            {
                if (pair.Key.Contains(Tablenames[0].ToString()))
                {
                    if (pair.Value.Equals("PrimaryCondition"))
                        coloumnNames[0] = pair.Key.Substring(0, pair.Key.IndexOf('-'));
                    if (pair.Value.Equals("SecondaryCondition"))
                        coloumnNames[1] = pair.Key.Substring(0, pair.Key.IndexOf('-'));
                }
            }

            LastShowId = Convert.ToInt32((doc.Descendants(Constants.ParentTagForDownloadedXml).Elements().Where(i => i.Name == Tablenames[0].ToString()).Elements().Where(i => i.Name == coloumnNames[0].ToString()).OrderByDescending(i => i.Value)).First().Value.ToString());
            FirstShowId = Convert.ToInt32((doc.Descendants(Constants.ParentTagForDownloadedXml).Elements().Where(i => i.Name == Tablenames[0].ToString()).Elements().Where(i => i.Name == coloumnNames[0].ToString()).OrderBy(i => i.Value)).First().Value.ToString());
            int FirstCount = (doc.Descendants(Constants.ParentTagForDownloadedXml).Elements().Where(i => Convert.ToInt32(i.Element(coloumnNames[0].ToString()).Value.ToString()) == FirstShowId)).Count();
            int NextId = Convert.ToInt32((doc.Descendants(Constants.ParentTagForDownloadedXml).Elements().Where(i => i.Name == Tablenames[0].ToString()).Elements().Where(i => i.Name == coloumnNames[0].ToString() && Convert.ToInt32(i.Value.ToString()) > AppSettings.LastShowInsertId).OrderBy(i => i.Value)).First().Value.ToString());
            int Count = (doc.Descendants(Constants.ParentTagForDownloadedXml).Elements().Where(i => Convert.ToInt32(i.Element(coloumnNames[0].ToString()).Value.ToString()) == NextId)).Count();
            if (AppSettings.LastShowInsertId == 0)
                if (XmlName.Contains("Movies"))
                    ListtoInsert = (doc.Descendants(Constants.ParentTagForDownloadedXml).Elements().Where(i => Convert.ToInt32(i.Element(coloumnNames[0].ToString()).Value.ToString()) > (FirstShowId - 1))).OrderBy(i => i.Element(coloumnNames[0].ToString()).Value).Take(FirstCount).ToList();
                else
                    ListtoInsert = (doc.Descendants(Constants.ParentTagForDownloadedXml).Elements().Where(i => Convert.ToInt32(i.Element(coloumnNames[0].ToString()).Value.ToString()) > (FirstShowId - 1))).OrderBy(i => i.Element(coloumnNames[0].ToString()).Value).Take(FirstCount).ToList();
            else
                if (XmlName.Contains("Movies"))
                    ListtoInsert = (doc.Descendants(Constants.ParentTagForDownloadedXml).Elements().Where(i => Convert.ToInt32(i.Element(coloumnNames[0].ToString()).Value.ToString()) > (AppSettings.LastShowInsertId))).OrderBy(i => i.Element(coloumnNames[0].ToString()).Value).Take(Count).ToList();
                else
                    ListtoInsert = (doc.Descendants(Constants.ParentTagForDownloadedXml).Elements().Where(i => Convert.ToInt32(i.Element(coloumnNames[0].ToString()).Value.ToString()) > (AppSettings.LastShowInsertId))).OrderBy(i => i.Element(coloumnNames[0].ToString()).Value).Take(Count).ToList();
#if WINDOWS_PHONE_APP
            foreach (XElement val in ListtoInsert.OfType<XElement>().Where(x => (XmlName.Contains("Movies") ? AppSettings.ShowTableNames_New.Contains(x.Name.ToString()) : AppSettings.PeopleTableNames_New.Contains(x.Name.ToString()))))
#endif
#if WINDOWS_APP
            foreach (XElement val in ListtoInsert.OfType<XElement>().Where(x => (XmlName.Contains("Movies") ? AppSettings.ShowTableNames.Contains(x.Name.ToString()) : AppSettings.PeopleTableNames.Contains(x.Name.ToString()))))
#endif
            {
				//val.ToString().Replace("IsFavourite_x0020_","IsFavourite");
				if (val.Name.ToString() == "ShowLinks")
                {
                    string ss = "";
                }

                coloumnNames = new string[5];
                foreach (KeyValuePair<string, string> pair in TableConditions)
                {

                    if (pair.Key.Contains(val.Name.ToString()))
                    {
                        if (pair.Value.Equals("PrimaryCondition"))
                            coloumnNames[0] = pair.Key.Substring(0, pair.Key.IndexOf('-'));
                        if (pair.Value.Equals("SecondaryCondition"))
                            coloumnNames[1] = pair.Key.Substring(0, pair.Key.IndexOf('-'));
                        if (pair.Value.Equals("DoNotInsert"))
                            coloumnNames[2] = pair.Key.Substring(0, pair.Key.IndexOf('-'));
                        if (pair.Value.Equals("DownloadCondition"))
                            coloumnNames[3] = pair.Key.Substring(0, pair.Key.IndexOf('-'));
                        if (pair.Value.Equals("ThirdCondition"))
                            coloumnNames[4] = pair.Key.Substring(0, pair.Key.IndexOf('-'));
                    }
                }
                foreach (KeyValuePair<string, string> pair in TableCondition)
                {
                    if (pair.Key.Contains(val.Name.ToString()))
                    {
                        ConditionBeforeInsert = pair.Value;
                    }
                }
                 # if (WINDOWS_APP && NOTANDROID) || (WINDOWS_PHONE_APP && NOTANDROID)
                if (ConditionBeforeInsert == "DownloadCondition")
                {
                    int LastInsertID = 0;

                    foreach (var X in val.Descendants())
                    {                      
                        bool CreateFolder = false;
                        StorageFolder store1 = ApplicationData.Current.LocalFolder;
                        lib.ShowImages s = default(lib.ShowImages);
                        string ImageName = string.Empty;
                        foreach (lib.ShowImages x in Enum.GetValues(typeof(lib.ShowImages)))
                        {
                            foreach (CreateFolderUsingShowId y in Enum.GetValues(typeof(CreateFolderUsingShowId)))
                            {
                                if (y.ToString() == x.ToString())
                                {
                                    CreateFolder = true;
                                }
                            }
                            if (x.ToString() == X.Name.ToString())
                            {
                                s = x;
                                break;
                            }
                            LastInsertID = Convert.ToInt32(val.Descendants().OfType<XElement>().Where(y => y.Name.ToString() == coloumnNames[0].ToString()).Select(i => i.Value).FirstOrDefault().ToString());
                        }
                        if (s != default(lib.ShowImages))
                        {
                            StorageFile file1 = default(StorageFile);
                            string[] path = GetPath(s);
                            if (CreateFolder == true)
                            {
                                if (!Task.Run(async () => await Storage.FolderExists(path[1].ToString() + val.Descendants().OfType<XElement>().Where(x => x.Name.ToString() == coloumnNames[0].ToString()).Select(i => i.Value).FirstOrDefault().ToString())).Result)
                                {
                                    StorageFolder store2 = ApplicationData.Current.LocalFolder;
                                    StorageFolder Folder = Task.Run(async () => await store2.CreateFolderAsync(path[1].ToString(), CreationCollisionOption.OpenIfExists)).Result;
                                    Task.Run(async () => await Folder.CreateFolderAsync(val.Descendants().OfType<XElement>().Where(x => x.Name.ToString() == coloumnNames[0].ToString()).Select(i => i.Value).FirstOrDefault().ToString(), CreationCollisionOption.OpenIfExists));
                                }
                                path[1] = path[1].ToString() + val.Descendants().OfType<XElement>().Where(x => x.Name.ToString() == coloumnNames[0].ToString()).Select(i => i.Value).FirstOrDefault().ToString() + "\\";
                            }
#if WINDOWS_APP
                            System.Net.Http.HttpClient http = new System.Net.Http.HttpClient();
                            HttpResponseMessage response = Task.Run(async () => await http.GetAsync(X.Value.ToString())).Result;
#endif
#if WINDOWS_PHONE_APP
                            HttpWebRequest request = (HttpWebRequest)
                            WebRequest.Create(X.Value.ToString());
                            request.Method = HttpMethod.Get;
                            HttpWebResponse response = (HttpWebResponse)Task.Run(async () => await request.GetResponseAsync()).Result;
#endif
                            if (response.StatusCode == HttpStatusCode.OK)
                            {
                                if (coloumnNames[1] != null)
                                {
                                    ImageName = val.Descendants().OfType<XElement>().Where(x => x.Name.ToString() == coloumnNames[1].ToString()).Select(i => i.Value).FirstOrDefault().ToString();
                                }
                                else
                                    ImageName = val.Descendants().OfType<XElement>().Where(x => x.Name.ToString() == coloumnNames[0].ToString()).Select(i => i.Value).FirstOrDefault().ToString();
#if WINDOWS_PHONE_APP
                                //System.IO.Stream responseStream = response.GetResponseStream();

                                byte[] Downloadstream = ReadToEnd(response.GetResponseStream());
#endif
#if WINDOWS_APP
                                    byte[] Downloadstream = Task.Run(async () => await response.Content.ReadAsByteArrayAsync()).Result;
#endif
                                if (ImageName.Contains("."))
                                {
                                    try
                                    {
#if WINDOWS_APP
                                        file1 = Task.Run(async () => await store1.CreateFileAsync(path[1].ToString() + ImageName.Substring(0, ImageName.LastIndexOf('.')) + path[0].ToString(), Windows.Storage.CreationCollisionOption.ReplaceExisting)).Result;
#endif
#if WINDOWS_PHONE_APP
                                        file1 = Task.Run(async () => await store1.CreateFileAsync(path[1].ToString() + ImageName.Substring(0, ImageName.LastIndexOf('.')) + path[0].ToString(), Windows.Storage.CreationCollisionOption.ReplaceExisting)).Result;                                        
#endif
                                    }
                                    catch (Exception ex)
                                    {
                                    }
                                }
                                else
                                {
                                    try
                                    {
#if WINDOWS_APP
                                        file1 = Task.Run(async () => await store1.CreateFileAsync(path[1].ToString() + ImageName + path[0].ToString(), Windows.Storage.CreationCollisionOption.ReplaceExisting)).Result;
#endif
#if WINDOWS_PHONE_APP
                                        file1 = Task.Run(async () => await store1.CreateFileAsync(path[1].ToString() + ImageName + path[0].ToString(), Windows.Storage.CreationCollisionOption.ReplaceExisting)).Result;
#endif
                                    }
                                    catch (Exception ex)
                                    {
                                        Exceptions.SaveOrSendExceptions("Exception in InsertorUpdateTable() method at condition near line 774 in MovieUpdates.cs file", ex);
#if WINDOWS_PHONE_APP
#endif
                                    }
                                }
                                if (file1 != default(StorageFile))
                                {
                                    try
                                    {
                                        var fquery1 = Task.Run(async () => await file1.OpenAsync(Windows.Storage.FileAccessMode.ReadWrite)).Result;
                                        var outputStream = fquery1.GetOutputStreamAt(0);
                                        var writer = new Windows.Storage.Streams.DataWriter(outputStream);
                                        writer.WriteBytes(Downloadstream);
                                        var fi = Task.Run(async () => await writer.StoreAsync()).Result;
                                        writer.DetachStream();
                                        var oi = Task.Run(async () => await outputStream.FlushAsync()).Result;
                                        outputStream.Dispose();
                                        Task.Run(async () => fquery1.FlushAsync());
                                        fquery1.Dispose();
                                    }
                                    catch (Exception ex)
                                    {
                                        Exceptions.SaveOrSendExceptions("Exception in InsertorUpdateTable() method at condition near line 796 in MovieUpdates.cs file", ex);
                                    }
                                }
                                else
                                {
#if WINDOWS_PHONE_APP
                                    if (ImageName.Contains("."))
                                    {
                                        StorageFolder isoStore6 = ApplicationData.Current.LocalFolder;
                                        var file11 = await ApplicationData.Current.LocalFolder.GetFileAsync(path[1].ToString() + ImageName.Substring(0, ImageName.LastIndexOf('.')) + path[0].ToString());
                                        var stream = await file11.OpenAsync(FileAccessMode.ReadWrite);
                                        using (var writer = new DataWriter(stream.GetOutputStreamAt(0)))
                                        {
                                            writer.WriteBytes(Downloadstream);
                                            await writer.StoreAsync();
                                            await writer.FlushAsync();
                                        }                                        
                                    }
                                    else
                                    {                                        
                                        StorageFolder isoStore7 = ApplicationData.Current.LocalFolder;
                                        var file11 = await ApplicationData.Current.LocalFolder.GetFileAsync(path[1].ToString() + ImageName + path[0].ToString());
                                        var stream1 = await file11.OpenAsync(FileAccessMode.ReadWrite);
                                        using (var writer = new DataWriter(stream1.GetOutputStreamAt(0)))
                                        {
                                            writer.WriteBytes(Downloadstream);
                                            await writer.StoreAsync();
                                            await writer.FlushAsync();
                                        }
                                    }
#endif
                                }
                            }
                            response.Dispose();
                        }
                        if (coloumnNames[1] != null && val.Name == "ShowImages")
                        {
                            if (!Downloadedmovieid.Contains(LastInsertID.ToString()))
                            {
                                DownloadHistory d = new DownloadHistory();
                                Downloadedmovieid.Add(LastInsertID.ToString());
                                d.SaveHistory(LastInsertID.ToString());
                            }
                        }
                    }
                    if (ListtoInsert.Last() == val)
                        AppSettings.LastShowInsertId = LastInsertID;
                }
#endif
#if ANDROID
                if (ConditionBeforeInsert == "DownloadCondition")
                {
                    string ImageName = string.Empty;
                    int LastInsertID = 0;

                    foreach (var X in val.Descendants())
                    {
                        string Showid = string.Empty;
                        string[] ImageDetails = new string[3];
                        bool check = Enum.IsDefined(typeof(lib.ShowImages), X.Name.ToString());
                        LastInsertID = Convert.ToInt32(val.Descendants().OfType<XElement>().Where(x => x.Name.ToString() == coloumnNames[0].ToString()).Select(i => i.Value).FirstOrDefault().ToString());

                        if (check == true)
                        {
                            string[] path = GetPath((lib.ShowImages)Enum.Parse(typeof(lib.ShowImages), X.Name.ToString(), true));

                            if (coloumnNames[1] != null)
                            {
                                ImageName = val.Descendants().OfType<XElement>().Where(x => x.Name.ToString() == coloumnNames[1].ToString()).Select(i => i.Value).FirstOrDefault().ToString();
                            }
                            else
                                ImageName = val.Descendants().OfType<XElement>().Where(x => x.Name.ToString() == coloumnNames[0].ToString()).Select(i => i.Value).FirstOrDefault().ToString();
                            if (coloumnNames[1] != null)
                            {
                                Showid = val.Descendants().OfType<XElement>().Where(x => x.Name.ToString() == coloumnNames[0].ToString()).Select(i => i.Value).FirstOrDefault().ToString();
                            }

                            if (!string.IsNullOrEmpty(Showid) && string.IsNullOrEmpty(path[0].ToString()))
                            {
                                ImageDetails[0] = ImageName;
                                ImageDetails[1] = path[1].ToString();
                                ImageDetails[2] = Showid;
                            }
                            else
                            {
                                ImageDetails[0] = ImageName;
                                ImageDetails[1] = path[1].ToString();
                            }

                            if (!string.IsNullOrEmpty(X.Value.ToString()))
                            {
                                Assembly DataAssembly = Assembly.Load(new AssemblyName("OnlineVideos.UI"));
                                Type ClassType = DataAssembly.GetType("OnlineVideos.UI.SaveDownLoadImages");
                                HttpWebRequest request = (HttpWebRequest)
                                WebRequest.Create(X.Value.ToString());
                                request.Method = HttpMethod.Get;
                                try {
                                HttpWebResponse response = (HttpWebResponse)Task.Run(async () =>  await request.GetResponseAsync()).Result;
                                if (ImageDetails[2] != null)
                                {
                                   ClassType.GetMethod(ImageDetails[1].ToString(), BindingFlags.FlattenHierarchy | BindingFlags.Static | BindingFlags.Public).Invoke(null, new object[] { ImageDetails[0].ToString(), ReadToEnd(response.GetResponseStream()), ImageDetails[2].ToString() });
                                }
                                else
                                {
                                 ClassType.GetMethod(ImageDetails[1].ToString(), BindingFlags.FlattenHierarchy | BindingFlags.Static | BindingFlags.Public).Invoke(null, new object[] { ImageDetails[0].ToString(), ReadToEnd(response.GetResponseStream()) });
                                }
                                }
								 catch (Exception ex) {
                                }
                            }
                        }
                    }
		      if (ListtoInsert.Last() == val ||ListtoInsert.FirstOrDefault()==val)
                        AppSettings.LastShowInsertId = LastInsertID;
                }
#endif
                else
                {
#if WINDOWS_PHONE_APP
                    Assembly DataAssembly = Assembly.Load(new AssemblyName(Constants.NamespaceForData));
#endif
#if WINDOWS_APP
                     Assembly DataAssembly = Assembly.Load(new AssemblyName("OnlineVideos.Entities"));
#endif
                    Type TableType = DataAssembly.GetType("OnlineVideos.Entities." + val.Name.ToString());
                    object TableInstance = Activator.CreateInstance(TableType);                    

                    if (TableConditions.Count > 0)
                    {
                        //long x = DeviceStatus.ApplicationMemoryUsageLimit - DeviceStatus.ApplicationCurrentMemoryUsage;
                        if (coloumnNames[1] != null && coloumnNames[4] == null)
                        {
                            rowExists = CheckShow(TableType, coloumnNames[0].ToString(), val.Element(coloumnNames[0].ToString()).Value.ToString(), coloumnNames[1].ToString(), val.Element(coloumnNames[1].ToString()).Value.ToString(), TableInstance);
                        }
                        else if (coloumnNames[4] != null)
                        {
                            rowExists = CheckShow(TableType, coloumnNames[0].ToString(), val.Element(coloumnNames[0].ToString()).Value.ToString(), coloumnNames[1].ToString(), val.Element(coloumnNames[1].ToString()).Value.ToString(), coloumnNames[4].ToString(), val.Element(coloumnNames[4].ToString()).Value.ToString(), TableInstance);
                        }
                        else
                        {
                            rowExists = CheckShow(TableType, coloumnNames[0].ToString(), val.Element(coloumnNames[0].ToString()).Value.ToString(), TableInstance);
                        }
                    }
                    //}
                    if (rowExists == null)
                    {
                        if (ConditionBeforeInsert == "DeleteCondition")
                        {
#if WINDOWS_PHONE_APP
                            AppSettings.DeletedTableNames_New = new List<string> { TableType.Name.ToString() };
#endif
                            AppSettings.DeletedTableNames = new List<string> { TableType.Name.ToString() };
                        }
                        InsertIntoTable(coloumnNames, val, TableType, LastShowId, XmlName, ListtoInsert);
                    }
                    else
                    {
                        if (ConditionBeforeInsert == "DeleteCondition")
                        {
#if WINDOWS_PHONE_APP
                            if (AppSettings.DeletedTableNames_New.Count > 0 ? !AppSettings.DeletedTableNames_New.Contains(TableType.Name.ToString()) : true)
#endif
#if WINDOWS_APP
                            if (AppSettings.DeletedTableNames.Count > 0 ? !AppSettings.DeletedTableNames.Contains(TableType.Name.ToString()) : true)
#endif
                            {
                                DeleteFromTable(TableType, coloumnNames[0].ToString(), val.Element(coloumnNames[0].ToString()).Value.ToString(),XmlName,val);
                            }
                            InsertIntoTable(coloumnNames, val, TableType, LastShowId, XmlName, ListtoInsert);
                        }
                        else
                        {
                            UpdateTable(coloumnNames, val, TableType, LastShowId, XmlName);
                        }
                    }
                }
				//Exceptions.UpdateAgentLog(val.Element(coloumnNames[0]).Value+ " " +" is Completed In "+" "+val.Name + "Table In InsertorUpdateTable Method In MovieUpdates.cs");
            }
            if (XmlName.Contains("Peoples.xml"))
            {
                Exceptions.UpdateAgentLog(AppSettings.DownLoadPersonName + " " + "CastProfile and PersonGallery  Completed");
            }
            else if (XmlName.Contains("Movies.xml"))
            {
                Exceptions.UpdateAgentLog(AppSettings.DownLoadPersonName + " " + "Audios,Songs,Chapters And Cast Completed");
            }
#if WINDOWS_PHONE_APP
            AppSettings.DeletedTableNames_New=new List<string>();
#endif
            AppSettings.DeletedTableNames = new List<string>();
            if (AppSettings.LastShowInsertId!=0)
            {
                AppSettings.LastShowInsertId = 0;
                if (XmlName.Contains("Movies"))
                    AppSettings.LastUpdatedDate = AppSettings.LastPublishedDate;
                else
                    AppSettings.PeopleLastUpdatedDate = AppSettings.LastPeoplePublishedDate;
            }
#if ANDROID
Constants.Timer.Interval=1000;
Constants.Timer.Start();
#endif
        }
        catch (Exception ex)
        {
	#if ANDROID
	Constants.Timer.Interval=1000;
	Constants.Timer.Start();
	#endif
            Exceptions.SaveOrSendExceptions("Exception in InsertorUpdateTable Method In MovieUpdates.cs file", ex);
        }
    }

    private static void DeleteFromTable(Type TableType, string ColoumnName, string ColoumnValue, string ColoumnName1, string ColoumnValue1, string ColoumnName2, string ColoumnValue2)
    {
        try
        {
            ParameterExpression pe = System.Linq.Expressions.Expression.Parameter(TableType, "i");
# if (WINDOWS_APP && NOTANDROID) || (WINDOWS_PHONE_APP && NOTANDROID)
            Expression left = System.Linq.Expressions.Expression.Property(pe, TableType.GetRuntimeProperty(ColoumnName));
            Expression right = System.Linq.Expressions.Expression.Constant(Convert.ChangeType(ColoumnValue, TableType.GetRuntimeProperty(ColoumnName).PropertyType, null));
            Expression left1 = System.Linq.Expressions.Expression.Property(pe, TableType.GetRuntimeProperty(ColoumnName1));
            Expression right1 = System.Linq.Expressions.Expression.Constant(Convert.ChangeType(ColoumnValue1, TableType.GetRuntimeProperty(ColoumnName1).PropertyType, null));
            Expression left2 = System.Linq.Expressions.Expression.Property(pe, TableType.GetRuntimeProperty(ColoumnName2));
            Expression right2 = System.Linq.Expressions.Expression.Constant(Convert.ChangeType(ColoumnValue2, TableType.GetRuntimeProperty(ColoumnName2).PropertyType, null));
#endif
#if ANDROID
			System.Linq.Expressions.Expression left = System.Linq.Expressions.Expression.Property(pe, TableType.GetProperty(ColoumnName));
			System.Linq.Expressions.Expression right = System.Linq.Expressions.Expression.Constant(Convert.ChangeType(ColoumnValue, TableType.GetProperty(ColoumnName).PropertyType, null));
			System.Linq.Expressions.Expression left1 = System.Linq.Expressions.Expression.Property(pe, TableType.GetProperty(ColoumnName1));
			System.Linq.Expressions.Expression right1 = System.Linq.Expressions.Expression.Constant(Convert.ChangeType(ColoumnValue1, TableType.GetProperty(ColoumnName1).PropertyType, null));
			System.Linq.Expressions.Expression left2 = System.Linq.Expressions.Expression.Property(pe, TableType.GetProperty(ColoumnName2));
			System.Linq.Expressions.Expression right2 = System.Linq.Expressions.Expression.Constant(Convert.ChangeType(ColoumnValue2, TableType.GetProperty(ColoumnName2).PropertyType, null));
#endif
            var comparison = System.Linq.Expressions.Expression.Equal(left, right);
            var comparison1 = System.Linq.Expressions.Expression.Equal(left1, right1);
            var comparison2 = System.Linq.Expressions.Expression.Equal(left2, right2);
            System.Linq.Expressions.Expression Combine = System.Linq.Expressions.Expression.And(comparison, comparison1);
            System.Linq.Expressions.Expression Combine1 = System.Linq.Expressions.Expression.And(Combine, comparison2);
            var delegateType = typeof(Func<,>).MakeGenericType(TableType, typeof(bool));
            var yourExpression = System.Linq.Expressions.Expression.Lambda(delegateType, Combine1, pe);
            var d1 = typeof(DataManager<>);
            Type[] typeArgs = { TableType };
            var makeme = d1.MakeGenericType(typeArgs);
            object o = Activator.CreateInstance(makeme);
            o.GetType().GetTypeInfo().GetDeclaredMethod("DeleteList").Invoke(o, new object[] { yourExpression });
            AppSettings.DeletedTableNames = new List<string> { TableType.Name.ToString() };
			Exceptions.UpdateAgentLog(ColoumnValue+ " " + "is Deleted In "+ " " +TableType.Name + " " +"Table In DeleteFromTable Method In MovieUpdates.cs");
        }
        catch (Exception ex)
        {
            Exceptions.SaveOrSendExceptions("Exception in DeleteFromTable Method In MovieUpdates.cs file", ex);
        }
    }
    private static void DeleteFromTable(Type TableType, string ColoumnName, string ColoumnValue,string XmlName,XElement val)
    {
        try
        {
            ParameterExpression pe = System.Linq.Expressions.Expression.Parameter(TableType, "i");
# if (WINDOWS_APP && NOTANDROID) || (WINDOWS_PHONE_APP && NOTANDROID)
            Expression left = System.Linq.Expressions.Expression.Property(pe, TableType.GetRuntimeProperty(ColoumnName));
            Expression right = System.Linq.Expressions.Expression.Constant(Convert.ChangeType(ColoumnValue, TableType.GetRuntimeProperty(ColoumnName).PropertyType, null));
#endif
#if ANDROID
			System.Linq.Expressions.Expression left = System.Linq.Expressions.Expression.Property(pe, TableType.GetProperty(ColoumnName));
			System.Linq.Expressions.Expression right = System.Linq.Expressions.Expression.Constant(Convert.ChangeType(ColoumnValue, TableType.GetProperty(ColoumnName).PropertyType, null));
#endif
            var comparison = System.Linq.Expressions.Expression.Equal(left, right);
            var delegateType = typeof(Func<,>).MakeGenericType(TableType, typeof(bool));
            var yourExpression = System.Linq.Expressions.Expression.Lambda(delegateType, comparison, pe);
            var d1 = typeof(DataManager<>);
            Type[] typeArgs = { TableType };
            var makeme = d1.MakeGenericType(typeArgs);
            object o = Activator.CreateInstance(makeme);
                       
            o.GetType().GetTypeInfo().GetDeclaredMethod("DeleteList").Invoke(o, new object[] { yourExpression });
            AppSettings.DeletedTableNames = new List<string> { TableType.Name.ToString() };
            UpDateUserLog(XmlName, "Deleted", val, TableType);
        }
        catch (Exception ex)
        {
            Exceptions.SaveOrSendExceptions("Exception in DeleteFromTable Method In MovieUpdates.cs file", ex);
        }
    }
    private static void UpdateTable(string[] coloumnNames, XElement val, Type TableType, int LastShowId, string XmlName)
    {
        try
        {
            var d1 = typeof(DataManager<>);
            Type[] typeArgs = { TableType };
            var makeme = d1.MakeGenericType(typeArgs);
            object o = Activator.CreateInstance(makeme);
            var d2 = typeof(StorageHelper<>);
            Type[] typeArgs2 = { TableType };
            var makeme2 = d2.MakeGenericType(typeArgs2);
#if (WINDOWS_APP && NOTANDROID) || (WINDOWS_PHONE_APP && NOTANDROID)
            object o2 = Activator.CreateInstance(makeme2, new object[] { StorageType.Local });
#endif
#if ANDROID
			object o2 = Activator.CreateInstance(makeme2);
#endif
            if (coloumnNames[1] == null)
               o.GetType().GetTypeInfo().GetDeclaredMethod("SaveToList").Invoke(o, new object[] { o2.GetType().GetTypeInfo().GetDeclaredMethod("ConvertXelementToEntity").Invoke(o2, new object[] { val }), coloumnNames[0].ToString(), "" });
		
            else
               o.GetType().GetTypeInfo().GetDeclaredMethod("SaveToList").Invoke(o, new object[] { o2.GetType().GetTypeInfo().GetDeclaredMethod("ConvertXelementToEntity").Invoke(o2, new object[] { val }), coloumnNames[0].ToString(), coloumnNames[1].ToString() });

              UpDateUserLog(XmlName, "Updated", val, TableType);
        }
        catch (Exception ex)
        {
            Exceptions.SaveOrSendExceptions("Exception in UpdateTable Method In MovieUpdates.cs file", ex);
        }
    }
    private static void InsertIntoTable(string[] coloumnNames, XElement val, Type TableType, int LastShowId, string XmlName, List<XElement> ListToinsert)
    {
        try
        {
            var d1 = typeof(DataManager<>);
            Type[] typeArgs = { TableType };
            var makeme = d1.MakeGenericType(typeArgs);
            object o = Activator.CreateInstance(makeme);
            var d2 = typeof(StorageHelper<>);
            Type[] typeArgs2 = { TableType };
            var makeme2 = d2.MakeGenericType(typeArgs2);
# if (WINDOWS_APP && NOTANDROID) || (WINDOWS_PHONE_APP && NOTANDROID)
            object o2 = Activator.CreateInstance(makeme2, new object[] { StorageType.Local });
#endif
#if ANDROID
			object o2 = Activator.CreateInstance(makeme2);
#endif
            o.GetType().GetTypeInfo().GetDeclaredMethod("InsertIntoXml").Invoke(o, new object[] { o2.GetType().GetTypeInfo().GetDeclaredMethod("ConvertXelementToEntity").Invoke(o2, new object[] { val }) });
            UpDateUserLog(XmlName, "Inserted", val, TableType);
        }
        catch (Exception ex)
        {
            Exceptions.SaveOrSendExceptions("Exception in InsertIntoTable Method In MovieUpdates.cs file", ex);
        }
    }

    public static void checkForPeopleUpdates()
    {
        try
        {
#if WINDOWS_APP
            if (SettingsHelper.getStringValue("People") != "False" && SettingsHelper.getStringValue("PeopleDownloadCompleted") != "0")
#endif
#if ANDROID || WINDOWS_PHONE_APP
            if (NetworkHelper.IsNetworkAvailableForDownloads() && SettingsHelper.getStringValue("People") != "False" && SettingsHelper.getStringValue("PeopleDownloadCompleted") != "0")
#endif
            {
                PeopleDownloadFromBlog people = new PeopleDownloadFromBlog();
                people.StartDownload();
            }
            else
            {
                AppSettings.BackgroundAgentStatus = SyncAgentStatus.UpdatePeople;
            }
            Exceptions.UpdateAgentLog(" checkForPeopleUpdates Method Completed In MovieUpdates ");
		#if ANDROID 
		Constants.Timer.Interval=10000;
		Constants.Timer.Start();
		#endif
        }
        catch (Exception ex)
        {
		#if ANDROID
		Constants.Timer.Interval=10000;
		Constants.Timer.Start();
		#endif
            Exceptions.SaveOrSendExceptions("Exception in checkForPeopleUpdates Method In MovieUpdates.cs file", ex);
        }
    }
    public static void GetPeopleCompleted(string xmlname)
    {
        try
        {
            if (AppSettings.LastPeoplePublishedDate > AppSettings.PeopleLastUpdatedDate)
            {
                InsertorUpdateTable(xmlname);
            }
            else
            {
                AppSettings.PeopleLastUpdatedDate = AppSettings.LastPeoplePublishedDate;
                AppSettings.BackgroundAgentStatus = SyncAgentStatus.DownloadShows;
                Storage.DeleteFile("peoples.xml");
			#if ANDROID
			Constants.Timer.Interval=10000;
			Constants.Timer.Start();
			#endif
				Exceptions.UpdateAgentLog(" GetPeopleCompleted Method Completed In MovieUpdates ");
            }
        }
        catch (Exception ex)
        {
            Exceptions.SaveOrSendExceptions("Exception in GetPeopleCompleted Method In MovieUpdates.cs file", ex);
        }
    }
   
    public static byte[] ReadToEnd(System.IO.Stream stream)
    {
        long originalPosition = 0;

        if (stream.CanSeek)
        {
            originalPosition = stream.Position;
            stream.Position = 0;
        }
        byte[] buffer = default(byte[]);
        try
        {
            byte[] readBuffer = new byte[4096];
            int totalBytesRead = 0;
            int bytesRead;
            while ((bytesRead = stream.Read(readBuffer, totalBytesRead, readBuffer.Length - totalBytesRead)) > 0)
            {
                totalBytesRead += bytesRead;
                if (totalBytesRead == readBuffer.Length)
                {
                    int nextByte = stream.ReadByte();
                    if (nextByte != -1)
                    {
                        byte[] temp = new byte[readBuffer.Length * 2];
                        System.Buffer.BlockCopy(readBuffer, 0, temp, 0, readBuffer.Length);
                        System.Buffer.SetByte(temp, totalBytesRead, (byte)nextByte);
                        readBuffer = temp;
                        totalBytesRead++;
                    }
                }
            }
            buffer = readBuffer;
            if (readBuffer.Length != totalBytesRead)
            {
                buffer = new byte[totalBytesRead];
                System.Buffer.BlockCopy(readBuffer, 0, buffer, 0, totalBytesRead);
            }
           // Exceptions.UpdateAgentLog("ReadToEnd completed In MovieUpdates");
			//Exceptions.UpdateAgentLog(" ReadToEnd Method Completed In MovieUpdates ");
            return buffer;
          
        }
        catch (Exception ex)
        {
            return buffer;
        }
        finally
        {
            if (stream.CanSeek)
            {
                stream.Position = originalPosition;
            }
        }
    }

    private static void UpDateUserLog(string XmlName, string MethodName, XElement val,Type TableType)
    {
        string Name = string.Empty;
        if (XmlName.Contains("Peoples.xml"))
        {
            if (val.ToString().Contains("CastProfile"))
            {
                Name = val.Element("Name").Value;
                AppSettings.DownLoadPersonName = Name;
                Exceptions.UpdateAgentLog("Person Name Is" + " " + Name + " " + MethodName +" " + "In" + " " + "CastProfile");
            }
            if (val.ToString().Contains("PersonGallery"))
            {
                Exceptions.UpdateAgentLog("Person Name ,Gallery Image NO Are" + " " + AppSettings.DownLoadPersonName + " " + ", " + val.Element("ImageNo").Value + " " + MethodName + " " + "In" + " " + "PersonGallery");
            }
        }
        else if (XmlName.Contains("Movies.xml"))
        {
            if (val.ToString().Contains("ShowList"))
            {
                Name = val.Element("Title").Value;
                AppSettings.DownLoadPersonName = Name;
                Exceptions.UpdateAgentLog("Movie Name Is" + " " + Name + " " + MethodName);
            }
            if (val.ToString().Contains("ShowLinks"))
            {
                Exceptions.UpdateAgentLog(val.Element("Title").Value + " " + val.Element("LinkType").Value.Replace("Movies",string.Empty) + " " + MethodName +"  "+"In"+" "+AppSettings.DownLoadPersonName+" "+"Movie");
            }
        }
    }    
}