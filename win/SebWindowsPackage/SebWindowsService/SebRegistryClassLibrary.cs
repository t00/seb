using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Win32;
using System.Security.Principal;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.ComponentModel;
using System.ServiceProcess;



namespace SebWindowsService
{
    public partial class SebWindowsService : ServiceBase
    {

        // Constants for socket communication

        // Since the longest possible message is
        // "EnableVmWareClientShade=1---SEB---",
        // a buffer size of 50 characters per message would be sufficient.

        // But the message containing the user SID can be longer.
        // Currently, we expect up to 100 characters.

        // However, if the SEB client sends the message strings
        // too fast, with a too short time interval in between,
        // it can happen that SEVERAL messages are received in ONE string.
        // The buffer should thus be large enough to store up to 5 messages,
        // that means about 500 characters.

        // By the way, this "message clumping" problem can be avoided
        // if the SEB client waits about 100 milliseconds (1/10 second)
        // after sending each message string.

        const int BUF_LEN = 512;
        const int MSG_LEN = 100;

        const String ipAddressOfLocalHost = "127.0.0.1";
        const String endOfStringKeyWord   = "---SEB---";

        const int SET_Def  = 1;
        const int SET_Old  = 2;
        const int SET_New  = 3;
        const int SET_Allow  = 4;
        const int SET_Forbid = 5;

        const int EDIT_Get     = 1;
        const int EDIT_Set     = 2;
        const int EDIT_Restore = 3;

        // Constants for indexing the ini file settings

        const int IND_EnableSwitchUser        = 0;
        const int IND_EnableLockThisComputer  = 1;
        const int IND_EnableChangeAPassword   = 2;
        const int IND_EnableStartTaskManager  = 3;
        const int IND_EnableLogOff            = 4;
        const int IND_EnableShutDown          = 5;
        const int IND_EnableEaseOfAccess      = 6;
        const int IND_EnableVmWareClientShade = 7;

        const int IND_RegistrySettingNone = -1;
        const int IND_RegistrySettingMin  =  0;
        const int IND_RegistrySettingMax  =  7;
        const int IND_RegistrySettingNum  =  8;

        // Constants for registry keys and values

        const String HIVE_HKCU = "HKEY_CURRENT_USER";
        const String HIVE_HKLM = "HKEY_LOCAL_MACHINE";

      //const String KEY_PoliciesSystem = @"Software\Microsoft\Windows\CurrentVersion\Policies\System";
      //const String KEY_ProfileList    = @"SOFTWARE\Microsoft\Windows NT\CurrentVersion\ProfileList";

        const String KEY_ProfileList      = "SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion\\ProfileList";
        const String KEY_PoliciesSystem   = "Software\\Microsoft\\Windows\\CurrentVersion\\Policies\\System";
        const String KEY_PoliciesExplorer = "Software\\Microsoft\\Windows\\CurrentVersion\\Policies\\Explorer";
        const String KEY_PoliciesSEB      = "Software\\Policies\\SEB";
        const String KEY_UtilmanExe       = "Software\\Microsoft\\Windows NT\\CurrentVersion\\Image File Execution Options\\Utilman.exe";
        const String KEY_VmWareClient     = "Software\\VMware, Inc.\\VMware VDM\\Client";

        const String VAL_HideFastUserSwitching  = "HideFastUserSwitching";
        const String VAL_DisableLockWorkstation = "DisableLockWorkstation";
        const String VAL_DisableChangePassword  = "DisableChangePassword";
        const String VAL_DisableTaskMgr         = "DisableTaskMgr";
        const String VAL_NoLogoff               = "NoLogoff";
        const String VAL_NoClose                = "NoClose";
        const String VAL_EnableEaseOfAccess     = "Debugger";
        const String VAL_EnableShade            = "EnableShade";

        const String MSG_EnableSwitchUser        = "EnableSwitchUser";
        const String MSG_EnableLockThisComputer  = "EnableLockThisComputer";
        const String MSG_EnableChangeAPassword   = "EnableChangeAPassword";
        const String MSG_EnableStartTaskManager  = "EnableStartTaskManager";
        const String MSG_EnableLogOff            = "EnableLogOff";
        const String MSG_EnableShutDown          = "EnableShutDown";
        const String MSG_EnableEaseOfAccess      = "EnableEaseOfAccess";
        const String MSG_EnableVmWareClientShade = "EnableVmWareClientShade";

        const String TYPE_EnableSwitchUser        = "REG_DWORD";
        const String TYPE_EnableLockThisComputer  = "REG_DWORD";
        const String TYPE_EnableChangeAPassword   = "REG_DWORD";
        const String TYPE_EnableStartTaskManager  = "REG_DWORD";
        const String TYPE_EnableLogOff            = "REG_DWORD";
        const String TYPE_EnableShutDown          = "REG_DWORD";
        const String TYPE_EnableEaseOfAccess      = "REG_SZ";
        const String TYPE_EnableVmWareClientShade = "REG_DWORD";

        const String DATA_EnableEaseOfAccessTrue  = "";
        const String DATA_EnableEaseOfAccessFalse = "SebDummy.exe";

        // Clicking on "Ease Of Access" in the Windows logon screen
        // launches the "Utilman.exe" application.
        // For disabling Ease Of Access, a dummy application
        // must replace this "Utilman.exe" application.
        // The dummy executable name is stored in the Windows Registry:
        //
        // hive  = "HKEY_LOCAL_MACHINE"
        // key   = "Software\\Microsoft\\Windows NT\\CurrentVersion\\Image File Execution Options\\Utilman.exe"
        // value = "Debugger"
        // type  =  REG_SZ   (string)
        // data  = "SebDummy.exe".



        // Global variables

        // Names of registry domains, keys, values, types
        static String[] hiveString = new String[IND_RegistrySettingNum + 1];
        static String[]  keyString = new String[IND_RegistrySettingNum + 1];
        static String[]  valString = new String[IND_RegistrySettingNum + 1];
        static String[]  msgString = new String[IND_RegistrySettingNum + 1];
        static String[] typeString = new String[IND_RegistrySettingNum + 1];

