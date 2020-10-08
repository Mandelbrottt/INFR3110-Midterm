#pragma once

#include "IUnityInterface.h"

#include "Checkpoint.h"

#ifdef __cplusplus
extern "C" {
#endif

	extern void UNITY_INTERFACE_EXPORT UNITY_INTERFACE_API
	CheckpointLogger_SetCheckpoints(
		Checkpoint a_checkpoints[],
		int        a_length
	);

	extern void UNITY_INTERFACE_EXPORT UNITY_INTERFACE_API
	CheckpointLogger_StartRun();

	extern void UNITY_INTERFACE_EXPORT UNITY_INTERFACE_API
	CheckpointLogger_EndRun();

	extern void UNITY_INTERFACE_EXPORT UNITY_INTERFACE_API
	CheckpointLogger_SaveCheckpointTime(
		int   a_index,
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
