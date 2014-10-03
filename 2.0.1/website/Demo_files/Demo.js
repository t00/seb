// Created by iWeb 2.0.4 local-build-20081029

setTransparentGifURL('Media/transparent.gif');function applyEffects()
{var registry=IWCreateEffectRegistry();registry.registerEffects({reflection_0:new IWReflection({opacity:0.50,offset:1.00}),stroke_0:new IWStrokeParts([{rect:new IWRect(-1,1,2,285),url:'Demo_files/stroke.png'},{rect:new IWRect(-1,-1,2,2),url:'Demo_files/stroke_1.png'},{rect:new IWRect(1,-1,482,2),url:'Demo_files/stroke_2.png'},{rect:new IWRect(483,-1,2,2),url:'Demo_files/stroke_3.png'},{rect:new IWRect(483,1,2,285),url:'Demo_files/stroke_4.png'},{rect:new IWRect(483,286,2,3),url:'Demo_files/stroke_5.png'},{rect:new IWRect(1,286,482,3),url:'Demo_files/stroke_6.png'},{rect:new IWRect(-1,286,2,3),url:'Demo_files/stroke_7.png'}],new IWSize(484,287)),shadow_0:new IWShadow({blurRadius:4,offset:new IWPoint(1.4142,1.4142),color:'#000000',opacity:0.500000})});registry.applyEffects();}
function hostedOnDM()
{return false;}
function onPageLoad()
{loadMozillaCSS('Demo_files/DemoMoz.css')
adjustLineHeightIfTooBig('id1');adjustFontSizeIfTooBig('id1');adjustLineHeightIfTooBig('id2');adjustFontSizeIfTooBig('id2');adjustLineHeightIfTooBig('id3');adjustFontSizeIfTooBig('id3');adjustLineHeightIfTooBig('id4');adjustFontSizeIfTooBig('id4');Widget.onload();fixAllIEPNGs('Media/transparent.gif');applyEffects()}
function onPageUnload()
{Widget.onunload();}
