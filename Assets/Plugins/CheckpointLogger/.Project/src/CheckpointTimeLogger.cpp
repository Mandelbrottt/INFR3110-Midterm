#include "CheckpointTimeLogger.h"

#include <vector>
#include <memory>

static std::vector<float> g_checkpointTimes;

static float g_totalRunningTime = 0.0f;

void UNITY_INTERFACE_API
CheckpointLogger_StartRun() {
	if (!g_checkpointTimes.empty())
		CheckpointLogger_EndRun();

	g_totalRunningTime = 0.0f;
}

void UNITY_INTERFACE_API
CheckpointLogger_EndRun() {
	g_checkpointTimes.clear();
	g_totalRunningTime = 0.0f;
}

void UNITY_INTERFACE_API
CheckpointLogger_SaveCheckpointTime(
	float a_checkpointTime
) {
	g_checkpointTimes.push_back(a_checkpointTime);
	g_totalRunningTime += a_checkpointTime;
}

float UNITY_INTERFACE_API
CheckpointLogger_GetTotalTime() {
	return g_totalRunningTime;
}

float UNITY_INTERFACE_API
CheckpointLogger_GetCheckpointTime(
	int a_index
) {
	if (a_index < 0 || a_index >= g_checkpointTimes.size())
		return -1.0f;

	return g_checkpointTimes.at(a_index);
}

int UNITY_INTERFACE_API
CheckpointLogger_GetNumCheckpoints() {
	return (int) g_checkpointTimes.size();
}
