using Common.Library;
using OnlineVideos.Data;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;
using Windows.UI.Xaml.Controls;

namespace InsertIntoDataBase
{
    public class retrieveframe
    {
        public static Frame getframe(string assemblyname)
        {
            Assembly assembly = Assembly.Load(new AssemblyName(assemblyname));
            string classname = assembly.DefinedTypes.Where(i => i.Name == "App").FirstOrDefault().FullName;
            Type typ = assembly.GetType(classname);
            return (Frame)typ.GetTypeInfo().GetDeclaredField("rootFrame").GetValue(typ);
        }
    }
    public class InsertData<T>
    {
        Type code = default(Type);

        public T ParameterList
        {
            get;
            set;
        }

        public void Insert()
        {
            try
            {
                Assembly DataAssembly = Assembly.Load(new AssemblyName("OnlineVideos.Entities"));
                Type TableType = DataAssembly.GetType("OnlineVideos.Entities." + typeof(T).Name);
                object TableInstance = Activator.CreateInstance(TableType);
                //OnlineVideoDataContext context = new OnlineVideoDataContext(Constants.DatabaseConnectionString);
                foreach (var PropertyName in TableType.GetRuntimeProperties())
                {
                    Attribute[] ConditionAttributes = PropertyName.GetCustomAttributes().Where(i => i is ColumnAttribute || i is PrimaryKeyAttribute).ToArray();
                    // Attribute[] ConditionAttributes = Attribute.GetCustomAttributes(PropertyName).Where(i => i is System.Data.Linq.Mapping.ColumnAttribute).ToArray();

                    if (ConditionAttributes.Length > 0)
                    {
                        code = PropertyName.PropertyType;
                        if (PropertyName.PropertyType == typeof(DateTime))
                        {
                            if ((DateTime)Convert.ChangeType(ParameterList.GetType().GetRuntimeProperty(PropertyName.Name.ToString()).GetValue(ParameterList, null).ToString(), PropertyName.PropertyType, null) == default(DateTime))
                            {
                                ParameterList.GetType().GetRuntimeProperty(PropertyName.Name.ToString()).SetValue(ParameterList, (DateTime)Convert.ChangeType(Defaultvalue(code).ToString(), PropertyName.PropertyType, null), null);
                            }
                            PropertyName.SetValue(TableInstance, (DateTime)Convert.ChangeType(ParameterList.GetType().GetRuntimeProperty(PropertyName.Name.ToString()).GetValue(ParameterList, null).ToString(), PropertyName.PropertyType, null), null);
                        }
                        else
                        {
                            if (ParameterList.GetType().GetRuntimeProperty(PropertyName.Name.ToString()).GetValue(ParameterList, null) == null)
                            {
                                ParameterList.GetType().GetRuntimeProperty(PropertyName.Name.ToString()).SetValue(ParameterList, Convert.ChangeType(Defaultvalue(code).ToString(), PropertyName.PropertyType, null), null);
                            }
                        }
                        PropertyName.SetValue(TableInstance, Convert.ChangeType(ParameterList.GetType().GetRuntimeProperty(PropertyName.Name.ToString()).GetValue(ParameterList, null).ToString(), PropertyName.PropertyType, null), null);
                    }
                }
                SQLite.SQLiteAsyncConnection conn = new SQLite.SQLiteAsyncConnection(Constants.DataBaseConnectionstringForSqlite);
                Constants.connection = conn;

                int result = Task.Run(async () => await Constants.connection.InsertAsync(TableInstance)).Result;
                int fd = result;
                int fd1 = fd;
                int fd2 = fd1;
            }
            catch (Exception ex)
            {
                string mes = ex.Message;
                Exceptions.SaveOrSendExceptions("Exception in  Insert Method In InsertData.cs file", ex);
            }
        }

        public object Defaultvalue(Type code)
        {

            switch (code.Name)
            {

                case "Boolean":
                    return false;

                case "Double":
                    return 3.5;

                case "String":
                    return string.Empty;

                case "Int32":
                    return 0;

                case "Int16":
                    return 0;

                case "Int64":
                    return 0;

                case "DateTime":
                    return DateTime.Now;

                default:
                    return default(object);
            }
        }

        public string EntityToXML(T list)
        {
            XDocument xmlDoc;
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            ns.Add("", "");
            using (MemoryStream xmlStream = new MemoryStream())
            {
                xmlSerializer.Serialize(xmlStream, list, ns);
                xmlStream.Position = 0;
                xmlDoc = XDocument.Load(xmlStream);
                return xmlDoc.ToString();
            }
        }

        public T XMLToEntity(string XMLString)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
            T list = (T)xmlSerializer.Deserialize(new StringReader(XMLString));
            return list;
        }
    }
}