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
* The Original Code is the SEB kiosk application.
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



//#pragma once

#ifndef __ERRORMSG_H__
#define __ERRORMSG_H__


// Name and location of Seb configuration file and logfile
#define SEB_INI_FILE    "..\\Seb.ini"
#define DRIVE_ROOT      "C:\\"
#define	TEMP_DIR        "tmp\\"
#define SEB_LOG_FILE    "Seb.log"
#define MSG_LOG_FILE    "MsgHook.log"

// C structures for logfile handling
bool logFileDesired;
char logFileDir [512];
char logFileName[512];
FILE* fp;

// Function for easier writing into the logfile
#define logg if (fp != NULL) fprintf



// Store the error message texts for different languages
// e.g. (German, English, French)

const int IND_LanguageMin     = 0;
const int IND_LanguageGerman  = 0;
const int IND_LanguageEnglish = 1;
const int IND_LanguageFrench  = 2;
const int IND_LanguageMax     = 2;
const int IND_LanguageNum     = 3;

const int IND_MessageTextMin    = 0;

const int IND_FileNotFound      = 0;
const int IND_PathNotFound      = 1;
const int IND_AccessDenied      = 2; 
const int IND_UndefinedError    = 3;
const int IND_NoWritePermission = 4;
const int IND_NoSebIniError     = 5;
const int IND_NoMsgHookIniError = 6;
const int IND_NoClientInfoError = 7;
const int IND_InitialiseError   = 8;
const int IND_RegistryEditError = 9;

const int IND_NotEnoughRegistryRightsError = 10;

const int IND_RegistryWarning       = 11;
const int IND_ProcessCallFailed     = 12;
const int IND_ProcessWindowNotFound = 13;

const int IND_LoadLibraryError      = 14;
const int IND_NoLanguageStringFound = 15;
const int IND_NoInstance            = 16;
const int IND_NoFileError           = 17;
const int IND_NoTaskbarHandle       = 18;
const int IND_FirefoxStartFailed    = 19;
const int IND_KeyloggerFailed       = 20;

const int IND_KioxTerminated    = 21;
const int IND_SebTerminated     = 22;

const int IND_NoOsSupport       = 23;
const int IND_KillProcessFailed = 24;

const int IND_MessageTextMax    = 24;
const int IND_MessageTextNum    = 25;


// MessageBox supports errors and warnings
const int IND_MessageKindError   = 0;
const int IND_MessageKindWarning = 1;
const int IND_MessageKindNum     = 2;


int languageIndex = 0;
int    errorIndex = 0;

// Global arrays for messages in different languages
LPCSTR languageString  [IND_LanguageNum];
LPCSTR   messageText   [IND_LanguageNum][IND_MessageTextNum];
LPCSTR   messageCaption[IND_LanguageNum][IND_MessageKindNum];
int      messageIcon                    [IND_MessageKindNum];



