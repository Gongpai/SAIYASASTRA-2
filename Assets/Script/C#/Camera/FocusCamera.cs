using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.Rendering.PostProcessing;

public class FocusCamera : MonoBehaviour
{
    private PostProcessVolume _postProcessVolume;

    [SerializeField] private GameObject FocusPoint;
    // Start is called before the first frame update
    void Start()
    {
        _postProcessVolume = gameObject.GetComponent<PostProcessVolume>();
    }

    // Update is called once per frame
    void Update()
    {
        _postProcessVolume.profile.GetSetting<DepthOfField>().focusDistance.value = Vector3.Distance(FocusPoint.transform.position, gameObject.transform.position) + 2.5f;
    }
}
