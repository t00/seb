// general preferences:
// have a look at http://www.firefox-browser.de/wiki/About:config_(Einstellungen)
pref('app.update.auto', false);
//pref("browser.startup.homepage", "resource:/kiox_browserconfig.properties");

pref("nglayout.debug.disable_xul_cache", true);
pref("javascript.options.strict", true);
pref("nglayout.debug.disable_xul_fastload", true);
pref("signed.applets.codebase_principal_support", true);
pref("xul_debug.box", false);
pref("browser.dom.window.dump.enabled", true);
pref("browser.link.open_newwindow", 1);
pref("browser.link.open_newwindow.restriction", 0);
pref("browser.link.open_external", 1);
pref("browser.shell.checkDefaultBrowser", false);
pref("network.protocol-handler.external.mailto", false);
pref("dom.popup_maximum", 1);
pref("security.warn_entering_secure", false);
pref("security.warn_leaving_secure", false);

// Block pop-ups from Flash :
pref("privacy.popups.disable_from_plugins", 3);
// Block pop-ups from events
pref("dom.popup_allowed_events", "dblclick");

//pref("dom.disable_window_open_feature.resizable", true);
// Make sure all pop-up windows are minimizable:
//pref("dom.disable_window_open_feature.minimizable", true);
// Always display the menu in pop-up windows:
//pref("dom.disable_window_open_feature.menubar", true);
// Always display the Navigation Toolbar in pop-up windows:
//pref("dom.disable_window_open_feature.location", true);
// Prevent sites from disabling scrollbars:
//pref("dom.disable_window_open_feature.scrollbars", true);

// kiox preferences
//pref('kiox.startup.homepage', 'http://hrzb8.hrz.uni-giessen.de/kmed/dev/10_361/ilias3/');
pref('kiox.startup.homepage', 'http://www.uni-giessen.de/~gz135/winkeyox');
pref('kiox.startup.auto', true);
pref('kiox.toolbar.enable', true);
pref('kiox.toolbar.navigation', true);
//pref("browser.chromeURL","chrome://kiox/content/");