// ****************************************************
// Initialise the error messages in different languages
// ****************************************************
void DefineErrorMessages()
{
	languageString[IND_LanguageGerman ] = "Deutsch";
	languageString[IND_LanguageEnglish] = "English";
	languageString[IND_LanguageFrench ] = "Fran�ais";


	messageIcon[IND_MessageKindError  ] = MB_ICONERROR;
	messageIcon[IND_MessageKindWarning] = MB_ICONWARNING;

	messageCaption[IND_LanguageGerman ][IND_MessageKindError] = "Fehler";
	messageCaption[IND_LanguageEnglish][IND_MessageKindError] = "Error";
	messageCaption[IND_LanguageFrench ][IND_MessageKindError] = "Erreur";

	messageCaption[IND_LanguageGerman ][IND_MessageKindWarning] = "Warnung";
	messageCaption[IND_LanguageEnglish][IND_MessageKindWarning] = "Warning";
	messageCaption[IND_LanguageFrench ][IND_MessageKindWarning] = "Avertissement";


	messageText[IND_LanguageGerman ][IND_FileNotFound] = "Datei nicht gefunden!";
	messageText[IND_LanguageEnglish][IND_FileNotFound] = "File not found!";
	messageText[IND_LanguageFrench ][IND_FileNotFound] = "Fichier pas trouv�!";

	messageText[IND_LanguageGerman ][IND_PathNotFound] = "Pfad nicht gefunden!";
	messageText[IND_LanguageEnglish][IND_PathNotFound] = "Path not found!";
	messageText[IND_LanguageFrench ][IND_PathNotFound] = "Path ne pas trouv�!";

	messageText[IND_LanguageGerman ][IND_AccessDenied] = "Zugriff verweigert!";
	messageText[IND_LanguageEnglish][IND_AccessDenied] = "Access denied!";
	messageText[IND_LanguageFrench ][IND_AccessDenied] = "Acc�s refus�!";

	messageText[IND_LanguageGerman ][IND_UndefinedError] = "Undefinierter Fehler beim Pr�fen der Schreibberechtigung!";
	messageText[IND_LanguageEnglish][IND_UndefinedError] = "Undefined error in CheckWritePermission!";
	messageText[IND_LanguageFrench ][IND_UndefinedError] = "Erreur ind�finie en v�rifiant le droit � l'�crire!";

	messageText[IND_LanguageGerman ][IND_NoWritePermission] = "Keine Schreibberechtigung!";
	messageText[IND_LanguageEnglish][IND_NoWritePermission] = "No write permission!";
	messageText[IND_LanguageFrench ][IND_NoWritePermission] = "Pas de droit � l'�crire!";

	messageText[IND_LanguageGerman ][IND_NoSebIniError] = "Konnte die Datei Seb.ini nicht finden!";
	messageText[IND_LanguageEnglish][IND_NoSebIniError] = "Could not find the file Seb.ini !";
	messageText[IND_LanguageFrench ][IND_NoSebIniError] = "Ne pouvais pas trouver le fichier Seb.ini !";

	messageText[IND_LanguageGerman ][IND_NoMsgHookIniError] = "Konnte die Datei MsgHook.ini nicht finden!";
	messageText[IND_LanguageEnglish][IND_NoMsgHookIniError] = "Could not find the file MsgHook.ini !";
	messageText[IND_LanguageFrench ][IND_NoMsgHookIniError] = "Ne pouvais pas trouver le fichier MsgHook.ini !";

	messageText[IND_LanguageGerman ][IND_NoClientInfoError] = "Keine Client-Information!";
	messageText[IND_LanguageEnglish][IND_NoClientInfoError] = "No client info!";
	messageText[IND_LanguageFrench ][IND_NoClientInfoError] = "Pas d'information sur le client!";

	messageText[IND_LanguageGerman ][IND_InitialiseError] = "Initialisierung fehlgeschlagen!";
	messageText[IND_LanguageEnglish][IND_InitialiseError] = "Initialisation failed!";
	messageText[IND_LanguageFrench ][IND_InitialiseError] = "Initialisation manqu�e / �chou�e!";

	messageText[IND_LanguageGerman ][IND_RegistryEditError] = "Fehler beim Bearbeiten der Windows Registry!";
	messageText[IND_LanguageEnglish][IND_RegistryEditError] = "Error editing the Windows Registry!";
	messageText[IND_LanguageFrench ][IND_RegistryEditError] = "Erreur en �ditant le Windows Registry!";


	messageText[IND_LanguageGerman ][IND_NotEnoughRegistryRightsError] = "Nicht gen�gend Rechte zum Bearbeiten der Registry-Schl�ssel!";
	messageText[IND_LanguageEnglish][IND_NotEnoughRegistryRightsError] = "Not enough rights to edit registry keys!";
	messageText[IND_LanguageFrench ][IND_NotEnoughRegistryRightsError] = "Pas de droits suffisants � �diter les cl�s du Registry!";


	messageText[IND_LanguageGerman ][IND_RegistryWarning] = "Registry-Warnung!";
	messageText[IND_LanguageEnglish][IND_RegistryWarning] = "Registry Warning!";
	messageText[IND_LanguageFrench ][IND_RegistryWarning] = "Avertissement du Registry!";

	messageText[IND_LanguageGerman ][IND_ProcessCallFailed] = "Proze�-Aufruf fehlgeschlagen!";
	messageText[IND_LanguageEnglish][IND_ProcessCallFailed] = "Process call failed!";
	messageText[IND_LanguageFrench ][IND_ProcessCallFailed] = "Activation du proc�s manqu� / �chou�!";

	messageText[IND_LanguageGerman ][IND_ProcessWindowNotFound] = "Proze�-Fenster nicht gefunden!";
	messageText[IND_LanguageEnglish][IND_ProcessWindowNotFound] = "Process window not found!";
	messageText[IND_LanguageFrench ][IND_ProcessWindowNotFound] = "Fen�tre du proc�s ne pas trouv�e!";



	messageText[IND_LanguageGerman ][IND_LoadLibraryError] = "Konnte die Bibliothek nicht laden!";
	messageText[IND_LanguageEnglish][IND_LoadLibraryError] = "Could not load library!";
	messageText[IND_LanguageFrench ][IND_LoadLibraryError] = "Ne pouvais pas charger la bibloth�que!";

	messageText[IND_LanguageGerman ][IND_NoLanguageStringFound] = "Kein Sprachen-String gefunden!";
	messageText[IND_LanguageEnglish][IND_NoLanguageStringFound] = "No language string found!";
	messageText[IND_LanguageFrench ][IND_NoLanguageStringFound] = "Ne pas trouv� ... de la langue!";

	messageText[IND_LanguageGerman ][IND_NoInstance] = "Konnte meine Instanz nicht holen!";
	messageText[IND_LanguageEnglish][IND_NoInstance] = "Could not get my instance!";
	messageText[IND_LanguageFrench ][IND_NoInstance] = "Ne pouvais pas obtenir mon instance!";

	messageText[IND_LanguageGerman ][IND_NoFileError] = "Konnte Datei nicht erzeugen oder finden!";
	messageText[IND_LanguageEnglish][IND_NoFileError] = "Could not create or find file!";
	messageText[IND_LanguageFrench ][IND_NoFileError] = "Ne pouvais pas cr�er ou trouver le fichier!";

	messageText[IND_LanguageGerman ][IND_NoTaskbarHandle] = "Kein Taskleisten-Handler!";
	messageText[IND_LanguageEnglish][IND_NoTaskbarHandle] = "No taskbar handle!";
	messageText[IND_LanguageFrench ][IND_NoTaskbarHandle] = "Pas de handler pour le taskbar!";

	messageText[IND_LanguageGerman ][IND_FirefoxStartFailed] = "Konnte Firefox nicht starten!";
	messageText[IND_LanguageEnglish][IND_FirefoxStartFailed] = "Could not start Firefox!";
	messageText[IND_LanguageFrench ][IND_FirefoxStartFailed] = "Ne pouvais pas lancer Firefox!";

	messageText[IND_LanguageGerman ][IND_KeyloggerFailed] = "Konnte KeyLogger nicht starten!";
	messageText[IND_LanguageEnglish][IND_KeyloggerFailed] = "Could not start KeyLogger!";
	messageText[IND_LanguageFrench ][IND_KeyloggerFailed] = "Ne pouvais pas lancer KeyLogger!";


	messageText[IND_LanguageGerman ][IND_KioxTerminated] = "Kiox wurde beendet!";
	messageText[IND_LanguageEnglish][IND_KioxTerminated] = "Kiox terminated!";
	messageText[IND_LanguageFrench ][IND_KioxTerminated] = "Kiox �tait termin�!";

	messageText[IND_LanguageGerman ][IND_SebTerminated] = "SEB beendet!";
	messageText[IND_LanguageEnglish][IND_SebTerminated] = "SEB terminated!";
	messageText[IND_LanguageFrench ][IND_SebTerminated] = "SEB termin�!";

	messageText[IND_LanguageGerman ][IND_NoOsSupport] = "Das Betriebssystem wird nicht unterst�tzt!";
	messageText[IND_LanguageEnglish][IND_NoOsSupport] = "The OS is not supported!";
	messageText[IND_LanguageFrench ][IND_NoOsSupport] = "Le syst�me d'exploitation n'est pas support�!";

	messageText[IND_LanguageGerman ][IND_KillProcessFailed] = "Abschie�en des Prozesses %s fehlgeschlagen: %d!";
	messageText[IND_LanguageEnglish][IND_KillProcessFailed] = "Killing process %s failed: %d!";
	messageText[IND_LanguageFrench ][IND_KillProcessFailed] = "Abattre le proc�s %s manqu� / �chou�: %d!";

	return;

} // end of method   DefineErrorMessages()





