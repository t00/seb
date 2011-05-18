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



// Store the error message texts for different languages
// e.g. (German, English, French)

const int IND_LanguageMin     = 0;
const int IND_LanguageGerman  = 0;
const int IND_LanguageEnglish = 1;
const int IND_LanguageFrench  = 2;
const int IND_LanguageMax     = 2;
const int IND_LanguageNum     = 3;

const int IND_ErrorMin          = 0;

const int IND_FileNotFound      = 0;
const int IND_PathNotFound      = 1;
const int IND_AccessDenied      = 2; 
const int IND_UndefinedError    = 3;
const int IND_NoWritePermission = 4;
const int IND_NoSebIniError     = 5;
const int IND_NoClientInfoError = 6;
const int IND_InitialiseError   = 7;
const int IND_RegEditError      = 8;

const int IND_NotEnoughRegistryRightsError = 9;

const int IND_RegistryWarning       = 10;
const int IND_ProcessCallFailed     = 11;
const int IND_ProcessWindowNotFound = 12;

const int IND_LoadLibraryError   = 13;
const int IND_NoLangStringFound  = 14;
const int IND_NoInstance         = 15;
const int IND_NoFileError        = 16;
const int IND_NoTaskbarHandle    = 17;
const int IND_FirefoxStartFailed = 18;
const int IND_KeyloggerFailed    = 19;

const int IND_KioxTerminated = 20;
const int IND_SebTerminated  = 21;

const int IND_NoOsSupport    = 22;
const int IND_KillProcFailed = 23;

const int IND_ErrorMax       = 23;
const int IND_ErrorNum       = 24;


// MessageBox supports errors and warnings
const int IND_MessageIconError   = 0;
const int IND_MessageIconWarning = 1;


static int languageIndex = 0;
static int    errorIndex = 0;

// Global arrays for messages in different languages
static LPCSTR   errorCaption[IND_LanguageNum];
static LPCSTR   errorMessage[IND_LanguageNum][IND_ErrorNum];
static LPCSTR languageString[IND_LanguageNum];



