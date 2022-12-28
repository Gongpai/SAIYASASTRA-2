using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingSceneStstem : MonoBehaviour
{
    [SerializeField] private GameObject LoadingScreen;

    [SerializeField] private Image LoadingBarfill;
    
    public void LoadScene(string SceneID)
    {
        StartCoroutine(LoadSceneAsync(SceneID));
    }

    IEnumerator LoadSceneAsync(string SceneID)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(SceneID);
        LoadingScreen.SetActive(true);

        print("PRRR : " + operation.priority);

        while (!operation.isDone)
        {
            float progrressValue = Mathf.Clamp01(operation.progress / 0.9f);

            LoadingBarfill.rectTransform.localScale = new Vector3((progrressValue - 1) * -1, 1, 1);
            print(LoadingBarfill.rectTransform.localScale);
            yield return null;
        }
    }
}
