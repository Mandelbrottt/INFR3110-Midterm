#pragma once

#include <string>

#include "IUnityInterface.h"

#include "Checkpoint.h"

#ifdef __cplusplus
extern "C" {
#endif

	extern void UNITY_INTERFACE_EXPORT UNITY_INTERFACE_API
	CheckpointLogger_GhostLoadFromPath(
		const wchar_t* a_path
	);
	
	extern void UNITY_INTERFACE_EXPORT UNITY_INTERFACE_API
	CheckpointLogger_GhostSaveToPath(
		const wchar_t* a_path
	);

	extern float UNITY_INTERFACE_EXPORT UNITY_INTERFACE_API
	CheckpointLogger_GhostGetTotalTime();

	extern Checkpoint UNITY_INTERFACE_EXPORT UNITY_INTERFACE_API
	CheckpointLogger_GhostGetCheckpoint(
		int a_index
	);

	extern int UNITY_INTERFACE_EXPORT UNITY_INTERFACE_API
	CheckpointLogger_GhostGetNumCheckpoints();

#ifdef __cplusplus
}
#endif