        // Registry settings as integers (0 or 1)
        static    int[] defSetting = new int[IND_RegistrySettingNum + 1];
        static    int[] oldSetting = new int[IND_RegistrySettingNum + 1];
        static    int[] newSetting = new int[IND_RegistrySettingNum + 1];

        static int[]  allowSetting = new int[IND_RegistrySettingNum + 1];
        static int[] forbidSetting = new int[IND_RegistrySettingNum + 1];


        // Socket communication
        static IPAddress          ipAddress;
        static TcpListener    serverSocket;
        static TcpClient      clientSocket;
        static NetworkStream networkStream;

        static byte[] sendBuffer = new byte[BUF_LEN];
        static byte[] recvBuffer = new byte[BUF_LEN];


        // readTimeout is the timespan to wait between AcceptTcpClient() and Read().
        // portNumber  is a dynamic/private port between 49152 and 65535.
        // hostName    is "localhost", since server and client run on same machine.

        // readTimeout  = 200 ms here on the SEB server side, which is twice the
        // sendInterval = 100 ms for messages on the SEB client side.

        static Boolean debugMode     = true;
        static int     readTimeout   = 200;
        static int     portNumber    = 57016;
        static String  hostName      = "localhost";
        static String  userName      = "";
        static String  userSid       = "";
        static String  registryFlags = "";


        // Setting the location of the logfile
/*
        static String  commandLine   = "";
        static String  commandPath   = "";
        static int     commandSlash  = -1;
        static String  commandDir    = "";
        static String  commandFile   = "";
        static int     commandPeriod = -1;
        static String  commandPrefix = "";
        static String  commandSuffix = "";
        static String  logDir        = "";
        static String  logFile       = "LogFile.txt";
        static String  logPath       = "";
*/

        // Example for setting the logfile path
        // (in the same directory as the .exe file):

        // commandLine   = "C:\Users\John\ConAppRegistryMitSockets\ConAppRegistry1\bin\Debug\ConAppRegistry1.exe" 
        // commandPath   =  C:\Users\John\ConAppRegistryMitSockets\ConAppRegistry1\bin\Debug\ConAppRegistry1.exe 
        // commandDir    =  C:\Users\John\ConAppRegistryMitSockets\ConAppRegistry1\bin\Debug\
        //
        // commandFile   =  ConAppRegistry1.exe
        // commandPrefix =  ConAppRegistry1.
        // commandSuffix =  exe
        //
        // logDir        =  C:\Users\John\ConAppRegistryMitSockets\ConAppRegistry1\bin\Debug\
        // logFile       =  ConAppRegistry1.logfile.txt
        // logPath       =  C:\Users\John\ConAppRegistryMitSockets\ConAppRegistry1\bin\Debug\ConAppRegistry1.logfile.txt

        static String commandLine  =  System.Environment.CommandLine;
        static String commandPath  =  commandLine.Replace("\"", "");

        static int    commandSlash =  commandPath.LastIndexOf("\\");
        static String commandDir   =  commandPath.Remove   (commandSlash + 1);
        static String commandFile  =  commandPath.Substring(commandSlash + 1);

        static int    commandPeriod = commandFile.LastIndexOf(".");
        static String commandPrefix = commandFile.Remove   (commandPeriod + 1);
        static String commandSuffix = commandFile.Substring(commandPeriod + 1);

        static String logDir       =  commandDir;
        static String logFile      =  commandPrefix + "logfile.txt";
        static String logPath      =  logDir        +  logFile;



        // *******************************
        // Debug output without line break
        // *******************************
        public static void DebugOutput(Boolean debugMode, String outputString)
        {
            if (debugMode == false) return;
            //lock(this)
            {
                Console.Write(DateTime.Now);
                Console.Write("   ");
                Console.Write(outputString);
                //sebEventLog.WriteEntry(outputString);
                File.AppendAllText(logPath, DateTime.Now.ToString());
                File.AppendAllText(logPath, "   ");
                File.AppendAllText(logPath, outputString);
            }
            return;
        }


        // ****************************
        // Debug output with line break
        // ****************************
        public static void DebugOutputLine(Boolean debugMode, String outputString)
        {
            if (debugMode == false) return;
            //lock (this)
            {
                DebugOutput(debugMode, outputString);
                Console.WriteLine();
                File.AppendAllText(logPath, System.Environment.NewLine);
            }
            return;
        }





