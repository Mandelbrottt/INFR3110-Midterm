using System;
using System.Runtime.InteropServices;
using UnityEditor;
using UnityEngine;

namespace INFR3110 {

	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	public struct CheckpointStruct {
		public float  timeStamp;
		public string name;
	}
	
	public class CheckpointLogger : MonoBehaviourSingleton<CheckpointLogger> {

		// Set in the inspector, drag the dll into the Library Object 
		public UnityEngine.Object libraryObject;

		private const string SET_CHECKPOINTS_SYMBOL      = "CheckpointLogger_SetCheckpoints";
		private const string RESET_RUN_SYMBOL            = "CheckpointLogger_ResetRun";
		private const string SAVE_CHECKPOINT_TIME_SYMBOL = "CheckpointLogger_SaveCheckpointTime";
		private const string GET_TOTAL_TIME_SYMBOL       = "CheckpointLogger_GetTotalTime";
		private const string GET_CHECKPOINT_SYMBOL       = "CheckpointLogger_GetCheckpoint";
		private const string GET_NUM_CHECKPOINTS_SYMBOL  = "CheckpointLogger_GetNumCheckpoints";

		private const string GHOST_LOAD_FROM_PATH_SYMBOL      = "CheckpointLogger_GhostLoadFromPath";
		private const string GHOST_SAVE_TO_PATH_SYMBOL        = "CheckpointLogger_GhostSaveToPath";
		private const string GHOST_GET_TOTAL_TIME_SYMBOL      = "CheckpointLogger_GhostGetTotalTime";
		private const string GHOST_GET_CHECKPOINT_SYMBOL      = "CheckpointLogger_GhostGetCheckpoint";
		private const string GHOST_GET_NUM_CHECKPOINTS_SYMBOL = "CheckpointLogger_GhostGetNumCheckpoints";

		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		public delegate void SetCheckpointsDelegate(CheckpointStruct[] a_checkpoints, int a_length);

		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		public delegate void ResetRunDelegate();

		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		public delegate void SaveCheckpointTimeDelegate(float a_checkpointTime);

		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		public delegate float GetTotalTimeDelegate();

		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		public delegate CheckpointStruct GetCheckpointDelegate(int a_index);

		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		public delegate int GetNumCheckpointsDelegate();

		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		public delegate void GhostLoadFromFileDelegate([MarshalAs(UnmanagedType.LPWStr)] string a_path);
		
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		public delegate void GhostSaveToFileDelegate([MarshalAs(UnmanagedType.LPWStr)] string a_path);
		
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		public delegate float GhostGetTotalTimeDelegate();

		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		public delegate CheckpointStruct GhostGetCheckpointDelegate(int a_index);

		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		public delegate int GhostGetNumCheckpointsDelegate();

		// ReSharper disable InconsistentNaming
		public SetCheckpointsDelegate     SetCheckpoints;
		public ResetRunDelegate           ResetRun;
		public SaveCheckpointTimeDelegate SaveCheckpointTime;
		public GetTotalTimeDelegate       GetTotalTime;
		public GetCheckpointDelegate      GetCheckpoint;
		public GetNumCheckpointsDelegate  GetNumCheckpoints;

		public GhostLoadFromFileDelegate      GhostLoadFromFile;
		public GhostSaveToFileDelegate        GhostSaveToFile;
		public GhostGetTotalTimeDelegate      GhostGetTotalTime;
		public GhostGetCheckpointDelegate     GhostGetCheckpoint;
		public GhostGetNumCheckpointsDelegate GhostGetNumCheckpoints;
		// ReSharper restore InconsistentNaming

		private DynamicLibrary m_dynamicLibrary;

		private void Awake() {
			// Dynamically load the DLL
			var path = DynamicLibrary.PathFromLibraryObject(libraryObject);
			m_dynamicLibrary = new DynamicLibrary(path);

			SetCheckpoints     = m_dynamicLibrary.GetDelegate<SetCheckpointsDelegate>    (SET_CHECKPOINTS_SYMBOL);
			ResetRun           = m_dynamicLibrary.GetDelegate<ResetRunDelegate>          (RESET_RUN_SYMBOL);
			SaveCheckpointTime = m_dynamicLibrary.GetDelegate<SaveCheckpointTimeDelegate>(SAVE_CHECKPOINT_TIME_SYMBOL);
			GetTotalTime       = m_dynamicLibrary.GetDelegate<GetTotalTimeDelegate>      (GET_TOTAL_TIME_SYMBOL);
			GetCheckpoint      = m_dynamicLibrary.GetDelegate<GetCheckpointDelegate>     (GET_CHECKPOINT_SYMBOL);
			GetNumCheckpoints  = m_dynamicLibrary.GetDelegate<GetNumCheckpointsDelegate> (GET_NUM_CHECKPOINTS_SYMBOL);

			GhostLoadFromFile  = m_dynamicLibrary.GetDelegate<GhostLoadFromFileDelegate> (GHOST_LOAD_FROM_PATH_SYMBOL);
			GhostSaveToFile    = m_dynamicLibrary.GetDelegate<GhostSaveToFileDelegate>   (GHOST_SAVE_TO_PATH_SYMBOL);
			GhostGetTotalTime  = m_dynamicLibrary.GetDelegate<GhostGetTotalTimeDelegate> (GHOST_GET_TOTAL_TIME_SYMBOL);
			GhostGetCheckpoint = m_dynamicLibrary.GetDelegate<GhostGetCheckpointDelegate>(GHOST_GET_CHECKPOINT_SYMBOL);
			GhostGetNumCheckpoints =
				m_dynamicLibrary.GetDelegate<GhostGetNumCheckpointsDelegate>             (GHOST_GET_NUM_CHECKPOINTS_SYMBOL);

			transform.parent = null;
			
			DontDestroyOnLoad(this);
		}

		private void OnDestroy() {
			m_dynamicLibrary?.Dispose();
		}
	}
}
