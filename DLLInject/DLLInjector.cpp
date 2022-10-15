#include <iostream>
#include <Windows.h>
#include <tlhelp32.h>



bool inject(HANDLE hProcess, LPVOID dllNamePtr) {
    try{
        LPTHREAD_START_ROUTINE funcLoadLibrary = (LPTHREAD_START_ROUTINE)GetProcAddress(GetModuleHandleA("Kernel32.dll"), "LoadLibraryW");
        if (!funcLoadLibrary)
            throw "Failed to retrieve a static function pointer to `LoadLbraryA`\n";
        
        HANDLE loadLibraryThread = CreateRemoteThread(hProcess, NULL, NULL, funcLoadLibrary, dllNamePtr, NULL, NULL);
        if (!loadLibraryThread || loadLibraryThread == INVALID_HANDLE_VALUE) 
            throw "Failed to create remote thread\n";

        WaitForSingleObject(loadLibraryThread, INFINITE);
        CloseHandle(loadLibraryThread);
    }
    catch (const char* err) {
        std::cout << "An error occurred: " << err << std::endl;
        return false;
    }

    return true;
}

bool tryEject(HANDLE hProcess, LPVOID dllNamePtr) {
    std::cout << "Input 'd' to detach NumbersMod from the game. Or anything else to close this window.\n";
    char buf;
    std::cin >> buf;
    std::cout << "'" << buf << "'\n";
    if (buf == 'd') {
        LPTHREAD_START_ROUTINE funcUnload = (LPTHREAD_START_ROUTINE)GetProcAddress(GetModuleHandleA("Kernel32.dll"), "FreeLibraryW");
        HANDLE unloadThread = CreateRemoteThread(hProcess, NULL, NULL, funcUnload, dllNamePtr, NULL, NULL);
        WaitForSingleObject(unloadThread, INFINITE);
        CloseHandle(unloadThread);
        std::cout << "Detached.\n";
    }
    return true;
}

HANDLE findProcess(const wchar_t* targetProcName)
{
    HANDLE res = 0;

    PROCESSENTRY32W entry;
    entry.dwSize = sizeof(PROCESSENTRY32W);
    HANDLE snapshot = CreateToolhelp32Snapshot(TH32CS_SNAPPROCESS, NULL);

    if (Process32FirstW(snapshot, &entry) == TRUE) {
        while (Process32NextW(snapshot, &entry) == TRUE) {
            if (wcscmp(entry.szExeFile, targetProcName) == 0) {
                res = OpenProcess(PROCESS_ALL_ACCESS, FALSE, entry.th32ProcessID);
                break;
            }
        }
    }

    CloseHandle(snapshot);
    return res;
}

LPVOID allocString(HANDLE hProcess, const wchar_t* str) {
    size_t nameSize = (wcslen(str) + 1) * sizeof(*str);
    LPVOID allocPtr = VirtualAllocEx(hProcess, 0, nameSize, MEM_COMMIT, PAGE_READWRITE);
    if (!allocPtr) {
        throw "Failed to allocate memory in the target process\n";
    }
    WriteProcessMemory(hProcess, allocPtr, str, nameSize, NULL);
    return allocPtr;
}


wchar_t* charToWChar(const char* text)
{
    const size_t size = strlen(text) + 1;
    wchar_t* wText = new wchar_t[size];
    mbstowcs(wText, text, size);
    return wText;
}

int main(int argc, char** argv)
{
    if (argc != 2) {
        fprintf(stderr, "Wrong number of arguments (must be 1)");
        return 1;
    }

    const wchar_t* dllName = charToWChar(argv[1]);
    const wchar_t* procName = L"sekiro.exe";

    std::wcout << "Searching for process " << procName << "...\n";
    HANDLE proc;
    while (true) {
        proc = findProcess(procName);
        if (proc != 0)
            break;
        std::cout << "Process not found.\n";
        Sleep(1000);
    }

   
    wchar_t fullName[MAX_PATH] = { 0 };
    GetFullPathNameW(dllName, MAX_PATH, fullName, NULL);
    std::wcout << "Full name of dll is:\n" << fullName << "\n";

    LPVOID dllNamePtr = allocString(proc, fullName);

    std::cout << "Injection started.\n";
    if (inject(proc, dllNamePtr))
        std::cout << "Looks like injection succeeded.\n";

    dllNamePtr = allocString(proc, fullName);
    //tryEject(proc, dllNamePtr);
   

    return 0;
}



