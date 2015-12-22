using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using OnlineVideos.Entities;
#if WINDOWS_APP
using Windows.Data.Xml.Dom;
#endif
#if NOTANDROID
using Windows.Storage;
using Windows.Storage.Streams;
#endif
using System.Reflection;
using Common.Library;
using SQLite;

namespace OnlineVideos.Data
{
    public class DataManager<T>
    {
        public void InsertIntoXml(T listtoAdd)
        {
            try
            {
                var ClassInstance = (T)Activator.CreateInstance(typeof(T));
                var d1 = typeof(SQLiteConnection);
                object o = Activator.CreateInstance(typeof(SQLiteConnection), new object[] { Constants.DataBaseConnectionstringForSqlite, false });
#if (WINDOWS_APP && NOTANDROID) || (WINDOWS_PHONE_APP && NOTANDROID)
                var method = o.GetType().GetRuntimeMethod("Insert", new Type[] { typeof(object) }).Invoke(o, new object[] { listtoAdd });
                o.GetType().GetTypeInfo().GetDeclaredMethod("Close").Invoke(o, null);
#endif
#if ANDROID
                var method = o.GetType().GetMethod("Insert", new Type[] { typeof(object) }).Invoke(o, new object[] { listtoAdd });
#endif
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in InsertIntoXml Method In DataManager.cs file", ex);
            }
        }
        public List<int> GetDistinctFromList(Expression<Func<T, bool>> predicate, Func<T, int> predicate1)
        {
            Type[] typ = new Type[2];
            int parametercount = 0;
            var ClassInstance = (T)Activator.CreateInstance(typeof(T));
            var d1 = typeof(SQLiteConnection);
            object o = Activator.CreateInstance(typeof(SQLiteConnection), new object[] { Constants.DataBaseConnectionstringForSqlite, false });
            List<MethodInfo> MethodList = o.GetType().GetRuntimeMethods().Where(m => m.Name == "GetDistinctFromList").ToList();
            foreach (var mn in MethodList)
            {
                List<ParameterInfo> pi = mn.GetParameters().ToList();
                foreach (ParameterInfo p in pi)
                {
                    if (p.Name.Contains("predicate") || p.Name.Contains("predicate1"))
                    {
                        typ[parametercount] = p.ParameterType;
                        parametercount++;
                    }
                }
            }


            var method = (List<int>)o.GetType().GetRuntimeMethod("GetDistinctFromList", typ).MakeGenericMethod(new Type[] { ClassInstance.GetType() }).Invoke(o, new object[] { predicate, predicate1 });
            o.GetType().GetTypeInfo().GetDeclaredMethod("Close").Invoke(o, null);
            return method;
        }
        public void Savemovies(T listtosave, string condition1)
        {
            try
            {
                var ClassInstance = (T)Activator.CreateInstance(typeof(T));
                var d1 = typeof(SQLiteConnection);
                object o = Activator.CreateInstance(typeof(SQLiteConnection), new object[] { Constants.DataBaseConnectionstringForSqlite, false });
                #if (WINDOWS_APP && NOTANDROID) || (WINDOWS_PHONE_APP && NOTANDROID)
                var method = o.GetType().GetRuntimeMethod("Update", new Type[] { typeof(object) }).Invoke(o, new object[] { listtosave });
                o.GetType().GetTypeInfo().GetDeclaredMethod("Close").Invoke(o, null);
#endif
#if ANDROID
                var method = o.GetType().GetMethod("Update", new Type[] { typeof(object) }).Invoke(o, new object[] { listtosave });
#endif
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in Savemovies Method In DataManager.cs file", ex);
            }
        }
        
