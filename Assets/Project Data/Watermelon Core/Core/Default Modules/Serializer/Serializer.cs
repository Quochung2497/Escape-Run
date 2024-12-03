using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Serialization;
using UnityEngine;

// Serializer module v 1.0.1
namespace Watermelon
{
    /// <summary>
    /// Provides different types of serrialization.
    /// </summary>
    public static class Serializer
    {
        // TODO: add check if T is Serializable

        /// <summary>
        /// Returns persistent data path used to accses files.
        /// </summary>
        public static readonly string persistentDataPath = Application.persistentDataPath + "/";

        public enum SerializeType
        {
            Binary,
            Xml,
            Json,
        }

        // ******************************************************************************************** //
#region DeserializeMethods

        /// <summary>
        /// Deserializes file located at Persistent Data Path.
        /// </summary>
        /// <param name="fileName">Name of input file.</param>
        /// <param name="serializationType">Type of serrialization.</param>
        /// <returns>Deserialized object if file exists or new instance if doesn't.</returns>
        public static T DeserializeFromPDP<T>(string fileName, SerializeType serializationType = SerializeType.Binary, string secureKey = "", bool logIfFileNotExists = true) where T : new()
        {
            switch (serializationType)
            {
                case SerializeType.Binary:
                    {
                        return BinaryDeserializeFromPath<T>(persistentDataPath + fileName, logIfFileNotExists);
                    }
                case SerializeType.Xml:
                    {
                        return XmlDeserializeFromPath<T>(persistentDataPath + fileName, logIfFileNotExists);
                    }
#if SERIALIZER_JSON
                case SerializeType.Json:
                    {
                        return JsonDeserializeFromPath<T>(persistentDataPath + fileName, secureKey, logIfFileNotExists);
                    }
#endif
                default:
                    {
                        Debug.LogError("Serialization type is not found.");
                        return BinaryDeserializeFromPath<T>(persistentDataPath + fileName, logIfFileNotExists);
                    }
            }
        }

        /// <summary>
        /// Deserializes file located at Resources folder.
        /// </summary>
        /// <param name="fileName">Name of input file.</param>
        /// <param name="serializationType">Type of serrialization.</param>
        /// <returns>Deserialized object if file exists or new instance if doesn't.</returns>
        public static T DeserializeFromResourses<T>(string fileName, SerializeType serializationType = SerializeType.Binary) where T : new()
        {
            switch (serializationType)
            {
                case SerializeType.Binary:
                    {
                        return BinaryDeserializeFromResourses<T>(fileName);
                    }
                case SerializeType.Xml:
                    {
                        return XmlDeserializeFromResourses<T>(fileName);
                    }
                case SerializeType.Json:
                    {
                        return JsonDeserializeFromResourses<T>(fileName);
                    }
                default:
                    {
                        Debug.LogError("Serialization type is not found.");
                        return BinaryDeserializeFromResourses<T>(fileName);
                    }
            }
        }

        /// <summary>
        /// Deserializes file located at spesified directory.
        /// </summary>
        /// <param name="fileName">Name of input file.</param>
        /// <param name="directoryPath">Full path to directory. Ends with directory name (without "/").</param>
        /// <param name="serializationType">Type of serrialization.</param>
        /// <returns>Deserialized object if file exists or new instance if doesn't.</returns>
        public static T DesirializeFromPath<T>(string directoryPath, string fileName, SerializeType serializationType = SerializeType.Binary) where T : new()
        {
            switch (serializationType)
            {
                case SerializeType.Binary:
                    {
                        return BinaryDeserializeFromPath<T>(directoryPath + "/" + fileName);
                    }
                case SerializeType.Xml:
                    {
                        return XmlDeserializeFromPath<T>(directoryPath + "/" + fileName);
                    }
                case SerializeType.Json:
                    {
                        return JsonDeserializeFromPath<T>(directoryPath + "/" + fileName);
                    }
                default:
                    {
                        Debug.LogError("Serialization type is not found.");
                        return BinaryDeserializeFromPath<T>(directoryPath + "/" + fileName);
                    }
            }
        }


        // ///////////////////////////////////////////////////////////////////////////////////////////////
        // Binary deserialization methods.

