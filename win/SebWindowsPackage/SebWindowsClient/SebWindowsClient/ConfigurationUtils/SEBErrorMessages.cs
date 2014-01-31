using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Windows.Forms;

namespace SebWindowsClient.ConfigurationUtils
{
    public class SEBErrorMessages
    {
        private static int _languageIndex = -1;
 
        // Global arrays for messages in different languages
        private static String[] languageString = new String[SEBGlobalConstants.IND_LANGUAGE_NUM] { "Deutsch", "English", "Français" };
        private static String[,] messageCaption = new String[SEBGlobalConstants.IND_LANGUAGE_NUM, SEBGlobalConstants.IND_MESSAGE_KIND_NUM]
                                        {{"Fehler", "Warnung", "Frage"},
                                        {"Error", "Warning", "Question" },
                                        {"Erreur", "Avertissement", ""}};
        private static int[] messageButtons = new int[SEBGlobalConstants.IND_MESSAGE_KIND_NUM] { SEBGlobalConstants.IND_MESSAGE_KIND_ERROR, SEBGlobalConstants.IND_MESSAGE_KIND_WARNING, SEBGlobalConstants.IND_MESSAGE_KIND_QUESTION };

        private static int[] messageIcon = new int[SEBGlobalConstants.IND_MESSAGE_KIND_NUM] { SEBGlobalConstants.IND_MESSAGE_KIND_ERROR, SEBGlobalConstants.IND_MESSAGE_KIND_WARNING, SEBGlobalConstants.IND_MESSAGE_KIND_QUESTION };

        private static String[,] messageText = new String[SEBGlobalConstants.IND_LANGUAGE_NUM, SEBGlobalConstants.IND_MESSAGE_TEXT_NUM];


