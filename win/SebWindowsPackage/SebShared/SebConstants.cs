namespace SebShared
{
	public static class SebConstants
	{
		// Error levels
		public const int ERROR = 0;
		public const int WARNING = 1;
		public const int INFORMATION = 2;
		public const int QUESTION = 3;

		// Name and location of SEB configuration files and logfiles
		public const string SEB_CLIENT_CONFIG = "SebClientSettings.seb";
		public const string SEB_CLIENT_LOG = "SebClient.log";
		public const string XUL_RUNNER = "xulrunner.exe";
		public const string XUL_RUNNER_CONFIG = "config.json";
		public const string XUL_RUNNER_INI = "seb.ini";

		// Application path contains [MANUFACTURER]\[PRODUCT_NAME]
		// (see also "SebWindowsPackageSetup" Project in MS Visual Studio 10)
		public const string MANUFACTURER_LOCAL = "SafeExamBrowser";
		//private const string MANUFACTURER         = "ETH Zuerich";
		public const string PRODUCT_NAME = "SafeExamBrowser";
		public const string SEB_BROWSER_DIRECTORY = "SebWindowsBrowser";
		public const string XUL_RUNNER_DIRECTORY = "xulrunner";
		public const string XUL_SEB_DIRECTORY = "xul_seb";
		public const string BROWSER_USERAGENT_DESKTOP = "Mozilla/5.0 (Windows NT 6.3; rv:38.0) Gecko/20100101 Firefox/38.0";
		public const string BROWSER_USERAGENT_TOUCH = "Mozilla/5.0 (Windows NT 6.3; rv:38.0; Touch) Gecko/20100101 Firefox/38.0";
		public const string BROWSER_USERAGENT_TOUCH_IPAD = "Mozilla/5.0 (iPad; CPU OS 8_1 like Mac OS X) AppleWebKit/600.1.4 (KHTML, like Gecko) Version/8.0 Mobile/12B410 Safari/600.1.4";
		public const string BROWSER_USERAGENT_SEB = "SEB";

		public const string END_OF_STRING_KEYWORD = "---SEB---";
		public const string DEFAULT_KEY = "Di𝈭l𝈖Ch𝈒ah𝉇t𝈁a𝉈Hai1972";

		public const string SEB_NEW_DESKTOP_NAME = "SEBDesktop";
		public const string SEB_WINDOWS_SERVICE_NAME = "SebWindowsService";
	}
}