        /// <summary>
        /// Provides binary deserialization of file located at specified loaction.
        /// </summary>
        /// <param name="absolutePath">Absolute path to file(including file name and extention.</param>
        /// <returns>Deserialized object if file exists or new instance if doesn't.</returns>
        public static T BinaryDeserializeFromPath<T>(string absolutePath, bool logIfFileNotExists = true) where T : new()
        {
            if (FileExistsAtPath(absolutePath))
            {
                BinaryFormatter bf = new BinaryFormatter();
                FileStream file = File.Open(absolutePath, FileMode.Open);

                try
                {
                    T deserializedObject = (T)bf.Deserialize(file);

                    return deserializedObject;
                }
                catch (Exception ex)
                {
                    Debug.LogError(ex.Message);
                    return new T();
                }
                finally
                {
                    file.Close();
                }
            }
            else
            {
                if (logIfFileNotExists)
                {
                    Debug.LogWarning("File at path : \"" + absolutePath + "\" does not exist.");
                }
                return new T();
            }
        }

        /// <summary>
        /// Provides binary deserialization of file located at Resources directory.
        /// </summary>
        /// <param name="fileName">Name of file to deserialize.</param>
        /// <returns>Deserialized object if file exists or new instance if doesn't.</returns>
        public static T BinaryDeserializeFromResourses<T>(string fileName, bool logIfFileNotExists = true) where T : new()
        {
            TextAsset text = Resources.Load(fileName) as TextAsset;

            if (text != null)
            {
                Stream stream = new MemoryStream(text.bytes);
                BinaryFormatter bf = new BinaryFormatter();

                try
                {
                    T deserializedObject = (T)bf.Deserialize(stream);
                    return deserializedObject;
                }
                catch (Exception ex)
                {
                    Debug.LogError(ex.Message);
                    return new T();
                }
            }
            else
            {
                if (logIfFileNotExists)
                {
                    Debug.LogError("File : \"" + fileName + "\" does not exist at Resources folder. Maybe file located at subsidiary folder.");
                }
                return new T();
            }
        }

        // ///////////////////////////////////////////////////////////////////////////////////////////////
        // Xml deserialization methods.


        /// <summary>
        /// Provides xml deserialization of file located at specified loaction.
        /// </summary>
        /// <param name="absolutePath">Absolute path to file(including file name and extention.</param>
        /// <returns>Deserialized object if file exists or new instance if doesn't.</returns>
        public static T XmlDeserializeFromPath<T>(string absolutePath, bool logIfFileNotExists = true) where T : new()
        {
            if (FileExistsAtPath(absolutePath))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                FileStream file = File.Open(absolutePath, FileMode.Open);
                T deserializedObject = (T)serializer.Deserialize(file);

                file.Close();

                return deserializedObject;
            }
            else
            {
                if (logIfFileNotExists)
                {
                    Debug.LogError("File at path : \"" + absolutePath + "\" does not exist.");
                }
                return new T();
            }
        }

        /// <summary>
        /// Provides xml deserialization of file located at Resources directory.
        /// </summary>
        /// <param name="fileName">Name of file to deserialize.</param>
        /// <returns>Deserialized object if file exists or new instance if doesn't.</returns>
        public static T XmlDeserializeFromResourses<T>(string fileName, bool logIfFileNotExists = true) where T : new()
        {
            TextAsset text = Resources.Load(fileName) as TextAsset;

            if (text != null)
            {
                Stream stream = new MemoryStream(text.bytes);
                XmlSerializer serializer = new XmlSerializer(typeof(T));

                return (T)serializer.Deserialize(stream);
            }
            else
            {
                if (logIfFileNotExists)
                {
                    Debug.LogError("File : \"" + fileName + "\" does not exist at Resources folder. Maybe file located at subsidiary folder or there is extention in the end of file name.");
                }
                return new T();
            }
        }

        // ///////////////////////////////////////////////////////////////////////////////////////////////
        // Json deserialization methods.

        /// <summary>
        /// Provides json deserialization of file located at specified loaction.
        /// </summary>
        /// <param name="absolutePath">Absolute path to file(including file name and extention.</param>
        /// <returns>Deserialized object if file exists or new instance if doesn't.</returns>
        public static T JsonDeserializeFromPath<T>(string absolutePath, string secureKey = "", bool logIfFileNotExists = true) where T : new()
        {
            if (FileExistsAtPath(absolutePath))
            {
                FileStream file = File.Open(absolutePath, FileMode.Open);

                using (StreamReader sr = new StreamReader(file))
                {
                    string jsonObject = sr.ReadToEnd();

                    if (!string.IsNullOrEmpty(secureKey))
                        jsonObject = Decrypt(jsonObject, secureKey);

                    try
                    {
                        T deserializedObject = JsonUtility.FromJson<T>(jsonObject);
                        return deserializedObject;
                    }
                    catch (Exception ex)
                    {
                        Debug.LogError(ex.Message);
                        return new T();
                    }
                    finally
                    {
                        file.Close();
                    }
                }
            }
            else
            {
                if (logIfFileNotExists)
                {
                    Debug.LogError("File at path : \"" + absolutePath + "\" does not exist.");
                }
                return new T();
            }
        }

