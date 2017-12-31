using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Xml.Serialization;

namespace WpfFileDialogs
{
    static public class LST
    {

        /// <summary>
        /// SaveClass
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="file"></param>
        static public void SaveClass<T>(T obj, string file)
        {
            try
            {
                if (!HasWriteAccessToFolder(Path.GetDirectoryName(".\\")))
                    file = Directory.GetParent(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)).FullName + "\\" + file;

                XmlSerializer xs = new XmlSerializer(typeof(T));
                using (StreamWriter wr = new StreamWriter(file))
                {
                    xs.Serialize(wr, obj);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
        }

        /// <summary>
        /// LoadClass
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="file"></param>
        /// <returns></returns>
        static public bool LoadClass<T>(ref T obj, string file)
        {
            try
            {
                if (!HasWriteAccessToFolder(Path.GetDirectoryName(".\\")))
                    file = Directory.GetParent(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)).FullName + "\\" + file;

                XmlSerializer serializer = new XmlSerializer(typeof(T));
                using (StreamReader rd = new StreamReader(file))
                {
                    var Obj = serializer.Deserialize(rd);
                    obj = (T)Obj;
                }
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return false;
            }
        }

        /// <summary>
        /// LoadList
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        static public List<T> LoadList<T>(string file)
        {
            List<T> list = null;

            try
            {
                if (!HasWriteAccessToFolder(Path.GetDirectoryName(".\\")))
                    file = Directory.GetParent(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)).FullName + "\\" + file;

                XmlSerializer xs = new XmlSerializer(typeof(List<T>));
                //XmlSerializer xs = XmlSerializer.FromTypes(new[] { typeof(List<T>) })[0]; // No warning but ATTENTION that generates Memory Leak
                using (StreamReader rd = new StreamReader(file))
                {
                    list = xs.Deserialize(rd) as List<T>;
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                list = new List<T>();
            }
            return list;
        }

        /// <summary>
        /// SaveList
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        static public void SaveList<T>(List<T> list, string file)
        {
            try
            {
                if (!HasWriteAccessToFolder(Path.GetDirectoryName(".\\")))
                    file = Directory.GetParent(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)).FullName + "\\" + file;

                XmlSerializer xs = new XmlSerializer(typeof(List<T>));
                //XmlSerializer xs = XmlSerializer.FromTypes(new[] { typeof(List<T>) })[0]; // No warning but ATTENTION that generates Memory Leak
                using (StreamWriter wr = new StreamWriter(file))
                {
                    xs.Serialize(wr, list);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
        }

        /// <summary>
        /// LoadRADOList
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="file"></param>
        /// <returns></returns>
        static public ObservableCollection<T> LoadOList<T>(string file)
        {
            return LToO<T>(LoadList<T>(file));
        }

        /// <summary>
        /// SaveRADOList
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="olist"></param>
        /// <param name="file"></param>
        static public void SaveOList<T>(ObservableCollection<T> olist, string file)
        {
            SaveList<T>(OToL<T>(olist), file);
        }

        /// <summary>
        /// OToL
        /// converting a ObservableCollection to List
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="olist"></param>
        /// <returns></returns>
        static public List<T> OToL<T>(ObservableCollection<T> olist)
        {
            List<T> l = new List<T>(olist);
            return l;
        }

        /// <summary>
        /// LToO
        /// converting a List to ObservableCollection
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="llist"></param>
        /// <returns></returns>
        static public ObservableCollection<T> LToO<T>(List<T> llist)
        {
            var oc = new ObservableCollection<T>();
            foreach (var item in llist)
                oc.Add(item);
            return oc;
        }

        /// <summary>
        /// HasWriteAccessToFolder
        /// </summary>
        /// <param name="folderPath"></param>
        /// <returns></returns>
        static public bool HasWriteAccessToFolder(string folderPath)
        {
            // For the moment, store the data in the user directory
            return false;
            try
            {
                System.Security.AccessControl.DirectorySecurity ds = Directory.GetAccessControl(folderPath);
                return true;
            }
            catch (UnauthorizedAccessException)
            {
                return false;
            }
        }
    }
}
