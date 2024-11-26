using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualLinkController : MonoBehaviour
{
    private Transform startPoint;
    private Transform endPoint;
    private LineRenderer lineRenderer;

    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    public void SetUpPoints(Transform startPoint, Transform endPoint)
    {
        this.startPoint = startPoint;
        this.endPoint = endPoint;
    }

    private void Update()
    {
        if (startPoint != null && endPoint != null)
        {
            lineRenderer.SetPosition(0, startPoint.position);
            lineRenderer.SetPosition(1, endPoint.position);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
}
