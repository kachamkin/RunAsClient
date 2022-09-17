#include "pch.h"

extern "C" __declspec(dllexport) BOOL __stdcall Decrypt(LPTSTR base64str);
extern "C" __declspec(dllexport) BOOL __stdcall Encrypt(LPWSTR base64str, LPWSTR b64Result);

#define BLOCK_SIZE 256
#define MAX_PLAIN_TEXT_BYTES 190

using namespace std;

wchar_t* a2w(const char* c, int codePage = CP_UTF8)
{
    int wchars_num = MultiByteToWideChar(codePage, 0, c, -1, NULL, 0);
    wchar_t* wc = new wchar_t[wchars_num];
    MultiByteToWideChar(codePage, 0, c, -1, wc, wchars_num);

    return wc;
}

void CryptCleanUp(BYTE** buffers, int numBuffers, BCRYPT_ALG_HANDLE hAlg = NULL, BCRYPT_KEY_HANDLE hKey = NULL, HGLOBAL hResource = NULL)
{
    if (hAlg)
        BCryptCloseAlgorithmProvider(hAlg, 0);
    if (hKey)
        BCryptDestroyKey(hKey);

    for (int i = 0; i < numBuffers; i++)
        free(buffers[i]);

    if (hResource)
    {
        UnlockResource(hResource);
        FreeResource(hResource);
    }
}

void AddBufferToArray(BYTE** bufArray, BYTE* buffer, int* numBuffers)
{
    bufArray[*numBuffers] = buffer;
    (*numBuffers)++;
}

BYTE* CreateBuffer(DWORD size, BYTE** buffers, int* pNumBuffers, BCRYPT_ALG_HANDLE hAlg = NULL, BCRYPT_KEY_HANDLE hKey = NULL)
{
    BYTE* pBuffer = (BYTE*)malloc(size);
    if (!pBuffer)
    {
        CryptCleanUp(buffers, *pNumBuffers, hAlg, hKey);
        return NULL;
    }
    memset(pBuffer, '\0', size);
    AddBufferToArray(buffers, pBuffer, pNumBuffers);
    return pBuffer;
}

//void XOR(wchar_t* source, size_t nSource, wchar_t* key, size_t nKey)
//{
//    size_t j = 0;
//
//    for (size_t i = 0; i < nSource; i++)
//    {
//        *(source + i) = *(source + i) xor *(key + j);
//        if (j == nKey - 1)
//            j = 0;
//        else
//            j++;
//    }
//}

HMODULE GetCurrentModule()
{ 
    HMODULE hModule = NULL;
    GetModuleHandleEx(
        GET_MODULE_HANDLE_EX_FLAG_FROM_ADDRESS,
        (LPCTSTR)GetCurrentModule,
        &hModule);

    return hModule;
}