        // **********************
        // SEB socket server loop
        // **********************
        public void SebSocketServerLoop()
        {
            // If an older logfile exists, delete it
            File.Delete(logPath);

            DebugOutputLine(debugMode, "");
            DebugOutputLine(debugMode, "");
            DebugOutputLine(debugMode, "");
            DebugOutputLine(debugMode, "CommandLine           = " + System.Environment.CommandLine);
            DebugOutputLine(debugMode, "CurrentDirectory      = " + System.Environment.CurrentDirectory);
            DebugOutputLine(debugMode, "Programs              = " + System.Environment.GetFolderPath(Environment.SpecialFolder.Programs));
            DebugOutputLine(debugMode, "ProgramFiles          = " + System.Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles));
            DebugOutputLine(debugMode, "CommonProgramFiles    = " + System.Environment.GetFolderPath(Environment.SpecialFolder.CommonProgramFiles));
            DebugOutputLine(debugMode, "ApplicationData       = " + System.Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData));
            DebugOutputLine(debugMode, "CommonApplicationData = " + System.Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData));
            DebugOutputLine(debugMode, "");
            DebugOutputLine(debugMode, "");
            DebugOutputLine(debugMode, "");
            DebugOutputLine(debugMode, "commandLine   = " + commandLine);
            DebugOutputLine(debugMode, "commandPath   = " + commandPath);
            DebugOutputLine(debugMode, "commandDir    = " + commandDir );
            DebugOutputLine(debugMode, "");
            DebugOutputLine(debugMode, "commandFile   = " + commandFile);
            DebugOutputLine(debugMode, "commandPrefix = " + commandPrefix);
            DebugOutputLine(debugMode, "commandSuffix = " + commandSuffix);
            DebugOutputLine(debugMode, "");
            DebugOutputLine(debugMode, "logDir        = " + logDir );
            DebugOutputLine(debugMode, "logFile       = " + logFile);
            DebugOutputLine(debugMode, "logPath       = " + logPath);
            DebugOutputLine(debugMode, "");
            DebugOutputLine(debugMode, "");
            DebugOutputLine(debugMode, "");

            // Initialise the global arrays

            int  index;
            for (index = IND_RegistrySettingMin; index <= IND_RegistrySettingMax; index++)
            {
                   oldSetting[index] = 0;
                   newSetting[index] = 0;
                   defSetting[index] = 0;
                 allowSetting[index] = 1;
                forbidSetting[index] = 0;
            }

            hiveString[IND_EnableSwitchUser       ] = HIVE_HKLM;
            hiveString[IND_EnableLockThisComputer ] = HIVE_HKCU;
            hiveString[IND_EnableChangeAPassword  ] = HIVE_HKCU;
            hiveString[IND_EnableStartTaskManager ] = HIVE_HKCU;
            hiveString[IND_EnableLogOff           ] = HIVE_HKCU;
            hiveString[IND_EnableShutDown         ] = HIVE_HKCU;
            hiveString[IND_EnableEaseOfAccess     ] = HIVE_HKLM;
            hiveString[IND_EnableVmWareClientShade] = HIVE_HKCU;

            keyString[IND_EnableSwitchUser       ] = KEY_PoliciesSystem;
            keyString[IND_EnableLockThisComputer ] = KEY_PoliciesSystem;
            keyString[IND_EnableChangeAPassword  ] = KEY_PoliciesSystem;
            keyString[IND_EnableStartTaskManager ] = KEY_PoliciesSystem;
            keyString[IND_EnableLogOff           ] = KEY_PoliciesExplorer;
            keyString[IND_EnableShutDown         ] = KEY_PoliciesExplorer;
            keyString[IND_EnableEaseOfAccess     ] = KEY_UtilmanExe;
            keyString[IND_EnableVmWareClientShade] = KEY_VmWareClient;

            valString[IND_EnableSwitchUser       ] = VAL_HideFastUserSwitching;
            valString[IND_EnableLockThisComputer ] = VAL_DisableLockWorkstation;
            valString[IND_EnableChangeAPassword  ] = VAL_DisableChangePassword;
            valString[IND_EnableStartTaskManager ] = VAL_DisableTaskMgr;
            valString[IND_EnableLogOff           ] = VAL_NoLogoff;
            valString[IND_EnableShutDown         ] = VAL_NoClose;
            valString[IND_EnableEaseOfAccess     ] = VAL_EnableEaseOfAccess;
            valString[IND_EnableVmWareClientShade] = VAL_EnableShade;

            msgString[IND_EnableSwitchUser       ] = MSG_EnableSwitchUser;
            msgString[IND_EnableLockThisComputer ] = MSG_EnableLockThisComputer;
            msgString[IND_EnableChangeAPassword  ] = MSG_EnableChangeAPassword;
            msgString[IND_EnableStartTaskManager ] = MSG_EnableStartTaskManager;
            msgString[IND_EnableLogOff           ] = MSG_EnableLogOff;
            msgString[IND_EnableShutDown         ] = MSG_EnableShutDown;
            msgString[IND_EnableEaseOfAccess     ] = MSG_EnableEaseOfAccess;
            msgString[IND_EnableVmWareClientShade] = MSG_EnableVmWareClientShade;

            typeString[IND_EnableSwitchUser       ] = TYPE_EnableSwitchUser;
            typeString[IND_EnableLockThisComputer ] = TYPE_EnableLockThisComputer;
            typeString[IND_EnableChangeAPassword  ] = TYPE_EnableChangeAPassword;
            typeString[IND_EnableStartTaskManager ] = TYPE_EnableStartTaskManager;
            typeString[IND_EnableLogOff           ] = TYPE_EnableLogOff;
            typeString[IND_EnableShutDown         ] = TYPE_EnableShutDown;
            typeString[IND_EnableEaseOfAccess     ] = TYPE_EnableEaseOfAccess;
            typeString[IND_EnableVmWareClientShade] = TYPE_EnableVmWareClientShade;

            // Debug output of initialised global arrays

            DebugOutputLine(debugMode, "Debug output of initialised global arrays");
            DebugOutputLine(debugMode, "");
            for (index = IND_RegistrySettingMin; index <= IND_RegistrySettingMax; index++)
            {
                DebugOutputLine(debugMode, index.ToString());
                DebugOutputLine(debugMode, hiveString[index]);
                DebugOutputLine(debugMode,  keyString[index]);
                DebugOutputLine(debugMode,  valString[index]);
                DebugOutputLine(debugMode,  msgString[index]);
                DebugOutputLine(debugMode, typeString[index]);
                DebugOutputLine(debugMode, "");
            }
            DebugOutputLine(debugMode, "");


            // Create socket connection between SEB socket server and SEB client

            ipAddress    = System.Net.IPAddress.Parse(ipAddressOfLocalHost);
            serverSocket =        new TcpListener(ipAddress, portNumber);
            clientSocket =    default(TcpClient);

            try
            {
                DebugOutputLine(debugMode, "");
                DebugOutputLine(debugMode, "Starting the SEB socket server...");
                serverSocket.Start();
            }
            catch (Exception e)
            {
                DebugOutputLine(debugMode, "serverSocket.Start() failed!");
                DebugOutputLine(debugMode, e.Message);
                DebugOutputLine(debugMode, "");
                DebugOutputLine(debugMode, "");
                DebugOutputLine(debugMode, "");
                return;
            }

            DebugOutputLine(debugMode, "");
            DebugOutputLine(debugMode, "");
            DebugOutputLine(debugMode, "");


            // Endless server loop, listening for clients

            while (true)
            {
                DebugOutputLine(debugMode, "SEB socket server listening for SEB client...");

                clientSocket  = serverSocket.AcceptTcpClient();
                networkStream = clientSocket.GetStream();

                // Wait a certain time after connection was established.
                // If no client message was received after this timeout,
                // close the clientSocket and create a new one.

                //clientSocket.ReceiveBufferSize = BUF_LEN;
                //clientSocket.ReceiveTimeout    = readTimeout;
                networkStream.ReadTimeout = readTimeout;

                DebugOutputLine(debugMode, "");
                DebugOutputLine(debugMode, "");
                DebugOutputLine(debugMode, "");
                DebugOutputLine(debugMode, "SEB socket server accepts connection from SEB client.");
                DebugOutputLine(debugMode, "   Timeout for receive messages is " + networkStream.ReadTimeout);
                DebugOutputLine(debugMode, "");
                DebugOutputLine(debugMode, "");
                DebugOutputLine(debugMode, "");


                // Connection established; now receive strings from SEB client

                int counter = 0;
                while (true)
                {
                    counter++;

                    try 
	                {
                        networkStream.Read(recvBuffer, 0, BUF_LEN);
                      //networkStream.Read(recvBuffer, 0, (int)clientSocket.ReceiveBufferSize);	
                        networkStream.Flush();
	                }
	                catch (Exception)
	                {
                      //DebugOutputLine(debugMode, e.Message);
                        DebugOutputLine(debugMode, "   Timeout for receive messages is " + networkStream.ReadTimeout);
                        DebugOutputLine(debugMode, "   networkStream.Read() timed out!");
                        DebugOutputLine(debugMode, "");
                        DebugOutputLine(debugMode, "");
                        networkStream.Flush();
                        break;
	                }

                    DebugOutputLine(debugMode, "Received from SEB client recvBuffer: " + recvBuffer);


                    // The received string contains data fields separated by "=" signs, e.g.
                    // "hostname=rembrandt".
                    // Assure that the received string terminates properly.
                    // Reason: sometimes there are unexplainable special characters.
                    // Solution: the client sends a special end-of-string keyword,
                    // in this case the keyword "---SEB---", at the end of the string.
                    // This way, we can enforce the proper termination of the string.

                    String clientStringRaw;
                    String clientStringCut;
                    int    clientStringPos;

                    clientStringRaw = Encoding.ASCII.GetString(recvBuffer);
                    clientStringRaw = clientStringRaw.Substring(0, MSG_LEN);

                    clientStringPos = clientStringRaw.IndexOf(endOfStringKeyWord);
                    DebugOutputLine(debugMode, "Received from SEB client string pos: " + clientStringPos);

                    if  (clientStringPos >= 0)
                         clientStringCut  = clientStringRaw.Substring(0, clientStringPos);
                    else clientStringCut  = "Invalid client message";

                    DebugOutputLine(debugMode, "Received from SEB client string raw: " + clientStringRaw);
                    DebugOutputLine(debugMode, "Received from SEB client string cut: " + clientStringCut);

                    // If no client message did not match the SEB protocol,
                    // close the clientSocket and create a new one.

                    if (clientStringPos < 0)
                    {
                        DebugOutputLine(debugMode, "Invalid client message");
                        DebugOutputLine(debugMode, "");
                        DebugOutputLine(debugMode, "");
                        break;
                    }


                    // Now assign the string tokens to appropriate data fields, e.g.
                    // the "leftSide=rightSide" format demands separating the string into two parts.

                    String  count;
			        String  leftSideString;
			        String rightSideString;

			        count = counter.ToString();

                    int leftSideStart  = 0;
                    int leftSideEnd    = clientStringCut.IndexOf("=");
                    int leftSideLength = leftSideEnd - leftSideStart;

                    int rightSideStart  = leftSideEnd + 1;
                  //int rightSideEnd    = clientString.IndexOf(endOfStringKeyWord);
                    int rightSideEnd    = clientStringCut.Length;
                    int rightSideLength = rightSideEnd - rightSideStart;

                     leftSideString =    clientStringCut.Substring( leftSideStart,  leftSideLength);
                    rightSideString =    clientStringCut.Substring(rightSideStart, rightSideLength);

                    String  newString  = rightSideString;
                    Boolean newBoolean = rightSideString.Equals("1");
                    int     newInteger = rightSideString.First() - '0';

                    if (rightSideString.Equals("1")) newInteger = 1;
                                                else newInteger = 0;

                    DebugOutputLine(debugMode, "Received from SEB client string raw: " + clientStringRaw);
                    DebugOutputLine(debugMode, "Received from SEB client string cut: " + clientStringCut);
                    debugMode = false;
                    DebugOutputLine(debugMode, "   clientString = ***" +    clientStringCut + "***");
                    DebugOutputLine(debugMode, " leftSideString = ***" +  leftSideString + "***");
                    DebugOutputLine(debugMode, "rightSideString = ***" + rightSideString + "***");
                    DebugOutputLine(debugMode, "newString  = ***" + newString  + "***");
                    DebugOutputLine(debugMode, "newBoolean = ***" + newBoolean + "***");
                    DebugOutputLine(debugMode, "newInteger = ***" + newInteger + "***");
                    DebugOutputLine(debugMode, "");
                    debugMode = true;


                    // Get hostname from client
                    // (actually not necessary, since it is "localhost" by default)
                    if (leftSideString.Equals("HostName"))
                    {
                        hostName = rightSideString;
                        DebugOutputLine(debugMode, "   Setting the machine to          : hostName=" + hostName);

                        // Response (acknowledgement) from server to client
                        SendServerAcknowledgement("HostName", hostName);
                        DebugOutputLine(debugMode, "Server sent acknowledgement to client");
                    }


                    // Get username from client
                    // (actually redundant, since the user SID is sent afterwards, too)
                    if (leftSideString.Equals("UserName"))
                    {
                        userName = rightSideString;
                      //userSid  = GetSidByUsername(userName);
                        DebugOutputLine(debugMode, "   Setting the user to             : userName=" + userName);
                      //DebugOutputLine(debugMode, "   Setting the SID  to             : userSid =" + userSid);

                        // Response (acknowledgement) from server to client
                        SendServerAcknowledgement("UserName", userName);
                        DebugOutputLine(debugMode, "Server sent acknowledgement to client");

                        // Now that he sent the first message, the SEB client is authenticated
                        // and occupies the socket until it shuts down.
                        // Therefore set the message receive timeout to Infinite.
                        // Should you ever choose another message than "UserName=..."
                        // as the client's first message, you will have to do this in the
                        // if-branch of this other message!
                        // In any case, you must do this directly after the first client message!
                        networkStream.ReadTimeout = System.Threading.Timeout.Infinite;
                        DebugOutputLine(debugMode, "   Setting the message receive timeout to Infinite");
                        DebugOutputLine(debugMode, "   Timeout for receive messages is " + networkStream.ReadTimeout);
                    }


                    // Determine security identifier (SID) of the user by the username
                    if (leftSideString.Equals("UserSid"))
                    {
                        userSid = rightSideString;
                        DebugOutputLine(debugMode, "   Setting the SID  to             : userSid=" + userSid);

                        // Response (acknowledgement) from server to client
                        SendServerAcknowledgement("UserSid", userSid);
                        DebugOutputLine(debugMode, "Server sent acknowledgement to client");

                        // Store the original registry settings (before SEB started)
                        DebugOutputLine(debugMode, "");
                        DebugOutputLine(debugMode, "");
                        DebugOutputLine(debugMode, "");
                        DebugOutputLine(debugMode, "");
                        DebugOutputLine(debugMode, "   Getting and storing original registry settings...");
                        EditAllRegistryValues(SET_Old, EDIT_Get);

                        // In case the setting transmission from SEB client is faulty,
                        // it could happen that e.g. the "EnableStartTaskManager=0" command
                        // does not reach the Seb Windows Service,
                        // and that the Task Manager is therefore not disabled
                        // although the SEB client wants it to be disabled!
                        // To prevent this undesirable behaviour from happening,
                        // set the SEB to the most restrictive values by default.
                        //
                        // In the rare case that the Task Manager shall be ENABLED,
                        // the "EnableStartTaskManager=1" command must be successfully received.
                        // Of course, a faulty transmission can happen also in this case,
                        // but most of the time the SEB client
                        // wants all settings to be as restrictive as possible,
                        // so the risk of undesired settings is much lower here.

                        DebugOutputLine(debugMode, "   Setting registry keys to default values...");
                        EditAllRegistryValues(SET_Def, EDIT_Set);
                    }


                    // Get registry flags from client
                    if (leftSideString.Equals("RegistryFlags"))
                    {
                        registryFlags = rightSideString;
                        DebugOutputLine(debugMode, "   SEB client demands              : registryFlags=" + registryFlags);

                        // Response (acknowledgement) from server to client
                        SendServerAcknowledgement("RegistryFlags", registryFlags);
                        DebugOutputLine(debugMode, "Server sent acknowledgement to client");

                        // Convert the letters of the string into integers
                        // and store the registry flags as new the settings
                        for (index = IND_RegistrySettingMin; index <= IND_RegistrySettingMax; index++)
                        {
                            Char flagCharacter = registryFlags.ElementAt(index);
                            int  flagInteger   = flagCharacter - '0';
                            if  (flagCharacter.Equals('1')) flagInteger = 1;
                                                       else flagInteger = 0;
                            DebugOutputLine(debugMode, "      registryFlags[" + index + "] = " + flagInteger);
                            newSetting[index] = flagInteger;
                        }

                        // Set the registry keys to the flag values
                        DebugOutputLine(debugMode, "   Setting registry keys to flag values...");
                        EditAllRegistryValues(SET_New, EDIT_Set);
                    }


                    // Determine the index in case it is a registry setting
                    int regIndex = IND_RegistrySettingNone;
                    for   (index = IND_RegistrySettingMin; index <= IND_RegistrySettingMax; index++)
                    {
                        if (leftSideString.Equals(msgString[index])) break;
                    }
                    regIndex = index;


                    // Change a certain registy setting
                    if ((regIndex >= IND_RegistrySettingMin) &&
                        (regIndex <= IND_RegistrySettingMax))
                    {
                        newSetting[regIndex] = newInteger;

                        String regHive = hiveString[regIndex];
                        String regKey  =  keyString[regIndex];
                        String regVal  =  valString[regIndex];
                        String regMsg  =  msgString[regIndex];
                        String regType = typeString[regIndex];
                        String regData = "";

                        if (newInteger == 0) regData = "0";
                        if (newInteger == 1) regData = "1";

                        if (regIndex == IND_EnableEaseOfAccess)
                        {
                            if (newInteger == 0) regData = DATA_EnableEaseOfAccessFalse;
                            if (newInteger == 1) regData = DATA_EnableEaseOfAccessTrue;
                        }

                        DebugOutputLine(debugMode, "   Setting registry value   : " + regVal + "="  + regData);
                        EditOneRegistryValue(userName, userSid, regHive, regKey, regVal, regMsg, regType, regData, EDIT_Set);
                    }


                    // Currently not used, since SEB does currently not send the
                    // "AllSettingsTransmitted" message:
                    // When all settings have been transmitted, set the registry values
                    if (leftSideString.Equals("AllSettingsTransmitted"))
                    {
                        DebugOutputLine(debugMode, "   Setting registry keys to new values...");
                        EditAllRegistryValues(SET_New, EDIT_Set);

                        // Set the message receive timeout to Infinite,
                        // because we expect the next message from the SEB client
                        // only when the exam is over and SEB client is shut down.
                        // This can take about 1 hour or something.
                        DebugOutputLine(debugMode, "   Set the message receive timeout to Infinite");
                        networkStream.ReadTimeout = System.Threading.Timeout.Infinite;
                    }


                    // When SEB client shuts down...
                    if (leftSideString.Equals("ShutDown"))
                    {
                        // Response (acknowledgement) from server to client
                        SendServerAcknowledgement("ShutDown", rightSideString);
                        DebugOutputLine(debugMode, "Server sent acknowledgement to client");

                        // Reset the registry keys to their original values
                        DebugOutputLine(debugMode, "   Setting registry keys to old values...");
                        EditAllRegistryValues(SET_Old, EDIT_Restore);

                        // Flush the registry keys
                        Boolean flushRegistryValues = true;
                        if (flushRegistryValues)
                        {
                            DebugOutputLine(debugMode, "   Setting registry keys to allow values...");
                            EditAllRegistryValues(SET_Allow, EDIT_Restore);
                        }

                        // Exit the receiving loop for this SEB client
                        DebugOutputLine(debugMode, "   SEB client shuts down.");
                        DebugOutputLine(debugMode, "");
                        DebugOutputLine(debugMode, "");
                        DebugOutputLine(debugMode, "");
                        break;
                    }

                    DebugOutputLine(debugMode, "");
                    DebugOutputLine(debugMode, "");
                    //Console.ReadLine();

                } // end while (true) clientSocket


                // Close the client connection and wait for the next client
                clientSocket.Close();

            } // end while (true) serverSocket (endless server loop listening for clients)


            // This code can never be reached,
            // due to the endless loop while (true) serverSocket
