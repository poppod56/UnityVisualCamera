using System.Net.WebSockets;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Vector3 positionOffset;  // Offset to apply to the position
    public Vector3 rotationOffset;  // Offset to apply to the rotation (in Euler angles)

    private Vector3 targetPosition;
    private Quaternion targetRotation;

    private void LateUpdate()
    {



        // Vector3 adjustedPosition = targetPosition + positionOffset;
        // Quaternion adjustedRotation = targetRotation * Quaternion.Euler(rotationOffset);
        Vector3 adjustedPosition = Vector3.Lerp(transform.position, targetPosition + positionOffset, Time.deltaTime * 10f);
        Quaternion adjustedRotation = Quaternion.Slerp(transform.rotation, targetRotation * Quaternion.Euler(rotationOffset), Time.deltaTime * 10f);

        transform.position = adjustedPosition;
        transform.rotation = adjustedRotation;

    }

    // Set the new target transform data
    public void SetTargetTransform(Vector3 position, Quaternion rotation)
    {
        targetPosition = position;
        targetRotation = rotation;
    }

}
