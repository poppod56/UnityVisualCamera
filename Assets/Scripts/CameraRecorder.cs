using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;
#if UNITY_EDITOR
public class CameraRecorder : MonoBehaviour
{
    public Transform cameraTransform;  // The camera you want to record
    public string animationClipName = "CameraAnimation";  // Name for the animation clip

    private List<KeyframeData> recordedData = new List<KeyframeData>();
    private bool isRecording = false;
    
    void Update()
    {
        if (isRecording)
        {
            // Record position and rotation every frame
            recordedData.Add(new KeyframeData
            {
                time = Time.time,
                position = cameraTransform.position,
                rotation = cameraTransform.rotation
            });
        }
    }

     [ContextMenu("Start Recording")]
    public void StartRecording()
    {
        isRecording = true;
        recordedData.Clear();
        Debug.Log("Recording Started");
    }

    [ContextMenu("Stop Recording")]
    public void StopRecording()
    {
        isRecording = false;
        SaveAnimationClip();
        Debug.Log("Recording Stopped and Animation Saved");
    }

    private void SaveAnimationClip()
    {
        AnimationClip clip = new AnimationClip();

        // Create position and rotation curves
        AnimationCurve posX = new AnimationCurve();
        AnimationCurve posY = new AnimationCurve();
        AnimationCurve posZ = new AnimationCurve();

        AnimationCurve rotX = new AnimationCurve();
        AnimationCurve rotY = new AnimationCurve();
        AnimationCurve rotZ = new AnimationCurve();
        AnimationCurve rotW = new AnimationCurve();

        foreach (var keyframe in recordedData)
        {
            posX.AddKey(keyframe.time, keyframe.position.x);
            posY.AddKey(keyframe.time, keyframe.position.y);
            posZ.AddKey(keyframe.time, keyframe.position.z);

            rotX.AddKey(keyframe.time, keyframe.rotation.x);
            rotY.AddKey(keyframe.time, keyframe.rotation.y);
            rotZ.AddKey(keyframe.time, keyframe.rotation.z);
            rotW.AddKey(keyframe.time, keyframe.rotation.w);
        }

        // Add curves to the animation clip
        clip.SetCurve("", typeof(Transform), "localPosition.x", posX);
        clip.SetCurve("", typeof(Transform), "localPosition.y", posY);
        clip.SetCurve("", typeof(Transform), "localPosition.z", posZ);

        clip.SetCurve("", typeof(Transform), "localRotation.x", rotX);
        clip.SetCurve("", typeof(Transform), "localRotation.y", rotY);
        clip.SetCurve("", typeof(Transform), "localRotation.z", rotZ);
        clip.SetCurve("", typeof(Transform), "localRotation.w", rotW);

         // Generate a timestamp and append it to the animation clip name
        string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");  // Format: YearMonthDay_HourMinuteSecond
        string uniqueAnimationClipName = $"{animationClipName}_{timestamp}.anim";

        // Save the animation clip with the unique name
        AssetDatabase.CreateAsset(clip, $"Assets/{uniqueAnimationClipName}");
        AssetDatabase.SaveAssets();

        Debug.Log($"Animation clip '{animationClipName}' saved successfully.");
    }
}

// Struct to store keyframe data
public struct KeyframeData
{
    public float time;
    public Vector3 position;
    public Quaternion rotation;
}

#endif