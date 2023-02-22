using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;

public class ZoomSmoothCameraSystem : MonoBehaviour
{
    [SerializeField, Range(0f, 100f)] float Zoom;
    [SerializeField] float ZoomSpeed = 0.5f;

    //ZoomSmooth
    private float ZDuration = 0;
    private float ZoffSet = 1;
    private float timeZoom = 0;
    public bool IsZoomCamera = false;

    private Vector3 start;
    private Vector3 end;
    private Transform endTransform;
    CinemachineVirtualCamera CVcam;

    // Start is called before the first frame update
    void Start()
    {
        CVcam = GetComponent<CinemachineVirtualCamera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (IsZoomCamera)
        {
            ZoomSmoothCamera(ZoomParcent(), ZoomSpeed);
        }

    }

    private float ZoomParcent()
    {
        float zoomBChar = CVcam.m_Lens.FieldOfView - 15;
        float zoomnum = (Zoom / 100) * zoomBChar;
        return CVcam.m_Lens.FieldOfView - zoomnum;
    }

    private void ZoomSmoothCamera(float Zoom, float Zspeed = 1)
    {
        ZDuration += (ZoffSet * Time.deltaTime) * Zspeed;
        ZoffSet -= (1 - ZDuration) * Time.deltaTime;

        if (ZDuration > 1)
        {
            ZDuration = 1;
            IsZoomCamera = false;
        }

        if (ZDuration < 1)
        {
            CVcam.m_Lens.FieldOfView = Mathf.Lerp(CVcam.m_Lens.FieldOfView, Zoom, math.smoothstep(0, 1, ZDuration));
        }
    }
}