        /// <summary>
        /// Provides json deserialization of file located at Resources directory.
        /// </summary>
        /// <param name="fileName">Name of file to deserialize.</param>
        /// <returns>Deserialized object if file exists or new instance if doesn't.</returns>
        public static T JsonDeserializeFromResourses<T>(string fileName, bool logIfFileNotExists = true) where T : new()
        {
            TextAsset text = Resources.Load(fileName) as TextAsset;

            if (text != null)
            {
                Stream stream = new MemoryStream(text.bytes);
                StreamReader sr = new StreamReader(stream);

                string jsonObject = sr.ReadToEnd();
                sr.Close();

                try
                {
                    T deserializedObject = JsonUtility.FromJson<T>(jsonObject);
                    return deserializedObject;
                }
                catch (Exception ex)
                {
                    Debug.LogError(ex.Message);
                    return new T();
                }
            }
            else
            {
                if (logIfFileNotExists)
                {
                    Debug.LogError("File : \"" + fileName + "\" does not exist at Resources folder. Maybe file located at subsidiary folder or there is extention in the end of file name.");
                }
                return new T();
            }
        }
#endregion

        // ******************************************************************************************** //
#region SerializeMethods

        // ///////////////////////////////////////////////////////////////////////////////////////////////
        // General public methods which define path and serialization type

        /// <summary>
        /// Serializes file to Persistent Data Path.
        /// </summary>
        /// <param name="serializationType">Type of serrialization.</param>
        /// <param name="fileName">Name of output file.</param>
        /// <param name="objectToSerialize">Reference to object that should be serialized.</param>
        public static void SerializeToPDP<T>(T objectToSerialize, string fileName, SerializeType serializationType = SerializeType.Binary, string secureKey = "")
        {
            switch (serializationType)
            {
                case SerializeType.Binary:
                    {
                        BinarySerializeToPath(objectToSerialize, persistentDataPath + fileName);
                        break;
                    }
                case SerializeType.Xml:
                    {
                        XmlSerializeToPath(objectToSerialize, persistentDataPath + fileName);
                        break;
                    }
                case SerializeType.Json:
                    {
                        JsonSerializeToPath(objectToSerialize, persistentDataPath + fileName, secureKey);
                        break;
                    }
                default:
                    {
                        Debug.LogError("Serialization type is not found.");
                        BinarySerializeToPath(objectToSerialize, persistentDataPath + fileName);
                        break;
                    }
            }
        }


        /// <summary>
        /// Serializes file to resources folder.
        /// </summary>
        /// <param name="serializationType">Type of serrialization.</param>
        /// <param name="fileName">Name of output file.</param>
        /// <param name="objectToSerialize">Reference to object that should be serialized.</param>
        public static void SerializeToResources<T>(T objectToSerialize, string fileName, SerializeType serializationType = SerializeType.Binary)
        {
            switch (serializationType)
            {
                case SerializeType.Binary:
                    {
                        BinarySeserializeToResourses(objectToSerialize, fileName);
                        break;
                    }
                case SerializeType.Xml:
                    {
                        XmlSeserializeToResourses(objectToSerialize, fileName);
                        break;
                    }
                case SerializeType.Json:
                    {
                        JsonSeserializeToResourses(objectToSerialize, fileName);
                        break;
                    }
                default:
                    {
                        Debug.LogError("Serialization type is not found.");
                        BinarySeserializeToResourses(objectToSerialize, fileName);
                        break;
                    }
            }
        }

        /// <summary>
        /// Serializes file to spesified directory.
        /// </summary>
        /// <param name="directoryPath">Full path to directory. Ends with directory name (without "/").</param>
        /// <param name="fileName">Name of output file.</param>
        /// <param name="objectToSerialize">Reference to object that should be serialized.</param>
        /// <param name="serializationType">Type of serrialization.</param>
        public static void SerializeToPath<T>(T objectToSerialize, string directoryPath, string fileName, SerializeType serializationType = SerializeType.Binary)
        {
            switch (serializationType)
            {
                case SerializeType.Binary:
                    {
                        BinarySerializeToPath(objectToSerialize, directoryPath + "/" + fileName);
                        break;
                    }
                case SerializeType.Xml:
                    {
                        XmlSerializeToPath(objectToSerialize, directoryPath + "/" + fileName);
                        break;
                    }
                case SerializeType.Json:
                    {
                        JsonSerializeToPath(objectToSerialize, directoryPath + "/" + fileName);
                        break;
                    }
                default:
                    {
                        Debug.LogError("Serialization type is not found.");
                        BinarySerializeToPath(objectToSerialize, directoryPath + "/" + fileName);
                        break;
                    }
            }
        }

