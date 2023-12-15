using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.Rendering.PostProcessing;

public class FocusCamera : MonoBehaviour
{
    private PostProcessVolume _postProcessVolume;

    [SerializeField] private GameObject FocusPoint;
    [Range(-10, 10)]
    [SerializeField] private float m_focus_offset = 0.5f;
    // Start is called before the first frame update
    void Start()
    {
        _postProcessVolume = gameObject.GetComponent<PostProcessVolume>();
    }

    // Update is called once per frame
    void Update()
    {
        _postProcessVolume.profile.GetSetting<DepthOfField>().focusDistance.value = Vector3.Distance(FocusPoint.transform.position, gameObject.transform.position) + m_focus_offset;
    }
}