BOOL Decrypt(LPTSTR base64str)
{
    int numBuffers = 0;
    BYTE* buffers[10] = {};

    BCRYPT_ALG_HANDLE hCryptProv = NULL;
    BCRYPT_KEY_HANDLE hKey = NULL;

    DWORD bytesReq = 0;
    if (!CryptStringToBinary(base64str, NULL, CRYPT_STRING_BASE64, NULL, &bytesReq, NULL, NULL))
        return FALSE;

    BYTE* pBuffer = CreateBuffer(bytesReq, buffers, &numBuffers, hCryptProv, hKey);
    if (!pBuffer)
        return FALSE;

    if (!CryptStringToBinary(base64str, NULL, CRYPT_STRING_BASE64, pBuffer, &bytesReq, NULL, NULL))
    {
        CryptCleanUp(buffers, numBuffers);
        return FALSE;
    }

    HMODULE curModule = GetCurrentModule();

    HRSRC hRsc = FindResource(curModule, MAKEINTRESOURCE(IDR_KEY1), L"Key");
    if (!hRsc)
    {
        CryptCleanUp(buffers, numBuffers);
        return FALSE;
    }

    HGLOBAL hGlobal = LoadResource(curModule, hRsc);
    if (!hGlobal)
    {
        CryptCleanUp(buffers, numBuffers);
        return FALSE;
    }

    BYTE* pKey = (BYTE*)LockResource(hGlobal);
    if (!pKey)
    {
        CryptCleanUp(buffers, numBuffers, NULL, NULL, hGlobal);
        return FALSE;
    }

    DWORD bytesReqKey = SizeofResource(curModule, hRsc);
    if (!bytesReqKey)
    {
        CryptCleanUp(buffers, numBuffers, NULL, NULL, hGlobal);
        return FALSE;
    }

    DWORD keyBlobLength = 0;
    if (!CryptDecodeObjectEx(X509_ASN_ENCODING | PKCS_7_ASN_ENCODING, PKCS_RSA_PRIVATE_KEY,
        pKey,
        bytesReqKey, CRYPT_DECODE_NOCOPY_FLAG, NULL, NULL, &keyBlobLength))
    {
        CryptCleanUp(buffers, numBuffers);
        return FALSE;
    }

    BYTE* keyBlob = CreateBuffer(keyBlobLength, buffers, &numBuffers, hCryptProv, hKey);
    if (!keyBlob)
        return FALSE;

    if (!CryptDecodeObjectEx(X509_ASN_ENCODING | PKCS_7_ASN_ENCODING, PKCS_RSA_PRIVATE_KEY,
        pKey,
        bytesReqKey, CRYPT_DECODE_NOCOPY_FLAG, NULL, keyBlob, &keyBlobLength))
    {
        CryptCleanUp(buffers, numBuffers, NULL, NULL, hGlobal);
        return FALSE;
    }

    if (FAILED(BCryptOpenAlgorithmProvider(
        &hCryptProv,
        BCRYPT_RSA_ALGORITHM, NULL,
        0)))
    {
        CryptCleanUp(buffers, numBuffers, NULL, NULL, hGlobal);
        return FALSE;
    }

    ULONG keySize = 0;
    if (FAILED(BCryptImportKeyPair(hCryptProv, NULL, LEGACY_RSAPRIVATE_BLOB, &hKey, keyBlob, keyBlobLength, BCRYPT_NO_KEY_VALIDATION)))
    {
        CryptCleanUp(buffers, numBuffers, hCryptProv, NULL, hGlobal);
        return FALSE;
    }

    BCRYPT_OAEP_PADDING_INFO pi = { 0 };
    pi.pszAlgId = BCRYPT_SHA256_ALGORITHM;

    //Decryption by blocks if plain text length is more than maxDataLen

    DWORD blockSize = BLOCK_SIZE; //for 2048-bit RSA key
    DWORD maxDataLen = MAX_PLAIN_TEXT_BYTES; //for 2048-bit RSA key and SHABLOCK_SIZE OAEP: KeySize - 2 * hashSize / 8 - 2 (sizes in bytes)

    BYTE* pMaxLenBuffer = CreateBuffer(blockSize, buffers, &numBuffers, hCryptProv, hKey);
    if (!pMaxLenBuffer)
        return FALSE;

    unsigned int blockNum = bytesReq / blockSize;
    unsigned long resLen = blockNum * maxDataLen;
    BYTE* pResultBuffer = CreateBuffer(resLen + 1, buffers, &numBuffers, hCryptProv, hKey);
    if (!pResultBuffer)
        return FALSE;

    *(pResultBuffer + resLen) = '\0';

    BYTE* pbRes = CreateBuffer(maxDataLen, buffers, &numBuffers, hCryptProv, hKey);
    if (!pbRes)
        return FALSE;

    for (unsigned int i = 0; i < blockNum; i++)
    {
        memcpy(pMaxLenBuffer, pBuffer + i * blockSize, blockSize);

        if (FAILED(BCryptDecrypt(
            hKey, pMaxLenBuffer, blockSize, &pi, NULL, 0, pbRes, maxDataLen, &maxDataLen, BCRYPT_PAD_OAEP)))
        {
            CryptCleanUp(buffers, numBuffers, hCryptProv, hKey, hGlobal);
            return FALSE;
        }

        memcpy(pResultBuffer + i * maxDataLen, pbRes, maxDataLen);
    }

    //LPTSTR pBufStr = a2w((char*)pResultBuffer);
    //_tcscpy_s(base64str, _tcslen(pBufStr) + 1, pBufStr);

    //delete pBufStr;

    _tcscpy_s(base64str, _tcslen((wchar_t*)pResultBuffer) + 1, (wchar_t*)pResultBuffer);

    CryptCleanUp(buffers, numBuffers, hCryptProv, hKey, hGlobal);

    return TRUE;
}

