// Created by iWeb 2.0.4 local-build-20081029

setTransparentGifURL('Media/transparent.gif');function applyEffects()
{var registry=IWCreateEffectRegistry();registry.registerEffects({stroke_0:new IWStrokeParts([{rect:new IWRect(-1,1,2,30),url:'Installation_Download_files/stroke.png'},{rect:new IWRect(-1,-1,2,2),url:'Installation_Download_files/stroke_1.png'},{rect:new IWRect(1,-1,42,2),url:'Installation_Download_files/stroke_2.png'},{rect:new IWRect(43,-1,2,2),url:'Installation_Download_files/stroke_3.png'},{rect:new IWRect(43,1,2,30),url:'Installation_Download_files/stroke_4.png'},{rect:new IWRect(43,31,2,2),url:'Installation_Download_files/stroke_5.png'},{rect:new IWRect(1,31,42,2),url:'Installation_Download_files/stroke_6.png'},{rect:new IWRect(-1,31,2,2),url:'Installation_Download_files/stroke_7.png'}],new IWSize(44,32)),reflection_0:new IWReflection({opacity:0.50,offset:1.00})});registry.applyEffects();}
function hostedOnDM()
{return false;}
function onPageLoad()
{loadMozillaCSS('Installation_Download_files/Installation_DownloadMoz.css')
adjustLineHeightIfTooBig('id1');adjustFontSizeIfTooBig('id1');adjustLineHeightIfTooBig('id2');adjustFontSizeIfTooBig('id2');adjustLineHeightIfTooBig('id3');adjustFontSizeIfTooBig('id3');Widget.onload();fixAllIEPNGs('Media/transparent.gif');applyEffects()}
function onPageUnload()
{Widget.onunload();}
