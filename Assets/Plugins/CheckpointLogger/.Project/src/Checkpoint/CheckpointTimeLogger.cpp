#include "CheckpointTimeLogger.h"

#include <vector>
#include <memory>

static std::shared_ptr<Checkpoint[]> g_checkpoints;

static int g_currentIndex   = 0;
static int g_numCheckpoints = 0;

static float g_totalRunningTime = 0.0f;

void UNITY_INTERFACE_API 
CheckpointLogger_SetCheckpoints(
	Checkpoint a_checkpoints[],
	int        a_length
) {
	if (a_length <= 0)
		return;
	
	g_checkpoints = std::shared_ptr<Checkpoint[]>(new Checkpoint[a_length]);

	memcpy(g_checkpoints.get(), a_checkpoints, a_length * sizeof(Checkpoint));

	g_numCheckpoints = a_length;
	
	g_currentIndex = 0;
	g_totalRunningTime = 0;
}

void UNITY_INTERFACE_API
CheckpointLogger_StartRun() {
	if (g_currentIndex != 0)
		CheckpointLogger_EndRun();
	
	g_currentIndex     = 0;
	g_totalRunningTime = 0.0f;
}

void UNITY_INTERFACE_API
CheckpointLogger_EndRun() {
	// TODO: Implement saving
}

void UNITY_INTERFACE_API
CheckpointLogger_SaveCheckpointTime(
	int   a_index,
	float a_checkpointTime
) {
	if (a_index < 0 || a_index >= g_numCheckpoints)
		return;

	g_checkpoints[a_index].timeStamp = a_checkpointTime;
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
