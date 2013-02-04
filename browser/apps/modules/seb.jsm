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
Components.utils.import("resource://gre/modules/NetUtil.jsm");

var seb = (function() {
	// XPCOM Services, Interfaces and Objects
	const	x					=	xullib,
			Cc					=	x.Cc,
			Ci					=	x.Ci,
			Cu					=	x.Cu,
			Cr					=	x.Cr,
			prefs				=	Services.prefs,
			prompt				= 	Services.prompt,
			wpl					=	Ci.nsIWebProgressListener,
			xulFrame			=	"xullib.frame",
			xulBrowser			=	"xullib.browser",
			xulErr				=	"chrome://seb/content/err.xul",
			xulLoad				=	"chrome://seb/content/load.xul",
			errDeck				=	0,
			loadDeck			=	0,
			contentDeck			=	1;
			
	var 	__initialized 		= 	false,
			locked				=	true,
			url					=	"",
			locs				=	null, 
			consts				=	null,
			mainWin				=	null,
			netMaxTimes			=	0,
			netTimeout			=	0,
			net_tries			=	0,
			whiteListRegs		=	[],
			blackListRegs		= 	[],
			convertReg			= 	/[-\[\]\/\{\}\(\)\+\?\.\\\^\$\|]/g,
			wildcardReg			=	/\*/g,
			shutdownObserver = {
				observe	: function(subject, topic, data) {
					if (topic == "xpcom-shutdown") {
						// x.getProfile().obj.remove(true);
						let p = x.getProfile();
						for (var i=0;i<p.dirs.length;i++) {
							p.dirs[i].remove(true);
						}
						// on Linux everything will be deleted
						x.getUserAppDir().remove(true);
						// on windows the read-only file "parent.lock" blocks recursive removing of app folder "seb"
						// setting permissions on parent.lock p.e. 0660 before removing throws an error
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
						x.debug("network stop: content to show: " + aRequest.name);
						let win = x.getChromeWin(aWebProgress.DOMWindow);											
						showContent(win);
					}
					if (aStateFlags & wpl.STATE_START) {												
						try {		
							x.debug("network start: request: " + aRequest.name);
							if (aRequest && aRequest.name) {
								let win = x.getChromeWin(aWebProgress.DOMWindow);
								if (!isValidUrl(aRequest.name)) {
									aRequest.cancel(aStatus);
									prompt.alert(win, getLocStr("seb.title"), getLocStr("seb.url.blocked"));
									if (win === mainWin) {
										shutdown();
									}
									return 1; // 0?
								}
								// don't allow multiple popup instances with the same url: experimental, does not work on links in popup windows
								if (x.getParam("seb.distinct.popup") && (win !== mainWin)) {
 									let w = x.getWinFromRequest(aRequest.name); // find already opened popup with the same url								
									if (typeof w === "object") {
										aRequest.cancel(aStatus); // if found, cancle new request
										x.removeWin(win); // remove the new window with canceled request from internal window array 
										win.close(); // close the win
										w.focus();	// focusing the already opened window from the internal array
										return 1; // 0?								
									}
									else {
										x.debug("set request " + aRequest.name + " for popup.");
										win.XulLibBrowser.setAttribute("request",aRequest.name);
									}
								}												
							}							
						}
						catch(e) {
							x.err(e);
						}
					}
					return 0;
				}
			};
			
	function toString () {
			return "seb";
	}
	
	function init(win) {
		Cc["@mozilla.org/net/osfileconstantsservice;1"].getService(Ci.nsIOSFileConstantsService).init();
		x.debug("init window");
		x.addWin(win);
		getKeys(win); 					// for every window call
		setBrowserHandler(win); 			// for every window call
		if  (x.getWinType(win) == "main") {
			setShutdownHandler(win);
			mainWin = win;
			initMain(win);
		}
		else {	
			let width = x.getParam("seb.openwin.width");
			let height = x.getParam("seb.openwin.height");
			if (width && height) {
				x.debug("resize secondary window to: " + width + "x" + height);
				win.resizeTo(width,height);
			}
		} 	
	}
	
	function initMain(win) {
		if (__initialized) {
			x.err("something is going wrong. The main seb window is already initialized!");
			return;
		}
				
		if (x.getParam("seb.removeProfile")) {
			shutdownObserver.register();
		}
		if (!x.getParam("seb.trusted.content")) {
			httpRequestObserver.register();	
		}
		url = getUrl();
		if (!url) {
			x.err("could not get url!");
			return false;
		}
		x.debug("seb init with url: " + url);
		setListRegex(); 								// compile regexs 
		locs = win.document.getElementById("locale");	
		consts = win.document.getElementById("const");
		setTitlebar(win);									
		setLocked();
		setUnlockEnabled();
		showLoading(win);
		netMaxTimes = x.getParam("seb.net.max.times");
		netTimeout = x.getParam("seb.net.timeout");
		loadPage(url);
		x.out("seb started...");
		__initialized = true;
	}
	
	function loadPage(url) {
		// loadPage wrapper:
		// sometimes there are initial connection problems with live boot media like sebian
		// try to get a valid connection for MAX_NET_TRIES times after NET_TRIES_TIMEOUT
		// call loadURI only if the NetChannel connection is successful		
		let uri = NetUtil.newURI(url);
		let channel = NetUtil.newChannel(uri);
		channel.QueryInterface(Components.interfaces.nsIHttpChannel);
		channel.requestMethod = "HEAD";
		
		//debug(channel.notificationCallbacks);
		NetUtil.asyncFetch(channel, function(inputStream, status) {
										net_tries += 1;
										if (net_tries > netMaxTimes) {
											net_tries = 0; // reset net_tries 
											// try anyway and take a look what happens (ugly: // ToDo: internal error page and detailed response headers)
											showError(mainWin);
											return;
										}
										if (!Components.isSuccessCode(status)) {  // ToDo: detailed response header
											x.debug(net_tries + ". try: could not open " + uri.spec + " - status " + status);
											mainWin.setTimeout(function() { loadPage(url); },netTimeout);
											//let metaString = NetUtil.readInputStreamToString(inputStream, inputStream.available());
											//debug(metaString);
											//return;  
										}
										else {
											//_debug("channel response code: " + channel.getResponse);
											x.loadPage(mainWin,url);
										}										
									});
	}
	
	function onclose(win) { // will only be handled for secondary wins
		// the close event does not fire on modal windows like the Sonderzeichen dialog in ILIAS
		// therefore only the onunload event will be catched for cleaning the internal array
		if (win === mainWin) {
			return;
		}
		x.debug("close secondary win");
	}
	
	function onunload(win) { // will only be handled for secondary wins
		if (win === mainWin) {
			return;
		}
		x.debug("unload secondary win");
		x.removeWin(win);
	}
	
	function setShutdownHandler(win) {
		x.debug("setShutdownHandler");
		win.addEventListener( "close", shutdown, true); // controlled shutdown for main window
	}
	
	function setBrowserHandler(win) { // Event handler for both wintypes
		x.debug("setBrowserHandler");
		x.addBrowserStateListener(win,browserStateListener); // for both types
	}
	
	// app lifecycle
	function shutdown(e) { // only for mainWin
		var w = mainWin;
		if (e != null) { // catch event
			e.preventDefault();
			e.stopPropagation();				
		}
		x.debug("try shutdown...");
		if (locked) {
			x.out("no way! seb is locked :-)");
		}
		else {
			for (var i=x.getWins().length-1;i>=0;i--) { // ich nehm Euch alle MIT!!!!
				try {
					x.debug("close window ...");
					x.getWins()[i].close();
				}
				catch(e) {
					x.err(e);
				}
			}
		}
	}
	
	// browser and seb windows
	function showLoading(win) {
		let w = (win) ? win : x.getRecentWin();
		x.debug("showLoading...");
		getFrameElement(w).setAttribute("src",xulLoad);
		setDeckIndex(w,loadDeck);
	}
	
	function showError(win) {
		let w = (win) ? win : x.getRecentWin();
		x.debug("showError...");
		getFrameElement(w).setAttribute("src",xulErr);
		setDeckIndex(w,errDeck);
	}
	
	function showContent(win) { 
		let w = (win) ? win : x.getRecentWin();;
		x.debug("showContent..." + x.getWinType(w));
		setDeckIndex(w,contentDeck);
		try {
			w.document.title = w.content.document.title;
		}
		catch(e) {}
		w.focus();
		w.XulLibBrowser.focus();
	}
	
	function showAll() {
		x.debug("show all...");
		x.showAllWin();
	}
	
	function reload(win) {
		x.debug("try reload...");
		net_tries = 0;
		var doc = (win) ? win.content.document : mainWin.content.document;
		doc.location.reload();
		x.debug("reload...");
	}
	
	function restart() { // only mainWin, experimental
		net_tries = 0;
		if (x.getParam("seb.restart.mode") === 0) {
			return;
		}
		if ((x.getParam("seb.restart.mode") === 1) && (getFrameElement().getAttribute("src") != xulErr)) {
			return;
		}
		x.debug("restart...");
		x.removeSecondaryWins();
		let url = getUrl();
		showLoading(mainWin);
		loadPage(url);
	}
	
	function load() {		
		x.debug("try load...");
		if (typeof x.getParam("seb.load") != "string" || x.getParam("seb.load") == "") return;
		var doc = mainWin.content.document;
		var url = x.getParam("seb.load");
		var ref = doc.location.href;
		var refreg = "";
		if (typeof x.getParam("seb.load.referrer.instring") === "string") {
			refreg = x.getParam("seb.load.referrer.instring");
		}
		if (refreg != "") {
			if (ref.indexOf(refreg) > -1) {
				if (isValidUrl(url)) {
					x.debug("load from command " + url);
					doc.location.href = url;
				}
				else {
					prompt.alert(mainWin, getLocStr("seb.title"), getLocStr("seb.url.blocked"));
				}
				return false;
			}
			else {
				x.debug("loading \"" + url + "\" is only allowed if string in referrer: \"" + refreg + "\"");
				return false;
			}
		}
		else {
			x.debug("load from command " + url);
			doc.location.href = url;
		}
	}
	
	/* locales const, UI keys and commands */
	function getDeck(win) {
		let w = (win) ? win	: x.getRecentWin();
		return w.document.getElementById("deckContents");
	}
	
	function getDeckIndex(win) {
		let w = (win) ? win	: x.getRecentWin();
		return getDeck(win).selectedIndex;
	}
	
	function setDeckIndex(win,index) {
		let w = (win) ? win	: x.getRecentWin();
		getDeck(win).selectedIndex = index;
	}
	
	function getFrameElement(win) {
		let w = (win) ? win	: x.getRecentWin();
		return w.document.getElementById(xulFrame);
	}
	
	function getBrowserElement(win) {
		let w = (win) ? win	: x.getRecentWin();
		return w.document.getElementById(xulBrowser);
	}
	
	function getBrowserElement(win) {
	}
	
	function getLocStr(k) {
		return locs.getString(k);
	}
	
	function getConstStr(k) {
		return consts.getString(k);
	}
	
	function setLocked(lock) {
		locked = (lock != null) ? lock : x.getParam('seb.locked');
	}
	
	function setUnlockEnabled() {
		unlockEnabled = x.getParam('seb.unlock.enabled');
	}
	
	function setTitlebar(win) {
		let w = (win) ? win : x.getRecentWin(); 
		w.document.getElementById("sebWindow").setAttribute("hidechrome",!x.getParam('seb.mainWindow.titlebar.enabled'));
		x.debug("hidechrome " + w.document.getElementById("sebWindow").getAttribute("hidechrome"));
	}
	
	function lock() {
		if (locked) return;
		setLocked(true);
		x.debug("lock...");
	}
	
	function unlock() {
		if (!locked) return;
		if (!unlockEnabled) return;
		setLocked(false);	
		x.debug("unlock...");
	}
	
	function getKeys(win) {		
		var ks = win.document.getElementsByTagName("key");		
		for (var i=0;i<ks.length;i++) {
			var p = ks[i].id;
			var kc = p + ".keycode";
			var md = p + ".modifiers";
			
			if (x.getParam(kc)) {
				ks[i].setAttribute("keycode", x.getParam(kc)); 
				x.debug(kc + " set to " + x.getParam(kc));
			}
			else {
				ks[i].setAttribute("keycode", "");
				x.debug(kc + " set to ''");
			}
			if (x.getParam(md)) {
				ks[i].setAttribute("modifiers", x.getParam(md));
				x.debug(md + " set to " + x.getParam(md));
			}
			else {
				ks[i].setAttribute("modifiers", "");
				x.debug(md + " set to ''");
			}
		}
	}
	
	// url processing
	function getUrl() {
		let url = x.getCmd("url");
		if (url !== null) {
			return url;
		}
		url = x.getParam("seb.url");
		if (url !== undefined) {
			return url;
		}
		return false;
	}
	
	
	function setListRegex() { // for better performance compile RegExp objects and push them into arrays 
		var bs;
		var ws
		var is_regex = (typeof x.getParam("seb.pattern.regex") === "boolean") ? x.getParam("seb.pattern.regex") : false;		
		var b = (typeof x.getParam("seb.blacklist.pattern") === "string") ? x.getParam("seb.blacklist.pattern") : false;
		var w = (typeof x.getParam("seb.whitelist.pattern") === "string") ? x.getParam("seb.whitelist.pattern") : false;		
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
		x.debug("check url: " + url);
		msg = "NOT VALID: " + url + " is not allowed!";							
		for (var i=0;i<blackListRegs.length;i++) {
			if (blackListRegs[i].test(url)) {
				m = true;
				break;
			}
		}
		if (m) {
			x.debug(msg);				
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
			x.debug(msg);
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
		init			:	init,
		onunload		:	onunload,
		onclose			:	onclose,
		lock			:	lock,
		unlock			:	unlock,
		shutdown		:	shutdown,
		reload			:	reload,
		restart			:	restart,
		load			:	load,
		showAll			:	showAll,
		showError		:	showError		
	};	
}());