        // ///////////////////////////////////////////////////////////////////////////////////////////////
        // Binary serialization methods.
        // ///////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Provides Binary serialization to file at specified absolute path.
        /// </summary>
        /// <param name="objectToSerialize">Reference to object that should be serialized.</param>
        /// <param name="absolutePath">Absolute path to file(including file name and extention.</param>
        public static void BinarySerializeToPath<T>(T objectToSerialize, string absolutePath)
        {
            BinaryFormatter bf = new BinaryFormatter();
            using (FileStream file = File.Open(absolutePath, FileMode.Create))
            {
                bf.Serialize(file, objectToSerialize);
            }
        }

        /// <summary>
        /// Provides binary deserialization of file located at Resources directory.
        /// </summary>
        /// <param name="objectToSerialize">Reference to object that should be serialized.</param>
        /// <param name="fileName">Name of output file without extention.</param>
        public static void BinarySeserializeToResourses<T>(T objectToSerialize, string fileName)
        {
            fileName += ".bytes";

            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.dataPath + "/Resources/" + fileName, FileMode.Create);

            bf.Serialize(file, objectToSerialize);
            file.Close();

#if UNITY_EDITOR
            UnityEditor.AssetDatabase.Refresh();
#endif
        }

        // ///////////////////////////////////////////////////////////////////////////////////////////////
        // Xml serialization methods.
        // ///////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Provides Binary serialization to file at specified absolute path.
        /// </summary>
        /// <param name="objectToSerialize">Reference to object that should be serialized.</param>
        /// <param name="absolutePath">Absolute path to file(including file name and extention.</param>
        public static void XmlSerializeToPath<T>(T objectToSerialize, string absolutePath)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            FileStream stream = new FileStream(absolutePath, FileMode.Create);
            serializer.Serialize(stream, objectToSerialize);
            stream.Close();
        }

        /// <summary>
        /// Provides binary deserialization of file located at Resources directory.
        /// </summary>
        /// <param name="objectToSerialize">Reference to object that should be serialized.</param>
        /// <param name="fileName">Name of output file without extention.</param>
        public static void XmlSeserializeToResourses<T>(T objectToSerialize, string fileName)
        {
            fileName += ".xml";

            XmlSerializer serializer = new XmlSerializer(typeof(T));
            FileStream file = File.Open(Application.dataPath + "/Resources/" + fileName, FileMode.Create);

            serializer.Serialize(file, objectToSerialize);
            file.Close();

#if UNITY_EDITOR
            UnityEditor.AssetDatabase.Refresh();
#endif
        }
        // ///////////////////////////////////////////////////////////////////////////////////////////////
        // Json serialization methods.
        // ///////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Provides Json serialization to file at specified absolute path.
        /// </summary>
        /// <param name="objectToSerialize">Reference to object that should be serialized.</param>
        /// <param name="absolutePath">Absolute path to file(including file name and extention.</param>
        public static void JsonSerializeToPath<T>(T objectToSerialize, string absolutePath, string secureKey = "")
        {
            string jsonObject = JsonUtility.ToJson(objectToSerialize);
            //string jsonObject = JsonConvert.SerializeObject(objectToSerialize);

            if (!string.IsNullOrEmpty(secureKey))
                jsonObject = Encrypt(jsonObject, secureKey);

            FileStream stream = File.Open(absolutePath, FileMode.Create);
            StreamWriter sw = new StreamWriter(stream);
            sw.Write(jsonObject);
            sw.Flush();

            sw.Close();
            stream.Close();
        }

        /// <summary>
        /// Provides Json serialization of file located at Resources directory.
        /// </summary>
        /// <param name="objectToSerialize">Reference to object that should be serialized.</param>
        /// <param name="fileName">Name of output file without extention.</param>
        public static void JsonSeserializeToResourses<T>(T objectToSerialize, string fileName)
        {
            fileName += ".json";

            string jsonObject = JsonUtility.ToJson(objectToSerialize);
            //string jsonObject = JsonConvert.SerializeObject(objectToSerialize);

            FileStream file = File.Open(Application.dataPath + "/Resources/" + fileName, FileMode.Create);
            StreamWriter sw = new StreamWriter(file);
            sw.Write(jsonObject);

            sw.Close();
            file.Close();

#if UNITY_EDITOR
            UnityEditor.AssetDatabase.Refresh();
#endif
        }