        public static void InitErrorMessages()
        {
            messageText[SEBGlobalConstants.IND_LANGUAGE_GERMAN, SEBGlobalConstants.IND_FILE_NOT_FOUND] = "Datei nicht gefunden!";
            messageText[SEBGlobalConstants.IND_LANGUAGE_ENGLISH, SEBGlobalConstants.IND_FILE_NOT_FOUND] = "File not found!";
            messageText[SEBGlobalConstants.IND_LANGUAGE_FRENCH, SEBGlobalConstants.IND_FILE_NOT_FOUND] = "Fichier pas trouvé!";

            messageText[SEBGlobalConstants.IND_LANGUAGE_GERMAN, SEBGlobalConstants.IND_PATH_NOT_FOUND] = "Pfad nicht gefunden!";
            messageText[SEBGlobalConstants.IND_LANGUAGE_ENGLISH, SEBGlobalConstants.IND_PATH_NOT_FOUND] = "Path not found!";
            messageText[SEBGlobalConstants.IND_LANGUAGE_FRENCH, SEBGlobalConstants.IND_PATH_NOT_FOUND] = "Path ne pas trouvé!";

            messageText[SEBGlobalConstants.IND_LANGUAGE_GERMAN, SEBGlobalConstants.IND_ACCESS_DENIED] = "Zugriff verweigert!";
            messageText[SEBGlobalConstants.IND_LANGUAGE_ENGLISH, SEBGlobalConstants.IND_ACCESS_DENIED] = "Access denied!";
            messageText[SEBGlobalConstants.IND_LANGUAGE_FRENCH, SEBGlobalConstants.IND_ACCESS_DENIED] = "Accès refusé!";

            messageText[SEBGlobalConstants.IND_LANGUAGE_GERMAN, SEBGlobalConstants.IND_UNDEFINED_ERROR] = "Undefinierter Fehler beim Prüfen der Schreibberechtigung!";
            messageText[SEBGlobalConstants.IND_LANGUAGE_ENGLISH, SEBGlobalConstants.IND_UNDEFINED_ERROR] = "Undefined error in CheckWritePermission!";
            messageText[SEBGlobalConstants.IND_LANGUAGE_FRENCH, SEBGlobalConstants.IND_UNDEFINED_ERROR] = "Erreur indéfinie en vérifiant le droit à l'écrire!";

            messageText[SEBGlobalConstants.IND_LANGUAGE_GERMAN, SEBGlobalConstants.IND_NO_WRITE_PERMISSION] = "Keine Schreibberechtigung!";
            messageText[SEBGlobalConstants.IND_LANGUAGE_ENGLISH, SEBGlobalConstants.IND_NO_WRITE_PERMISSION] = "No write permission!";
            messageText[SEBGlobalConstants.IND_LANGUAGE_FRENCH, SEBGlobalConstants.IND_NO_WRITE_PERMISSION] = "Pas de droit à l'écrire!";

            messageText[SEBGlobalConstants.IND_LANGUAGE_GERMAN, SEBGlobalConstants.IND_SEB_CLIENT_SEB_ERROR] = "Fehler beim Öffnen der Datei SebClientSettings.seb!";
            messageText[SEBGlobalConstants.IND_LANGUAGE_ENGLISH, SEBGlobalConstants.IND_SEB_CLIENT_SEB_ERROR] = "Error when opening the file SebClientSettings.seb!";
            messageText[SEBGlobalConstants.IND_LANGUAGE_FRENCH, SEBGlobalConstants.IND_SEB_CLIENT_SEB_ERROR] = "Erreur en ouvrant le fichier SebClientSettings.seb!";

            messageText[SEBGlobalConstants.IND_LANGUAGE_GERMAN, SEBGlobalConstants.IND_CONFIG_JSON_ERROR] = "Fehler beim Öffnen der Datei config.json!";
            messageText[SEBGlobalConstants.IND_LANGUAGE_ENGLISH, SEBGlobalConstants.IND_CONFIG_JSON_ERROR] = "Error when opening the file config.json!";
            messageText[SEBGlobalConstants.IND_LANGUAGE_FRENCH, SEBGlobalConstants.IND_CONFIG_JSON_ERROR] = "Erreur en ouvrant le fichier config.json!";

            messageText[SEBGlobalConstants.IND_LANGUAGE_GERMAN, SEBGlobalConstants.IND_NO_CLIENT_INFO_ERROR] = "Keine Client-Information!";
            messageText[SEBGlobalConstants.IND_LANGUAGE_ENGLISH, SEBGlobalConstants.IND_NO_CLIENT_INFO_ERROR] = "No client info!";
            messageText[SEBGlobalConstants.IND_LANGUAGE_FRENCH, SEBGlobalConstants.IND_NO_CLIENT_INFO_ERROR] = "Pas d'information sur le client!";

            messageText[SEBGlobalConstants.IND_LANGUAGE_GERMAN, SEBGlobalConstants.IND_INITIALISE_ERROR] = "Initialisierung fehlgeschlagen!";
            messageText[SEBGlobalConstants.IND_LANGUAGE_ENGLISH, SEBGlobalConstants.IND_INITIALISE_ERROR] = "Initialisation failed!";
            messageText[SEBGlobalConstants.IND_LANGUAGE_FRENCH, SEBGlobalConstants.IND_INITIALISE_ERROR] = "Initialisation manquée / échouée!";

            messageText[SEBGlobalConstants.IND_LANGUAGE_GERMAN, SEBGlobalConstants.IND_REGISTRY_EDIT_ERROR] = "Fehler beim Bearbeiten der Windows Registry!";
            messageText[SEBGlobalConstants.IND_LANGUAGE_ENGLISH, SEBGlobalConstants.IND_REGISTRY_EDIT_ERROR] = "Error editing the Windows Registry!";
            messageText[SEBGlobalConstants.IND_LANGUAGE_FRENCH, SEBGlobalConstants.IND_REGISTRY_EDIT_ERROR] = "Erreur en éditant le Windows Registry!";


            messageText[SEBGlobalConstants.IND_LANGUAGE_GERMAN, SEBGlobalConstants.IND_NOT_ENOUGH_REGISTRY_RIGHTS_ERROR] = "Nicht genügend Rechte zum Bearbeiten der Registry-Schlüssel!";
            messageText[SEBGlobalConstants.IND_LANGUAGE_ENGLISH, SEBGlobalConstants.IND_NOT_ENOUGH_REGISTRY_RIGHTS_ERROR] = "Not enough rights to edit registry keys!";
            messageText[SEBGlobalConstants.IND_LANGUAGE_FRENCH, SEBGlobalConstants.IND_NOT_ENOUGH_REGISTRY_RIGHTS_ERROR] = "Pas de droits suffisants à éditer les clés du Registry!";


            messageText[SEBGlobalConstants.IND_LANGUAGE_GERMAN, SEBGlobalConstants.IND_REGISTRY_WARNING] = "Registry-Warnung!";
            messageText[SEBGlobalConstants.IND_LANGUAGE_ENGLISH, SEBGlobalConstants.IND_REGISTRY_WARNING] = "Registry Warning!";
            messageText[SEBGlobalConstants.IND_LANGUAGE_FRENCH, SEBGlobalConstants.IND_REGISTRY_WARNING] = "Avertissement du Registry!";

            messageText[SEBGlobalConstants.IND_LANGUAGE_GERMAN, SEBGlobalConstants.IND_PROCESS_CALL_FAILED] = "Prozess-Aufruf fehlgeschlagen!";
            messageText[SEBGlobalConstants.IND_LANGUAGE_ENGLISH, SEBGlobalConstants.IND_PROCESS_CALL_FAILED] = "Process call failed!";
            messageText[SEBGlobalConstants.IND_LANGUAGE_FRENCH, SEBGlobalConstants.IND_PROCESS_CALL_FAILED] = "Activation du procès manqué / échoué!";

            messageText[SEBGlobalConstants.IND_LANGUAGE_GERMAN, SEBGlobalConstants.IND_PROCESS_WINDOW_NOT_FOUND] = "Prozess-Fenster nicht gefunden!";
            messageText[SEBGlobalConstants.IND_LANGUAGE_ENGLISH, SEBGlobalConstants.IND_PROCESS_WINDOW_NOT_FOUND] = "Process window not found!";
            messageText[SEBGlobalConstants.IND_LANGUAGE_FRENCH, SEBGlobalConstants.IND_PROCESS_WINDOW_NOT_FOUND] = "Fenêtre du procès ne pas trouvée!";

            messageText[SEBGlobalConstants.IND_LANGUAGE_GERMAN, SEBGlobalConstants.IND_LOAD_LIBRARY_ERROR] = "Konnte die Bibliothek nicht laden!";
            messageText[SEBGlobalConstants.IND_LANGUAGE_ENGLISH, SEBGlobalConstants.IND_LOAD_LIBRARY_ERROR] = "Could not load library!";
            messageText[SEBGlobalConstants.IND_LANGUAGE_FRENCH, SEBGlobalConstants.IND_LOAD_LIBRARY_ERROR] = "Ne pouvais pas charger la biblothèque!";

            messageText[SEBGlobalConstants.IND_LANGUAGE_GERMAN, SEBGlobalConstants.IND_NO_LANGUAGE_STRING_FOUND] = "Kein Sprachen-String gefunden!";
            messageText[SEBGlobalConstants.IND_LANGUAGE_ENGLISH, SEBGlobalConstants.IND_NO_LANGUAGE_STRING_FOUND] = "No language string found!";
            messageText[SEBGlobalConstants.IND_LANGUAGE_FRENCH, SEBGlobalConstants.IND_NO_LANGUAGE_STRING_FOUND] = "Ne pas trouvé ... de la langue!";

            messageText[SEBGlobalConstants.IND_LANGUAGE_GERMAN, SEBGlobalConstants.IND_NO_INSTANCE] = "Konnte meine Instanz nicht holen!";
            messageText[SEBGlobalConstants.IND_LANGUAGE_ENGLISH, SEBGlobalConstants.IND_NO_INSTANCE] = "Could not get my instance!";
            messageText[SEBGlobalConstants.IND_LANGUAGE_FRENCH, SEBGlobalConstants.IND_NO_INSTANCE] = "Ne pouvais pas obtenir mon instance!";

            messageText[SEBGlobalConstants.IND_LANGUAGE_GERMAN, SEBGlobalConstants.IND_NO_FILE_ERROR] = "Konnte Datei nicht erzeugen oder finden!";
            messageText[SEBGlobalConstants.IND_LANGUAGE_ENGLISH, SEBGlobalConstants.IND_NO_FILE_ERROR] = "Could not create or find file!";
            messageText[SEBGlobalConstants.IND_LANGUAGE_FRENCH, SEBGlobalConstants.IND_NO_FILE_ERROR] = "Ne pouvais pas créer ou trouver le fichier!";

            messageText[SEBGlobalConstants.IND_LANGUAGE_GERMAN, SEBGlobalConstants.IND_NO_TASKBAR_HANDLE] = "Kein Taskleisten-Handler!";
            messageText[SEBGlobalConstants.IND_LANGUAGE_ENGLISH, SEBGlobalConstants.IND_NO_TASKBAR_HANDLE] = "No taskbar handle!";
            messageText[SEBGlobalConstants.IND_LANGUAGE_FRENCH, SEBGlobalConstants.IND_NO_TASKBAR_HANDLE] = "Pas de handler pour le taskbar!";

            messageText[SEBGlobalConstants.IND_LANGUAGE_GERMAN, SEBGlobalConstants.IND_FIREFOX_START_FAILED] = "Konnte Firefox nicht starten!";
            messageText[SEBGlobalConstants.IND_LANGUAGE_ENGLISH, SEBGlobalConstants.IND_FIREFOX_START_FAILED] = "Could not start Firefox!";
            messageText[SEBGlobalConstants.IND_LANGUAGE_FRENCH, SEBGlobalConstants.IND_FIREFOX_START_FAILED] = "Ne pouvais pas lancer Firefox!";

            messageText[SEBGlobalConstants.IND_LANGUAGE_GERMAN, SEBGlobalConstants.IND_KEY_LOGGER_FAILED] = "Konnte KeyLogger nicht starten!";
            messageText[SEBGlobalConstants.IND_LANGUAGE_ENGLISH, SEBGlobalConstants.IND_KEY_LOGGER_FAILED] = "Could not start KeyLogger!";
            messageText[SEBGlobalConstants.IND_LANGUAGE_FRENCH, SEBGlobalConstants.IND_KEY_LOGGER_FAILED] = "Ne pouvais pas lancer KeyLogger!";


            messageText[SEBGlobalConstants.IND_LANGUAGE_GERMAN, SEBGlobalConstants.IND_KIOX_TERMINATED] = "Kiox wurde beendet!";
            messageText[SEBGlobalConstants.IND_LANGUAGE_ENGLISH, SEBGlobalConstants.IND_KIOX_TERMINATED] = "Kiox terminated!";
            messageText[SEBGlobalConstants.IND_LANGUAGE_FRENCH, SEBGlobalConstants.IND_KIOX_TERMINATED] = "Kiox était terminé!";

            messageText[SEBGlobalConstants.IND_LANGUAGE_GERMAN, SEBGlobalConstants.IND_SEB_TERMINATED] = "SEB beendet!";
            messageText[SEBGlobalConstants.IND_LANGUAGE_ENGLISH, SEBGlobalConstants.IND_SEB_TERMINATED] = "SEB terminated!";
            messageText[SEBGlobalConstants.IND_LANGUAGE_FRENCH, SEBGlobalConstants.IND_SEB_TERMINATED] = "SEB terminé!";

            messageText[SEBGlobalConstants.IND_LANGUAGE_GERMAN, SEBGlobalConstants.IND_NO_OS_SUPPORT] = "Dieses Betriebssystem wird nicht unterstützt!";
            messageText[SEBGlobalConstants.IND_LANGUAGE_ENGLISH, SEBGlobalConstants.IND_NO_OS_SUPPORT] = "This OS is not supported!";
            messageText[SEBGlobalConstants.IND_LANGUAGE_FRENCH, SEBGlobalConstants.IND_NO_OS_SUPPORT] = "Le système d'exploitation n'est pas supporté!";

            messageText[SEBGlobalConstants.IND_LANGUAGE_GERMAN, SEBGlobalConstants.IND_KILL_PROCESS_FAILED] = "Abschiessen des Prozesses %s fehlgeschlagen: %d!";
            messageText[SEBGlobalConstants.IND_LANGUAGE_ENGLISH, SEBGlobalConstants.IND_KILL_PROCESS_FAILED] = "Killing process %s failed: %d!";
            messageText[SEBGlobalConstants.IND_LANGUAGE_FRENCH, SEBGlobalConstants.IND_KILL_PROCESS_FAILED] = "Abattre le procès %s manqué / échoué: %d!";

            messageText[SEBGlobalConstants.IND_LANGUAGE_GERMAN, SEBGlobalConstants.IND_VIRTUAL_MACHINE_FORBIDDEN] = "SEB darf nicht auf einer virtuellen Maschine ausgeführt werden!";
            messageText[SEBGlobalConstants.IND_LANGUAGE_ENGLISH, SEBGlobalConstants.IND_VIRTUAL_MACHINE_FORBIDDEN] = "SEB may not be executed on a virtual machine!";
            messageText[SEBGlobalConstants.IND_LANGUAGE_FRENCH, SEBGlobalConstants.IND_VIRTUAL_MACHINE_FORBIDDEN] = "Défense d'exécuter SEB sur une machine virtuelle!";

            messageText[SEBGlobalConstants.IND_LANGUAGE_GERMAN, SEBGlobalConstants.IND_CLOSE_PROCESS_FAILED] = "Schliessen des Prozesses %s fehlgeschlagen. Soll der Prozess abgeschossen werden? ";
            messageText[SEBGlobalConstants.IND_LANGUAGE_ENGLISH, SEBGlobalConstants.IND_CLOSE_PROCESS_FAILED] = "Closing process %s failed! Kill this process?";
            messageText[SEBGlobalConstants.IND_LANGUAGE_FRENCH, SEBGlobalConstants.IND_CLOSE_PROCESS_FAILED] = "Abattre le procès %s manqué / échoué!";

            messageText[SEBGlobalConstants.IND_LANGUAGE_GERMAN, SEBGlobalConstants.IND_WINDOWS_SERVICE_NOT_AVAILABLE] = "SEB Windows-Dienst läuft nicht";
            messageText[SEBGlobalConstants.IND_LANGUAGE_ENGLISH, SEBGlobalConstants.IND_WINDOWS_SERVICE_NOT_AVAILABLE] = "SEB Windows service is not available";
            messageText[SEBGlobalConstants.IND_LANGUAGE_FRENCH, SEBGlobalConstants.IND_WINDOWS_SERVICE_NOT_AVAILABLE] = "";

            messageText[SEBGlobalConstants.IND_LANGUAGE_GERMAN, SEBGlobalConstants.IND_CLOSE_SEB_FAILED] = "Schliessen von SEB fehlgeschlagen. Falsches Kennwort.";
            messageText[SEBGlobalConstants.IND_LANGUAGE_ENGLISH, SEBGlobalConstants.IND_CLOSE_SEB_FAILED] = "Closing SEB failed! Password incorrect.";
            messageText[SEBGlobalConstants.IND_LANGUAGE_FRENCH, SEBGlobalConstants.IND_CLOSE_SEB_FAILED] = "Abattre SEB manqué / échoué!";

            messageText[SEBGlobalConstants.IND_LANGUAGE_GERMAN, SEBGlobalConstants.IND_DECRYPTING_SETTINGS_FAILED] = "Einstellungen können nicht entschlüsselt werden.\nSie haben entweder ein falsches Passwort eingegeben oder diese Einstellungen wurden mit einer inkompatiblen Version gespeichert.";
            messageText[SEBGlobalConstants.IND_LANGUAGE_ENGLISH, SEBGlobalConstants.IND_DECRYPTING_SETTINGS_FAILED] = "Cannot decrypt settings!\nYou either entered the wrong password or these settings were saved with an incompatible SEB version.";
            messageText[SEBGlobalConstants.IND_LANGUAGE_FRENCH, SEBGlobalConstants.IND_DECRYPTING_SETTINGS_FAILED] = "Cannot decrypt settings!\nYou either entered the wrong password or these settings were saved with an incompatible SEB version.";

            messageText[SEBGlobalConstants.IND_LANGUAGE_GERMAN, SEBGlobalConstants.IND_SETTINGS_NOT_USABLE] = "Öffnen der neuen Einstellungen fehlgeschlagen!\nDiese Einstellungen können nicht benutzt werden. Sie wurden möglicherweise mit einer neueren, inkompatiblen Version von SEB erstellt oder sie sind beschädigt.";
            messageText[SEBGlobalConstants.IND_LANGUAGE_ENGLISH, SEBGlobalConstants.IND_SETTINGS_NOT_USABLE] = "Opening new settings failed!\nThese settings cannot be used. They may have been created by an newer, incompatible version of SEB or are corrupted.";
            messageText[SEBGlobalConstants.IND_LANGUAGE_FRENCH, SEBGlobalConstants.IND_SETTINGS_NOT_USABLE] = "Opening new settings failed!\nThese settings cannot be used. They may have been created by an newer, incompatible version of SEB or are corruptedd.";
        }

