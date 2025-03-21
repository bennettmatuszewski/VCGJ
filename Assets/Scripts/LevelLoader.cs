using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelLoader : MonoBehaviour
{
    public Animator transition;
    public float transitionTime = 1f;
    private string certainScened;
    private IEnumerator Start()
    {
        transition.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.3f);
        transition.SetTrigger("End");
    }

    public void LoadNextLevel()
    {
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1));
    }
    public void LoadCertainScene(string certainScene)
    {
        Time.timeScale = 1f;
        StartCoroutine(LoadCertainScene());
        certainScened = certainScene;
    }
    public void Restart()
    {
        Time.timeScale = 1f;
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex));
    }
    IEnumerator LoadLevel(int levelIndex)
    {
        transition.gameObject.SetActive(true);
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(transitionTime);
        AsyncOperation operation = SceneManager.LoadSceneAsync(levelIndex);
        while (!operation.isDone)
        {
            yield return null;
        }
        yield return new WaitForSeconds(0.5f);
    }
    IEnumerator LoadCertainScene()
    {
        transition.SetTrigger("Start");        
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadSceneAsync(certainScened);
        
    }
    
}
