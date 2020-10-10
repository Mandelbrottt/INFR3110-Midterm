#pragma once

#include "IUnityInterface.h"

#include "Checkpoint.h"

#include <memory>
#include <string>

extern std::shared_ptr<Checkpoint[]> g_checkpoints;
extern std::shared_ptr<std::wstring[]> g_checkpointNames;

extern int g_currentIndex;
extern int g_numCheckpoints;

extern float g_totalRunningTime;

#ifdef __cplusplus
extern "C" {
#endif

	extern void UNITY_INTERFACE_EXPORT UNITY_INTERFACE_API
	CheckpointLogger_SetCheckpoints(
		Checkpoint a_checkpoints[],
		int        a_length
	);

	extern void UNITY_INTERFACE_EXPORT UNITY_INTERFACE_API
	CheckpointLogger_ResetRun();

	extern void UNITY_INTERFACE_EXPORT UNITY_INTERFACE_API
	CheckpointLogger_SaveCheckpointTime(
		float a_checkpointTime
	);

	extern float UNITY_INTERFACE_EXPORT UNITY_INTERFACE_API
	CheckpointLogger_GetTotalTime();

	extern Checkpoint UNITY_INTERFACE_EXPORT UNITY_INTERFACE_API
	CheckpointLogger_GetCheckpoint(
		int a_index
	);

	extern int UNITY_INTERFACE_EXPORT UNITY_INTERFACE_API
	CheckpointLogger_GetNumCheckpoints();

#ifdef __cplusplus
}
#endif
