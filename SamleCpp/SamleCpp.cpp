// SamleCpp.cpp : Defines the exported functions for the DLL application.
//

#include "stdafx.h"
extern "C"__declspec(dllexport)void Method1()
{
	int* p = NULL;
	*p = 1;
}


extern "C"__declspec(dllexport)int Method2(int i)
{
	int r = i * 100;
	return r;
}

