// Created by iWeb 2.0.4 local-build-20081029

setTransparentGifURL('Media/transparent.gif');function applyEffects()
{var registry=IWCreateEffectRegistry();registry.registerEffects({reflection_0:new IWReflection({opacity:0.50,offset:1.00}),stroke_0:new IWStrokeParts([{rect:new IWRect(-1,1,2,35),url:'Support_files/stroke.png'},{rect:new IWRect(-1,-1,2,2),url:'Support_files/stroke_1.png'},{rect:new IWRect(1,-1,123,2),url:'Support_files/stroke_2.png'},{rect:new IWRect(124,-1,2,2),url:'Support_files/stroke_3.png'},{rect:new IWRect(124,1,2,35),url:'Support_files/stroke_4.png'},{rect:new IWRect(124,36,2,2),url:'Support_files/stroke_5.png'},{rect:new IWRect(1,36,123,2),url:'Support_files/stroke_6.png'},{rect:new IWRect(-1,36,2,2),url:'Support_files/stroke_7.png'}],new IWSize(125,37))});registry.applyEffects();}
function hostedOnDM()
{return false;}
function onPageLoad()
{loadMozillaCSS('Support_files/SupportMoz.css')
adjustLineHeightIfTooBig('id1');adjustFontSizeIfTooBig('id1');adjustLineHeightIfTooBig('id2');adjustFontSizeIfTooBig('id2');Widget.onload();fixAllIEPNGs('Media/transparent.gif');applyEffects()}
function onPageUnload()
{Widget.onunload();}