/*
            serverSocket.Stop();
            DebugOutputLine(debugMode, "SEB socket server terminates.");
            DebugOutputLine(debugMode, "Press a key to continue...");
            Console.ReadLine();

            return;
*/
        } // end of Main()





        // ************************************************************
        // Get all subkeys of a local machine key and fill in reference
        // ************************************************************
        private void GetSubKeysLocalMachine(RegistryKey subKey, ref LinkedList<String> subKeyNames)
        {
            foreach (String sub in subKey.GetSubKeyNames())
            {
                DebugOutputLine(false, "sub = " + sub);
                subKeyNames.AddLast(sub);
                RegistryKey local = Registry.LocalMachine;
                local = subKey.OpenSubKey(sub, true);
                // By recalling itself, it makes sure it gets all the subkey names
                //GetSubKeysLM(local, ref subKeyNames);
            }
            DebugOutputLine(false, "");
        }



        // **************************************************
        // Get all registered and already logged-in user SIDs
        // **************************************************
        private LinkedList<String> GetSids()
        {
            LinkedList<String> sids = new LinkedList<String>();
            RegistryKey ProfileListKey;
            ProfileListKey = Registry.LocalMachine;
            ProfileListKey = ProfileListKey.OpenSubKey(KEY_ProfileList, false);
            GetSubKeysLocalMachine(ProfileListKey, ref sids);
            return sids;
        }



        // ************************************
        // Get the SID for a specific user name
        // ************************************
        private String GetSidByUsername(String userName)
        {
            String key   = KEY_ProfileList;
            String value = "ProfileImagePath";

            DebugOutputLine(false, "userName = " + userName);
            DebugOutputLine(false, "");

            LinkedList<String> sids = GetSids();

            foreach (String keySid in sids)
            {
                String keykeySid = key + "\\" + keySid;

                DebugOutputLine(false, "   key    = " + key);
                DebugOutputLine(false, "   keySid = " + keySid);
                DebugOutputLine(false, "keykeySid = " + keykeySid);

                RegistryKey ProfileKey;
                ProfileKey = Registry.LocalMachine;
                ProfileKey = ProfileKey.OpenSubKey(keykeySid, false);
                String valueProfileImagePath = ProfileKey.GetValue(value).ToString();

                DebugOutputLine(false, "valueProfileImagePath = " + valueProfileImagePath);
                DebugOutputLine(false, "");

                if (valueProfileImagePath.Contains(userName))
                {
                    DebugOutputLine(false, "returning keySid = " + keySid);
                    DebugOutputLine(false, "");
                    return keySid;
                }
            }
            DebugOutputLine(false, "");

            // Return the SID if found else throw exception
            throw new ApplicationException("Username not found");
        }



        // *********************************************************************
        // Change the registry value for a specific user SID and registry value.
        // Sets the new integer and returns the old integer.
        // *********************************************************************
        private String EditOneRegistryValue(String userName, String userSid,
											String  regHive, String  regKey,
                                            String  regVal , String  regMsg,
                                            String  regType, String  setString,
											int     editMode)
        {
            // Example:
            // regDom     = "HKEY_LOCAL_MACHINE" or "HKEY_CURRENT_USER"
            // regKey     = @"Software\Microsoft\Windows\CurrentVersion\Policies\System"
            // regVal     = "DisableTaskMgr"
            // regMsg     = "DisableTaskMgr        "
            // regType    = REG_DWORD or REG_SZ
            // setString  = "0" or "1"
            // editMode   = EDIT_Get or EDIT_Set

            String            keyName = "";
            RegistryKey       ourKey  = null;
            RegistryKey       subKey  = null;
            RegistryValueKind regKind = RegistryValueKind.Unknown;

            if (regType == "REG_DWORD") regKind = RegistryValueKind.DWord;
            if (regType == "REG_SZ"   ) regKind = RegistryValueKind.String;

            //DebugOutputLine(debugMode, "regKind = " + regKind.ToString());

            // Initialise the get and set values
            Object getObject  = null;
            String getString  = "";
            int    getInteger = -1;
            int    setInteger = -1;

            // Most of the registry values are in negative format
            // (e.g. DisableTaskMgr=1).
            // Since we store our settings in positive format
            // (e.g. EnableStartTaskManager=0),
            // we must in these cases invert the settings
            // when editing the Windows Registry.
            if (regMsg.Equals(MSG_EnableSwitchUser      ) ||
                regMsg.Equals(MSG_EnableLockThisComputer) ||
                regMsg.Equals(MSG_EnableChangeAPassword ) ||
                regMsg.Equals(MSG_EnableStartTaskManager) ||
                regMsg.Equals(MSG_EnableLogOff          ) ||
                regMsg.Equals(MSG_EnableShutDown        ))
            {
                DebugOutputLine(debugMode, "Converting:");
                DebugOutputLine(debugMode, "Set regMsg " + regMsg + "=" + setString);
                if (setString.Equals("0")) setString = "1";
                if (setString.Equals("1")) setString = "0";
                DebugOutputLine(debugMode, "Set regVal " + regVal + "=" + setString);
            }

            // Convert the registry value integer to a string
            if (setString == "" ) setInteger = -1;
            if (setString == "0") setInteger =  0;
            if (setString == "1") setInteger =  1;

            //debugMode = false;

            DebugOutputLine(debugMode, "");
            DebugOutputLine(debugMode, "");
            DebugOutputLine(debugMode, "userName   = " + userName);
            DebugOutputLine(debugMode, "userSid    = " + userSid);
            DebugOutputLine(debugMode, "regHive    = " + regHive);
            DebugOutputLine(debugMode, "regKey     = " + regKey);
            DebugOutputLine(debugMode, "regVal     = " + regVal);
            DebugOutputLine(debugMode, "regMsg     = " + regMsg);
            DebugOutputLine(debugMode, "regType    = " + regType);
            DebugOutputLine(debugMode, "setString  = " + setString);
            DebugOutputLine(debugMode, "editMode   = " + editMode);
            DebugOutputLine(debugMode, "");

            if (regHive == HIVE_HKCU) keyName = userSid + "\\" + regKey;
            if (regHive == HIVE_HKLM) keyName = regKey;

            DebugOutputLine(debugMode, "    keyName = " + keyName);

            if (regHive == HIVE_HKCU) ourKey = Registry.Users;
            if (regHive == HIVE_HKLM) ourKey = Registry.LocalMachine;

            Boolean keyAlreadyExists  = true;
            Boolean keyHasBeenCreated = true;

            // Determine whether the registry key (value) already exists
            try
            {
                subKey = ourKey.OpenSubKey(keyName, true);
            }
            catch (Exception e)
            {
                keyAlreadyExists = false;
                DebugOutputLine(debugMode, "OpenSubKey() failed!");
                DebugOutputLine(debugMode, "Error type = " + e.Message);
                if (subKey == null) DebugOutputLine(debugMode, "subKey == null");
                               else DebugOutputLine(debugMode,  subKey.Name);
            }
            if (subKey == null) keyAlreadyExists = false;

            if (keyAlreadyExists)
                 DebugOutputLine(debugMode, "       subKey " + keyName + " already exists.");
            else DebugOutputLine(debugMode, "       subKey " + keyName + " must be created first...");


            // If the registry key (value) does not already exist, create it
            if (!keyAlreadyExists)
            {
                try
                {
                    subKey = ourKey.CreateSubKey(keyName);
                }
                catch (Exception e)
                {
                    keyHasBeenCreated = false;
                    DebugOutputLine(debugMode, "CreateSubKey() failed!");
                    DebugOutputLine(debugMode, "Error type = " + e.Message);
                    if (subKey == null) DebugOutputLine(debugMode, "subKey == null");
                                   else DebugOutputLine(debugMode,  subKey.Name);
                }
            }
            if (subKey == null) keyHasBeenCreated = false;


            if (keyAlreadyExists || keyHasBeenCreated)
                 DebugOutputLine(debugMode, "       subKey " + keyName + " is now available.");
            else DebugOutputLine(debugMode, "       subKey " + keyName + " could not be created!");

            // If the registry key (value) still does not exist, give up
            if ((!keyAlreadyExists) && (!keyHasBeenCreated)) return "";


            // Attention! TODO: It is highly possible that on first run System Subkey doesn't exist!


            // From now on, we assume that the registry key (value) exists and is open
            DebugOutputLine(debugMode, "    keyName = " +     keyName);
            DebugOutputLine(debugMode, "ourKey.Name = " + ourKey.Name);
            DebugOutputLine(debugMode, "subKey.Name = " + subKey.Name);
            DebugOutputLine(debugMode, "");


            // Directly getting the old INTEGER or STRING value does not work,
            // so we must get the old setting as an OBJECT
            // and convert it to an integer or string.

            if (keyAlreadyExists)
            {
                DebugOutputLine(debugMode, "Getting " + regVal + "...");

                // Try to get the value of the key
                getObject = subKey.GetValue(regVal);

                // If the value does not exist, we assume the default value
                if (getObject == null) getString = "";
                                 else  getString = getObject.ToString();

                // Convert the registry value string to an integer
                if (getString == "" ) getInteger = -1;
                if (getString == "0") getInteger =  0;
                if (getString == "1") getInteger =  1;

                DebugOutputLine(debugMode, "Got " + regVal + " as " + getInteger);
                DebugOutputLine(debugMode, "Got " + regVal + " as " + getString);
            }


            // Actual setting of the new registry value.
            // Here, we must set the new INTEGER value
            // rather than a STRING! This is somewhat strange.

            if ((editMode == EDIT_Set) || (editMode == EDIT_Restore))
            {
                DebugOutputLine(debugMode, "Setting " + regVal + " to " + setString + "...");

                if (regKind == RegistryValueKind.DWord ) subKey.SetValue(regVal, setInteger, RegistryValueKind.DWord);
                if (regKind == RegistryValueKind.String) subKey.SetValue(regVal, setString , RegistryValueKind.String);
                if (regKind == RegistryValueKind.DWord ) DebugOutputLine(debugMode, "Set " + regVal + " to " + setInteger);
                if (regKind == RegistryValueKind.String) DebugOutputLine(debugMode, "Set " + regVal + " to " + setString);

                DebugOutputLine(debugMode, "Set " + regVal + " to " + setInteger);
                DebugOutputLine(debugMode, "Set " + regVal + " to " + setString);
            }


            // If the registry value did not exist before starting SEB,
            // delete it again before SEB shuts down.
            if ((editMode == EDIT_Restore) && (setInteger == -1))
            {
                DebugOutputLine(debugMode, "Deleting " + regVal + "...");
                subKey.DeleteValue(regVal);
                DebugOutputLine(debugMode, "Deleted  " + regVal);
            }

            subKey.Close();
            ourKey.Close();

            //DebugOutputLine(debugMode, "");
            //DebugOutputLine(debugMode, "");
            //DebugOutputLine(debugMode, "");

            // Most of the registry values are in negative format
            // (e.g. DisableTaskMgr=1).
            // Since we store our settings in positive format
            // (e.g. EnableStartTaskManager=0),
            // we must in these cases invert the settings
            // when editing the Windows Registry.
            if (regMsg.Equals(MSG_EnableSwitchUser      ) ||
                regMsg.Equals(MSG_EnableLockThisComputer) ||
                regMsg.Equals(MSG_EnableChangeAPassword ) ||
                regMsg.Equals(MSG_EnableStartTaskManager) ||
                regMsg.Equals(MSG_EnableLogOff          ) ||
                regMsg.Equals(MSG_EnableShutDown        ))
            {
                DebugOutputLine(debugMode, "Converting:");
                DebugOutputLine(debugMode, "regVal " + regVal + "=" + getString);
                if (getString.Equals("0")) getString = "1";
                if (getString.Equals("1")) getString = "0";
                DebugOutputLine(debugMode, "regMsg " + regMsg + "=" + getString);
            }

            debugMode = true;
            return getString;

        } // end of method EditOneRegistryValue()



        // ****************************************************************
        // Get/Change/Restore all registry values for the Safe Exam Browser
        // ****************************************************************
        private void EditAllRegistryValues(int wishedSettings, int editMode)
        {
            int  regIndex;
            for (regIndex = IND_RegistrySettingMin; regIndex <= IND_RegistrySettingMax; regIndex++)
            {

                // Fetch the necessary strings from the global arrays
                String regHive = hiveString[regIndex];
                String regKey  =  keyString[regIndex];
                String regVal  =  valString[regIndex];
                String regMsg  =  msgString[regIndex];
                String regType = typeString[regIndex];

                // Initialise the get and set values
                String getString  = "";
                String setString  = "";
                int    getInteger = -1;
                int    setInteger = -1;


                // Only necessary for setting operation
                if ((editMode == EDIT_Set) || (editMode == EDIT_Restore))
                {
                    int    defInteger =    defSetting[regIndex];
                    int    oldInteger =    oldSetting[regIndex];
                    int    newInteger =    newSetting[regIndex];
                    int  allowInteger =  allowSetting[regIndex];
                    int forbidInteger = forbidSetting[regIndex];

                    if (wishedSettings == SET_Def   ) setInteger =    defInteger;
                    if (wishedSettings == SET_Old   ) setInteger =    oldInteger;
                    if (wishedSettings == SET_New   ) setInteger =    newInteger;
                    if (wishedSettings == SET_Allow ) setInteger =  allowInteger;
                    if (wishedSettings == SET_Forbid) setInteger = forbidInteger;

                    // Convert the registry value integer to a string
                    if (setInteger == -1) setString = "" ;
                    if (setInteger ==  0) setString = "0";
                    if (setInteger ==  1) setString = "1";

                    if (regIndex == IND_EnableEaseOfAccess)
                    {
                        if (setInteger == -1) setString = "";
                        if (setInteger ==  0) setString = DATA_EnableEaseOfAccessFalse;
                        if (setInteger ==  1) setString = DATA_EnableEaseOfAccessTrue;
                    }
                }


                //if (editMode == EDIT_Get) DebugOutputLine(debugMode, "      Getting " + regMsg + "...");
                //if (editMode == EDIT_Set) DebugOutputLine(debugMode, "      Setting " + regMsg + " to " + setString + "...");

                getString = EditOneRegistryValue(userName, userSid, regHive, regKey, regVal, regMsg, regType, setString, editMode);

                // Convert the registry value string to an integer
                if (getString == "" ) getInteger = -1;
                if (getString == "0") getInteger =  0;
                if (getString == "1") getInteger =  1;

                if (editMode == EDIT_Get) DebugOutputLine(debugMode, "      Got " + regMsg + " as string  " + getString);
                if (editMode == EDIT_Set) DebugOutputLine(debugMode, "      Set " + regMsg + " to string  " + setString);

                if (editMode == EDIT_Get) DebugOutputLine(debugMode, "      Got " + regMsg + " as integer " + getInteger);
                if (editMode == EDIT_Set) DebugOutputLine(debugMode, "      Set " + regMsg + " to integer " + setInteger);

                // If we GET the registry setting, store it as the old setting.
                // If we SET the registry setting, store it as the new setting.
                if (editMode == EDIT_Get) oldSetting[regIndex] = getInteger;
                if (editMode == EDIT_Set) newSetting[regIndex] = setInteger;

            } // next regIndex

            DebugOutputLine(debugMode, "");
            DebugOutputLine(debugMode, "");
            DebugOutputLine(debugMode, "");
            DebugOutputLine(debugMode, "");

            //Console.WriteLine("Please press the ENTER key to continue...");
            //Console.ReadLine();
			return;

        } // end of method EditAllRegistryValues()



        // **********************************************
        // Server sends back an acknlowedgement to client
        // **********************************************
        private static void SendServerAcknowledgement(String leftSide, String rightSide)
        {
            String serverAcknowledgement;

            serverAcknowledgement = leftSide + "=" + rightSide + endOfStringKeyWord;
            DebugOutputLine(debugMode, "SEB server sends back: serverAcknowledgement = ***" + serverAcknowledgement + "***");
            sendBuffer = Encoding.ASCII.GetBytes(serverAcknowledgement);
            DebugOutputLine(debugMode, "SEB server sends back: sendBuffer.Length     = " + sendBuffer.Length);
            networkStream.Write(sendBuffer, 0, sendBuffer.Length);
            networkStream.Flush();

            return;
        }


    } // end public class SebWindowsService
}     // end namespace    SebWindowsService
