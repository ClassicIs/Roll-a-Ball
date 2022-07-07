using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class NextLevelMenu : MonoBehaviour {

    public void NextLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

    }

    public void Menu ()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu");


    }


    public void Quit()
    {
        Application.Quit();
    }

}