// ****************************************************
// Initialise the error messages in different languages
// ****************************************************
void DefineErrorMessages()
{
	languageString[IND_LanguageGerman ] = "Deutsch";
	languageString[IND_LanguageEnglish] = "English";
	languageString[IND_LanguageFrench ] = "Français";

	errorCaption[IND_LanguageGerman ] = "Fehler";
	errorCaption[IND_LanguageEnglish] = "Error";
	errorCaption[IND_LanguageFrench ] = "Erreur";

	errorMessage[IND_LanguageGerman ][IND_FileNotFound] = "Datei nicht gefunden!";
	errorMessage[IND_LanguageEnglish][IND_FileNotFound] = "File not found!";
	errorMessage[IND_LanguageFrench ][IND_FileNotFound] = "Fichier pas trouvé!";

	errorMessage[IND_LanguageGerman ][IND_PathNotFound] = "Pfad nicht gefunden!";
	errorMessage[IND_LanguageEnglish][IND_PathNotFound] = "Path not found!";
	errorMessage[IND_LanguageFrench ][IND_PathNotFound] = "Path pas trouvé!";

	errorMessage[IND_LanguageGerman ][IND_AccessDenied] = "Zugriff verweigert!";
	errorMessage[IND_LanguageEnglish][IND_AccessDenied] = "Access denied!";
	errorMessage[IND_LanguageFrench ][IND_AccessDenied] = "Accès refusé!";

	errorMessage[IND_LanguageGerman ][IND_UndefinedError] = "Undefinierter Fehler beim Prüfen der Schreibberechtigung!";
	errorMessage[IND_LanguageEnglish][IND_UndefinedError] = "Undefined error in CheckWritePermission!";
	errorMessage[IND_LanguageFrench ][IND_UndefinedError] = "Erreur indéfinie en vérifiant le droit à l'écrire!";

	errorMessage[IND_LanguageGerman ][IND_AccessDenied] = "Keine Schreibberechtigung!";
	errorMessage[IND_LanguageEnglish][IND_AccessDenied] = "No write permission!";
	errorMessage[IND_LanguageFrench ][IND_AccessDenied] = "Pas de droit à l'écrire!";

	errorMessage[IND_LanguageGerman ][IND_NoSebIniError] = "Konnte die Datei Seb.ini nicht finden!";
	errorMessage[IND_LanguageEnglish][IND_NoSebIniError] = "Could not find the file Seb.ini !";
	errorMessage[IND_LanguageFrench ][IND_NoSebIniError] = "Ne pouvais pas trouver le fichier Seb.ini !";

	errorMessage[IND_LanguageGerman ][IND_NoClientInfoError] = "Keine Client-Information!";
	errorMessage[IND_LanguageEnglish][IND_NoClientInfoError] = "No client info!";
	errorMessage[IND_LanguageFrench ][IND_NoClientInfoError] = "Pas d'information sur le client!";

	errorMessage[IND_LanguageGerman ][IND_InitialiseError] = "Initialisierung fehlgeschlagen!";
	errorMessage[IND_LanguageEnglish][IND_InitialiseError] = "Initialization failed!";
	errorMessage[IND_LanguageFrench ][IND_InitialiseError] = "Initialisation manquée / échouée!";

	errorMessage[IND_LanguageGerman ][IND_RegEditError] = "Fehler beim Bearbeiten der Windows Registry!";
	errorMessage[IND_LanguageEnglish][IND_RegEditError] = "Error editing the Windows Registry!";
	errorMessage[IND_LanguageFrench ][IND_RegEditError] = "Erreur en traitant le Windows Registry!";



	errorMessage[IND_LanguageGerman ][IND_NotEnoughRegistryRightsError] = "Nicht genügend Rechte zum Bearbeiten der Registry-Schlüssel!";
	errorMessage[IND_LanguageEnglish][IND_NotEnoughRegistryRightsError] = "Not enough rights to edit registry keys!";
	errorMessage[IND_LanguageFrench ][IND_NotEnoughRegistryRightsError] = "Pas de droits suffisants au traiter les clés du Registry!";



	errorMessage[IND_LanguageGerman ][IND_RegistryWarning] = "Initialisierung fehlgeschlagen!";
	errorMessage[IND_LanguageEnglish][IND_RegistryWarning] = "Registry Warning!";
	errorMessage[IND_LanguageFrench ][IND_RegistryWarning] = "Initialisation manquée / échouée!";

	errorMessage[IND_LanguageGerman ][IND_ProcessCallFailed] = "Prozeß fehlgeschlagen!";
	errorMessage[IND_LanguageEnglish][IND_ProcessCallFailed] = "Process call failed!";
	errorMessage[IND_LanguageFrench ][IND_ProcessCallFailed] = "Procès manqué / échoué!";

	errorMessage[IND_LanguageGerman ][IND_ProcessWindowNotFound] = "Kein Prozeß-Fenster gefunden!";
	errorMessage[IND_LanguageEnglish][IND_ProcessWindowNotFound] = "No process window found!";
	errorMessage[IND_LanguageFrench ][IND_ProcessWindowNotFound] = "Aucune fenêtre du procès trouvée!";



	errorMessage[IND_LanguageGerman ][IND_LoadLibraryError] = "Konnte die Bibliothek nicht laden!";
	errorMessage[IND_LanguageEnglish][IND_LoadLibraryError] = "Could not load library!";
	errorMessage[IND_LanguageFrench ][IND_LoadLibraryError] = "Ne pouvais pas charger la biblothèque!";

	errorMessage[IND_LanguageGerman ][IND_NoLangStringFound] = "Kein Sprachen-String gefunden!";
	errorMessage[IND_LanguageEnglish][IND_NoLangStringFound] = "No language string found!";
	errorMessage[IND_LanguageFrench ][IND_NoLangStringFound] = "Ne pas trouvé ... de la langue!";

	errorMessage[IND_LanguageGerman ][IND_NoInstance] = "Konnte meine Instanz nicht holen!";
	errorMessage[IND_LanguageEnglish][IND_NoInstance] = "Could not get my instance!";
	errorMessage[IND_LanguageFrench ][IND_NoInstance] = "Ne pouvais pas obtenir mon instance!";

	errorMessage[IND_LanguageGerman ][IND_NoFileError] = "Konnte Datei nicht erzeugen oder finden!";
	errorMessage[IND_LanguageEnglish][IND_NoFileError] = "Could not create or find file!";
	errorMessage[IND_LanguageFrench ][IND_NoFileError] = "Ne pouvais pas ... ou trouver le fichier!";

	errorMessage[IND_LanguageGerman ][IND_NoTaskbarHandle] = "Kein ... Taskleiste!";
	errorMessage[IND_LanguageEnglish][IND_NoTaskbarHandle] = "No taskbar handle!";
	errorMessage[IND_LanguageFrench ][IND_NoTaskbarHandle] = "Pas de ...!";

	errorMessage[IND_LanguageGerman ][IND_FirefoxStartFailed] = "Konnte den Firefox nicht starten!";
	errorMessage[IND_LanguageEnglish][IND_FirefoxStartFailed] = "Could not start Firefox!";
	errorMessage[IND_LanguageFrench ][IND_FirefoxStartFailed] = "Ne pouvais pas lancer Firefox!";

	errorMessage[IND_LanguageGerman ][IND_KeyloggerFailed] = "Konnte den KeyLogger nicht starten!";
	errorMessage[IND_LanguageEnglish][IND_KeyloggerFailed] = "Could not start KeyLogger!";
	errorMessage[IND_LanguageFrench ][IND_KeyloggerFailed] = "Ne pouvais pas lancer KeyLogger!";



	errorMessage[IND_LanguageGerman ][IND_KioxTerminated] = "Kiox wurde beendet!";
	errorMessage[IND_LanguageEnglish][IND_KioxTerminated] = "Kiox terminated!";
	errorMessage[IND_LanguageFrench ][IND_KioxTerminated] = "Kiox était terminé!";

	errorMessage[IND_LanguageGerman ][IND_SebTerminated] = "SEB wurde beendet!";
	errorMessage[IND_LanguageEnglish][IND_SebTerminated] = "SEB terminated!";
	errorMessage[IND_LanguageFrench ][IND_SebTerminated] = "SEB était terminé!";

	errorMessage[IND_LanguageGerman ][IND_NoOsSupport] = "Das Betriebssystem wird nicht unterstützt!";
	errorMessage[IND_LanguageEnglish][IND_NoOsSupport] = "The OS is not supported!";
	errorMessage[IND_LanguageFrench ][IND_NoOsSupport] = "Le système d'exploitation n'est pas ...!";

	errorMessage[IND_LanguageGerman ][IND_KillProcFailed] = "Killen des Prozesses %s fehlgeschlagen: %d!";
	errorMessage[IND_LanguageEnglish][IND_KillProcFailed] = "Killing process %s failed: %d!";
	errorMessage[IND_LanguageFrench ][IND_KillProcFailed] = "Abattre le procès manqué / échoué!";

	languageIndex = IND_LanguageFrench;

	return;

} // end of method   DefineErrorMessages()





