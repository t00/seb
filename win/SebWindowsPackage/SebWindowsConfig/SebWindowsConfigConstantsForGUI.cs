using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Xml.Serialization;
using SebWindowsClient;
using SebWindowsClient.CryptographyUtils;
using PlistCS;



namespace SebWindowsConfig
{
    public partial class SebWindowsConfigForm : Form
    {

        // *************************
        // Constants for GUI widgets
        // *************************

        // Boolean values
        const int IntFalse = 0;
        const int IntTrue  = 1;

        // Operating systems
        const int IntOSX = 0;
        const int IntWin = 1;

        const String StringOSX = "OS X";
        const String StringWin = "Win";

        // URL filter actions
        const int IntBlock = 0;
        const int IntAllow = 1;
        const int IntSkip  = 2;
        const int IntAnd   = 3;
        const int IntOr    = 4;

        const String StringBlock = "block";
        const String StringAllow = "allow";
        const String StringSkip  = "skip";
        const String StringAnd   = "and";
        const String StringOr    = "or";

        // URL filter table operations
        const String StringCollapse = "Collapse";
        const String StringExpand   = "Expand";

        const int IntOperationInsert = 0;
        const int IntOperationPaste  = 1;
        const int IntOperationDelete = 2;
        const int IntOperationCut    = 3;
        const int IntOperationCopy   = 4;

        const int IntLocationBefore = 0;
        const int IntLocationAfter  = 1;
        const int IntLocationAt     = 2;

        // Embedded Certificate types
        const int IntSSLClientCertificate = 0;
        const int IntIdentity             = 1;

        const String StringSSLClientCertificate = "SSL Certificate";
        const String StringIdentity             = "Identity";

        // Proxy Protocol types
        const int IntProxyAutoDiscovery     = 0;
        const int IntProxyAutoConfiguration = 1;
        const int IntProxyHTTP              = 2;
        const int IntProxyHTTPS             = 3;
        const int IntProxyFTP               = 4;
        const int IntProxySOCKS             = 5;
        const int IntProxyRTSP              = 6;
        const int NumProxyProtocols = 7;

        // Captions for table dataGridViewProxyProtocols
        const String StringTableCaptionProxyAutoDiscovery     = "Auto Proxy Discovery";
        const String StringTableCaptionProxyAutoConfiguration = "Automatic Proxy Configuration";
        const String StringTableCaptionProxyHTTP              = "Web Proxy (HTTP)";
        const String StringTableCaptionProxyHTTPS             = "Secure Web Proxy (HTTPS)";
        const String StringTableCaptionProxyFTP               = "FTP Proxy";
        const String StringTableCaptionProxySOCKS             = "SOCKS Proxy";
        const String StringTableCaptionProxyRTSP              = "Streaming Proxy (RTSP)";

        // Texts for labelProxyServerHost
        const String StringServerLabelProxyAutoDiscovery     = "";
        const String StringServerLabelProxyAutoConfiguration = "";
        const String StringServerLabelProxyHTTP              = "Web";
        const String StringServerLabelProxyHTTPS             = "Secure Web";
        const String StringServerLabelProxyFTP               = "FTP";
        const String StringServerLabelProxySOCKS             = "SOCKS";
        const String StringServerLabelProxyRTSP              = "Streaming";

        // Permitted and Prohibited Processes table columns (0,1,2,3).
        // Permitted  Processes: Active, OS, Executable, Title
        // Prohibited Processes: Active, OS, Executable, Description
        // Process    Arguments: ArgumentActive, ArgumentParameter
        const int IntColumnProcessActive      = 0;
        const int IntColumnProcessOS          = 1;
        const int IntColumnProcessExecutable  = 2;
        const int IntColumnProcessTitle       = 3;
        const int IntColumnProcessDescription = 3;

        const int IntColumnProcessArgument = 1;
/*
        const String StringColumnProcessActive      = "Active";
        const String StringColumnProcessOS          = "OS";
        const String StringColumnProcessExecutable  = "Executable";
        const String StringColumnProcessTitle       = "Title";
        const String StringColumnProcessDescription = "Description";

        const String StringColumnProcessArgument = "Argument";
*/
        // URL Filter Rules table columns (0,1,2,3,4).
        // Show, Active, Regex, Expression, Action
        const int IntColumnURLFilterRuleShow       = 0;
        const int IntColumnURLFilterRuleActive     = 1;
        const int IntColumnURLFilterRuleRegex      = 2;
        const int IntColumnURLFilterRuleExpression = 3;
        const int IntColumnURLFilterRuleAction     = 4;
/*
        const String StringColumnURLFilterRuleShow       = "Show";
        const String StringColumnURLFilterRuleActive     = "Active";
        const String StringColumnURLFilterRuleRegex      = "Regex";
        const String StringColumnURLFilterRuleExpression = "Expression";
        const String StringColumnURLFilterRuleAction     = "Action";
*/
        // Embedded Certificates table columns (0,1).
        // Type, Name
        const int       IntColumnCertificateType = 0;
        const int       IntColumnCertificateName = 1;
      //const String StringColumnCertificateType = "Type";
      //const String StringColumnCertificateName = "Name";

