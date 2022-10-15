#include "includes.h"
#include <iostream>
#include <stdio.h>
#include <io.h>
#include <fcntl.h>
#include "config.h"

extern LRESULT ImGui_ImplWin32_WndProcHandler(HWND hWnd, UINT msg, WPARAM wParam, LPARAM lParam);

Present oPresent;
HMODULE dllModule = NULL;
HWND window = NULL;
WNDPROC oWndProc;
ID3D11Device* pDevice = NULL;
ID3D11DeviceContext* pContext = NULL;
ID3D11RenderTargetView* mainRenderTargetView;

void NumbersModLoop(HWND window);

void InitImGui(ImVec2 screenSize)
{
	ImGui::CreateContext();
	ImGuiIO& io = ImGui::GetIO();
	io.ConfigFlags = ImGuiConfigFlags_NoMouseCursorChange;
    io.Fonts->AddFontFromFileTTF("NumbersMod\\MinionPro-Bold.ttf", min(screenSize.x / 75, screenSize.y / 40));
	ImGui_ImplWin32_Init(window);
	ImGui_ImplDX11_Init(pDevice, pContext);
}

LRESULT __stdcall WndProc(const HWND hWnd, UINT uMsg, WPARAM wParam, LPARAM lParam) {

	if (true && ImGui_ImplWin32_WndProcHandler(hWnd, uMsg, wParam, lParam))
		return true;

	return CallWindowProc(oWndProc, hWnd, uMsg, wParam, lParam);
}

bool init = false;
HRESULT __stdcall hkPresent(IDXGISwapChain* pSwapChain, UINT SyncInterval, UINT Flags)
{
	if (!init)
	{
		if (SUCCEEDED(pSwapChain->GetDevice(__uuidof(ID3D11Device), (void**)& pDevice)))
		{
			pDevice->GetImmediateContext(&pContext);
			DXGI_SWAP_CHAIN_DESC sd;
			pSwapChain->GetDesc(&sd);
			window = sd.OutputWindow;
			ID3D11Texture2D* pBackBuffer;
			pSwapChain->GetBuffer(0, __uuidof(ID3D11Texture2D), (LPVOID*)& pBackBuffer);
			pDevice->CreateRenderTargetView(pBackBuffer, NULL, &mainRenderTargetView);
			pBackBuffer->Release();
			oWndProc = (WNDPROC)SetWindowLongPtr(window, GWLP_WNDPROC, (LONG_PTR)WndProc);
            RECT rect;
            GetWindowRect(window, &rect);
            printf("screen size: %ld %ld\n", rect.right - rect.left, rect.bottom - rect.top);
			InitImGui(ImVec2(rect.right - rect.left, rect.bottom - rect.top));
			init = true;
            Config::init(window);
		}

		else
			return oPresent(pSwapChain, SyncInterval, Flags);
	}

	ImGui_ImplDX11_NewFrame();
	ImGui_ImplWin32_NewFrame();
	ImGui::NewFrame();
    NumbersModLoop(window);
    Config::update();
	ImGui::Render();

	pContext->OMSetRenderTargets(1, &mainRenderTargetView, NULL);
	ImGui_ImplDX11_RenderDrawData(ImGui::GetDrawData());
	return oPresent(pSwapChain, SyncInterval, Flags);
}

DWORD WINAPI MainThread(LPVOID lpReserved)
{
	bool init_hook = false;
	do
	{
		if (kiero::init(kiero::RenderType::D3D11) == kiero::Status::Success)
		{
			kiero::bind(8, (void**)& oPresent, hkPresent);
			init_hook = true;
		}
	} while (!init_hook);
	return TRUE;
}

void setupConsoleOutput();

BOOL WINAPI DllMain(HMODULE hMod, DWORD dwReason, LPVOID lpReserved)
{
	switch (dwReason)
	{
	case DLL_PROCESS_ATTACH:
        dllModule = hMod;
		DisableThreadLibraryCalls(hMod);
		CreateThread(nullptr, 0, MainThread, hMod, 0, nullptr);
        //setupConsoleOutput();
		break;
	case DLL_PROCESS_DETACH:
		kiero::shutdown();
		break;
	}
	return TRUE;
}


//copy-paste from https://stackoverflow.com/questions/311955/redirecting-cout-to-a-console-in-windows
void setupConsoleOutput() {
	AllocConsole();
	// Re-initialize the C runtime "FILE" handles with clean handles bound to "nul". We do this because it has been
	// observed that the file number of our standard handle file objects can be assigned internally to a value of -2
	// when not bound to a valid target, which represents some kind of unknown internal invalid state. In this state our
	// call to "_dup2" fails, as it specifically tests to ensure that the target file number isn't equal to this value
	// before allowing the operation to continue. We can resolve this issue by first "re-opening" the target files to
	// use the "nul" device, which will place them into a valid state, after which we can redirect them to our target
	// using the "_dup2" function.

	FILE* dummyFile;
	freopen_s(&dummyFile, "nul", "w", stdout);
	freopen_s(&dummyFile, "nul", "w", stderr);

	HANDLE stdHandle = GetStdHandle(STD_OUTPUT_HANDLE);
	if (stdHandle != INVALID_HANDLE_VALUE) {
		int fileDescriptor = _open_osfhandle((intptr_t)stdHandle, _O_TEXT);
		if (fileDescriptor != -1) {
			FILE* file = _fdopen(fileDescriptor, "w");
			if (file != NULL) {
				int dup2Result = _dup2(_fileno(file), _fileno(stdout));
				if (dup2Result == 0) {
					setvbuf(stdout, NULL, _IONBF, 0);
				}
			}
		}
	}


	stdHandle = GetStdHandle(STD_ERROR_HANDLE);
	if (stdHandle != INVALID_HANDLE_VALUE) {
		int fileDescriptor = _open_osfhandle((intptr_t)stdHandle, _O_TEXT);
		if (fileDescriptor != -1) {
			FILE* file = _fdopen(fileDescriptor, "w");
			if (file != NULL) {
				int dup2Result = _dup2(_fileno(file), _fileno(stderr));
				if (dup2Result == 0) {
					setvbuf(stderr, NULL, _IONBF, 0);
				}
			}
		}
	}

	// Clear the error state for each of the C++ standard stream objects. We need to do this, as attempts to access the
	// standard streams before they refer to a valid target will cause the iostream objects to enter an error state. In
	// versions of Visual Studio after 2005, this seems to always occur during startup regardless of whether anything
	// has been read from or written to the targets or not.
	std::wcout.clear();
	std::cout.clear();

	std::wcerr.clear();
	std::cerr.clear();
}