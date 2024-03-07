using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.Rendering.PostProcessing;

public class FocusCamera : MonoBehaviour
{
    private PostProcessVolume _postProcessVolume;

    [SerializeField] private GameObject FocusPoint;

    [SerializeField] private float focusOffset = 0;
    // Start is called before the first frame update
    void Start()
    {
        _postProcessVolume = gameObject.GetComponent<PostProcessVolume>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 start = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, FocusPoint.transform.position.z);
        _postProcessVolume.profile.GetSetting<DepthOfField>().focusDistance.value = Vector3.Distance(start, gameObject.transform.position) + focusOffset;
    }
}
