using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public Canvas[] screens;
    public bool isPaused { get; private set; }
    private void Awake()
    {
        if (screens.Length > 0) {
            SetActiveScreen(0);
        }
        isPaused = false;
    }
    public void DeactivateAll() {
        for (int i = 0; i < screens.Length; i++)
        {
            screens[i].gameObject.SetActive(false);
        }
    }
    public void SetActiveScreen(int i) {
        DeactivateAll();
        i = Mathf.Clamp(i, 0, screens.Length - 1);
        screens[i].gameObject.SetActive(true);
    }
    public void SetScene(string sceneName) {
        SceneManager.LoadScene(sceneName);
    }
    public void TogglePause(int pauseScreenIndex = 1)
    {
        isPaused = !isPaused;
        if (isPaused)
        {
            Time.timeScale = 0;
            if (pauseScreenIndex > 0 && pauseScreenIndex < screens.Length) {
                screens[pauseScreenIndex].gameObject.SetActive(true);
            }
        }
        else {
            Time.timeScale = 1;
            if (pauseScreenIndex > 0 && pauseScreenIndex < screens.Length)
            {
                screens[pauseScreenIndex].gameObject.SetActive(false);
            }
        }
    }
    public void Quit()
    {
        Application.Quit();
    }
}
