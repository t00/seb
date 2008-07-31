// Globals
var myBrowser = null;
var appCore = null;

const STATE_START = Components.interfaces.nsIWebProgressListener.STATE_START;
const STATE_STOP = Components.interfaces.nsIWebProgressListener.STATE_STOP;
const STATE_IS_WINDOW = Components.interfaces.nsIWebProgressListener.STATE_IS_WINDOW;
const STATE_IS_DOCUMENT = Components.interfaces.nsIWebProgressListener.STATE_IS_DOCUMENT;
const STATE_IS_NETWORK = Components.interfaces.nsIWebProgressListener.STATE_IS_NETWORK;

function initBrowser(url)  {					
    myBrowser = getBrowser();
		
    appCore = Components.classes["@mozilla.org/appshell/component/browser/instance;1"]
               .createInstance(Components.interfaces.nsIBrowserInstance);
    appCore.setWebShellWindow(window);
    
    // Following lines needed for observing Forward and Back buttons
    window.XULBrowserWindow = new nsBrowserStatusHandler();
    // hook up UI through progress listener
    var interfaceRequestor = myBrowser.docShell.QueryInterface(Components.interfaces.nsIInterfaceRequestor);
    var webProgress = interfaceRequestor.getInterface(Components.interfaces.nsIWebProgress);
    webProgress.addProgressListener(window.XULBrowserWindow, Components.interfaces.nsIWebProgress.NOTIFY_ALL);		    
    if (url) loadPage(url);
    //return myBrowser;
}

function getBrowser()  {
    if (!myBrowser)
        myBrowser = document.getElementById("browserContent");
    return myBrowser;
}

function loadPage(uri,loadFlag)  {
		const nsIWebNavigation = Components.interfaces.nsIWebNavigation;						
		if (typeof(loadFlag) == "undefined") {
    	//loadFlag = nsIWebNavigation.LOAD_FLAGS_BYPASS_HISTORY;
    	loadFlag = nsIWebNavigation.LOAD_FLAGS_BYPASS_HISTORY;
  	}  	
    //getBrowser().webNavigation.loadURI(uri, nsIWebNavigation.LOAD_FLAGS_BYPASS_HISTORY, null, null, null);
    getBrowser().webNavigation.loadURI(uri, nsIWebNavigation.LOAD_FLAGS_NONE, null, null, null);		
}

function goBack()
{
    var webNavigation = getBrowser().webNavigation;
    if (webNavigation.canGoBack)
        webNavigation.goBack();
}

function goForward()
{
    var webNavigation = getBrowser().webNavigation;
    if (webNavigation.canGoForward)
        webNavigation.goForward();
}

function nsBrowserStatusHandler()
{
}

nsBrowserStatusHandler.prototype =
{
  onStateChange : function(aWebProgress, aRequest, aStateFlags, aStatus) 
  {			
   	if(aStateFlags & STATE_IS_NETWORK)
   	{
    	if (aStateFlags & STATE_STOP) 
    	{    		
    		if (typeof(KioxSystem) == "object") {
    			KioxSystem.showContent();
    		}
    	}
    	if (aStateFlags & STATE_START) 
    	{    		
    		if (typeof(KioxSystem) == "object") {
    			KioxSystem.showLoading();
    		}
    	}
   	}	
   	return 0;
  },
  
  onProgressChange : function(aWebProgress, aRequest, aCurSelfProgress,
                              aMaxSelfProgress, aCurTotalProgress, aMaxTotalProgress) 
  {
  	
  },
  onSecurityChange : function(aWebProgress, aRequest, state) {},
  onLocationChange : function(aWebProgress, aRequest, aLocation)
  {
    UpdateBackForwardButtons();   
  },
  QueryInterface : function(aIID)
  {
    if (aIID.equals(Components.interfaces.nsIWebProgressListener) ||
      aIID.equals(Components.interfaces.nsISupportsWeakReference) ||
      aIID.equals(Components.interfaces.nsIXULBrowserWindow) ||
      aIID.equals(Components.interfaces.nsISupports))
      return this;
    throw Components.results.NS_NOINTERFACE;
  },
  setJSStatus : function(status) {},
  setJSDefaultStatus : function(status) {},
  setOverLink : function(link) {}
}

function UpdateBackForwardButtons()
{
    var backBroadcaster = document.getElementById("bcCanGoBack");
    var forwardBroadcaster = document.getElementById("bcCanGoForward");
    var webNavigation = getBrowser().webNavigation;

    var backDisabled = (backBroadcaster.getAttribute("disabled") == "true");
    var forwardDisabled = (forwardBroadcaster.getAttribute("disabled") == "true");

    if (backDisabled == webNavigation.canGoBack)
        backBroadcaster.setAttribute("disabled", !backDisabled);
  
    if (forwardDisabled == webNavigation.canGoForward)
        forwardBroadcaster.setAttribute("disabled", !forwardDisabled);
}
