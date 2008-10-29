// Created by iWeb 2.0.4 local-build-20081029

setTransparentGifURL('Media/transparent.gif');function applyEffects()
{var registry=IWCreateEffectRegistry();registry.registerEffects({reflection_0:new IWReflection({opacity:0.50,offset:1.00}),shadow_0:new IWShadow({blurRadius:4,offset:new IWPoint(1.4142,1.4142),color:'#000000',opacity:0.500000}),stroke_0:new IWStrokeParts([{rect:new IWRect(-1,1,2,164),url:'Installation_Download_1_files/stroke.png'},{rect:new IWRect(-1,-1,2,2),url:'Installation_Download_1_files/stroke_1.png'},{rect:new IWRect(1,-1,496,2),url:'Installation_Download_1_files/stroke_2.png'},{rect:new IWRect(497,-1,3,2),url:'Installation_Download_1_files/stroke_3.png'},{rect:new IWRect(497,1,3,164),url:'Installation_Download_1_files/stroke_4.png'},{rect:new IWRect(497,165,3,3),url:'Installation_Download_1_files/stroke_5.png'},{rect:new IWRect(1,165,496,3),url:'Installation_Download_1_files/stroke_6.png'},{rect:new IWRect(-1,165,2,3),url:'Installation_Download_1_files/stroke_7.png'}],new IWSize(498,167))});registry.applyEffects();}
function hostedOnDM()
{return false;}
function onPageLoad()
{loadMozillaCSS('Installation_Download_1_files/Installation_Download_1Moz.css')
adjustLineHeightIfTooBig('id1');adjustFontSizeIfTooBig('id1');adjustLineHeightIfTooBig('id2');adjustFontSizeIfTooBig('id2');adjustLineHeightIfTooBig('id3');adjustFontSizeIfTooBig('id3');adjustLineHeightIfTooBig('id4');adjustFontSizeIfTooBig('id4');Widget.onload();fixAllIEPNGs('Media/transparent.gif');applyEffects()}
function onPageUnload()
{Widget.onunload();}