        public void DeleteList(Expression<Func<T, bool>> predicate)
        {
            try
            {
                #if WINDOWS_PHONE_APP
                
                List<T> myList = new List<T>();
                Type typ = default(Type);
                var ClassInstance = (T)Activator.CreateInstance(typeof(T));
                var d1 = typeof(SQLiteConnection);
                object o = Activator.CreateInstance(typeof(SQLiteConnection), new object[] { Constants.DataBaseConnectionstringForSqlite, false });
                //myList = (List<T>)o.GetType().GetTypeInfo().GetDeclaredMethod("LoadData").MakeGenericMethod(new Type[] { ClassInstance.GetType() }).Invoke(o, null);
                List<MethodInfo> li = o.GetType().GetRuntimeMethods().Where(m => m.Name == "FindAll").ToList();

                if (li != null && li.Count != 0)
                {
                    foreach (var mn in li)
                    {
                        List<ParameterInfo> pi = mn.GetParameters().ToList();
                        foreach (ParameterInfo p in pi)
                        {
                            if (p.Name.Contains("predicate"))
                            {
                                typ = p.ParameterType;
                            }
                        }
                    }

                    List<T> listtodelete = (List<T>)o.GetType().GetRuntimeMethod("FindAll", new Type[] { typ }).MakeGenericMethod(new Type[] { ClassInstance.GetType() }).Invoke(o, new object[] { predicate });
                    foreach (var ls in listtodelete)
                    {
                        var method = o.GetType().GetRuntimeMethod("Delete1", new Type[] { typeof(object) }).Invoke(o, new object[] { ls });
                    }
                    o.GetType().GetTypeInfo().GetDeclaredMethod("Close").Invoke(o, null);
                }
#else
                Type typ = default(Type);
                var ClassInstance = (T)Activator.CreateInstance(typeof(T));
                var d1 = typeof(SQLiteConnection);
                object o = Activator.CreateInstance(typeof(SQLiteConnection), new object[] { Constants.DataBaseConnectionstringForSqlite, false });
#if (WINDOWS_APP && NOTANDROID) || (WINDOWS_PHONE_APP && NOTANDROID)
                List<MethodInfo> li = o.GetType().GetRuntimeMethods().Where(m => m.Name == "FindAll").ToList();
#endif
#if ANDROID
                List<MethodInfo> li = o.GetType().GetMethods().Where(m => m.Name == "FindAll").ToList();
#endif
                foreach (var mn in li)
                {
                    List<ParameterInfo> pi = mn.GetParameters().ToList();
                    foreach (ParameterInfo p in pi)
                    {
                        if (p.Name.Contains("predicate"))
                        {
                            typ = p.ParameterType;
                        }
                    }
                }
#if (WINDOWS_APP && NOTANDROID) || (WINDOWS_PHONE_APP && NOTANDROID)
                List<T> listtodelete =(List<T>) o.GetType().GetRuntimeMethod("FindAll", new Type[] { typ }).MakeGenericMethod(new Type[] { ClassInstance.GetType() }).Invoke(o, new object[] { predicate });
#endif
#if ANDROID
                List<T> listtodelete = (List<T>)o.GetType().GetMethod("FindAll", new Type[] { typ }).MakeGenericMethod(new Type[] { ClassInstance.GetType() }).Invoke(o, new object[] { predicate });

#endif
                foreach (var ls in listtodelete)
                {
#if (WINDOWS_APP && NOTANDROID) || (WINDOWS_PHONE_APP && NOTANDROID)
                   var method = o.GetType().GetRuntimeMethod("Delete1", new Type[] { typeof(object) }).Invoke(o, new object[] { ls });
#endif
#if ANDROID
                    var method = o.GetType().GetMethod("Delete1", new Type[] { typeof(object) }).Invoke(o, new object[] { ls });

#endif
                }
#endif
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in DeleteFromList Method In DataManager.cs file", ex);
            }
        }
        public void DeleteFromList(Expression<Func<T, bool>> predicate)
        {
            try
            {
                Type typ = default(Type);
                var ClassInstance = (T)Activator.CreateInstance(typeof(T));
                var d1 = typeof(SQLiteConnection);
                object o = Activator.CreateInstance(typeof(SQLiteConnection), new object[] { Constants.DataBaseConnectionstringForSqlite, false });
#if (WINDOWS_APP && NOTANDROID) || (WINDOWS_PHONE_APP && NOTANDROID)
                List<MethodInfo> li = o.GetType().GetRuntimeMethods().Where(m => m.Name == "Find").ToList();
#endif
# if ANDROID
                List<MethodInfo> li = o.GetType().GetMethods().Where(m => m.Name == "Find").ToList();
#endif
                foreach (var mn in li)
                {
                    List<ParameterInfo> pi = mn.GetParameters().ToList();
                    foreach (ParameterInfo p in pi)
                    {
                        if (p.Name.Contains("predicate"))
                        {
                            typ = p.ParameterType;
                        }
                    }
                }
#if (WINDOWS_APP && NOTANDROID) || (WINDOWS_PHONE_APP && NOTANDROID)
                var listtodelete = o.GetType().GetRuntimeMethod("Find", new Type[] { typ }).MakeGenericMethod(new Type[] { ClassInstance.GetType() }).Invoke(o, new object[] { predicate });
               
                var method = o.GetType().GetRuntimeMethod("Delete", new Type[] { typeof(object) }).Invoke(o, new object[] { listtodelete });
                o.GetType().GetTypeInfo().GetDeclaredMethod("Close").Invoke(o, null);
#endif
#if ANDROID
                var listtodelete = o.GetType().GetMethod("Find", new Type[] { typ }).MakeGenericMethod(new Type[] { ClassInstance.GetType() }).Invoke(o, new object[] { predicate });

                var method = o.GetType().GetMethod("Delete", new Type[] { typeof(object) }).Invoke(o, new object[] { listtodelete });
#endif
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in DeleteFromList Method In DataManager.cs file", ex);
            }
           
        }
        public void SaveToList(T listtosave, string condition1, string condition2)
        {
            try
            {
                var ClassInstance = (T)Activator.CreateInstance(typeof(T));
                var d1 = typeof(SQLiteConnection);
                object o = Activator.CreateInstance(typeof(SQLiteConnection), new object[] { Constants.DataBaseConnectionstringForSqlite, false });
#if (WINDOWS_APP && NOTANDROID) || (WINDOWS_PHONE_APP && NOTANDROID)
                var method = o.GetType().GetRuntimeMethod("Update", new Type[] { typeof(object) }).Invoke(o, new object[] { listtosave });
                o.GetType().GetTypeInfo().GetDeclaredMethod("Close").Invoke(o, null);
#endif
#if ANDROID
                var method = o.GetType().GetMethod("Update", new Type[] { typeof(object) }).Invoke(o, new object[] { listtosave });
#endif
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in SaveToList Method In DataManager.cs file", ex);
            }
        }

