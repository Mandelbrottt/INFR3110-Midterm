using System.Runtime.InteropServices;
using UnityEditor;
using UnityEngine;

namespace INFR3110 {
	public class CheckpointLogger : MonoBehaviour {

		public static CheckpointLogger Instance { get; private set; }

		// Set in the inspector, drag the dll into the Library Object 
		public Object libraryObject;

		private const string START_RUN_SYMBOL            = "CheckpointLogger_StartRun";
		private const string END_RUN_SYMBOL              = "CheckpointLogger_EndRun";
		private const string SAVE_CHECKPOINT_TIME_SYMBOL = "CheckpointLogger_SaveCheckpointTime";
		private const string GET_TOTAL_TIME_SYMBOL       = "CheckpointLogger_GetTotalTime";
		private const string GET_CHECKPOINT_TIME_SYMBOL  = "CheckpointLogger_GetCheckpointTime";
		private const string GET_NUM_CHECKPOINTS_SYMBOL  = "CheckpointLogger_GetNumCheckpoints";

		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		public delegate void StartRunDelegate();

		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		public delegate void EndRunDelegate();

		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		public delegate void SaveCheckpointTimeDelegate(float a_checkpointTime);

		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		public delegate float GetTotalTimeDelegate();

		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		public delegate float GetCheckpointTimeDelegate(int a_index);

		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		public delegate int GetNumCheckpointsDelegate();

		// ReSharper disable InconsistentNaming
		public StartRunDelegate           StartRun;
		public EndRunDelegate             EndRun;
		public SaveCheckpointTimeDelegate SaveCheckpointTime;
		public GetTotalTimeDelegate       GetTotalTime;
		public GetCheckpointTimeDelegate  GetCheckpointTime;
		public GetNumCheckpointsDelegate  GetNumCheckpoints;
		// ReSharper restore InconsistentNaming

		private DynamicLibrary m_dynamicLibrary;

		private void Awake() {
			Debug.Assert(Instance == null, "There should only be one CheckpointLogger!");
			Instance = this;

			// Dynamically load the DLL
			var path = DynamicLibrary.PathFromLibraryObject(libraryObject);
			m_dynamicLibrary = new DynamicLibrary(path);

			StartRun           = m_dynamicLibrary.GetDelegate<StartRunDelegate>          (START_RUN_SYMBOL);
			EndRun             = m_dynamicLibrary.GetDelegate<EndRunDelegate>            (END_RUN_SYMBOL);
			SaveCheckpointTime = m_dynamicLibrary.GetDelegate<SaveCheckpointTimeDelegate>(SAVE_CHECKPOINT_TIME_SYMBOL);
			GetTotalTime       = m_dynamicLibrary.GetDelegate<GetTotalTimeDelegate>      (GET_TOTAL_TIME_SYMBOL);
			GetCheckpointTime  = m_dynamicLibrary.GetDelegate<GetCheckpointTimeDelegate> (GET_CHECKPOINT_TIME_SYMBOL);
			GetNumCheckpoints  = m_dynamicLibrary.GetDelegate<GetNumCheckpointsDelegate> (GET_NUM_CHECKPOINTS_SYMBOL);
		}

		private void OnDestroy() {
			m_dynamicLibrary.Dispose();

			Instance = null;
		}
	}
}
