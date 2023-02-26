using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class SplashScreen : MonoBehaviour
{
    private VideoPlayer mVideoPlayer;

    [SerializeField] private string Intro_Scene;
    [SerializeField] private GameObject LoadingScreenWidget;

    bool Is_Open = false;
    // Start is called before the first frame update
    void Start()
    {
        mVideoPlayer = gameObject.GetComponent<VideoPlayer>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (mVideoPlayer.isPaused == true && !Is_Open)
        {
            LoadingScreenWidget.GetComponent<LoadingSceneStstem>().LoadScene(Intro_Scene);
            Is_Open = true;
        }
    }
}
