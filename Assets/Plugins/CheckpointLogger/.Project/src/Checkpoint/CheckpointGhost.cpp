#include "CheckpointGhost.h"
#include "CheckpointTimeLogger.h"

#include <io.h>

#include <chrono>
#include <fstream>
#include <memory>

#include <WinSock2.h>
#pragma comment(lib, "Ws2_32.lib")

static std::shared_ptr<Checkpoint[]>   g_ghostCheckpoints = nullptr;
static std::shared_ptr<std::wstring[]> g_ghostCheckpointNames;

static float g_ghostTotalRunningTime;
static int   g_ghostNumCheckpoints;

const size_t BUFFER_SZ = 1024;

template<size_t BUF_SZ, class T>
static void WriteToBuffer(char (&a_buf)[BUF_SZ], size_t& a_writeIndex, const T& a_value);

void UNITY_INTERFACE_API
CheckpointLogger_GhostLoadFromPath(
	const wchar_t* a_path
) {

	//(void) a_path;
	using std::ios;
	std::ifstream fin;
	fin.open(a_path, ios::in | ios::binary);

	// Load the data into a binary buffer
	char buf[BUFFER_SZ]{ 0 };

	size_t readIndex = 0;

	if (!fin) {
		return;
	}
	
	fin.read(buf, BUFFER_SZ);

	// Read time
	{
		// TODO read the epoch
		readIndex += sizeof(long long);
	}
	// Read time and checkpoint attributes
	{
		// Extract the number of checkpoints
		g_ghostNumCheckpoints = static_cast<int>(*reinterpret_cast<char*>(buf + readIndex));
		readIndex += sizeof(char);

		g_ghostCheckpoints     = std::shared_ptr<Checkpoint[]>(new Checkpoint[g_ghostNumCheckpoints]);
		g_ghostCheckpointNames = std::shared_ptr<std::wstring[]>(new std::wstring[g_ghostNumCheckpoints]);

		for (int i = 0; i < g_ghostNumCheckpoints; i++) {
			// Extract the RTBC
			short shortTimeStamp = *reinterpret_cast<short*>(buf + readIndex);
			readIndex += sizeof(short);

			float fullTimeStamp = static_cast<float>(ntohs(shortTimeStamp)) / 100.f;
			g_ghostTotalRunningTime += fullTimeStamp;
			g_ghostCheckpoints[i].timeStamp = fullTimeStamp;

			// Extract length of the name
			short strLength = *reinterpret_cast<short*>(buf + readIndex);
			strLength = htons(strLength);
			readIndex += sizeof(short);

			// Extract the name
			wchar_t strBuf[BUFFER_SZ]{ L'\0' };
			memcpy_s(strBuf, BUFFER_SZ, reinterpret_cast<wchar_t*>(buf + readIndex), strLength * 2);
			readIndex += strLength * sizeof(wchar_t);

			g_ghostCheckpointNames[i] = strBuf;
			g_ghostCheckpoints[i].name = g_ghostCheckpointNames[i].c_str();
		}
	}
}

void UNITY_INTERFACE_API
CheckpointLogger_GhostSaveToPath(
	const wchar_t* a_path
) {
	// Load the data into a binary buffer
	char buf[BUFFER_SZ]{ 0 };

	size_t writeIndex = 0;

	// Write time to file
	{
		namespace ch = std::chrono;
		const auto timePoint     = ch::system_clock::now();
		long long timeSinceEpoch = ch::duration_cast<ch::seconds>(timePoint.time_since_epoch()).count();

		// Convert to big endian to ensure consistent values across different systems
		WriteToBuffer(buf, writeIndex, htonll(timeSinceEpoch));
	}
	// Write time and checkpoint attributes
	{
		WriteToBuffer(buf, writeIndex, static_cast<char>(g_numCheckpoints));

		for (int i = 0; i < g_numCheckpoints; i++) {
			short shortTimeStamp = static_cast<short>(g_checkpoints[i].timeStamp * 100.0f);
			WriteToBuffer(buf, writeIndex, htons(shortTimeStamp));
			
			const std::wstring& cpName = g_checkpointNames[i];

			short shortNameLength = static_cast<short>(cpName.length());
			WriteToBuffer(buf, writeIndex, htons(shortNameLength));

			wcscpy_s(reinterpret_cast<wchar_t*>(buf + writeIndex), BUFFER_SZ / 2 - writeIndex, cpName.c_str());
			writeIndex += cpName.length() * sizeof(wchar_t);
		}
	}

	std::wstring path = a_path;

	auto str = path.substr(0, path.find_last_of('/'));
	
	_wmkdir(str.c_str());
	
	//(void) a_path;
	using std::ios;
	std::ofstream fout;
	fout.open(path, ios::out | ios::binary);
	
	if (fout) {
		fout.write(buf, writeIndex);
	}
}

float UNITY_INTERFACE_API
CheckpointLogger_GhostGetTotalTime() {
	return g_ghostTotalRunningTime;
}

Checkpoint UNITY_INTERFACE_API
CheckpointLogger_GhostGetCheckpoint(
	int a_index
) {

	if (a_index < 0 || a_index >= g_ghostNumCheckpoints)
		return {};

	return g_ghostCheckpoints[a_index];
}

int UNITY_INTERFACE_API
CheckpointLogger_GhostGetNumCheckpoints() {
	return g_ghostNumCheckpoints;
}

// Helper function, copies the value of a_value into the byte buffer a_buf at write index a_writeIndex and
// adjusts the index
template<size_t BUF_SZ, class T>
static void WriteToBuffer(char (&a_buf)[BUF_SZ], size_t& a_writeIndex, const T& a_value) {
	size_t valuesz = sizeof(T);
	memcpy_s(a_buf + a_writeIndex, BUF_SZ - a_writeIndex, &a_value, valuesz);
	a_writeIndex += valuesz;
}
