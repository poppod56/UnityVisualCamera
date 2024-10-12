using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;

#if UNITY_EDITOR


    public class CameraRecorder : MonoBehaviour
    {
        [Header("Camera to Record")]
        public Transform cameraTransform;  // The camera you want to record

        [Header("Recording Settings")]
        public string animationClipName = "CameraAnimation";  // Base name for the animation clip

        private List<KeyframeData> recordedData = new List<KeyframeData>(); // Store recorded keyframes
        private bool isRecording = false;  // Recording state
        private float recordingStartTime;  // Timestamp when recording starts

        void Update()
        {
            if (isRecording)
            {
                RecordFrame();
            }
        }

        // Record camera position and rotation at the current frame
        private void RecordFrame()
        {
            float elapsedTime = Time.time - recordingStartTime;
            recordedData.Add(new KeyframeData(elapsedTime, cameraTransform.position, cameraTransform.rotation));
        }

        [ContextMenu("Start Recording")]
        public void StartRecording()
        {
            isRecording = true;
            recordedData.Clear();
            recordingStartTime = Time.time;  // Capture the current time as the start time
            Debug.Log("Recording started.");
        }

        [ContextMenu("Stop Recording")]
        public void StopRecording()
        {
            isRecording = false;
            AdjustKeyframeTimesToStartAtZero();  // Ensure animation starts at time 0
            SaveAnimationClip();
            Debug.Log("Recording stopped and animation saved.");
        }

        // Adjust keyframe times so the animation starts at time 0
        private void AdjustKeyframeTimesToStartAtZero()
        {
            if (recordedData.Count == 0) return;

            float firstFrameTime = recordedData[0].time;  // Capture the time of the first frame

            // Adjust keyframe times by subtracting the first frame's time from each keyframe
            for (int i = 0; i < recordedData.Count; i++)
            {
                // Retrieve, modify, and then reassign the modified struct to avoid the CS1612 error
                KeyframeData modifiedKeyframe = recordedData[i];
                modifiedKeyframe.time -= firstFrameTime;
                recordedData[i] = modifiedKeyframe;
            }
        }

        // Save the recorded animation as an AnimationClip asset
        private void SaveAnimationClip()
        {
            AnimationClip clip = new AnimationClip();
            CreatePositionCurves(clip);
            CreateRotationCurves(clip);

            string uniqueClipName = GenerateUniqueClipName();
            AssetDatabase.CreateAsset(clip, $"Assets/{uniqueClipName}");
            AssetDatabase.SaveAssets();
            Debug.Log($"Animation clip '{uniqueClipName}' saved successfully.");
        }

        // Generate a unique clip name using a timestamp
        private string GenerateUniqueClipName()
        {
            string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");  // Format: YearMonthDay_HourMinuteSecond
            return $"{animationClipName}_{timestamp}.anim";
        }

        // Create position animation curves from recorded data
        private void CreatePositionCurves(AnimationClip clip)
        {
            AnimationCurve posX = new AnimationCurve();
            AnimationCurve posY = new AnimationCurve();
            AnimationCurve posZ = new AnimationCurve();

            foreach (var keyframe in recordedData)
            {
                posX.AddKey(keyframe.time, keyframe.position.x);
                posY.AddKey(keyframe.time, keyframe.position.y);
                posZ.AddKey(keyframe.time, keyframe.position.z);
            }

            clip.SetCurve("", typeof(Transform), "localPosition.x", posX);
            clip.SetCurve("", typeof(Transform), "localPosition.y", posY);
            clip.SetCurve("", typeof(Transform), "localPosition.z", posZ);
        }

        // Create rotation animation curves from recorded data
        private void CreateRotationCurves(AnimationClip clip)
        {
            AnimationCurve rotX = new AnimationCurve();
            AnimationCurve rotY = new AnimationCurve();
            AnimationCurve rotZ = new AnimationCurve();
            AnimationCurve rotW = new AnimationCurve();

            foreach (var keyframe in recordedData)
            {
                rotX.AddKey(keyframe.time, keyframe.rotation.x);
                rotY.AddKey(keyframe.time, keyframe.rotation.y);
                rotZ.AddKey(keyframe.time, keyframe.rotation.z);
                rotW.AddKey(keyframe.time, keyframe.rotation.w);
            }

            clip.SetCurve("", typeof(Transform), "localRotation.x", rotX);
            clip.SetCurve("", typeof(Transform), "localRotation.y", rotY);
            clip.SetCurve("", typeof(Transform), "localRotation.z", rotZ);
            clip.SetCurve("", typeof(Transform), "localRotation.w", rotW);
        }
    }

    // Struct to store keyframe data
    [Serializable]
    public struct KeyframeData
    {
        public float time;
        public Vector3 position;
        public Quaternion rotation;

        public KeyframeData(float time, Vector3 position, Quaternion rotation)
        {
            this.time = time;
            this.position = position;
            this.rotation = rotation;
        }
    }



#endif
