using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public void GoHome()
    {
        SceneManager.LoadScene(0);
    }
    public void PlayQuiz()
    {
        SceneManager.LoadScene(1);
    }

    public void StartAboutEyam()
    {
        SceneManager.LoadScene(2);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

}
