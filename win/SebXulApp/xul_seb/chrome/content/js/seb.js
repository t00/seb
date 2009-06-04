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
 * The Initial Developer of the Original Code is Justus-Liebig-Universitaet Giessen.
 * Portions created by the Initial Developer are Copyright (C) 2005
 * the Initial Developer. All Rights Reserved.
 *
 * Contributor(s):
 *   Stefan Schneider <stefan.schneider@hrz.uni-giessen.de>
 *   Oliver Rahs <rahs@net.ethz.ch>
 *
 * ***** END LICENSE BLOCK ***** */

var SebSystem = {
	_chrome :false,
	_prefs :null,
	_profile :null,
	_dirUtils :null,
	_browser :null,

	startup : function() {
		try {
			this._chrome = (top.window.toString().indexOf('ChromeWindow') > -1);
			this._prefs = new Prefs();
			this._profile = new Profile();
			this._dirutils = new DirUtils();
			this._browser = getBrowser();

			/** **** EventListener ****** */
			this._browser.addEventListener("load", function(e) {
				SebSystem.onContentLoad();
			}, false);

			if (this._chrome) {
				var urlExam = this.getUrlExam();
				if (urlExam == null || urlExam == "") {
					alert("A problem has occured:\nCannot find an URL for the exam.");
				}
				initBrowser(urlExam);
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
		this._browser.focus();
	},

	/** ***** Internal Callback functions ******* */
	onContentLoad : function() {
		alert("loaded");
	},

	getUrlExam : function() {
		var urlExam = this.getUrlExamFromWindowsRegistry();
		if (urlExam == null || urlExam == "") {
			urlExam = this.getUrlExamFromFileSebIni();
		}
		if (urlExam == null || urlExam == "") {
			urlExam = this.getUrlExamFromPreferences();
		}
		return urlExam;
	},

	getUrlExamFromWindowsRegistry : function() {
		try {
			var url = "";
			var reg = Components.classes["@mozilla.org/windows-registry-key;1"].createInstance(Components.interfaces.nsIWindowsRegKey);
			reg.open(reg.ROOT_KEY_LOCAL_MACHINE, "SOFTWARE\\Policies", reg.ACCESS_READ);
			if (reg.hasChild("SEB")) {
				var subKey = reg.openChild("SEB", reg.ACCESS_READ);
				if (subKey.hasValue("UrlExam")) {
					url = subKey.readStringValue("UrlExam");
				}
			}
			reg.close();
			return url;
		} catch (ex) {
			return "";
		}
	},

	getUrlExamFromFileSebIni : function() {
		try {
			var url = "";
			var sebConfig = new File(this._prefs.getChar('seb.configuration.file'));
			var lines = sebConfig.readAllLines();
			for ( var i = 0; i < lines.length; i++) {
				if (lines[i].indexOf("URL_EXAM") != -1) {
					url = lines[i].substr(lines[i].indexOf("=") + 1);
				}
			}
			return url;
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