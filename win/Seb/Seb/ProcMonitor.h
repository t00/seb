

struct threadParameters{
	vector< long > * allowedProcesses;
	HDESK			desktop;
	long			hold;
	long			confirm;
	long			procedureReady;
};


BOOL CALLBACK FindFirefoxWindow(HWND hWnd, LPARAM lParam) {
	char String[255];
	
	HWND * ohWnd = (HWND*)lParam;
	string firefox = "Mozilla Firefox";

	if (!hWnd)
		return TRUE;		// Not a window
	if (!::IsWindowVisible(hWnd))
		return TRUE;		// Not visible
	if (!SendMessage(hWnd, WM_GETTEXT, sizeof(String), (LPARAM)String))
		return TRUE;		// No window title
	

	string s = String;
	if(s.find(firefox) != string::npos){
		*ohWnd = hWnd;
		
	}

	return TRUE;
}





// function to monitor the running processes
VOID GetRunningProcesses(vector< long > & inoutPreviousProcesses){
	PROCESSENTRY32 pe32;
	ZeroMemory(&pe32,sizeof(PROCESSENTRY32));
	pe32.dwSize = sizeof(PROCESSENTRY32);

	//get_longest_process_name_length(&nProcessNameLength);
	//nProcessNameLength = nProcessNameLength + 3;
	
	HANDLE hProcSnapShot = CreateToolhelp32Snapshot(TH32CS_SNAPPROCESS,0);

	inoutPreviousProcesses.clear();

	if(Process32First(hProcSnapShot,&pe32) == TRUE){
		//trim_process_name(pe32.szExeFile,lpszProcessNameBuffer,pe32.th32ProcessID);
		//strip_extension_from_executable(pe32.szExeFile, lpszProcessNameBuffer);
		//printf("%s;%d;%d;%d\n",lpszProcessNameBuffer,pe32.th32ProcessID,pe32.th32ParentProcessID,pe32.pcPriClassBase);
		inoutPreviousProcesses.push_back (pe32.th32ProcessID);
		while(Process32Next(hProcSnapShot,&pe32) == TRUE){

		//	trim_process_name(pe32.szExeFile,lpszProcessNameBuffer,pe32.th32ProcessID);
		//	strip_extension_from_executable(pe32.szExeFile, lpszProcessNameBuffer);
		//	printf("%s;%d;%d;%d;%d\n",lpszProcessNameBuffer,pe32.th32ProcessID,pe32.th32ParentProcessID,pe32.pcPriClassBase,pe32.cntThreads);
			inoutPreviousProcesses.push_back (pe32.th32ProcessID);
		}

	}

	CloseHandle(hProcSnapShot);
}





VOID KillAllNotInList(vector< long > & allowedProcesses){
	vector< long >  nowRunningProcesses;
	vector< long >::iterator sourceIterator;
	vector< long >::iterator destIterator;
	vector< long > killList;

	GetRunningProcesses(nowRunningProcesses);

	sourceIterator = nowRunningProcesses.begin();

	while(sourceIterator != nowRunningProcesses.end()){
			destIterator = allowedProcesses.begin();
			while(destIterator != allowedProcesses.end()){
				if((*sourceIterator) == (*destIterator)){
					break;
				}
				destIterator++;
			}
			// process was not found
			if(destIterator == allowedProcesses.end()){
				killList.push_back(*sourceIterator);
			}
			sourceIterator++;
		}

		sourceIterator = killList.begin();
		while(sourceIterator != killList.end()){
 			KILL_PROC_BY_ID(*sourceIterator);
			sourceIterator++;
		}
		killList.clear();
}


VOID MonitorProcesses(threadParameters & parameters){
	HWND hWnd;
	
	
	//ostream file;
	//file = fopen("C:\Temp\Log.txt","ba+");

	while(true){
		// kills the processes
		KillAllNotInList(*(parameters.allowedProcesses));

		// find all open windows of the desktop thread
		hWnd = 0;
		EnumDesktopWindows(parameters.desktop, FindFirefoxWindow, (LPARAM)&hWnd);
		if(hWnd != 0)
		{
				PostMessage(hWnd,0x0112, 0xF060, 0);
				hWnd = 0;
		}

		Sleep (500);

		if(parameters.hold != 0){
			parameters.confirm = 1;
			Sleep (5);
			while(parameters.hold!=0){
				Sleep (5);
			}
			parameters.confirm = 0;
			Sleep (5000); // allow all processes to launch
			parameters.allowedProcesses->clear();
			GetRunningProcesses(*(parameters.allowedProcesses));
			Sleep (7000);
		}
		parameters.confirm = 0;

	}

}