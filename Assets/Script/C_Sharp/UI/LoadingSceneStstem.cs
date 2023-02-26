using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingSceneStstem : MonoBehaviour
{
    [SerializeField] private GameObject LoadingScreen;

    [SerializeField] private Image LoadingBarfill;

    private float progressValue;
    
    public void LoadScene(string SceneID)
    {
        StartCoroutine(LoadSceneAsync(SceneID));
    }

    IEnumerator LoadSceneAsync(string SceneID)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(SceneID);
        operation.allowSceneActivation = false;

        LoadingScreen.SetActive(true);

        print("PRRR : " + operation);

        while (!operation.isDone)
        {
            progressValue = Mathf.Clamp01(operation.progress / 0.9f);

            LoadingBarfill.GetComponent<Image>().fillAmount = progressValue;
            print("Loading : " + operation.progress + " ----------------------------------------------");
            yield return null;
            operation.allowSceneActivation = true;
        }
    }
}