        // ************************
        // Get the current language
        // ************************
        public static int SetCurrentLanguage()
        {
	        //int languageIndex;

            if (_languageIndex < 0)
            {

                _languageIndex = SEBGlobalConstants.IND_LANGUAGE_ENGLISH;

                String language = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;
                if (language.CompareTo("de") == 0) _languageIndex = SEBGlobalConstants.IND_LANGUAGE_GERMAN;
                if (language.CompareTo("en") == 0) _languageIndex = SEBGlobalConstants.IND_LANGUAGE_ENGLISH;
                if (language.CompareTo("fr") == 0) _languageIndex = SEBGlobalConstants.IND_LANGUAGE_FRENCH;
            }

            return _languageIndex;

        } // end of method   GetCurrentLanguage()

        // **********************************
        // Output an error or warning message
        // **********************************
        public static bool OutputErrorMessage(int messageTextIndex, int messageKindIndex,
            string sparam = null, int iparam = -1)
        {
            // If we are running in SebWindowsClient we need to activate it before showing the password dialog
            //if (SEBClientInfo.SebWindowsClientForm != null) SebWindowsClientForm.SEBToForeground(); //SEBClientInfo.SebWindowsClientForm.Activate();

            bool result = false;

            MessageBoxIcon icon;
            MessageBoxButtons buttons;
            string caption = "";
            string text = "";

            //logg(fp, "Enter OutputErrorMessage()\n\n");
            if (messageIcon[messageKindIndex] == SEBGlobalConstants.IND_MESSAGE_KIND_ERROR)
            {
                icon = MessageBoxIcon.Error;
            }
            else if (messageIcon[messageKindIndex] == SEBGlobalConstants.IND_MESSAGE_KIND_QUESTION)
            {
                icon = MessageBoxIcon.Question;
            }
            else 
            {
                icon = MessageBoxIcon.Warning;
            }

            //logg(fp, "Enter OutputErrorMessage()\n\n");
            if (messageButtons[messageKindIndex] == SEBGlobalConstants.IND_MESSAGE_KIND_ERROR)
            {
                buttons = MessageBoxButtons.OK;
            }
            else if (messageButtons[messageKindIndex] == SEBGlobalConstants.IND_MESSAGE_KIND_QUESTION)
            {
                buttons = MessageBoxButtons.YesNo;
            }
            else
            {
                buttons = MessageBoxButtons.OK;
            }


            caption = messageCaption[_languageIndex, messageKindIndex];
            text = messageText[_languageIndex, messageTextIndex];

            if (sparam != null)
            {
                text = text.Replace("%s", sparam);
            }
            if (iparam != -1)
            {
                text = text.Replace("%d", iparam.ToString());
            }

            if (MessageBox.Show(text, caption, buttons, icon) == DialogResult.Yes)
                result = true;

            //logg(fp, "Leave OutputErrorMessage()\n\n");
            return result;

        } // end of method   OutputErrorMessage()

