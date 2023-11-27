// dllmain.cpp : Defines the entry point for the DLL application.
#include "pch.h"

/**
 * @brief Function is called when DLL is loaded or unloaded (Entry point)
 * @param ul_reason_for_call - reason for calling function
 * @param lpReserved - reserved
 * @return TRUE if success
 */
BOOL APIENTRY DllMain(HMODULE hModule,
                      DWORD ul_reason_for_call,
                      LPVOID lpReserved
)
{
	switch (ul_reason_for_call)
	{
	case DLL_PROCESS_ATTACH:
	case DLL_THREAD_ATTACH:
	case DLL_THREAD_DETACH:
	case DLL_PROCESS_DETACH:
		break;
	default:
		return FALSE;
	}
	return TRUE;
}
