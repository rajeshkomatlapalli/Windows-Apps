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

namespace OnlineVideos.Data
{
    public class XMLDataManager<T>
    {
#if NOTANDROID
        public void InsertIntoXml(T listtoAdd)
        {
            List<T> LoadXml = LoadData(typeof(T), null, null, null);
            try
            {
                LoadXml.Add(listtoAdd);
                SaveData(LoadXml);
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in InsertIntoXml Method In DataManager.cs file", ex);
            }
        }
        public List<int> GetDistinctFromList(Expression<Func<T, bool>> predicate, Expression<Func<T, int>> predicate1)
        {
            List<int> FilteredList = new List<int>();
            List<T> LoadXml = LoadData(typeof(T), null, null, null);
            FilteredList = LoadXml.AsQueryable().Where(predicate).Select(predicate1).Distinct().ToList();
            return FilteredList;
        }
        public void Savemovies1(T listtosave, string condition1)
        {
            List<T> LoadXml = LoadDataforShows1(typeof(T), listtosave, condition1);
            try
            {
                SavemovieData1(LoadXml);
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in Savemovies Method In DataManager.cs file", ex);
            }
        }
        public void Savemovies(T listtosave, string condition1)
        {
            List<T> LoadXml = LoadDataforShows(typeof(T), listtosave, condition1);
            try
            {
                SavemovieData1(LoadXml);
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in Savemovies Method In DataManager.cs file", ex);
            }
        }
        public void DeleteFromList(Expression<Func<T, bool>> predicate)
        {
            List<T> FilteredList = new List<T>();
            List<T> LoadXml = LoadData(typeof(T), null, null, null);
            try
            {
                FilteredList = LoadXml.AsQueryable().Where(predicate).ToList();
                if (FilteredList.Count > 0)
                {
                    foreach (var f in FilteredList)
                    {
                        LoadXml.Remove(f);
                    }
                }
                SaveData(LoadXml);
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in GalleryPopup Method In DataManager.cs file", ex);
            }
        }
        public void SaveToList(T listtosave, string condition1, string condition2)
        {
            List<T> LoadXml = LoadData(typeof(T), listtosave, condition1, condition2);
            try
            {
                SaveData(LoadXml);
            }
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in SaveToList Method In DataManager.cs file", ex);
            }
        }
        public T GetFromList(Expression<Func<T, bool>> predicate)
        {
            List<T> FilteredList = new List<T>();
            var ClassInstance = (T)Activator.CreateInstance(typeof(T));
            List<T> LoadXml = LoadData(typeof(T), null, null, null);
            FilteredList = LoadXml.AsQueryable().Where(predicate).ToList();
            if (FilteredList.Count > 0)
                ClassInstance = FilteredList[0];
            else
                ClassInstance = default(T);

            return ClassInstance;
        }
        public T Get(Expression<Func<T, bool>> predicate)
        {
            object ClassInstance = null;
            List<T> LoadXml = LoadData(typeof(T), null, null, null);
            var List = LoadXml.AsQueryable().Where(predicate).ToList();
            if (List.Count > 0)
                ClassInstance = (T)List[0];

            return (T)ClassInstance;
        }

        public List<T> GetList(Expression<Func<T, bool>> predicate, Expression<Func<T, string>> predicate1, Expression<Func<T, double>> predicate2, string Type)
        {
            List<T> SelectedItem = new List<T>();
            List<T> LoadXml = LoadData(typeof(T), null, null, null);
            if (Type == "Recent")
            {

                SelectedItem = LoadXml.AsQueryable().Where(predicate).OrderByDescending(predicate1).ThenByDescending(predicate2).ToList();
            }
            else
            {
                SelectedItem = LoadXml.AsQueryable().Where(predicate).OrderByDescending(predicate2).ThenByDescending(predicate1).ToList();
            }
            return SelectedItem;
        }
        public List<T> GetTopratedList(Expression<Func<T, bool>> predicate, Expression<Func<T, int>> predicate1, Expression<Func<T, double>> predicate2, string Type)
        {
            List<T> SelectedItem = new List<T>();
            List<T> LoadXml = LoadData(typeof(T), null, null, null);
            if (Type == "Recent")
            {

                SelectedItem = LoadXml.AsQueryable().Where(predicate).OrderByDescending(predicate1).ThenByDescending(predicate2).ToList();
            }
            else
            {
                SelectedItem = LoadXml.AsQueryable().Where(predicate).OrderByDescending(predicate2).ThenByDescending(predicate1).ToList();
            }
            return SelectedItem;
        }
        public List<T> GetList(Expression<Func<T, bool>> predicate, Expression<Func<T, int>> predicate1, string ordertype)
        {
            List<T> SelectedItem = new List<T>();
            List<T> LoadXml = LoadData(typeof(T), null, null, null);
            if (ordertype == "D")
                SelectedItem = LoadXml.AsQueryable().Where(predicate).OrderByDescending(predicate1).ToList();
            else
                SelectedItem = LoadXml.AsQueryable().Where(predicate).OrderBy(predicate1).ToList();
            return SelectedItem;
        }
        public List<T> GetParentalList( Expression<Func<T, int>> predicate1, string ordertype)
        {
            List<T> SelectedItem = new List<T>();
            List<T> LoadXml = LoadData(typeof(T), null, null, null);
            if (ordertype == "D")
                SelectedItem = LoadXml.AsQueryable().OrderByDescending(predicate1).ToList();
            else
                SelectedItem = LoadXml.AsQueryable().OrderBy(predicate1).ToList();
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
        public void SavemovieData1(List<T> listtosave)
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
                Data = tx.ToString().Replace("<ArrayOfTotalExpensive xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">", "<NewDataSet>");
                Data = Data.Replace("</ArrayOfTotalExpensive>", "</NewDataSet>");
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
        public List<T> LoadData(Type T, object changedlist, string condition1, string condition2)
        {
            List<T> myList = new List<T>();
            try
            {
                StorageFile file = default(StorageFile);
                var ListofElements = default(List<XElement>);
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
                if (ClassInstance.GetType().Name == "Historys")
                    ListofElements = (from i in xdoc.Descendants("History").Elements() select i).ToList();
                else
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
            catch (Exception ex)
            {
                Exceptions.SaveOrSendExceptions("Exception in LoadData Method In DataManager.cs file", ex);
            }
            return myList;
        }
        List<T> LoadDataforShows1(Type T, object changedlist, string condition1)
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

            }
            return myList;
        }
        public void SaveQuizData(List<T> listtosave)
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
        public void SaveToQuizList(T listtosave, string condition1, string condition2)
        {
            List<T> LoadXml = LoadData(typeof(T), listtosave, condition1, condition2);
            SaveQuizList(LoadXml);
        }
        public void SaveQuizList(List<T> listtosave)
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

        public List<T> GetList(Expression<Func<T, bool>> predicate, Expression<Func<T, DateTime>> predicate1, Expression<Func<T, double>> predicate2, string Type)
        {
            List<T> SelectedItem = new List<T>();
            List<T> LoadXml = LoadData(typeof(T), null, null, null);
            if (Type == "Recent")
            {

                SelectedItem = LoadXml.AsQueryable().Where(predicate).OrderByDescending(predicate1).ThenByDescending(predicate2).ToList();
            }
            else
            {
                SelectedItem = LoadXml.AsQueryable().Where(predicate).OrderByDescending(predicate2).ThenByDescending(predicate1).ToList();
            }
            return SelectedItem;
        }
#endif
    }

}