BOOL Encrypt(LPWSTR base64str, LPWSTR b64Result)
{
    int numBuffers = 0;
    BYTE* buffers[10] = {};

    BCRYPT_ALG_HANDLE hCryptProv = NULL;
    BCRYPT_KEY_HANDLE hKey = NULL;

    DWORD bytesReq = _tcslen(base64str) * sizeof(TCHAR);
    BYTE* pBuffer = (BYTE*)base64str;

    HMODULE curModule = GetCurrentModule();

    HRSRC hRsc = FindResource(curModule, MAKEINTRESOURCE(IDR_KEY1), L"Key");
    if (!hRsc)
    {
        CryptCleanUp(buffers, numBuffers);
        return FALSE;
    }

    HGLOBAL hGlobal = LoadResource(curModule, hRsc);
    if (!hGlobal)
    {
        CryptCleanUp(buffers, numBuffers);
        return FALSE;
    }

    BYTE* pKey = (BYTE*)LockResource(hGlobal);
    if (!pKey)
    {
        CryptCleanUp(buffers, numBuffers, NULL, NULL, hGlobal);
        return FALSE;
    }

    DWORD bytesReqKey = SizeofResource(curModule, hRsc);
    if (!bytesReqKey)
    {
        CryptCleanUp(buffers, numBuffers, NULL, NULL, hGlobal);
        return FALSE;
    }

    DWORD keyBlobLength = 0;
    if (!CryptDecodeObjectEx(X509_ASN_ENCODING | PKCS_7_ASN_ENCODING, PKCS_RSA_PRIVATE_KEY,
        pKey,
        bytesReqKey, CRYPT_DECODE_NOCOPY_FLAG, NULL, NULL, &keyBlobLength))
    {
        CryptCleanUp(buffers, numBuffers, NULL, NULL, hGlobal);
        return FALSE;
    }

    BYTE* keyBlob = CreateBuffer(keyBlobLength, buffers, &numBuffers, hCryptProv, hKey);
    if (!keyBlob)
        return FALSE;

    if (!CryptDecodeObjectEx(X509_ASN_ENCODING | PKCS_7_ASN_ENCODING, PKCS_RSA_PRIVATE_KEY,
        pKey,
        bytesReqKey, CRYPT_DECODE_NOCOPY_FLAG, NULL, keyBlob, &keyBlobLength))
    {
        CryptCleanUp(buffers, numBuffers, NULL, NULL, hGlobal);
        return FALSE;
    }

    if (FAILED(BCryptOpenAlgorithmProvider(
        &hCryptProv,
        BCRYPT_RSA_ALGORITHM, NULL,
        0)))
    {
        CryptCleanUp(buffers, numBuffers, NULL, NULL, hGlobal);
        return FALSE;
    }

    ULONG keySize = 0;
    if (FAILED(BCryptImportKeyPair(hCryptProv, NULL, LEGACY_RSAPRIVATE_BLOB, &hKey, keyBlob, keyBlobLength, BCRYPT_NO_KEY_VALIDATION)))
    {
        CryptCleanUp(buffers, numBuffers, hCryptProv, NULL, hGlobal);
        return FALSE;
    }

    BCRYPT_OAEP_PADDING_INFO pi = { 0 };
    pi.pszAlgId = BCRYPT_SHA256_ALGORITHM;

    //Decryption by blocks if plain text length is more than maxDataLen

    DWORD blockSize = BLOCK_SIZE; //for 2048-bit RSA key
    DWORD maxDataLen = MAX_PLAIN_TEXT_BYTES; //for 2048-bit RSA key and SHABLOCK_SIZE OAEP: KeySize - 2 * hashSize / 8 - 2 (sizes in bytes)

    BYTE* pMaxLenBuffer = CreateBuffer(maxDataLen, buffers, &numBuffers, hCryptProv, hKey);
    if (!pMaxLenBuffer)
        return FALSE;

    unsigned int blockNum = bytesReq / maxDataLen;
    if (bytesReq % maxDataLen)
        blockNum++;

    unsigned long resLen = blockNum * blockSize;
    BYTE* pResultBuffer = CreateBuffer(resLen, buffers, &numBuffers, hCryptProv, hKey);
    if (!pResultBuffer)
        return FALSE;

    BYTE* pbRes = CreateBuffer(blockSize, buffers, &numBuffers, hCryptProv, hKey);
    if (!pbRes)
        return FALSE;

    for (unsigned int i = 0; i < blockNum; i++)
    {
        memcpy(pMaxLenBuffer, pBuffer + i * maxDataLen, maxDataLen);
        memset(pbRes, '\0', blockSize);

        if (FAILED(BCryptEncrypt(
            hKey, pMaxLenBuffer, maxDataLen, &pi, NULL, 0, pbRes, blockSize, &blockSize, BCRYPT_PAD_OAEP)))
        {
            CryptCleanUp(buffers, numBuffers, hCryptProv, hKey, hGlobal);
            return FALSE;
        }

        memcpy(pResultBuffer + i * blockSize, pbRes, blockSize);
    }

    DWORD b64Length = 0;
    if (!CryptBinaryToString(pResultBuffer, resLen, CRYPT_STRING_BASE64, NULL, &b64Length))
    {
        CryptCleanUp(buffers, numBuffers, hCryptProv, hKey, hGlobal);
        return FALSE;
    }

    wchar_t* result = new wchar_t[b64Length + 1];
    if (!CryptBinaryToString(pResultBuffer, resLen, CRYPT_STRING_BASE64, result, &b64Length))
    {
        CryptCleanUp(buffers, numBuffers, hCryptProv, hKey, hGlobal);
        return FALSE;
    }
    result[b64Length] = L'\0';
    _tcscpy_s(b64Result, _tcslen(result) + 1, result);

    delete[] result;

    CryptCleanUp(buffers, numBuffers, hCryptProv, hKey, hGlobal);

    return TRUE;
}