using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using SebWindowsServiceWCF.ServiceContracts;

namespace SebWindowsServiceWCF.ServiceImplementations
{
    public class PersistentRegistryFile : IDisposable
    {
        private readonly string _filePath;

        public Dictionary<RegistryIdentifiers, object> RegistryValues = new Dictionary<RegistryIdentifiers, object>();
        public string Username = "";

        /// <summary>
        /// Create an in-memory instance of a persistent registry file.
        /// If a file is existing it gets automatically loaded into memory
        /// </summary>
        /// <param name="username">The username of the currently logged in user - needed to identify the correct registry key path</param>
        public PersistentRegistryFile(string username = null)
        {
            try
            {
                //The file is stored where the executable of the service is
                _filePath = String.Format(@"{0}\sebregistry.srg", Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));
            }
            catch (Exception ex)
            {
                Logger.Log(ex,"Unable to build path for persistent registry file");
                throw;
            }
            

            if (username != null)
                this.Username = username;

            if (File.Exists(_filePath))
            {
                Load();
            }
        }

        /// <summary>
        /// Loads the content of a saved file into memory
        /// Throws Exception if something goes wrong
        /// </summary>
        private void Load()
        {
            FileStream stream = null;
            try
            {
                stream = File.OpenRead(_filePath);
                var deserializer = new BinaryFormatter();
                var fileContent = (Tuple<string, Dictionary<RegistryIdentifiers, object>>)deserializer.Deserialize(stream);
                this.Username = fileContent.Item1;
                this.RegistryValues = fileContent.Item2;
                stream.Close();
            }
            catch (Exception ex)
            {
                if (stream != null)
                    stream.Close();
                Logger.Log(ex, String.Format("Unable to open persistent registry file:{0}",_filePath));
                throw;
            }
        }

        /// <summary>
        /// Saves the currently stored registry information into a binary encoded file
        /// Throws Exception if something goes wrong
        /// </summary>
        public void Save()
        {
            FileStream stream = null;
            try
            {
                Delete();
                stream = File.OpenWrite(_filePath);
                var serializer = new BinaryFormatter();
                serializer.Serialize(stream, Tuple.Create(this.Username, this.RegistryValues));
                stream.Close();
            }
            catch (Exception ex)
            {
                if(stream != null)
                    stream.Close();
                Logger.Log(ex, String.Format("Unable to save persistent registry file: {0}",_filePath));
                throw;
            }
        }

        /// <summary>
        /// Delete the persistens registry file if it exists
        /// Throws Exception if something goes wrong
        /// </summary>
        public void Delete()
        {
            try
            {
                if (File.Exists(_filePath))
                    File.Delete(_filePath);
            }
            catch (Exception ex)
            {
                Logger.Log(ex, String.Format("Unable to delete persistent registry reset file: {0}",_filePath));
                throw ex;
            }
        }

        public void Dispose()
        {
            this.Username = null;
            this.RegistryValues.Clear();
            this.RegistryValues = null;
        }
    }
}
