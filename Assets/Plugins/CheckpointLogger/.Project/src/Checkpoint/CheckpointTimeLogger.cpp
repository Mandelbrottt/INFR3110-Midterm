#include "CheckpointTimeLogger.h"

#include <vector>
#include <memory>
#include <string>

std::shared_ptr<Checkpoint[]>   g_checkpoints     = nullptr;
std::shared_ptr<std::wstring[]> g_checkpointNames = nullptr;

int g_currentIndex   = 0;
int g_numCheckpoints = 0;

float g_totalRunningTime = 0.0f;

void UNITY_INTERFACE_API
CheckpointLogger_SetCheckpoints(
	Checkpoint a_checkpoints[],
	int        a_length
) {

	g_numCheckpoints = a_length;

	g_currentIndex     = 0;
	g_totalRunningTime = 0;

	if (a_length <= 0)
		return;

	g_checkpoints = std::shared_ptr<Checkpoint[]>(new Checkpoint[a_length]);

	g_checkpointNames = std::shared_ptr<std::wstring[]>(new std::wstring[a_length]);

	for (int i = 0; i < a_length; i++) {
		g_checkpoints[i].timeStamp = a_checkpoints[i].timeStamp;

		g_checkpointNames[i] = a_checkpoints[i].name;

		g_checkpoints[i].name = g_checkpointNames[i].c_str();
	}

}

void UNITY_INTERFACE_API
CheckpointLogger_ResetRun() {
	g_currentIndex     = 0;
	g_totalRunningTime = 0.0f;
}

void UNITY_INTERFACE_API
CheckpointLogger_SaveCheckpointTime(
	float a_checkpointTime
) {
	if (g_currentIndex < 0 || g_currentIndex >= g_numCheckpoints)
		return;

	g_checkpoints[g_currentIndex].timeStamp = a_checkpointTime;
	g_currentIndex++;
	g_totalRunningTime += a_checkpointTime;
}

float UNITY_INTERFACE_API
CheckpointLogger_GetTotalTime() {
	return g_totalRunningTime;
}

Checkpoint UNITY_INTERFACE_API
CheckpointLogger_GetCheckpoint(
	int a_index
) {
	if (a_index < 0 || a_index >= g_numCheckpoints)
		return {};

	return g_checkpoints[a_index];
}

int UNITY_INTERFACE_API
CheckpointLogger_GetNumCheckpoints() {
	return g_numCheckpoints;
}