        // **********************************
        // Output an error or warning message
        // **********************************
        public static bool OutputErrorMessageNew(string messageTitle, string messageText, int messageKindIndex, MessageBoxButtons messageButtons,
            string sparam = null, int iparam = -1)
        {
            // If we are running in SebWindowsClient we need to activate it before showing the password dialog
            if (SEBClientInfo.SebWindowsClientForm != null) SEBClientInfo.SebWindowsClientForm.Activate(); //SebWindowsClientForm.SEBToForeground();
            
            bool result = false;

            MessageBoxIcon icon;

            if (messageIcon[messageKindIndex] == SEBGlobalConstants.IND_MESSAGE_KIND_ERROR)
            {
                icon = MessageBoxIcon.Error;
            }
            else if (messageIcon[messageKindIndex] == SEBGlobalConstants.IND_MESSAGE_KIND_QUESTION)
            {
                icon = MessageBoxIcon.Question;
            }
            else
            {
                icon = MessageBoxIcon.Warning;
            }

            if (sparam != null)
            {
                messageText = messageText.Replace("%s", sparam);
            }
            if (iparam != -1)
            {
                messageText = messageText.Replace("%d", iparam.ToString());
            }
            DialogResult messageBoxResult = MessageBox.Show(messageText, messageTitle, messageButtons, icon);
            if (messageBoxResult == DialogResult.OK || messageBoxResult == DialogResult.Yes || messageBoxResult == DialogResult.Retry)
                result = true;

            return result;

        } // end of method   OutputErrorMessageNew()

    }
}
