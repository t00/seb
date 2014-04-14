using System;
using System.Collections.Generic;
using SebWindowsServiceWCF.RegistryHandler;
using SebWindowsServiceWCF.ServiceContracts;

namespace SebWindowsServiceWCF.ServiceImplementations
{
    public class RegistryService : IRegistryServiceContract, IDisposable
    {
        //Simply returns true
        public bool TestServiceConnetcion()
        {
            return true;
        }

        /// <summary>
        /// Sets the registry values
        /// </summary>
        /// <param name="registryValues">The registry values to set</param>
        /// <param name="username">The username of the currently logged in user - needed to identify the correct registry key path</param>
        /// <returns>true if all operations succeeded, false if something went wrong. See the logfile for details then.</returns>
        public bool SetRegistryEntries(Dictionary<RegistryIdentifiers, object> registryValues, string username)
        {
            bool res = true;
            try
            {
                using (var persistentRegistryFile = new PersistentRegistryFile(username))
                {
                    foreach (var registryValue in registryValues)
                    {
                        RegistryEntry regEntry;
                        
                        //If the class could not be instantiated it means either reflection did not work properly or the registry-class does not exists
                        //don't interrupt the whole process but set the return value to false to indicate a possible error
                        try
                        {
                            //Use Reflection
                            var type = Type.GetType(String.Format("SebWindowsServiceWCF.RegistryHandler.Reg{0}", registryValue.Key));
                            if (type == null) continue;
                            regEntry = (RegistryEntry)Activator.CreateInstance(type, SIDHandler.GetSIDFromUsername(username));
                        }
                        catch (Exception ex)
                        {
                            Logger.Log(ex, String.Format("Unable to instantiate registryclass: {0}", registryValue.Key));
                            res = false;
                            continue;
                        }

                        //If the registry value could not have been set correctly or something went wrong with the persistent registry file
                        //don't interrupt the whole process but set the return value to false to indicate a possible error
                        //but never change a registry key without successfully write the persistent registry file
                        try
                        {
                            //If there is nothing to change, then do not change anything
                            if (registryValue.Value == regEntry.DataValue) continue;

                            //Only store the entry in the persistent file if not already existing
                            if (!persistentRegistryFile.RegistryValues.ContainsKey(registryValue.Key))
                            {
                                persistentRegistryFile.RegistryValues.Add(registryValue.Key, regEntry.DataValue);
                                //Save after every change
                                persistentRegistryFile.Save();
                            }
                            //Change the registry value if all operations succeeded until here
                            regEntry.DataValue = registryValue.Value;
                        }
                        catch (Exception ex)
                        {
                            Logger.Log(ex, String.Format("Unable to set the registry value for {0}", registryValue.Key));
                            res = false;
                            continue;
                        }
                    }
                }
            }
            catch (Exception)
            {
                res = false;
            }
            
            return res;
        }

        public bool ResetRegistry()
        {
            bool res = true;
            try
            {
                using (var persistentRegistryFile = new PersistentRegistryFile())
                {
                    res = this.SetRegistryEntries(persistentRegistryFile.RegistryValues, persistentRegistryFile.Username);
                    persistentRegistryFile.Delete();
                }
            }
            catch (Exception)
            {
                res = false;
            }
            return res;
        }

        public void Dispose()
        {
        }
    }
}