// ************************
// Get the current language
// ************************
int GetCurrentLanguage()
{
	int languageIndex;

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

	logg(fp, "systemDefaultUILanguage   hex = %x   dec = %d\n", systemDefaultUILanguage, systemDefaultUILanguage);
	logg(fp, "  userDefaultUILanguage   hex = %x   dec = %d\n",   userDefaultUILanguage,   userDefaultUILanguage);
	logg(fp, "  keyboardLayout          hex = %x   dec = %d\n",          keyboardLayout,          keyboardLayout);
	logg(fp, "inputLocalIdentifier      hex = %x   dec = %d\n",    inputLocalIdentifier,    inputLocalIdentifier);
	logg(fp, "completeLanguageIndex     hex = %x   dec = %d\n",    completeLanguageIndex,   completeLanguageIndex);
	logg(fp, "     subLanguageIndex     hex = %x   dec = %d\n",         subLanguageIndex,        subLanguageIndex);
	logg(fp, " primaryLanguageIndex     hex = %x   dec = %d\n",     primaryLanguageIndex,    primaryLanguageIndex);
	logg(fp, "        languageIndex                dec = %d\n",            languageIndex);

	return languageIndex;

} // end of method   GetCurrentLanguage()




// **********************************
// Output an error or warning message
// **********************************
void OutputErrorMessage(int languageIndex, int errorIndex, UINT messageIcon)
{
	LPCSTR caption = "";
	LPCSTR message = "";
	UINT   icon    = 0;

	//logg(fp, "Enter PrintErrorMessage()\n\n");

	caption = errorCaption[languageIndex];
	message = errorMessage[languageIndex][errorIndex];
	icon    = messageIcon;

	MessageBox(NULL, message, caption, icon);
	logg(fp, "Error: %s\n", message);

  //logg(fp, "Leave PrintErrorMessage()\n\n");
	return;

} // end of method   OutputErrorMessage()