        public T GetFromList(Expression<Func<T, bool>> predicate)
        {
            Type typ = default(Type);
            var ClassInstance = (T)Activator.CreateInstance(typeof(T));
            var d1 = typeof(SQLiteConnection);
            object o = Activator.CreateInstance(typeof(SQLiteConnection), new object[] { Constants.DataBaseConnectionstringForSqlite, false });
#if (WINDOWS_APP && NOTANDROID) || (WINDOWS_PHONE_APP && NOTANDROID)
            List<MethodInfo> li = o.GetType().GetRuntimeMethods().Where(m => m.Name == "Find").ToList();
#endif
#if ANDROID
            List<MethodInfo> li = o.GetType().GetMethods().Where(m => m.Name == "Find").ToList();
#endif
            foreach (var mn in li)
            {
                List<ParameterInfo> pi = mn.GetParameters().ToList();
                foreach (ParameterInfo p in pi)
                {
                    if (p.Name.Contains("predicate"))
                    {
                        typ = p.ParameterType;
                    }
                }
            }
#if (WINDOWS_APP && NOTANDROID) || (WINDOWS_PHONE_APP && NOTANDROID)
            var method = o.GetType().GetRuntimeMethod("Find", new Type[] { typ }).MakeGenericMethod(new Type[] { ClassInstance.GetType() }).Invoke(o, new object[] { predicate });
            o.GetType().GetTypeInfo().GetDeclaredMethod("Close").Invoke(o, null);
#endif
# if ANDROID
            var method = o.GetType().GetMethod("Find", new Type[] { typ }).MakeGenericMethod(new Type[] { ClassInstance.GetType() }).Invoke(o, new object[] { predicate });

#endif
            return (T)method;
        }

        public T GetData(Expression<Func<T, bool>> predicate)
        {
            try
            {
#if (WINDOWS_APP && NOTANDROID) || (WINDOWS_PHONE_APP && NOTANDROID)
                List<T> myList = new List<T>();
#endif
                Type typ = default(Type);
                var ClassInstance = (T)Activator.CreateInstance(typeof(T));
                var d1 = typeof(SQLiteConnection);
                object o = Activator.CreateInstance(typeof(SQLiteConnection), new object[] { Constants.DataBaseConnectionstringForSqlite, false });
#if (WINDOWS_APP && NOTANDROID) || (WINDOWS_PHONE_APP && NOTANDROID)

                myList = (List<T>)o.GetType().GetTypeInfo().GetDeclaredMethod("LoadData").MakeGenericMethod(new Type[] { ClassInstance.GetType() }).Invoke(o, null);

#endif
#if ANDROID
                List<MethodInfo> li = o.GetType().GetMethods().Where(m => m.Name == "Find").ToList();

                foreach (var mn in li)
                {
                    List<ParameterInfo> pi = mn.GetParameters().ToList();
                    foreach (ParameterInfo p in pi)
                    {
                        if (p.Name.Contains("predicate"))
                        {
                            typ = p.ParameterType;
                        }
                    }
                }
#endif
#if (WINDOWS_APP && NOTANDROID) || (WINDOWS_PHONE_APP && NOTANDROID)
                var method = myList.AsQueryable().Where(predicate).FirstOrDefault();
                o.GetType().GetTypeInfo().GetDeclaredMethod("Close").Invoke(o, null);
#endif
#if ANDROID
                var method = o.GetType().GetMethod("Find", new Type[] { typ }).MakeGenericMethod(new Type[] { ClassInstance.GetType() }).Invoke(o, new object[] { predicate });

#endif
                return (T)method;
            }
            catch (Exception ex)
            {

                string mes = ex.Message;
                return default(T);
            }

        }
        
        public T Get(Expression<Func<T, bool>> predicate)
        {
            try
            {
                Type typ = default(Type);
                var ClassInstance = (T)Activator.CreateInstance(typeof(T));
                var d1 = typeof(SQLiteConnection);
                object o = Activator.CreateInstance(typeof(SQLiteConnection), new object[] { Constants.DataBaseConnectionstringForSqlite, false });
#if (WINDOWS_APP && NOTANDROID) || (WINDOWS_PHONE_APP && NOTANDROID)
                List<MethodInfo> li = o.GetType().GetRuntimeMethods().Where(m => m.Name == "Find").ToList();
#endif
#if ANDROID
                List<MethodInfo> li = o.GetType().GetMethods().Where(m => m.Name == "Find").ToList();
#endif
                foreach (var mn in li)
                {
                    List<ParameterInfo> pi = mn.GetParameters().ToList();
                    foreach (ParameterInfo p in pi)
                    {
                        if (p.Name.Contains("predicate"))
                        {
                            typ = p.ParameterType;
                        }
                    }
                }
#if (WINDOWS_APP && NOTANDROID) || (WINDOWS_PHONE_APP && NOTANDROID)
                var method = o.GetType().GetRuntimeMethod("Find", new Type[] { typ }).MakeGenericMethod(new Type[] { ClassInstance.GetType() }).Invoke(o, new object[] { predicate });
                o.GetType().GetTypeInfo().GetDeclaredMethod("Close").Invoke(o, null);
#endif
#if ANDROID
                var method = o.GetType().GetMethod("Find", new Type[] { typ }).MakeGenericMethod(new Type[] { ClassInstance.GetType() }).Invoke(o, new object[] { predicate });

#endif
                return (T)method;
            }
            catch (Exception ex)
            {

                string mes = ex.Message;
                return default(T);
            }

        }

