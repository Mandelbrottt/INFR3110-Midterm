#pragma once

#include "IUnityInterface.h"

#ifdef __cplusplus
extern "C" {
#endif

	extern void UNITY_INTERFACE_EXPORT UNITY_INTERFACE_API
	CheckpointLogger_StartRun();
	
	extern void UNITY_INTERFACE_EXPORT UNITY_INTERFACE_API
	CheckpointLogger_EndRun();
	
	extern void UNITY_INTERFACE_EXPORT UNITY_INTERFACE_API
	CheckpointLogger_SaveCheckpointTime(
		float a_checkpointTime
	);
	
	extern float UNITY_INTERFACE_EXPORT UNITY_INTERFACE_API
	CheckpointLogger_GetTotalTime();
	
	extern float UNITY_INTERFACE_EXPORT UNITY_INTERFACE_API
	CheckpointLogger_GetCheckpointTime(
		int a_index
	);
	
	extern int UNITY_INTERFACE_EXPORT UNITY_INTERFACE_API
	CheckpointLogger_GetNumCheckpoints();
	
#ifdef __cplusplus
}
#endif