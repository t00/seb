/* ***** BEGIN LICENSE BLOCK *****
 * Version: MPL 1.1
 *
 * The contents of this file are subject to the Mozilla Public License Version
 * 1.1 (the "License"); you may not use this file except in compliance with
 * the License. You may obtain a copy of the License at
 * http://www.mozilla.org/MPL/
 *
 * Software distributed under the License is distributed on an "AS IS" basis,
 * WITHOUT WARRANTY OF ANY KIND, either express or implied. See the License
 * for the specific language governing rights and limitations under the
 * License.
 *
 * The Original Code is the browser component of SEB.
 *
 * The Initial Developer of the Original Code is Stefan Schneider <stefan.schneider@uni-hamburg.de>.
 * Portions created by the Initial Developer are Copyright (C) 2005
 * the Initial Developer. All Rights Reserved.
 *
 * Contributor(s):
 *   Stefan Schneider <stefan.schneider@uni-hamburg.de>
 *   Oliver Rahs <rahs@net.ethz.ch>
 *
 * ***** END LICENSE BLOCK ***** */

var SebSystem = {
	_chrome :false,
	_prefs :null,
	_profile :null,
	_dirUtils :null,
	_chromeDir : null,
	_appDir : null,
	_sebIni : null,
	_browser :null,
	_locale  :null,
	_const: null,

	startup : function() {
		try {
			this._chrome = (top.window.toString().indexOf('ChromeWindow') > -1);
			this._prefs = new Prefs();
			this._profile = new Profile();
			this._dirutils = new DirUtils();
			this._browser = getBrowser();
			this._chromeDir = new Dir(this._dirutils.getChromeDir());
			this._appDir = this._chromeDir.parent.parent;
			this._sebIni = this._appDir.clone();
			this._sebIni.append(this._prefs.getChar('seb.configuration.filename'));
			this._locale = document.getElementById("locale");
			this._const = document.getElementById("const");
			
			/** **** EventListener ****** */
			
			if (this._chrome) {
				var url = this.getUrl();
				if (url == null || url == "") {
					alert("A problem has occured:\nNo URL defined for SEB.");
				}
				initBrowser(url);
			}
		} catch (e) {
			alert(e);
		}
	},

	shutdown : function() {
	},

	showLoading : function() {
		if (this._browser.contentWindow.location.href == "about:blank") {
			document.getElementById("deckContents").selectedIndex = 0;
		}
	},
	
	showContent : function() {
		document.getElementById("deckContents").selectedIndex = 1;
		window.title = this._browser.webNavigation.document.title;
		this._browser.focus();
	},
	
	/** ***** Internal Callback functions ******* */
	
	getUrl : function() {
		var url = "";
		if (window.arguments[0] != null) {
			var cmdLine = window.arguments[0].QueryInterface(Components.interfaces.nsICommandLine);
			url = cmdLine.handleFlagWithParam("url", false);
		}
		if (url == null || url == "") {
			url = this.getUrlExamFromWindowsRegistry();
		}
		if (url == null || url == "") {
			url = this.getUrlExamFromFileSebIni();
		}
		if (url == null || url == "") {
			url = this.getUrlExamFromPreferences();
		}
		return url;
	},

	getUrlExamFromWindowsRegistry : function() {
		try {
			var urlExam = "";
			var reg = Components.classes["@mozilla.org/windows-registry-key;1"].createInstance(Components.interfaces.nsIWindowsRegKey);
			reg.open(reg.ROOT_KEY_LOCAL_MACHINE, "SOFTWARE\\Policies", reg.ACCESS_READ);
			if (reg.hasChild("SEB")) {
				var subKey = reg.openChild("SEB", reg.ACCESS_READ);
				if (subKey.hasValue("UrlExam")) {
					urlExam = subKey.readStringValue("UrlExam");
				}
			}
			reg.close();
			return urlExam;
		} catch (ex) {
			return "";
		}
	},

	getUrlExamFromFileSebIni : function() {
		try {
			var urlExam = "";
			// first try to load hard coded path to *.ini file in prefs.js
			var sebConfig = new File(this._prefs.getChar('seb.configuration.file'));
			// if not exist get file with 'seb.configuration.filename' in app directory
			if (!sebConfig.exists()) {
				sebConfig = new File(this._sebIni.path); 
			}
			var lines = sebConfig.readAllLines();
			for ( var i = 0; i < lines.length; i++) {
				var reg = /^\s?URL_EXAM\s?=\s?(.+)?\s?$/;
				var ret = reg.exec(lines[i]);
				if (ret) {
					urlExam = ret[1];
				}
			}
			return urlExam;
		} catch (ex) {
			return "";
		}
	},

	getUrlExamFromPreferences : function() {
		return this._prefs.getChar('seb.startup.homepage');
	}
}

/** ****** Top EventListener ***** */

window.addEventListener("load", function(e) {
	SebSystem.startup();
}, false);

window.addEventListener("unload", function(e) {
	SebSystem.shutdown();
}, false);