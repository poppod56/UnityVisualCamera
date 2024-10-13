using Poppod;
using UnityEditor;
using UnityEngine;



#if UNITY_EDITOR
[CustomEditor(typeof(CameraRecorder))]
    public class CameraRecorderEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            CameraRecorder recorder = (CameraRecorder)target;

            // Add a button to start recording
            if (GUILayout.Button("Start Recording")) {
                recorder.StartRecording();
            }

            // Add a button to stop recording
            if (GUILayout.Button("Stop Recording")) {
                recorder.StopRecording();
            }
        }
    }

#endif
