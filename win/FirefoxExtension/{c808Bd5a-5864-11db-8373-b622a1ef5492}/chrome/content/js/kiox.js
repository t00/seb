/*
ToDos:
	
1. All preferences 
	- newwindow
	- proxy
	- allowed urls
	- hide.* ("aktuelle Nachrichten" und den ganzen Spökes)
	- Why firefox opens google or java.sun page if it can not reach a given address????
	
2. Error-Handling
	
*/

var KioxSystem = {
	_chrome        	: false,
	_prefs         	: null,
	_profile       	: null,
	_dirUtils      	: null,
	_browser		: null,
	
	startup: function() {
		try {
			//is opened as standalone application and not in Firefox with url "chrome://kiox/content/"
			this._chrome = (top.window.toString().indexOf('ChromeWindow') > -1);
			this._prefs = new Prefs();
			this._profile = new Profile();
			this._dirutils = new DirUtils();
			this._browser = getBrowser();
			//alert(this._prefs.getBool('kiox.toolbar.enable'));
			//this._prefs.save('\\\\three\\kiox_dev\\extensions\\{c808Bd5a-5864-11db-8373-b622a1ef5492}\\test.js');
			//alert(this._dirutils.getUserChromeDir());
			
			/****** EventListener *******/					
			this._browser.addEventListener("load", function(e) { KioxSystem.onContentLoad(); }, false);
						
			/****** Broadcasters *******/ 
			document.getElementById("bcShowToolbar").setAttribute("hidden",(!(this._prefs.getBool('kiox.toolbar.enable'))));	
			document.getElementById("bcShowToolbarNavigation").setAttribute("hidden",(!(this._prefs.getBool('kiox.toolbar.navigation'))));      
			
			//standalone application (start-kiox.bat)
			if (this._chrome) { 
				initBrowser(this._prefs.getChar('kiox.startup.homepage'));				
			}
			//opened in a firefox window (start-firefox.bat) 
			//put "chrome://kiox/content/" into the locationbar if it doesn't start automatically
			else {
				this.showOptions();
			}
		}
		catch (e) {
			alert(e);
		}
	},
	
	shutdown : function() {
	},
	
	showLoading : function() {
		if (this._browser.contentWindow.location.href == "about:blank") {
			//Initial Loading IFrame
			document.getElementById("deckContents").selectedIndex = 0;
			document.getElementById("deckLoader").hidden = true;
		}
		//change loading circle in toolbar if shown
		else {
			document.getElementById("deckLoader").selectedIndex = 1;
		}
	},
		
	showContent : function() {
		document.getElementById("deckContents").selectedIndex = 1;
		if (document.getElementById("deckLoader").hidden)  document.getElementById("deckLoader").hidden = false;
		document.getElementById("deckLoader").selectedIndex = 0;
		this._browser.focus();
	},
	
	showOptions : function() {
		document.getElementById("deckContents").selectedIndex = 2;
	},
	
	/******* Internal Callback functions ********/
	onContentLoad : function () {
		alert("loaded");
	}
} 
/******** Top EventListener ******/

window.addEventListener("load", function(e) { KioxSystem.startup(); }, false);
window.addEventListener("unload", function(e) { KioxSystem.shutdown(); }, false);

