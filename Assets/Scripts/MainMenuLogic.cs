using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenuLogic : MonoBehaviour
{
    public void OnStartNewGameButtonClick()
    {
        SceneManager.LoadScene("PlayScene");
    }
    public void OnOpenCreditsButtonClick()
    {

    }
    public void OnCloseGameButtonClick()
    {
        Application.Quit();
    }
}
