typedef struct {
	BYTE	DISABLE_CTRL_ESC : 1,
			DISABLE_ALT_TAB : 1,
			DISABLE_ALT_ESC : 1,
			DISABLE_ALT_F4 : 1,
			DISABLE_START_MENU : 1,
			DISABLE_RIGHT_MOUSE : 1,
			DISABLE_LEFT_MOUSE : 1,
			DISABLE_F1 : 1,
			DISABLE_F2 : 1,
			DISABLE_F3 : 1,
			DISABLE_F4 : 1,
			DISABLE_F5 : 1,
			DISABLE_F6 : 1,
			DISABLE_F7 : 1,
			DISABLE_F8 : 1,
			DISABLE_F9 : 1,
			DISABLE_F10 : 1,
			DISABLE_F11 : 1,
			DISABLE_F12 : 1,
			DISABLE_ESCAPE : 1;
} ALTER_FLAGS;

LPCTSTR KEY_RegPolicySystem = "Software\\Microsoft\\Windows\\CurrentVersion\\Policies\\System";
LPCTSTR KEY_RegPolicyExplorer = "Software\\Microsoft\\Windows\\CurrentVersion\\Policies\\Explorer";
LPCTSTR VAL_DisableLockWorkstation = "DisableLockWorkstation";
LPCTSTR VAL_DisableTaskMgr = "DisableTaskMgr";
LPCTSTR VAL_DisableChangePassword = "DisableChangePassword";
LPCTSTR VAL_NoClose = "NoClose";
LPCTSTR VAL_NoLogoff = "NoLogoff";