        public List<T> GetList(Expression<Func<T, bool>> predicate, Func<T, string> predicate1, Func<T, double> predicate2, string Type)
        {
            Type[] typ = new Type[4];
            int parametercount = 0;
            var ClassInstance = (T)Activator.CreateInstance(typeof(T));
            var d1 = typeof(SQLiteConnection);
            object o = Activator.CreateInstance(typeof(SQLiteConnection), new object[] { Constants.DataBaseConnectionstringForSqlite, false });

            List<MethodInfo> li = o.GetType().GetRuntimeMethods().Where(m => m.Name == "GetList").ToList();
            foreach (var mn in li)
            {
                List<ParameterInfo> pi = mn.GetParameters().ToList();
                foreach (ParameterInfo p in pi)
                {
                    if (p.Name.Contains("predicate") || p.Name.Contains("predicate1") || p.Name.Contains("predicate2") || p.Name.Contains("Type"))
                    {
                        typ[parametercount] = p.ParameterType;
                        parametercount++;
                    }
                }
            }

            var method= (List<T>)o.GetType().GetRuntimeMethod("GetList",  typ).MakeGenericMethod(new Type[] { ClassInstance.GetType() }).Invoke(o, new object[] { predicate, predicate1, predicate2,Type});
            o.GetType().GetTypeInfo().GetDeclaredMethod("Close").Invoke(o, null);
            return method;
        }
        public List<T> GetTopratedList(Expression<Func<T, bool>> predicate, Func<T, int> predicate1, Func<T, double> predicate2, string Type)
        {
            Type[] typ = new Type[4];
            int parametercount = 0;
            var ClassInstance = (T)Activator.CreateInstance(typeof(T));
            var d1 = typeof(SQLiteConnection);
            object o = Activator.CreateInstance(typeof(SQLiteConnection), new object[] { Constants.DataBaseConnectionstringForSqlite, false });

            List<MethodInfo> li = o.GetType().GetRuntimeMethods().Where(m => m.Name == "GetTopratedList").ToList();
            foreach (var mn in li)
            {
                List<ParameterInfo> pi = mn.GetParameters().ToList();
                foreach (ParameterInfo p in pi)
                {
                    if (p.Name.Contains("predicate") || p.Name.Contains("predicate1") || p.Name.Contains("predicate2") || p.Name.Contains("Type"))
                    {
                        typ[parametercount] = p.ParameterType;
                        parametercount++;
                    }
                }
            }

            var method= (List<T>)o.GetType().GetRuntimeMethod("GetTopratedList",  typ).MakeGenericMethod(new Type[] { ClassInstance.GetType() }).Invoke(o, new object[] { predicate, predicate1, predicate2,Type});
            o.GetType().GetTypeInfo().GetDeclaredMethod("Close").Invoke(o, null);
            return method;
        }
        
        public List<T> GetList(Expression<Func<T, bool>> predicate, Func<T, int> predicate1, string ordertype)
        {
            Type[] typ = new Type[3];
            int parametercount = 0;
            var ClassInstance = (T)Activator.CreateInstance(typeof(T));
            var d1 = typeof(SQLiteConnection);
            object o = Activator.CreateInstance(typeof(SQLiteConnection), new object[] { Constants.DataBaseConnectionstringForSqlite, false });
            
            List<MethodInfo> li = o.GetType().GetRuntimeMethods().Where(m => m.Name == "GetList1").ToList();          
                foreach (var mn in li)
                {
                    List<ParameterInfo> pi = mn.GetParameters().ToList();
                    foreach (ParameterInfo p in pi)
                    {
                        if (p.Name.Contains("predicate") || p.Name.Contains("predicate1") || p.Name.Contains("ordertype"))
                        {
                            typ[parametercount] = p.ParameterType;
                            parametercount++;
                        }
                    }
                }

                var method= (List<T>)o.GetType().GetRuntimeMethod("GetList1",  typ).MakeGenericMethod(new Type[] { ClassInstance.GetType() }).Invoke(o, new object[] { predicate, predicate1, ordertype });
                o.GetType().GetTypeInfo().GetDeclaredMethod("Close").Invoke(o, null);
                return method;             
        }
        public List<T> GetListData(Expression<Func<T, bool>> predicate)
        {

            Type typ = default(Type);
            var ClassInstance = (T)Activator.CreateInstance(typeof(T));
            var d1 = typeof(SQLiteConnection);
            object o = Activator.CreateInstance(typeof(SQLiteConnection), new object[] { Constants.DataBaseConnectionstringForSqlite, false });

            List<MethodInfo> li = o.GetType().GetRuntimeMethods().Where(m => m.Name == "FindAll").ToList();
            foreach (var mn in li)
            {
                List<ParameterInfo> pi = mn.GetParameters().ToList();
                foreach (ParameterInfo p in pi)
                {
                    if (p.Name.Contains("predicate"))
                    {
                        typ = p.ParameterType;
                    }
                }
            }

            var method= (List<T>)o.GetType().GetRuntimeMethod("FindAll", new Type[] { typ }).MakeGenericMethod(new Type[] { ClassInstance.GetType() }).Invoke(o, new object[] { predicate });
            o.GetType().GetTypeInfo().GetDeclaredMethod("Close").Invoke(o, null);
            return method;
        }
        public List<T> GetListForDownLoadHistroy(Expression<Func<T, bool>> predicate)
        {

            Type typ = default(Type);
            var ClassInstance = (T)Activator.CreateInstance(typeof(T));
            var d1 = typeof(SQLiteConnection);
            object o = Activator.CreateInstance(typeof(SQLiteConnection), new object[] { Constants.DataBaseConnectionstringForSqlite, false });

            List<MethodInfo> li = o.GetType().GetRuntimeMethods().Where(m => m.Name == "Find").ToList();
            foreach (var mn in li)
            {
                List<ParameterInfo> pi = mn.GetParameters().ToList();
                foreach (ParameterInfo p in pi)
                {
                    if (p.Name.Contains("predicate"))
                    {
                        typ = p.ParameterType;
                    }
                }
            }
          
            var method= (List<T>) o.GetType().GetRuntimeMethod("Find", new Type[] { typ }).MakeGenericMethod(new Type[] { ClassInstance.GetType() }).Invoke(o, new object[] { predicate });
            o.GetType().GetTypeInfo().GetDeclaredMethod("Close").Invoke(o, null);
            return method;
        }
        public List<T> GetParentalList(Func<T, int> predicate1, string ordertype)
        {
            Type[] typ = new Type[2];
            int parametercount = 0;
            var ClassInstance = (T)Activator.CreateInstance(typeof(T));
            var d1 = typeof(SQLiteConnection);
            object o = Activator.CreateInstance(typeof(SQLiteConnection), new object[] { Constants.DataBaseConnectionstringForSqlite, false });

            List<MethodInfo> li = o.GetType().GetRuntimeMethods().Where(m => m.Name == "GetParentalList").ToList();
            foreach (var mn in li)
            {
                List<ParameterInfo> pi = mn.GetParameters().ToList();
                foreach (ParameterInfo p in pi)
                {
                    if ( p.Name.Contains("predicate1") || p.Name.Contains("ordertype"))
                    {
                        typ[parametercount] = p.ParameterType;
                        parametercount++;
                    }
                }
            }

            var method= (List<T>)o.GetType().GetRuntimeMethod("GetParentalList",  typ ).MakeGenericMethod(new Type[] { ClassInstance.GetType() }).Invoke(o, new object[] { predicate1, ordertype });
            o.GetType().GetTypeInfo().GetDeclaredMethod("Close").Invoke(o, null);
            return method;
        }
#if (WINDOWS_APP && NOTANDROID) || (WINDOWS_PHONE_APP && NOTANDROID)
        public List<T> GetListfromxml(Expression<Func<T, bool>> predicate, Expression<Func<T, int>> predicate1, string ordertype)
        {
            List<T> SelectedItem = new List<T>();
            List<T> LoadXml = LoadDataxml(typeof(T), null, null, null);
            if (ordertype == "D")
                SelectedItem = LoadXml.AsQueryable().Where(predicate).OrderByDescending(predicate1).ToList();
            else
                SelectedItem = LoadXml.AsQueryable().Where(predicate).OrderBy(predicate1).ToList();
            return SelectedItem;
        }


