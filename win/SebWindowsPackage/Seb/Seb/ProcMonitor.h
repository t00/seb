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
 * The Original Code is responsible for monitoring processes.  
 *
 * The Initial Developer of the Original Code is ETH Zuerich.
 * Portions created by the Initial Developer are Copyright (C) 2008
 * the Initial Developer. All Rights Reserved.
 *
 * Contributor(s):
 *   Georg Troxler <gtroxler@student.ethz.ch>
 *   Oliver Rahs <rahs@net.ethz.ch>
 *
 * ***** END LICENSE BLOCK ***** */

struct threadParameters
{
	vector<long> *allowedProcesses;
	HDESK		  desktop;
	long		  hold;
	long		  confirm;
	long		  procedureReady;
	map<string, string> mpProcesses;
};


BOOL CALLBACK FindFirefoxWindow(HWND hWnd, LPARAM lParam)
{
	char String[255];
	
	HWND* ohWnd = (HWND*)lParam;
	string firefox = "Mozilla Firefox";

	if (!hWnd)					  return TRUE;	// Not a window
	if (!::IsWindowVisible(hWnd)) return TRUE;	// Not visible
	if (!SendMessage(hWnd, WM_GETTEXT, sizeof(String), (LPARAM)String)) return TRUE;	// No window title

	string s = String;
	if (s.find(firefox) != string::npos)
	{
		*ohWnd = hWnd;	
	}

	return TRUE;
}





// function to monitor the running processes
VOID GetRunningProcesses(vector<long> & inoutPreviousProcesses)
{
	PROCESSENTRY32 pe32;
	ZeroMemory(&pe32, sizeof(PROCESSENTRY32));
	pe32.dwSize		= sizeof(PROCESSENTRY32);

	//get_longest_process_name_length(&nProcessNameLength);
	//nProcessNameLength = nProcessNameLength + 3;
	
	HANDLE hProcSnapShot = CreateToolhelp32Snapshot(TH32CS_SNAPPROCESS,0);

	inoutPreviousProcesses.clear();

	if (Process32First(hProcSnapShot, &pe32) == TRUE)
	{
		//trim_process_name(pe32.szExeFile,lpszProcessNameBuffer,pe32.th32ProcessID);
		//strip_extension_from_executable(pe32.szExeFile, lpszProcessNameBuffer);
		//printf("%s;%d;%d;%d\n",lpszProcessNameBuffer,pe32.th32ProcessID,pe32.th32ParentProcessID,pe32.pcPriClassBase);
		inoutPreviousProcesses.push_back(pe32.th32ProcessID);
		while (Process32Next(hProcSnapShot, &pe32) == TRUE)
		{

		//	trim_process_name(pe32.szExeFile,lpszProcessNameBuffer,pe32.th32ProcessID);
		//	strip_extension_from_executable(pe32.szExeFile, lpszProcessNameBuffer);
		//	printf("%s;%d;%d;%d;%d\n",lpszProcessNameBuffer,pe32.th32ProcessID,pe32.th32ParentProcessID,pe32.pcPriClassBase,pe32.cntThreads);
			inoutPreviousProcesses.push_back(pe32.th32ProcessID);
		}
	}

	CloseHandle(hProcSnapShot);
}



string StringToUpper(string strToConvert)
{
	if (!strToConvert.empty())
	{
		for (unsigned int i = 0; i < strToConvert.length(); i++)
		{
			strToConvert[i] = toupper(strToConvert[i]);
		}
	}
	return strToConvert;
}



VOID KillAllNotInList(vector<long> & allowedProcesses, map<string,string> mpProcessNames, bool terminateAll)
{
	vector<long>           nowRunningProcesses;
	vector<long>::iterator sourceIterator;
	vector<long>::iterator   destIterator;
	vector<long> killList;

	GetRunningProcesses(nowRunningProcesses);
	sourceIterator = nowRunningProcesses.begin();

	while (sourceIterator != nowRunningProcesses.end())
	{
		destIterator = allowedProcesses.begin();
		while (destIterator != allowedProcesses.end())
		{
			if ((*sourceIterator) == (*destIterator))
			{
				break;
			}
			destIterator++;
		}
		// process was not found
		if (destIterator == allowedProcesses.end())
		{
			killList.push_back(*sourceIterator);
		}
		sourceIterator++;
	}

	sourceIterator = killList.begin();
	while (sourceIterator != killList.end())
	{
		bool killProc = true;
		if (!terminateAll)
		{
			string procName = StringToUpper(GetProcessNameFromID(*sourceIterator));
			map<string,string>::iterator it;
			for (it = mpProcessNames.begin(); it != mpProcessNames.end(); it++)
			{
				string permittedProcName = StringToUpper((*it).second);
				if (permittedProcName.find(procName) != string::npos)
				{
					killProc = false;
					allowedProcesses.insert(allowedProcesses.end(), *sourceIterator);
					break;
				}
			}
		}
		if (killProc)
		{
			logg(fp, "Calling  KILL_PROC_BY_ID(%d)\n", *sourceIterator);
			KILL_PROC_BY_ID(*sourceIterator);
		}
		sourceIterator++;
	}
	killList.clear();
}



VOID MonitorProcesses(threadParameters & parameters)
{
	HWND hWnd;

	//ostream file;
	//file = fopen("C:\Temp\Log.txt","ba+");

	while (true)
	{
		// kills the processes
		KillAllNotInList(*(parameters.allowedProcesses), parameters.mpProcesses, false);

		// find all open windows of the desktop thread
		hWnd = 0;
		EnumDesktopWindows(parameters.desktop, FindFirefoxWindow, (LPARAM)&hWnd);
		if (hWnd != 0)
		{
			PostMessage(hWnd, 0x0112, 0xF060, 0);
			hWnd = 0;
		}

		Sleep(500);

		if (parameters.hold != 0)
		{
			parameters.confirm = 1;
			Sleep (5);
			while (parameters.hold != 0)
			{
				Sleep(5);
			}
			parameters.confirm = 0;
			Sleep(5000); // allow all processes to launch
			parameters.allowedProcesses->clear();
			GetRunningProcesses(*(parameters.allowedProcesses));
			Sleep(7000);
		}
		parameters.confirm = 0;

	} // end while (true)

}