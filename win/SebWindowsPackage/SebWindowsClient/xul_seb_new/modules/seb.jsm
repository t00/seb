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
 * The Original Code is the browser component of seb.
 *
 * The Initial Developer of the Original Code is Stefan Schneider <schneider@hrz.uni-marburg.de>.
 * Portions created by the Initial Developer are Copyright (C) 2005
 * the Initial Developer. All Rights Reserved.
 *
 * Contributor(s):
 *   Stefan Schneider <schneider@hrz.uni-marburg.de>
 *   
 * ***** END LICENSE BLOCK ***** */

/* ***** GLOBAL seb SINGLETON *****

* *************************************/ 

/* 	for javascript module import
	see: https://developer.mozilla.org/en/Components.utils.import 
*/
var EXPORTED_SYMBOLS = ["seb"];
Components.utils.import("resource://modules/xullib.jsm");
Components.utils.import("resource://gre/modules/FileUtils.jsm");
Components.utils.import("resource://gre/modules/Services.jsm");

var seb = (function() {
	// XPCOM Services, Interfaces and Objects
	const	x					=	xullib,
			Cc					=	x.Cc,
			Ci					=	x.Ci,
			Cu					=	x.Cu,
			Cr					=	x.Cr,
			prefs				=	Services.prefs,
			prompt				= 	Services.prompt,
			wpl					=	Ci.nsIWebProgressListener;
			
	let 	__initialized 		= 	false,
			locked				=	true,
			url					=	"",
			locs				=	null, 
			consts				=	null,
			mainWin				=	null,
			whiteListRegs		=	[],
			blackListRegs		= 	[],
			convertReg			= 	/[-\[\]\/\{\}\(\)\+\?\.\\\^\$\|]/g,
			wildcardReg			=	/\*/g,
			out					=	function(msg) 			{ return x.out(msg, "seb"); },
			debug				=	function(msg) 			{ return x.debug(msg, "seb"); },
			err					=	function(msg) 			{ return x.err(msg, "seb"); },
			getParam			=	function(param)			{ return x.getParam(param);	},
			shutdownObserver = {
				observe	: function(subject, topic, data) {
					if (topic == "xpcom-shutdown") {
						x.getProfile().obj.remove(true);
						
						// on windows the read-only file "parent.lock" blocks recursive removing of app folder "seb"
						// setting permissions on parent.lock p.e. 0660 before removing throws an error
						// the next time seb is shutting down, the unused and unlocked folder will be deleted.
						// ToDo: force Unlock of profile see nsIProfileUnlocker, nsIProfileLocker
						// don't know how to get a reference to these objects, needs a hook in startup to observe the lock event?
						
						// on Linux everything will be deleted
						x.getUserAppDir().remove(true); 
					}	  
				},
				get observerService() {  
					return Cc["@mozilla.org/observer-service;1"].getService(Ci.nsIObserverService);  
				},
				register: function() {  
					this.observerService.addObserver(this, "xpcom-shutdown", false);  
				},  
				unregister: function()  {  
					this.observerService.removeObserver(this, "xpcom-shutdown");  
				}  
			},
			httpRequestObserver = {
				observe	: function(subject, topic, data) {
					if (topic == "http-on-modify-request") {
						let url;
						subject.QueryInterface(Ci.nsIHttpChannel);
						url = subject.URI.spec; 
						if (!isValidUrl(url)) {
							subject.cancel(Cr.NS_BINDING_ABORTED);
						}
						//httpChannel.setRequestHeader("X-Hello", "World", false);  
					}  
				},
  
				get observerService() {  
					return Components.classes["@mozilla.org/observer-service;1"].getService(Components.interfaces.nsIObserverService);  
				},
			  
				register: function() {  
					this.observerService.addObserver(this, "http-on-modify-request", false);  
				},  
			  
				unregister: function()  {  
					this.observerService.removeObserver(this, "http-on-modify-request");  
				}  
			},
			browserStateListener = function(aWebProgress, aRequest, aStateFlags, aStatus) {
				if(aStateFlags & wpl.STATE_IS_NETWORK) {
					if (aStateFlags & wpl.STATE_STOP) {
						showContent();
					}
					if (aStateFlags & wpl.STATE_START) {
						try {		
							if (aRequest && aRequest.name) {
								if (!isValidUrl(aRequest.name)) {
									aRequest.cancel(aStatus);
									prompt.alert(mainWin, getLocStr("seb.title"), getLocStr("seb.url.blocked"));
									shutdown();
									return 1; // 0?
								}														
							}
						}
						catch(e) {
							err(e);
						}
						showLoading();
					}
					return 0;
				}
			};
			
	function toString () {
			return "seb";
	}
	
	function init(win) {			
		x.addWin(win); 					// for every window call		
		getKeys(win); 					// for every window call
		setEventHandler(win); 			// for every window call		
		if  (__initialized) return;	
		mainWin = x.getWin();				
		if (getParam("seb.removeProfile")) {
			shutdownObserver.register();
		}
		if (!getParam("seb.trusted.content")) {
			httpRequestObserver.register();	
		}
		url = getUrl();
		if (!url) {
			err("could not get url!");
			return false;
		}
		debug("seb init with url: " + url);
		setListRegex(); 								// compile regexs 
		locs = win.document.getElementById("locale");	
		consts = win.document.getElementById("const");
		setTitlebar();									
		setLocked();
		setUnlockEnabled();
		x.loadPage(mainWin, url);
		out("seb started...");
		__initialized = true;
	}
	
	function setEventHandler(win) {
		win.addEventListener( "close", shutdown, true); // controlled shutdown
		x.addBrowserStateListener(win,browserStateListener);
		//x.addBrowserStatusListener(win,browserStatusListener);
	}
	
	// locales consts
	function getLocStr(k) {
		return locs.getString(k);
	}
	function getConstStr(k) {
		return consts.getString(k);
	}
	
	// app lifecycle
	function shutdown(e) {
		var w = (e) ? e.originalTarget : mainWin;
		if (e != null) { // catch event
				e.preventDefault();
				e.stopPropagation();				
		}
		var wintype = x.getWinType(w);
		if (wintype == "secondary") { // fired on popup: just remove it from internal array 
			x.removeWin(w);
			w.close();
			debug("close " + x.getWinType(w) + ": " + w.document.title);
		}
		else { // fired on main: control locked and shutdown everything else
			debug("try shutdown...");
			if (locked) {
				out("no way! seb is locked :-)");
			}
			else {
				Services.startup.quit(Services.startup.eForceQuit);
				/*
				for (var i=x.getWins().length-1;i>=0;i--) { // ich nehm Euch alle MIT!!!!
					try {
						var n = (x.getWins()[i].document) ? x.getWinType(x.getWins()[i]) + ": " + x.getWins()[i].document.title : " empty document";
						debug("close " + n);
						x.getWins()[i].close();
					}
					catch(e) {
						err(e);
					}
				}
				*/
			}
		}
	}
	
	// browser
	function showContent() {
		debug("showContent...");
		mainWin.document.getElementById("deckContents").selectedIndex = 1;
		mainWin.document.title = mainWin.content.document.title;
		mainWin.XulLibBrowser.focus();
	}
	
	function showLoading() {
		if (mainWin.content.location.href == "about:blank") {
			debug("loading...");
			mainWin.document.getElementById("deckContents").selectedIndex = 0;
		}
	}
	
	function reload(win) {
		debug("try reload...");
		var doc = (win) ? win.content.document : mainWin.content.document;
		doc.location.reload();
		out("reload...");
	}
	
	function load() {		
		debug("try load...");
		if (typeof getParam("seb.load") != "string" || getParam("seb.load") == "") return;
		var doc = mainWin.content.document;
		var url = getParam("seb.load");
		var ref = doc.location.href;
		var refreg = "";
		if (typeof getParam("seb.load.referrer.instring") === "string" && getParam("seb.load.referrer.instring") != "") {
			refreg = getParam("seb.load.referrer.instring");
		}
		if (refreg != "") {
			if (ref.indexOf(refreg) > -1) {
				if (isValidUrl(url)) {
					debug("load from command " + url);
					doc.location.href = url;
				}
				else {
					prompt.alert(mainWin, getLocStr("seb.title"), getLocStr("seb.url.blocked"));
				}
				return false;
			}
			else {
				out("loading \"" + url + "\" is only allowed if string in referrer: \"" + refreg + "\"");
				return false;
			}
		}
		else {
			out("load from command " + url);
			doc.location.href = url;
		}
	}
	
	/* UI keys and commands */
	function setLocked(lock) {
		locked = (lock != null) ? lock : getParam('seb.locked');
	}
	
	function setUnlockEnabled() {
		unlockEnabled = getParam('seb.unlock.enabled');
	}
	
	function setTitlebar() {
		mainWin.document.getElementById("sebWindow").setAttribute("hidechrome",!getParam('seb.mainWindow.titlebar.enabled'));
		debug("hidechrome " + mainWin.document.getElementById("sebWindow").getAttribute("hidechrome"));
	}
	
	function lock() {
		if (locked) return;
		setLocked(true);
		out("lock...");
	}
	
	function unlock() {
		if (!locked) return;
		if (!unlockEnabled) return;
		setLocked(false);	
		out("unlock...");
	}
	
	function getKeys(win) {		
		var ks = win.document.getElementsByTagName("key");		
		for (var i=0;i<ks.length;i++) {
			var p = ks[i].id;
			var kc = p + ".keycode";
			var md = p + ".modifiers";
			
			if (getParam(kc)) {
				ks[i].setAttribute("keycode", getParam(kc)); 
				debug(kc + " set to " + getParam(kc));
			}
			if (getParam(md)) {
				ks[i].setAttribute("modifiers", getParam(md));
				debug(md + " set to " + getParam(md));
			}
		}
	}
	
	// url processing
	function getUrl() {
		let url = x.getCmd("url");
		if (url !== null) {
			return url;
		}
		url = getParam("seb.url");
		if (url !== undefined && url != "") {
			return url;
		}
		return false;
	}
	
	function setListRegex() { // for better performance compile RegExp objects and push them into arrays 
		var bs;
		var ws
		var is_regex = (typeof getParam("seb.pattern.regex") === "boolean") ? getParam("seb.pattern.regex") : false;		
		var b = (typeof getParam("seb.blacklist.pattern") === "string" && getParam("seb.blacklist.pattern") != "") ? getParam("seb.blacklist.pattern") : false;
		var w = (typeof getParam("seb.whitelist.pattern") === "string" && getParam("seb.whitelist.pattern") != "") ? getParam("seb.whitelist.pattern") : false;		
		if (b) {
			bs = b.split(";");
			for (var i=0;i<bs.length;i++) {
				if (is_regex) {
					blackListRegs.push(new RegExp(bs[i]));
				}
				else {
					blackListRegs.push(new RegExp(getRegex(bs[i])));
				}
			}
		}
		if (w) {
			ws = w.split(";");
			for (var i=0;i<ws.length;i++) {
				if (is_regex) {
					whiteListRegs.push(new RegExp(ws[i]));
				}
				else {
					whiteListRegs.push(new RegExp(getRegex(ws[i])));
				}
			}
		}
	}
	
	function getRegex(p) {
		var reg = p.replace(convertReg, "\\$&");
		reg = reg.replace(wildcardReg,".*?");
		return reg;
	}
	
	function isValidUrl(url) {
		if (whiteListRegs.length == 0 && blackListRegs.length == 0) return true;
		var m = false;
		var msg = "";		
		debug("check url: " + url);
		msg = "NOT VALID: " + url + " is not allowed!";							
		for (var i=0;i<blackListRegs.length;i++) {
			if (blackListRegs[i].test(url)) {
				m = true;
				break;
			}
		}
		if (m) {
			debug(msg);				
			return false; 
		}
		if (whiteListRegs.length == 0) {
			return true;
		}
		for (var i=0;i<whiteListRegs.length;i++) {
			if (whiteListRegs[i].test(url)) {
				m = true;
				break;
			}
		}
		if (!m) {								
			debug(msg);
			return false;
		}
		return true;	
	}
	
	String.prototype.trim = function () {
		return this.replace(/^\s*/, "").replace(/\s*$/, "");
	}
	
	/* export public functions */
	return {
		toString 		: 	toString,
		init 			: 	init,
		lock			:	lock,
		unlock			:	unlock,
		shutdown		:	shutdown,
		reload			:	reload,
		load			:	load		
	};	
}());