        public void SaveData(List<T> listtosave)
        {
            try
            {             
                string Data = string.Empty;
                var ClassInstance = (T)Activator.CreateInstance(typeof(T));
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<T>));
                MemoryStream memStream = new MemoryStream();
                StreamWriter streamWriter = new StreamWriter(memStream);
                xmlSerializer.Serialize(streamWriter, listtosave);
                memStream.Position = 0;
                StreamReader streamReader = new StreamReader(memStream);
                XDocument serializedXML = new XDocument();
                serializedXML = XDocument.Load(memStream);
                StringBuilder sb = new StringBuilder();
                TextWriter tx = new StringWriter(sb);
                serializedXML.Save(tx);
                StorageFolder store1 = ApplicationData.Current.LocalFolder;
                StorageFile file1 = Task.Run(async () => await store1.CreateFileAsync(@"XmlData\" + ClassInstance.GetType().Name + ".xml", Windows.Storage.CreationCollisionOption.ReplaceExisting)).Result;
                var fquery1 = Task.Run(async () => await file1.OpenAsync(Windows.Storage.FileAccessMode.ReadWrite)).Result;
                var outputStream = fquery1.GetOutputStreamAt(0);
                var writer = new Windows.Storage.Streams.DataWriter(outputStream);
                Data = tx.ToString().Replace("<ArrayOf" + ClassInstance.GetType().Name + " xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">", "<NewDataSet>");
                Data = Data.Replace("</ArrayOf" + ClassInstance.GetType().Name + ">", "</NewDataSet>");
                Data = Data.Replace("utf-16", "utf-8");
                writer.WriteString(Data);
                var fi = Task.Run(async () => await writer.StoreAsync()).Result;
                writer.DetachStream();
                var oi = Task.Run(async () => await outputStream.FlushAsync()).Result;
                outputStream.Dispose();
                fquery1.Dispose();
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in SaveData Method In DataManager.cs file", ex);
            }

        }
        public List<T> LoadDataxml(Type T, object changedlist, string condition1, string condition2)
        {
            List<T> myList = new List<T>();
            try
            {
                StorageFile file = default(StorageFile);
                var ListofElements = default(List<XElement>);
                T ClassInstance = (T)Activator.CreateInstance(typeof(T));
                XDocument xdoc = new XDocument();
                StorageFolder store = ApplicationData.Current.LocalFolder;
                if (Storage.FileExists(ClassInstance.GetType().Name + ".xml").Result)
                {
                    file = Task.Run(async () => await store.GetFileAsync(ClassInstance.GetType().Name + ".xml")).Result;
                    var f = Task.Run(async () => await file.OpenReadAsync()).Result;
                    IInputStream inputStream = f.GetInputStreamAt(0);
                    DataReader dataReader = new DataReader(inputStream);
                    uint numBytesLoaded = Task.Run(async () => await dataReader.LoadAsync((uint)f.Size)).Result;
                    string xmlData = (dataReader.ReadString(numBytesLoaded)).ToString();
                    System.IO.MemoryStream ms = new System.IO.MemoryStream(System.Text.Encoding.UTF8.GetBytes(xmlData));
                    xdoc = XDocument.Load(ms);
                    XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));

                    ListofElements = (from i in xdoc.Descendants("NewDataSet").Elements() select i).ToList();

                    foreach (XElement element in ListofElements)
                    {
                        System.IO.MemoryStream ms1 = new System.IO.MemoryStream(System.Text.Encoding.UTF8.GetBytes(element.ToString()));
                        object o = xmlSerializer.Deserialize(ms1);
                        ClassInstance = (T)o;
                        if (string.IsNullOrEmpty(condition2))
                        {
                            if ((changedlist != null) ? (ClassInstance.GetType().GetTypeInfo().GetDeclaredProperty(condition1).GetValue((object)ClassInstance).ToString() == changedlist.GetType().GetTypeInfo().GetDeclaredProperty(condition1).GetValue(changedlist).ToString()) : (changedlist != null))
                                myList.Add((T)changedlist);
                            else
                                myList.Add(ClassInstance);
                        }
                        else
                        {
                            if ((changedlist != null) ? (ClassInstance.GetType().GetTypeInfo().GetDeclaredProperty(condition1).GetValue((object)ClassInstance).ToString() == changedlist.GetType().GetTypeInfo().GetDeclaredProperty(condition1).GetValue(changedlist).ToString() && ClassInstance.GetType().GetTypeInfo().GetDeclaredProperty(condition2).GetValue((object)ClassInstance).ToString() == changedlist.GetType().GetTypeInfo().GetDeclaredProperty(condition2).GetValue(changedlist).ToString()) : (changedlist != null))
                                myList.Add((T)changedlist);
                            else
                                myList.Add(ClassInstance);
                        }
                    }
                    dataReader.DetachStream();
                    dataReader.Dispose();
                    inputStream.Dispose();
                }
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in LoadDataxml Method In DataManager.cs file", ex);
            }
            return myList;
        }