        // Proxy Protocols table columns (0,1).
        // Enable, Type
        const int       IntColumnProxyProtocolEnable = 0;
        const int       IntColumnProxyProtocolType   = 1;
      //const String StringColumnProxyProtocolEnable = "Enable";
      //const String StringColumnProxyProtocolType   = "Type";

        // Bypassed Proxies table column (0).
        // DomainHostPort
        const int       IntColumnDomainHostPort = 0;
      //const String StringColumnDomainHostPort = "Domain, Host, Port";



        // ********************************
        // Global variables for GUI widgets
        // ********************************

        // The current SEB configuration file
        String currentDireSebConfigFile;
        String currentFileSebConfigFile;
        String currentPathSebConfigFile;

        // The default SEB configuration file
        String defaultDireSebConfigFile;
        String defaultFileSebConfigFile;
        String defaultPathSebConfigFile;

        // Strings for encryption identities (KeyChain, Certificate Store)
        //static ArrayList chooseIdentityStringArrayList = new ArrayList();
        //static String[]  chooseIdentityStringArray = new String[1];
        static List<String> StringCryptoIdentity = new List<String>();

        // Entries of ListBoxes
      //static    Byte[]    ByteArrayExamKeySalt          = new Byte[] {};
        static  String[]  StringCryptoIdentityArray;
        static  String[]  StringSebPurpose                = new  String[2];
        static  String[]  StringSebMode                   = new  String[2];
        static  String[]  StringBrowserViewMode           = new  String[2];
        static  String[]  StringWindowWidth               = new  String[4];
        static  String[]  StringWindowHeight              = new  String[4];
        static  String[]  StringWindowPositioning         = new  String[3];
        static  String[]  StringPolicyLinkOpening         = new  String[3];
        static  String[]  StringPolicyFileUpload          = new  String[3];
        static  String[]  StringPolicyProxySettings       = new  String[2];
        static  String[]  StringPolicySebService          = new  String[3];
        static  String[]  StringFunctionKey               = new  String[12];
        static  String[]  StringActive                    = new  String[2];
        static  String[]  StringOS                        = new  String[2];
        static  String[]  StringAction                    = new  String[5];
        static  String[]  StringCertificateType           = new  String[2];
        static  String[]  StringProxyProtocolTableCaption = new  String[7];
        static  String[]  StringProxyProtocolServerLabel  = new  String[7];
        static Boolean[] BooleanProxyProtocolEnabled      = new Boolean[7];

        static  String[]  MessageProxyProtocolType      = new  String[7];
        static  String[]  MessageProxyProtocolAttribute = new  String[7];
        static  String[]  MessageProxyProtocolEnableKey = new  String[7];


        // Global variable: index of current table row (selected row)
        // Global variable:   is the current table row a title row?
        // Lookup table: row  ->   ruleIndex (of current table row)
        // Lookup table: row  -> actionIndex (of current table row)
        // Lookup table: row  -> is this row a title row (or action row)?
        // Lookup table: rule -> startRow of rule (in the table)
        // Lookup table: rule ->   endRow of rule (in the table)
        // Lookup table: rule -> show this rule or not (expand/collapse)?
        static int           urlFilterTableRow;
        static Boolean       urlFilterIsTitleRow;
        static List<int>     urlFilterTableRuleIndex   = new List<int    >();
        static List<int>     urlFilterTableActionIndex = new List<int    >();
        static List<Boolean> urlFilterTableIsTitleRow  = new List<Boolean>();
        static List<int>     urlFilterTableStartRow    = new List<int    >();
        static List<int>     urlFilterTableEndRow      = new List<int    >();
        static List<Boolean> urlFilterTableShowRule    = new List<Boolean>();

        // Two-dimensional array: shall this cell be disabled (painted over)?
        static List<List<Boolean>> urlFilterTableCellIsDisabled = new List<List<Boolean>>();

        // Default disabled values for title row (rule) and action row (action)
        static Boolean[] urlFilterTableDisabledColumnsOfRule   = { false, false,  true, false,  true };
        static Boolean[] urlFilterTableDisabledColumnsOfAction = {  true, false, false, false, false };

    } // end of   class     SebWindowsConfigForm
}     // end of   namespace SebWindowsConfig