// ************************
// Get the current language
// ************************
int GetCurrentLanguage()
{
	int languageIndex;

	logg(fp, "\n\n");
	logg(fp, "Enter GetCurrentLanguage()\n");

	LANGID systemDefaultUILanguage = GetSystemDefaultUILanguage();
	LANGID   userDefaultUILanguage = GetUserDefaultUILanguage();
  //LANGID        threadUILanguage = GetThreadUILanguage();

	HKL WINAPI keyboardLayout    = GetKeyboardLayout(0);
	DWORD  inputLocalIdentifier  = (DWORD) keyboardLayout;
	WORD   completeLanguageIndex = inputLocalIdentifier % 65536;

	BYTE        subLanguageIndex = completeLanguageIndex / 256;
	BYTE    primaryLanguageIndex = completeLanguageIndex % 256;

	languageIndex = IND_LanguageEnglish;

	if (primaryLanguageIndex == 0x07) languageIndex = IND_LanguageGerman;
	if (primaryLanguageIndex == 0x09) languageIndex = IND_LanguageEnglish;
	if (primaryLanguageIndex == 0x0C) languageIndex = IND_LanguageFrench;

	logg(fp, "\n");
	logg(fp, "systemDefaultUILanguage   hex = %8x   dec = %d\n", systemDefaultUILanguage, systemDefaultUILanguage);
	logg(fp, "  userDefaultUILanguage   hex = %8x   dec = %d\n",   userDefaultUILanguage,   userDefaultUILanguage);
	logg(fp, "       keyboardLayout     hex = %8x   dec = %d\n",        keyboardLayout,          keyboardLayout);
	logg(fp, "inputLocalIdentifier      hex = %8x   dec = %d\n",    inputLocalIdentifier,    inputLocalIdentifier);
	logg(fp, "completeLanguageIndex     hex = %8x   dec = %d\n",    completeLanguageIndex,   completeLanguageIndex);
	logg(fp, "     subLanguageIndex     hex = %8x   dec = %d\n",         subLanguageIndex,        subLanguageIndex);
	logg(fp, " primaryLanguageIndex     hex = %8x   dec = %d\n",     primaryLanguageIndex,    primaryLanguageIndex);
	logg(fp, "        languageIndex                      dec = %d\n",       languageIndex);
	logg(fp, "\n");

	logg(fp, "Leave GetCurrentLanguage()\n\n\n");

	return languageIndex;

} // end of method   GetCurrentLanguage()




// **********************************
// Output an error or warning message
// **********************************
void OutputErrorMessage(int languageIndex, int messageTextIndex, int messageKindIndex)
{
	UINT   icon    = 0;
	LPCSTR caption = "";
	LPCSTR text    = "";

  //logg(fp, "Enter OutputErrorMessage()\n\n");

	icon    = messageIcon                  [messageKindIndex];
	caption = messageCaption[languageIndex][messageKindIndex];
	text    = messageText   [languageIndex][messageTextIndex];

	MessageBox(NULL, text, caption, icon);
	logg(fp, "%s: %s\n", caption, text);

  //logg(fp, "Leave OutputErrorMessage()\n\n");
	return;

} // end of method   OutputErrorMessage()



#endif /* __ERRORMSG_H__ */