        public void SavemovieData(List<T> listtosave)
        {
            try
            {
                string Data = string.Empty;
                var ClassInstance = (T)Activator.CreateInstance(typeof(T));
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<T>));
                MemoryStream memStream = new MemoryStream();
                StreamWriter streamWriter = new StreamWriter(memStream);
                xmlSerializer.Serialize(streamWriter, listtosave);
                memStream.Position = 0;
                StreamReader streamReader = new StreamReader(memStream);
                XDocument serializedXML = new XDocument();
                serializedXML = XDocument.Load(memStream);
                StringBuilder sb = new StringBuilder();
                TextWriter tx = new StringWriter(sb);
                serializedXML.Save(tx);
                StorageFolder store1 = ApplicationData.Current.LocalFolder;
                StorageFile file1 = Task.Run(async () => await store1.CreateFileAsync(@"XmlData\" + ClassInstance.GetType().Name + ".xml", Windows.Storage.CreationCollisionOption.ReplaceExisting)).Result;
                var fquery1 = Task.Run(async () => await file1.OpenAsync(Windows.Storage.FileAccessMode.ReadWrite)).Result;
                var outputStream = fquery1.GetOutputStreamAt(0);
                var writer = new Windows.Storage.Streams.DataWriter(outputStream);
                Data = tx.ToString().Replace("<ArrayOfShowList xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">", "<NewDataSet>");
                Data = Data.Replace("</ArrayOfShowList>", "</NewDataSet>");
                Data = Data.Replace("utf-16", "utf-8");
                writer.WriteString(Data);
                var fi = Task.Run(async () => await writer.StoreAsync()).Result;
                writer.DetachStream();
                var oi = Task.Run(async () => await outputStream.FlushAsync()).Result;
                outputStream.Dispose();
                fquery1.Dispose();
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in SavemovieData Method In DataManager.cs file", ex);
            }

        }
        public T GetFromListxml(string coloumnname, string coloumnvalue)
        {

            var ClassInstance1 = (T)Activator.CreateInstance(typeof(T));
            try
            {
                StorageFile file = default(StorageFile);
                T ClassInstance = (T)Activator.CreateInstance(typeof(T));
                XDocument xdoc = new XDocument();
                StorageFolder store = ApplicationData.Current.LocalFolder;
                if (ClassInstance.GetType().Name == "Historys")
                    file = Task.Run(async () => await store.GetFileAsync(ClassInstance.GetType().Name + ".xml")).Result;
                else
                    file = Task.Run(async () => await store.GetFileAsync(@"XmlData\" + ClassInstance.GetType().Name + ".xml")).Result;
                var f = Task.Run(async () => await file.OpenReadAsync()).Result;
                IInputStream inputStream = f.GetInputStreamAt(0);
                DataReader dataReader = new DataReader(inputStream);
                uint numBytesLoaded = Task.Run(async () => await dataReader.LoadAsync((uint)f.Size)).Result;
                string xmlData = (dataReader.ReadString(numBytesLoaded)).ToString();
                System.IO.MemoryStream ms = new System.IO.MemoryStream(System.Text.Encoding.UTF8.GetBytes(xmlData));
                xdoc = XDocument.Load(ms);
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
                int Firstcount = xdoc.Descendants("NewDataSet").Elements().Where(i => i.Element(coloumnname).Value.ToString() == coloumnvalue.ToString()).Count();
                var ListofElements = xdoc.Descendants("NewDataSet").Elements().Where(i => i.Element(coloumnname).Value.ToString() == coloumnvalue.ToString()).Take(Firstcount).ToList();
                foreach (XElement element in ListofElements)
                {
                    System.IO.MemoryStream ms1 = new System.IO.MemoryStream(System.Text.Encoding.UTF8.GetBytes(element.ToString()));
                    object o = xmlSerializer.Deserialize(ms1);
                    ClassInstance1 = (T)o;

                }
                dataReader.DetachStream();
                dataReader.Dispose();
                inputStream.Dispose();
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in LoadData Method In DataManager.cs file", ex);
            }

            return ClassInstance1;
        }
        public List<T> LoadData1(string coloumnname, string coloumnvalue)
        {
            List<T> myList = new List<T>();
            try
            {
                StorageFile file = default(StorageFile);

                T ClassInstance = (T)Activator.CreateInstance(typeof(T));
                XDocument xdoc = new XDocument();
                StorageFolder store = ApplicationData.Current.LocalFolder;
                if (ClassInstance.GetType().Name == "Historys")
                    file = Task.Run(async () => await store.GetFileAsync(ClassInstance.GetType().Name + ".xml")).Result;
                else
                    file = Task.Run(async () => await store.GetFileAsync(@"XmlData\" + ClassInstance.GetType().Name + ".xml")).Result;

                var f = Task.Run(async () => await file.OpenReadAsync()).Result;
                IInputStream inputStream = f.GetInputStreamAt(0);
                DataReader dataReader = new DataReader(inputStream);
                uint numBytesLoaded = Task.Run(async () => await dataReader.LoadAsync((uint)f.Size)).Result;
                string xmlData = (dataReader.ReadString(numBytesLoaded)).ToString();
                System.IO.MemoryStream ms = new System.IO.MemoryStream(System.Text.Encoding.UTF8.GetBytes(xmlData));
                xdoc = XDocument.Load(ms);
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
                int Firstcount = xdoc.Descendants("NewDataSet").Elements().Where(i => i.Element(coloumnname).Value.ToString() == coloumnvalue.ToString()).Count();
                var ListofElements = xdoc.Descendants("NewDataSet").Elements().Where(i => i.Element(coloumnname).Value.ToString() == coloumnvalue.ToString()).Take(Firstcount).ToList();
                foreach (XElement element in ListofElements)
                {
                    System.IO.MemoryStream ms1 = new System.IO.MemoryStream(System.Text.Encoding.UTF8.GetBytes(element.ToString()));
                    object o = xmlSerializer.Deserialize(ms1);
                    ClassInstance = (T)o;
                    myList.Add(ClassInstance);
                }
                dataReader.DetachStream();
                dataReader.Dispose();
                inputStream.Dispose();
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in LoadData Method In DataManager.cs file", ex);
            }
            return myList;
        }
        
        List<T> LoadDataforShows(Type T, object changedlist, string condition1)
        {
            List<T> myList = new List<T>();
            try
            {
                T ClassInstance = (T)Activator.CreateInstance(typeof(T));
                XDocument xdoc = new XDocument();
                StorageFolder store = ApplicationData.Current.LocalFolder;
                StorageFile file = Task.Run(async () => await store.CreateFileAsync(@"XmlData\" + ClassInstance.GetType().Name + ".xml", Windows.Storage.CreationCollisionOption.OpenIfExists)).Result;
                var f = Task.Run(async () => await file.OpenAsync(FileAccessMode.Read)).Result;
                IInputStream inputStream = f.GetInputStreamAt(0);
                DataReader dataReader = new DataReader(inputStream);
                uint numBytesLoaded = Task.Run(async () => await dataReader.LoadAsync((uint)f.Size)).Result;
                string xmlData = (dataReader.ReadString(numBytesLoaded)).ToString();
                System.IO.MemoryStream ms = new System.IO.MemoryStream(System.Text.Encoding.UTF8.GetBytes(xmlData));
                xdoc = XDocument.Load(ms);
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
                var ListofElements = (from i in xdoc.Descendants("NewDataSet").Elements() select i).ToList();
                foreach (XElement element in ListofElements)
                {
                    System.IO.MemoryStream ms1 = new System.IO.MemoryStream(System.Text.Encoding.UTF8.GetBytes(element.ToString()));
                    object o = xmlSerializer.Deserialize(ms1);
                    ClassInstance = (T)o;


                    if ((changedlist != null) ? ClassInstance.GetType().GetTypeInfo().GetDeclaredProperty(condition1).GetValue((object)ClassInstance).ToString() == changedlist.GetType().GetTypeInfo().GetDeclaredProperty(condition1).GetValue(changedlist).ToString() : (changedlist != null))
                        myList.Add((T)changedlist);
                    else
                        myList.Add(ClassInstance);
                }
                dataReader.DetachStream();
                dataReader.Dispose();
                inputStream.Dispose();
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in LoadDataforShows Method In DataManager.cs file", ex);
            }
            return myList;
        }
        public void SaveQuizData(List<T> listtosave)
        {
            try
            {
                string Data = string.Empty;
                var ClassInstance = (T)Activator.CreateInstance(typeof(T));
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<T>));
                MemoryStream memStream = new MemoryStream();
                StreamWriter streamWriter = new StreamWriter(memStream);
                xmlSerializer.Serialize(streamWriter, listtosave);
                memStream.Position = 0;
                StreamReader streamReader = new StreamReader(memStream);
                XDocument serializedXML = new XDocument();
                serializedXML = XDocument.Load(memStream);
                StringBuilder sb = new StringBuilder();
                TextWriter tx = new StringWriter(sb);
                serializedXML.Save(tx);
                StorageFolder store1 = ApplicationData.Current.LocalFolder;
                StorageFile file1 = Task.Run(async () => await store1.CreateFileAsync(@"XmlData\" + ClassInstance.GetType().Name + ".xml", Windows.Storage.CreationCollisionOption.ReplaceExisting)).Result;
                var fquery1 = Task.Run(async () => await file1.OpenAsync(Windows.Storage.FileAccessMode.ReadWrite)).Result;
                var outputStream = fquery1.GetOutputStreamAt(0);
                var writer = new Windows.Storage.Streams.DataWriter(outputStream);
                if (listtosave.Count == 0)
                {
                    Data = tx.ToString().Replace("<ArrayOfQuizUserAnswers xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" />",
                        "<NewDataSet xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" />");
                    Data = Data.Replace("utf-16", "utf-8");
                }
                else
                {
                    Data = tx.ToString().Replace("<ArrayOfQuizUserAnswers xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">", "<NewDataSet>");
                    Data = Data.Replace("</ArrayOfQuizUserAnswers>", "</NewDataSet>");
                    Data = Data.Replace("utf-16", "utf-8");
                }
                writer.WriteString(Data);
                var fi = Task.Run(async () => await writer.StoreAsync()).Result;
                writer.DetachStream();
                var oi = Task.Run(async () => await outputStream.FlushAsync()).Result;
                outputStream.Dispose();
                fquery1.Dispose();
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in SaveQuizData Method In DataManager.cs file", ex);
            }
        }
          public void SaveQuizList(List<T> listtosave)
        {
            try
            {
                string Data = string.Empty;
                var ClassInstance = (T)Activator.CreateInstance(typeof(T));
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<T>));
                MemoryStream memStream = new MemoryStream();
                StreamWriter streamWriter = new StreamWriter(memStream);
                xmlSerializer.Serialize(streamWriter, listtosave);
                memStream.Position = 0;
                StreamReader streamReader = new StreamReader(memStream);
                XDocument serializedXML = new XDocument();
                serializedXML = XDocument.Load(memStream);
                StringBuilder sb = new StringBuilder();
                TextWriter tx = new StringWriter(sb);
                serializedXML.Save(tx);
                StorageFolder store1 = ApplicationData.Current.LocalFolder;
                StorageFile file1 = Task.Run(async () => await store1.CreateFileAsync(@"XmlData\" + ClassInstance.GetType().Name + ".xml", Windows.Storage.CreationCollisionOption.ReplaceExisting)).Result;
                var fquery1 = Task.Run(async () => await file1.OpenAsync(Windows.Storage.FileAccessMode.ReadWrite)).Result;
                var outputStream = fquery1.GetOutputStreamAt(0);
                var writer = new Windows.Storage.Streams.DataWriter(outputStream);
                Data = tx.ToString().Replace("<ArrayOfQuizList xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">", "<NewDataSet>");
                Data = Data.Replace("</ArrayOfQuizList>", "</NewDataSet>");
                Data = Data.Replace("utf-16", "utf-8");
                writer.WriteString(Data);
                var fi = Task.Run(async () => await writer.StoreAsync()).Result;
                writer.DetachStream();
                var oi = Task.Run(async () => await outputStream.FlushAsync()).Result;
                outputStream.Dispose();
                fquery1.Dispose();
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in SaveQuizList Method In DataManager.cs file", ex);
            }
        }
#endif
		public List<T> LoadData()
		{
			List<T> myList = new List<T>();
			try
			{
				var ClassInstance = (T)Activator.CreateInstance(typeof(T));
				var d1 = typeof(SQLiteConnection);
				object o = Activator.CreateInstance(typeof(SQLiteConnection), new object[] { Constants.DataBaseConnectionstringForSqlite, false });
				myList = (List<T>)o.GetType().GetTypeInfo().GetDeclaredMethod("LoadData").MakeGenericMethod(new Type[] { ClassInstance.GetType() }).Invoke(o, null);
                o.GetType().GetTypeInfo().GetDeclaredMethod("Close").Invoke(o, null);
            }
			catch (Exception ex)
			{
				Exceptions.SaveOrSendExceptions("Exception in LoadData Method In DataManager.cs file", ex);
			}
			return myList;
		}
        public void SaveToQuizList(T listtosave, string condition1, string condition2)
        {
            //List<T> LoadXml = LoadData(typeof(T), listtosave, condition1, condition2);
            //SaveQuizList(LoadXml);
        }
      

        public List<T> GetList(Expression<Func<T, bool>> predicate,Func<T, DateTime> predicate1, Func<T, double> predicate2, string Type)
        {
            Type[] typ = new Type[4];
            int parametercount = 0;
            var ClassInstance = (T)Activator.CreateInstance(typeof(T));
            var d1 = typeof(SQLiteConnection);
            object o = Activator.CreateInstance(typeof(SQLiteConnection), new object[] { Constants.DataBaseConnectionstringForSqlite, false });

            List<MethodInfo> li = o.GetType().GetRuntimeMethods().Where(m => m.Name == "GetList2").ToList();
            foreach (var mn in li)
            {
                List<ParameterInfo> pi = mn.GetParameters().ToList();
                foreach (ParameterInfo p in pi)
                {
                    if (p.Name.Contains("predicate") || p.Name.Contains("predicate1") || p.Name.Contains("predicate2")|| p.Name.Contains("Type"))
                    {
                        typ[parametercount] = p.ParameterType;
                        parametercount++;
                    }
                }
            }

            var method= (List<T>)o.GetType().GetRuntimeMethod("GetList2", typ).MakeGenericMethod(new Type[] { ClassInstance.GetType() }).Invoke(o, new object[] { predicate, predicate1,predicate2,Type });
            o.GetType().GetTypeInfo().GetDeclaredMethod("Close").Invoke(o, null);
            return method;
        }    
    }

}
