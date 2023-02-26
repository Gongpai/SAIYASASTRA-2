using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu_Ui : MonoBehaviour
{
    [SerializeField] private GameObject LoadingScreenWidget;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void Play()
    {
        LoadingScreenWidget.GetComponent<LoadingSceneStstem>().LoadScene("Game_Level");
    }

    public void Quit()
    {
        Application.Quit();
    }
}
