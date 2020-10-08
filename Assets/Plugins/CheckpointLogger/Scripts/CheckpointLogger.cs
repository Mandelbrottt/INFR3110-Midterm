using System;
using System.Runtime.InteropServices;
using UnityEditor;
using UnityEngine;

namespace INFR3110 {

	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	public struct Checkpoint {
		public int    index;
		public float  timeStamp;
		public string name;
	}
	
	public class CheckpointLogger : MonoBehaviour {

		public static CheckpointLogger Instance { get; private set; }

		// Set in the inspector, drag the dll into the Library Object 
		public UnityEngine.Object libraryObject;

		private const string SET_CHECKPOINTS_SYMBOL      = "CheckpointLogger_SetCheckpoints";
		private const string START_RUN_SYMBOL            = "CheckpointLogger_StartRun";
		private const string END_RUN_SYMBOL              = "CheckpointLogger_EndRun";
		private const string SAVE_CHECKPOINT_TIME_SYMBOL = "CheckpointLogger_SaveCheckpointTime";
		private const string GET_TOTAL_TIME_SYMBOL       = "CheckpointLogger_GetTotalTime";
		private const string GET_CHECKPOINT_SYMBOL       = "CheckpointLogger_GetCheckpoint";
		private const string GET_NUM_CHECKPOINTS_SYMBOL  = "CheckpointLogger_GetNumCheckpoints";

		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		public delegate void SetCheckpointsDelegate(Checkpoint[] a_checkpoints, int a_length);

		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		public delegate void StartRunDelegate();

		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		public delegate void EndRunDelegate();

		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		public delegate void SaveCheckpointTimeDelegate(int a_index, float a_checkpointTime);

		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		public delegate float GetTotalTimeDelegate();

		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		public delegate Checkpoint GetCheckpointDelegate(int a_index);

		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		public delegate int GetNumCheckpointsDelegate();

		// ReSharper disable InconsistentNaming
		public SetCheckpointsDelegate     SetCheckpoints;
		public StartRunDelegate           StartRun;
		public EndRunDelegate             EndRun;
		public SaveCheckpointTimeDelegate SaveCheckpointTime;
		public GetTotalTimeDelegate       GetTotalTime;
		public GetCheckpointDelegate      GetCheckpoint;
		public GetNumCheckpointsDelegate  GetNumCheckpoints;
		// ReSharper restore InconsistentNaming

		private DynamicLibrary m_dynamicLibrary;

		private void Awake() {
			Debug.Assert(Instance == null, "There should only be one CheckpointLogger!");
			Instance = this;

			// Dynamically load the DLL
			var path = DynamicLibrary.PathFromLibraryObject(libraryObject);
			m_dynamicLibrary = new DynamicLibrary(path);

			SetCheckpoints     = m_dynamicLibrary.GetDelegate<SetCheckpointsDelegate>    (SET_CHECKPOINTS_SYMBOL);
			StartRun           = m_dynamicLibrary.GetDelegate<StartRunDelegate>          (START_RUN_SYMBOL);
			EndRun             = m_dynamicLibrary.GetDelegate<EndRunDelegate>            (END_RUN_SYMBOL);
			SaveCheckpointTime = m_dynamicLibrary.GetDelegate<SaveCheckpointTimeDelegate>(SAVE_CHECKPOINT_TIME_SYMBOL);
			GetTotalTime       = m_dynamicLibrary.GetDelegate<GetTotalTimeDelegate>      (GET_TOTAL_TIME_SYMBOL);
			GetCheckpoint      = m_dynamicLibrary.GetDelegate<GetCheckpointDelegate>     (GET_CHECKPOINT_SYMBOL);
			GetNumCheckpoints  = m_dynamicLibrary.GetDelegate<GetNumCheckpointsDelegate> (GET_NUM_CHECKPOINTS_SYMBOL);
		}

		private void OnDestroy() {
			m_dynamicLibrary?.Dispose();

			Instance = null;
		}
	}
}
