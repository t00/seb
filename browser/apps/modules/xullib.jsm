/*
    This program is free software; you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation; version 2 of the License.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program; if not, write to the Free Software
    Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA  02110-1301  USA

   Stefan Schneider <schneider@hrz.uni-marburg.de> 2012
*/
Components.utils.import("resource://gre/modules/Services.jsm");
Components.utils.import("resource://gre/modules/FileUtils.jsm");
Components.utils.import("resource://gre/modules/ctypes.jsm");

var EXPORTED_SYMBOLS = ["xullib"];
		
// public object 
var xullib = (function () {	
	// XPCOM Services, Interfaces and Objects
	const	Cc					=	Components.classes,
			Ci					=	Components.interfaces,
			Cu					=	Components.utils,
			Cr					=	Components.results,
			appshell			=	Services.appShell,			
			cs					= 	Cc["@mozilla.org/consoleservice;1"].getService(Ci.nsIConsoleService),
			fph 				= 	Services.io.getProtocolHandler("file").QueryInterface(Ci.nsIFileProtocolHandler),
			prefs				=	Services.prefs,
			profs 				= 	Cc["@mozilla.org/toolkit/profile-service;1"].createInstance(Ci.nsIToolkitProfileService),
			uuidg				=	Cc["@mozilla.org/uuid-generator;1"].getService(Ci.nsIUUIDGenerator),
			wm					=	Cc["@mozilla.org/appshell/window-mediator;1"].getService(Ci.nsIWindowMediator),
			wnav				= 	Ci.nsIWebNavigation,
			wpl					=	Ci.nsIWebProgressListener,
			zipr				= 	Cc["@mozilla.org/libjar/zip-reader;1"].createInstance(Ci.nsIZipReader),
			
			// const values
			APPNAME				=	Services.appinfo.name,
			MAX_DUMP_DEPTH 		= 	10,
			XUL_NS				=	"http://www.mozilla.org/keymaster/gatekeeper/there.is.only.xul",
			XULLIB_WIN			=	"chrome://xullib/content/window.xul",
			XULLIB_ERR_WIN		=	"chrome://xullib/content/err.xul",
			STRING				=	0,
			NUMBER				=	1,
			BOOLEAN				=	2,
			ARRAY				=	3,
			OBJECT				=	4;							
			
	var 	__initialized 		= 	false,
			app					=	null,
			errPage				=	"",
			wins				=	[], 		// windows array
			//hiddenWin			=	null,		// hidddenWindow
			cl					=	null, 		// commandline Interface initialized from component xulApplication.js		
			profile				=	{},			// object with obj(=nsIToolkitProfile), dirs[nsIFile]
			userAppDir			=	null, 		// the HOME/.eqsoft/seb (UNIX)
			params				=	{}, 		
			DEBUG				=	false, 		// upper case: correspondent function "_debug()"
			lf					=	"\n",						
			
			winObserver			= 	{ // needed?
				observe	: function(aSubject, aTopic, aData) {
					_debug("observe: " + aTopic);
					switch (aTopic) {
						case "domwindowopened" :
						break;
						case "domwindowclosed" :
						break;
					}
				}
			};
	
	function nsBrowserStatusHandler() {}; // real object instance: muliple window contexts
	
	nsBrowserStatusHandler.prototype = { // override functions with addBrowserXXXListener
		onStateChange : function(aWebProgress, aRequest, aStateFlags, aStatus) {			
				if(aStateFlags & wpl.STATE_IS_NETWORK) {
					if (aStateFlags & wpl.STATE_START) {
							_debug("start loading ...");
					}
					if (aStateFlags & wpl.STATE_STOP) {
							_debug("loading finished!");	
							this.onStatusChange(aWebProgress, aRequest, 0, "Done");
					}					
				}
		},
		
		onStatusChange : function(aWebProgress, aRequest, aStatus, aMessage) {				
				if (aStatus == 0) {
						_debug("onStatusChange " + aMessage);
				}						
		},
		
		onProgressChange : function(aWebProgress, aRequest, aCurSelfProgress,
								  aMaxSelfProgress, aCurTotalProgress, aMaxTotalProgress) {
		},

		onSecurityChange : function(aWebProgress, aRequest, state) {
		},
	  
		onLocationChange : function(aWebProgress, aRequest, aLocation) {
		},

		QueryInterface : function(aIID) {
				if (aIID.equals(Ci.nsIWebProgressListener) ||
				aIID.equals(Ci.nsISupportsWeakReference) ||
				aIID.equals(Ci.nsIXULBrowserWindow) ||
				aIID.equals(Ci.nsISupports)) {
				return this;
			}
			throw Cr.NS_NOINTERFACE;
		},

		setJSStatus : function(status) {
		},
	  
		setJSDefaultStatus : function(status) {
		},
	  
		setOverLink : function(link) {
		}
	}
	
	// application context
	switch (Services.appinfo.OS) { // line feed for dump messages
		case "WINNT" :
			lf = "\n\r";
			break;
		case "UNIX" :
			lf = "\n";
			break;
		default :
			lf = "\n";
	}
	
	// application
	function toString() {
			return "xullib";
	}
		
	function init(cmdLine) {
		try {
			if (__initialized == true) return;	
			Cc["@mozilla.org/net/osfileconstantsservice;1"].getService(Ci.nsIOSFileConstantsService).init();					
			// Services.ww.registerNotification(winObserver); // needed?
			// profileObserver.register();
			cl = cmdLine;
			DEBUG = getBool(getCmd("debug"));
			let autostart = getBool(getCmd("autostart")); // if true, xullib will try to inject application module jsm and execute the init() function of the module;			
			let errMsg = "Error importing app module " + APPNAME;
			try {					
				Components.utils.import("resource://modules/" + APPNAME + ".jsm");				
				app = eval(APPNAME);
				if (!app) {
					_err(errMsg);
					return false;
				}
				if (typeof app.init != "function") {
					_err(errMsg + "\nno init() function in module");
					return false;
				}				
			}
			catch(e) {
				_err(errMsg + "\n" + e);
				return false;
			}
			
			_debug("debug:" + DEBUG);
			if (DEBUG) {
				let debugPrefs = FileUtils.getFile("AChrom",["defaults",APPNAME,"preferences","debug.js"], null);
				if (debugPrefs.exists()) {
					prefs.readUserPrefs(debugPrefs);
				}
				else {
					_debug("no debug preferences: " + debugPrefs.path);
				}
			}
			//hiddenWin = Services.appShell.hiddenDOMWindow;
			userAppDir = FileUtils.getDir("AppRegD",[], null);			 
			_debug("userAppDir :" + userAppDir.path);
			let defaultPrefs = FileUtils.getFile("AChrom",["defaults",APPNAME,"preferences","prefs.js"], null);
			if (defaultPrefs.exists()) {
				prefs.readUserPrefs(defaultPrefs);				
			}
			else {
				_debug("no default preferences: " + defaultPrefs.path);
			}
			prefs.readUserPrefs(null); // tricky: for current prefs file use profile prefs, so my original prefs will never be overridden ;-)						
			prefs.savePrefFile(null);
			
			// profile
			// use the new OS.File API ( >= Gecko 18) for profile Handling
			try {
				profile["dirs"] = [];
				let profilePath = OS.Constants.Path.profileDir;
				let profileDir = Cc["@mozilla.org/file/local;1"].createInstance(Ci.nsILocalFile);
				profileDir.initWithPath(profilePath);
				_debug("push profile: " + profilePath);
				profile["dirs"].push(profileDir);
				// push AppData Local profile directory
				if (Services.appinfo.OS == "WINNT") {
					let localProfilePath = profilePath.replace(/AppData\\Roaming/,"AppData\\Local"); // WIN7 o.k, XP?
					if (localProfilePath) {
						let localProfileDir = Cc["@mozilla.org/file/local;1"].createInstance(Ci.nsILocalFile);
						localProfileDir.initWithPath(localProfilePath);
						if (localProfileDir.exists()) {
							_debug("push local profile: " + localProfilePath);
							profile["dirs"].push(localProfileDir);
						}
					}
				}
			}
			catch (e) {
				_err(e);
				return false;
			}
			
			// see for special files and dirs: 
			//	https://developer.mozilla.org/en/Code_snippets/File_I%2F%2FO#Getting_special_files
			// 	http://mxr.mozilla.org/mozilla-central/source/xpcom/io/nsDirectoryServiceDefs.h
			//	http://mxr.mozilla.org/mozilla-central/source/xpcom/io/nsAppDirectoryServiceDefs.h
			
			// copy defaults to profile, important for trusted ssl certificates in cert_override.txt! 
			let defaultProfile = FileUtils.getDir("AChrom",["defaults",APPNAME,"profile"],null); // see http://mxr.mozilla.org/mozilla-central/source/xpcom/io/nsAppDirectoryServiceDefs.h
			if (defaultProfile.exists()) {
				let entries = defaultProfile.directoryEntries; 
				profile["customFiles"] = [];
				while(entries.hasMoreElements()) {  
					let entry = entries.getNext();  
					entry.QueryInterface(Components.interfaces.nsIFile);
					// don't copy .svn
					if (entry.leafName === ".svn") {
						continue;
					}
					var cf = profile.dirs[0].clone();
					cf.append(entry.leafName);
					profile.customFiles.push(cf);
					if (!cf.exists()) {
						entry.copyTo(profile.dirs[0],entry.leafName);
						_debug("copy " + entry.leafName + " to " + profile.dirs[0].path);
												
					}
					else {
						_debug(entry.leafName + " already exists");
					}														
				}
			}
			else {
				_debug("no default profile: " + defaultProfile.path);
			}								
			
			// callback for async loading of json config, the function will finish the initialisation
			function cb(obj) {				
				// catch json config params
				if (typeof obj === "object" ) {
					for (k in obj) {						
						var k2 = (k.indexOf(".")<0) ? APPNAME + "." + k : k;
						params[k2] = obj[k];
					}
				}
								
				// app prefs from config.json 
				let appPrefs = params[APPNAME + ".prefs"];
				if (typeof appPrefs === "object") {
					setPrefs(appPrefs);
				}
				else {
					_debug("no extra prefs in config.json"); 
				}
				_out("xullib started with application " + APPNAME);	
				if (autostart) {
					_out("autostart " + APPNAME);
					app.init();
				}
				__initialized = true;
			};
			// load config.json from commandline
			let conf = getCmd("config");
			if (conf != null) {
				getJSON(conf,cb);
			}
			else { // load default config.json in defaults/app.name/
				try {
					let defaultConfig = FileUtils.getFile("AChrom",["defaults",APPNAME,"config.json"],false);
					if (!defaultConfig.exists()) {
						_debug("no default config file exists: " + defaultConfig.path);
						return false;
					}	
					getJSON(fph.newFileURI(defaultConfig).spec,cb);				
				}
				catch(e) {
					_err(e);
					return false;
				}
			}
		}
		catch (e) {
			_err(e);
			return false;
		}
	}
	
	function quit() {
		_debug("quit: " + APPNAME);
		// Services.startup.quit(Services.startup.eForceQuit);
	}
	
	function getUserAppDir() {
		return userAppDir;
	}
	
	// commandline: 
	function getCmd(k) { // convert strings to data types
		let v = cl.handleFlagWithParam(k,false); // beware this will remove the key and the value from the commandline list!
		let t = (v === "" || v === null) ? null : v;
		if (t) {
			var num = parseFloat(t);
			// try to parseFloat
			if (isNaN(num)) { // not a number
				// try bool
				if (/^(true|false)$/i.test(t)) {
					return /^true$/i.test(t);
				}
				else {
					return t;
				}
			}
			else {
				return num;
			}
		}
		else {
			return t;
		}		
	}
	
	// profile
	
	function getProfile() {
		return profile;
	}
	
	// prefs
	// IMPORTANT for developers: changes in ".jsm" have no effect without "-purgecaches" in xulrunner commandline!  
	
	function setPrefs(prfs) {
		for (var k in prfs) {
			try {
				setPref(k,prfs[k]);							
			}
			catch(e) {
				_err("error setting pref: " + k + "\n" + e);
			}
		}
	}
	
	function clearPrefs(prfs) {
		for (var k in prfs) {	
			try {
				clearPref(k);							
			}
			catch(e) {
				_err("error clearning pref: " + k + "\n" + e);
			}
		}
	}
	
	function getPrefType(v) {
		var t = typeof v;		
		switch (t) {
				case "string" :
					t = prefs.PREF_STRING;
				break;
				case "boolean" :
					t = prefs.PREF_BOOL;
				break;
				case "number" :
					t = prefs.PREF_INT;
					break;
				default :
					t = prefs.PREF_INVALID;
			}
		return t;
	}
	
	function getPref(k) {
		switch (prefs.getPrefType(k)) {
			case  prefs.PREF_STRING :
				return prefs.getCharPref(k);
			break;
			case prefs.PREF_INT :
				return prefs.getIntPref(k);
			break;
			case prefs.PREF_BOOL :
				return prefs.getBoolPref(k);
			break;
			default :
				return false;
		}		
	}
	
	function setPref(k,v,t) {			
		var typ = (t) ? t : (prefs.getPrefType(k) != prefs.PREF_INVALID) ? prefs.getPrefType(k) : getPrefType(v);  
		switch (typ) {
			case  prefs.PREF_STRING :
				prefs.setCharPref(k,v);				
			break;
			case prefs.PREF_INT :
				prefs.setIntPref(k,v);
			break;
			case prefs.PREF_BOOL :
				prefs.setBoolPref(k,v);
			break;
			default :
				// nothing to do				
		}
	}
	
	function clearPref(k) {
		if (prefs.prefHasUserValue(k)) {
			prefs.clearUserPref(k);
		}
	}
	
	// params
	// addParam with key from cmdLine (type casting), config.json, default value v 	(assertion that types are correct from config.json and v)
	// for setting explicit param values use "setParam(k,v)"
	function addParam(k,v,t) { // key, value, asserted type
		let appKey = "";
		let cmdVal = null;
		let jsonVal = null;
		let val = null;
		if (k.indexOf(".") < 0 ) { // don't allow params without a prefix -> defaults to app prefix!
			appKey = APPNAME + "." + k;
			_debug("warning: param " + k + " renamed to " + appKey);
		}
		else {
			appKey = k;
		}
		// if force from commandline cast commands into asserted types and overrides values
		
		let c = getCmd(k); 			// keys without prefix from commandLine
		let j = getParam(appKey);	// with prefixes in config.json
		
		if (c !== null) {
			switch (t) {
				case STRING :
					cmdVal = c.toString();  // does not matter if v is already correct type
				break;
				case NUMBER : 
					cmdVal = parseFloat(c); // does not matter if v is already correct type
				break;
				case BOOLEAN : 
					cmdVal = getBool(c); // does not matter if v is already correct type
				break;
				case ARRAY :
					cmdVal = c.split(",");
				break;
				case OBJECT : 
					cmdVal = JSON.parse(c); // try to parse commandLine if not null (experimental) 					
				break;
			}
		}
		if (j !== undefined) {
			jsonVal = j;
		}		
		val = (c !== null) ? cmdVal : (j !== undefined) ? jsonVal : v;	
		delete params[appKey];
		return params[appKey] = val;
	}
	
	function setParam(k,v) {
		if (k.indexOf(".") < 0 ) { // don't allow params without a prefix -> defaults to app prefix!
			k2 = APPNAME + "." + k;
			_debug("warning: param " + k + " renamed to " + k2);
		}
		else {
			k2 = k;
		}
		delete params[k2];
		return params[k2] = v;
	}
	
	function addParams(obj,f) {
		let k2 = "";
		for (var k in obj) {
			if (k.indexOf(".") < 0 ) { // don't allow params without a prefix -> defaults to app prefix!
				k2 = APPNAME + "." + k;
				_debug("warning: param " + k + " renamed to " + k2);
			}
			else {
				k2 = k;
			}
			if (f) { // force override existing params
				params[k2] = obj[k];
			}
			else { // don't override params
				if (typeof params[k2] === "undefined") { // add param only if not exists
					params[k2] = obj[k];
				}
			}
		}
		return params;
	}
	
	function getParam(k) {
		var k2 = (k.indexOf(".") < 0) ? APPNAME + "." + k : k;	
		return params[k2];
	}
	
	function getParams() {
		return params;
	}		
	
	// window handling
	function getDefaultWindow() {
		var aShell = new Object();
		var URLClass = Cc['@mozilla.org/network/standard-url;1'];
		var URLObj = URLClass.createInstance(Ci.nsIURL);
		URLObj.spec = XULLIB_WIN;
		let wb = Ci.nsIWebBrowserChrome;
		// let chromeMask = wb.CHROME_OPENAS_CHROME | wb.CHROME_EXTRA | wb.CHROME_MODAL;
		// debug("chromeMask :" + chromeMask);
		let w = appshell.createTopLevelWindow(null,URLObj,true,false,wb.CHROME_MODAL,800,600,aShell);
		//let w = appshell.createTopLevelWindow(null,URLObj,false,false,Ci.nsIWebBrowserChrome.CHROME_ALL,0,0,aShell); 
		w.showModal();
		appshell.registerTopLevelWindow(w);
	}
	
	function getChromeWin(win) {
		return win.QueryInterface(Components.interfaces.nsIInterfaceRequestor)
                   .getInterface(Components.interfaces.nsIWebNavigation)
                   .QueryInterface(Components.interfaces.nsIDocShellTreeItem)
                   .rootTreeItem
                   .QueryInterface(Components.interfaces.nsIInterfaceRequestor)
                   .getInterface(Components.interfaces.nsIDOMWindow);
	}
	
	function getWins() {
		return wins;
	}
	
	function getWinFromUrl(url) {
		for (i=0;i<wins.length;i++) {
			//_debug("compare: " + wins[i].content.location.href + " : " + url);			
			if (wins[i].content && (wins[i].content.location.href == url)) {
				_debug("getWinFromUrl found: " + url);
				return wins[i];
				break;
			}
		}
		return false;
	}
	
	function getWinFromRequest(req) {
		for (i=0;i<wins.length;i++) {
			//_debug("compare: " + wins[i].content.location.href + " : " + url);			
			if (wins[i].XulLibBrowser.getAttribute("request") == req) {
				_debug("getWinFromRequest found: " + req);
				return wins[i];
				break;
			}
		}
		return false;
	}
	
	function getMainWin() {
		if (wins.length === 0) {
			return false;
		}
		for (var i=0;i<wins.length;i++) {
			_debug("getWin: " + i + " : " + getWinType(wins[i]));
		}
		return wins[0];
	}
	
	function addWin(w) {
		//var w = (obj.currentTarget) ? obj.currentTarget : obj; // must be ChromwWindow: 
		// deprecated since all new windows are inherited from seb.xul 
		if (wins.length >= 1) { // secondary
			setWinType(w,"secondary");
		}
		initBrowser(w);
		wins.push(w);
		_debug("window added with type: " + getWinType(w) + " - " + w.content.location.href);
		_debug("windows count: " + wins.length);
	}
	
	// deprecated since the unload event removes modal popups too
	function cleanWins() {
		_debug("cleanWins");
		for (var i=0;i<wins.length;i++) {
			if (!wins[i].document) { // no content document object -> remove
				removeWin(wins[i]);
				//cleanWins(); // recursive call because array length changed
				//break;
			}
		}
	}
	
	function removeWin(win) {
		if (getWinType(win) == "main") { // never remove the main window, this must be controlled by the host app 
			return;
		} 
		for (var i=0;i<wins.length;i++) {
			if (wins[i] === win) {
				//var n = (win.document && win.content) ? getWinType(win) + ": " + win.document.title : " empty document";
				//_debug("remove win from array: " + ;
				_debug("windows count: " + wins.length);
				_debug("remove win from array ...");
				wins.splice(i,1);
				_debug("windows count: " + wins.length);
				break;
			}
		}
	}
	
	function removeSecondaryWins() {
		let main = null;
		for (var i=0;i<wins.length;i++) {
			let win = wins[i];
			if (getWinType(win) != "main") {
				var n = (win.document && win.content) ? getWinType(win) + ": " + win.document.title : " empty document";
				_debug("close win from array: " + n);
				win.close();
			} 
			else {
				main = win;
			}
		}
		wins = [];
		wins.push(main);
	}
	
	
	function getWinType(win) {
		return win.document.getElementsByTagName("window")[0].getAttribute("windowtype");
	}
	
	function setWinType(win,type) {
		win.document.getElementsByTagName("window")[0].setAttribute("windowtype",type);
	}
	
	function getRecentWin() {
		return wm.getMostRecentWindow(null);
	}
	
	function showAllWin() { 
		for (i=0;i<wins.length;i++) {
			let w = wins[i];
			if (getWinType(w) != "main") {			
				_debug("show window: " + w.content.title);
				w.focus();
			}	
		}
	}
	
	function openXullibWin(cb) { // a little bit complicated....
		_debug("openXullibWin");
		var winobserver = {
				observe	: function(aSubject, aTopic, aData) {
					_debug("observe: " + aTopic);
					switch (aTopic) {
						case "domwindowopened" :
							aSubject.addEventListener("load",function(e){ addWin(e.currentTarget); cb.call(app,wins[0]) }, false);
							break;
						case "domwindowclosed" :
							break;
					}
			}
		};
		Services.ww.registerNotification(winobserver);	
		// ToDo: problem of required jsconsole in server mode with xvfb
		Services.ww.openWindow(null,XULLIB_WIN,"xullibwindow","chrome,extrachrome,modal,dialog=yes,width=1,height=1",null); 
	}
	
	
	// browser
	function initBrowser(win) {
		if (!win) {
			_err("wrong arguments for initBrowser(win)");
			return false;
		}
		var br = getBrowser(win);
		if (!br) {
			_debug("no xullib.browser in ChromeWindow!");
			return false;
		}		
		win.XulLibBrowser = br; // extend window property to avoid multiple getBrowser() calls
		win.XULBrowserWindow = new nsBrowserStatusHandler();
		// hook up UI through progress listener
		var interfaceRequestor = win.XulLibBrowser.docShell.QueryInterface(Ci.nsIInterfaceRequestor);
		var webProgress = interfaceRequestor.getInterface(Ci.nsIWebProgress);
		webProgress.addProgressListener(win.XULBrowserWindow, Ci.nsIWebProgress.NOTIFY_ALL);
		_debug("initBrowser");
	}
	
	function loadPage(win,url,loadFlag) {	// only use for real http requests
		_debug("try to load: " + url);	
		if (!win.XulLibBrowser) {
			_err("no xullib.browser in ChromeWindow!");
			return false;
		}
		if (typeof(loadFlag) == "undefined") {
    		loadFlag = wnav.LOAD_FLAGS_BYPASS_HISTORY | wnav.LOAD_FLAGS_BYPASS_CACHE;
		}
		win.XulLibBrowser.webNavigation.loadURI(url, loadFlag, null, null, null);
	}
	
	function getBrowser(win) {
		try {
			return win.document.getElementById("xullib.browser");
		}
		catch(e) {}
	} 
	
	function addBrowserStateListener(win,listener) {
			if (!win.XulLibBrowser) {
				_err("no xullib.browser in ChromeWindow!");
				return false;
			}
			win.XULBrowserWindow.onStateChange = listener;
	}
	function addBrowserStatusListener(win,listener) {
			if (!win.XulLibBrowser) {
				_err("no xullib.browser in ChromeWindow!");
				return false;
			}
			win.XULBrowserWindow.onStatusChange = listener;
	}
	
	// utils, debugging and misc
	function getJSON(url,callback) {				
		Cc["@mozilla.org/network/io-service;1"]
			.getService(Ci.nsIIOService)
			.newChannel(url, "", null)
			.asyncOpen({
			_data: "",
			onDataAvailable: function (req, ctx, str, del, n) {
			  var ins = Cc["@mozilla.org/scriptableinputstream;1"]
				.createInstance(Ci.nsIScriptableInputStream)
			  ins.init(str)
			  this._data += ins.read(ins.available())
			},
			onStartRequest: function () {},

			onStopRequest: function () {
				try {
					var obj = JSON.parse(this._data);
					callback(obj);
				}
				catch(e) {
					_err(e);
					callback(false);
				}
			}
		}, null)
	}
	
	// for internal messages
	function _debug(msg) {
		debug(msg, "xullib");
	}
	function _out(msg) {
		out(msg, "xullib");
	}
	function _err(obj) {
		err(obj, "xullib");
	}
	
	function debug(msg, ctx) { // for debugging
		if (!DEBUG) return;	
		ctx = (ctx) ? ctx : APPNAME;		
		var str = ctx + ": " + msg;
		cs.logStringMessage(str);
		dump(str + lf);
	}
	
	function out(msg, ctx) { // for app messages
		ctx = (ctx) ? ctx : APPNAME;		
		var str = ctx + ": " + msg;
		cs.logStringMessage(str);
		dump(str + lf);
	}
	
	function err(obj,ctx) { // error messages
		ctx = (ctx) ? ctx : APPNAME;
		var str = ctx + ": " + obj;
		Cu.reportError(str);
		dump(str + lf);
	}	
	
	function getDebug() {
		return DEBUG;
	}
	
	function getType(obj) { // obsolet
		_debug("warn: getType() is obsolet");
		if (obj === null) return "[object Null]";
		try {
			return Object.prototype.toString.call(obj);
		}
		catch (e) {
			return typeof obj;
		}
	}
	
	function merge(o1,o2) {	
		var out = {};
		if(!arguments.length)
			return out;
		for(var i=0; i<arguments.length; i++) {
			for(var key in arguments[i]) {
				out[key] = arguments[i][key];
			}
		}
		return out;
	}
	
	function dumpObj(obj, name, indent, depth) {
		if (depth > MAX_DUMP_DEPTH) {
			 return indent + name + ": <Maximum Depth Reached>\n";
		}
		if (typeof obj == "object") {
			 var child = null;
			 var output = indent + name + "\n";
			 indent += "\t";
			 for (var item in obj)
			 {
				   try {
						  child = obj[item];
				   } catch (e) {
						  child = "<Unable to Evaluate>";
				   }
				   if (typeof child == "object") {
						  output += dumpObj(child, item, indent, depth + 1);
				   } else {
						  output += indent + item + ": " + child + "\n";
				   }
			 }
			 return output;
		} else {
			 return obj;
		}
	}
	
	function cloneObj(source) {
		for (i in source) {
			if (typeof source[i] == 'source') {
				this[i] = new cloneObject(source[i]);
			}
        else {
            this[i] = source[i];
		}
    }
}
	// checks namespaces: if no namespace exists add "app.name". namespace prefix
	function validateNamespace(a) {
		for (var k in a) {
			if (k.indexOf(".")<0) {
				try {
					_debug(APPNAME + '.' + k);
					a[APPNAME + '.' + k] = a.k;				
					delete a.k;
				}
				catch(e) {
					_err(e);
				}
			}
		}
		_debug(JSON.stringify(a));
		return a;
	}
	
	function getUUID() {
		//var uuid = uuidg.generateUUID();		
		return uuidg.generateUUID().toString().replace(/[^\da-f]/g,"");
	}
	
	function extractZip(zipFile, zipDir) { // both as nsIFile Objects
		zipr.open(zipFile);
		zipr.test(null);
		// first create directories		
		var entries = zipr.findEntries("*/");
		var root = zipDir.clone();
		while (entries.hasMore()) {
			root.append(entries.getNext().replace(/\/$/,""));
			if (root.exists()) {
				continue
			}
			else {
				root.create(Ci.nsIFile.DIRECTORY_TYPE,0777);
			}
			//zipDir.append()		
			//_debug(entries.getNext());
		}
	}
	
	// resolve Uri from string
	function resolveURI(path) {
		var ret;
		try {
			ret = cl.resolveURI(path);
		}
		catch (e) {
			_err(e);
			ret = false;
		}
		return ret;
	} 
	
	function getBool(b) {
		var ret;
		switch (b) {
			case "1":
			case 1:
			case "true":
			case true :
				ret = true;
			break;
			default: 
				ret = false;
		}
		return ret;
	}
	
	String.prototype.normalize = function () {
		return this.replace(/\W/g, "");
	}
	
	String.prototype.trim = function () {
		return this.replace(/^\s*/, "").replace(/\s*$/, "");
	}
	
	String.prototype.isEmpty = function () {
		return (!this || this == "[object Null]" || this == "" || this == "undefined");
	}
	
	// public functions
	return {
		Cc							:	Cc,
		Ci							:	Ci,
		Cu							:	Cu,
		Cr							:	Cr,
		APPNAME						:	APPNAME,
		ARRAY						:	ARRAY,
		BOOLEAN						:	BOOLEAN,
		NUMBER						:	NUMBER,
		OBJECT						:	OBJECT,
		STRING						:	STRING,
		addBrowserStateListener		:	addBrowserStateListener,
		addBrowserStatusListener	:	addBrowserStatusListener,
		addParams					:	addParams,
		addParam					:	addParam,
		addWin						:	addWin,
		debug						:	debug,
		dumpObj						:	dumpObj,
		err							:	err,
		getBool						:	getBool,
		getChromeWin				:	getChromeWin,
		getCmd						:	getCmd,
		getDebug					:	getDebug,
		getParam					:	getParam,
		getParams					:	getParams,
		getProfile					:	getProfile,
		getRecentWin				:	getRecentWin,		
		getType						:	getType,
		getMainWin					:	getMainWin,
		getWinFromRequest			:	getWinFromRequest,
		getWinFromUrl				:	getWinFromUrl,	
		getWins						:	getWins,
		getWinType					:	getWinType,
		getUserAppDir				:	getUserAppDir,
		init						:	init,
		loadPage					:	loadPage,
		out							:	out,
		openXullibWin				:	openXullibWin,
		quit						:	quit,
		removeSecondaryWins			:	removeSecondaryWins,
		removeWin					:	removeWin,
		resolveURI					:	resolveURI,
		setParam					:	setParam,
		showAllWin					:	showAllWin,
		toString					:	toString,
		winObserver					:	winObserver,
		XULLIB_WIN					:	XULLIB_WIN
	};
}());
