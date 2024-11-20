using Cinemachine;
using UnityEngine;

public class CameraConfineSetup : MonoBehaviour
{

    private string cameraConfineTag = "CameraConfine";

    private void Start()
    {
        GameObject cameraConfine = GameObject.FindGameObjectWithTag(cameraConfineTag);

        if (cameraConfine == null)
        {
            Debug.LogWarning($"No objects with tag '{cameraConfineTag}' were found in the scene.");
            return;
        }

        PolygonCollider2D polygonCollider2D = cameraConfine.GetComponent<PolygonCollider2D>();

        if (polygonCollider2D == null)
        {
            Debug.LogWarning($"The object '{cameraConfine.name}' does not have a PolygonCollider2D.");
            return;
        }

        CinemachineConfiner cinemachineConfiner = GetComponent<CinemachineConfiner>();

        if (cinemachineConfiner == null)
        {
            Debug.LogWarning($"A CinemachineConfiner was not found on the object '{gameObject.name}'.");
            return;
        }

        cinemachineConfiner.m_BoundingShape2D = polygonCollider2D;
    }
}