#endregion

        // ******************************************************************************************** //
#region OtherMethods

        /// <summary>
        /// Checks if file exists at Persistent Data Path.
        /// </summary>
        /// <param name="fileName">Name of file to check.</param>
        /// <returns>True if file exists ans false otherwise.</returns>
        public static bool FileExistsAtPDP(string fileName)
        {
            return File.Exists(Application.persistentDataPath + "/" + fileName);
        }

        /// <summary>
        /// Checks if file exists at Persistent Data Path.
        /// </summary>
        /// <param name="absolutePath">Absolute path to file(including file name and extention.</param>
        /// <returns>True if file exists ans false otherwise.</returns>
        public static bool FileExistsAtPath(string absolutePath)
        {
            return File.Exists(absolutePath);
        }

        /// <summary>
        /// Checks if file exists add specified directory.
        /// </summary>
        /// <param name="directoryPath">Full path to directory. Ends with directory name (without "/").</param>
        /// <param name="fileName">Name of file to check.</param>
        /// <returns>True if file exists and false otherwise.</returns>
        public static bool FileExistsAtPath(string directoryPath, string fileName)
        {
            return File.Exists(directoryPath + "/" + fileName);
        }

        /// <summary>
        /// Checks if file exists at Resourses folder.
        /// </summary>
        /// <param name="fileName">Name of file to check.</param>
        /// <returns>True if file exists ans false otherwise.</returns>
        public static bool FileExistsAtResources(string fileName)
        {
            TextAsset text = Resources.Load(fileName) as TextAsset;

            if (text != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static string LoadTextFileAtPath(string absolutePath)
        {
            return File.ReadAllText(absolutePath);
        }


        /// <summary>
        /// Delete file at Persistent Data Path.
        /// </summary>
        /// <param name="fileName">Name of file to check.</param>
        public static void DeleteFileAtPDP(string fileName)
        {
            File.Delete(persistentDataPath + fileName);
        }

        /// <summary>
        /// Delete file at specified path.
        /// </summary>
        /// <param name="absolutePath">Absolute path to file(including file name and extention.</param>
        public static void DeleteFileAtPath(string absolutePath)
        {
            File.Delete(absolutePath);
        }

        /// <summary>
        /// Delete file at specified path.
        /// </summary>
        /// <param name="fileName">Name of file to check.</param>
        /// <param name="directoryPath">Full path to directory. Ends with directory name (without "/").</param>
        public static void DeleteFileAtPath(string directoryPath, string fileName)
        {
            File.Delete(directoryPath + "/" + fileName);
        }

        /// <summary>
        /// Delete file at Resourses folder.
        /// </summary>
        /// <param name="fileName">Name of file to check.</param>
        public static void DeleteFileResourses(string fileName)
        {
            File.Delete(Application.dataPath + "/Resources/" + fileName);
        }

        /// <summary>
        /// Prints some tips about using of Serializer.
        /// </summary>
        public static void PrintHelp()
        {
            Debug.Log("\t\tSERIALIZER HELP:\n" +
                "\tBinary Serialization:\n" +
                "Class should be marked as Serializable\n\n" +
                "\tXML Serialization:\n" +
                "For better formating you should use XmlAtributes above classes, lists and fields. More info: http://wiki.unity3d.com/index.php?title=Saving_and_Loading_Data:_XmlSerializer \n\n" +
                "\tJson Serialization:\n" +
                "Class should be marked as Serializable\n");
        }


        public static T CreateDeepCopy<T>(T obj)
        {
            using (var ms = new MemoryStream())
            {
                IFormatter formatter = new BinaryFormatter();
                formatter.Serialize(ms, obj);
                ms.Seek(0, SeekOrigin.Begin);
                return (T)formatter.Deserialize(ms);
            }
        }

        #endregion

        #region Encrypt 
        public static string Encrypt(string clearText, string EncryptionKey)
        {
            byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    clearText = Convert.ToBase64String(ms.ToArray());
                }
            }
            return clearText;
        }

        public static string Decrypt(string cipherText, string EncryptionKey)
        {
            cipherText = cipherText.Replace(" ", "+");
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    cipherText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return cipherText;
        }
#endregion
    }
}

// Log
// v 1.0.0 
// Basic serri

// v 1.0.1 
// Added logIfFileNotExists parameter which helps hide FileNotExist error if needed
