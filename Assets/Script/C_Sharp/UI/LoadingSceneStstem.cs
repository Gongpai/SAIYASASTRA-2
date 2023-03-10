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
    string Scene_ID;
    private Animator animator;
    AsyncOperation operation;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void LoadScene(string SceneID)
    {
        LoadingScreen.SetActive(true);
        Scene_ID = SceneID;
        animator.SetBool("IsIn", true);
        animator.SetBool("IsOut", false);
    }

    public void EndLoadScene()
    {
        operation.allowSceneActivation = true;
    }

    private void OpenScene()
    {
        animator.SetBool("IsIn", false);
        animator.SetBool("IsOut", true);
    }

    public void StartLoading()
    {
        StartCoroutine(LoadSceneAsync(Scene_ID));
    }

    IEnumerator LoadSceneAsync(string SceneID)
    {
        operation = SceneManager.LoadSceneAsync(SceneID);
        operation.allowSceneActivation = false;

        print("PRRR : " + operation);

        while (!operation.isDone)
        {
            progressValue = Mathf.Clamp01(operation.progress / 0.9f);

            LoadingBarfill.GetComponent<Image>().fillAmount = progressValue;
            print("Loading : " + operation.progress + " ----------------------------------------------");
            yield return null;
            if((operation.progress / 0.9f) >= 1)
                OpenScene();
            //operation.allowSceneActivation = true;
        }
    }
}
