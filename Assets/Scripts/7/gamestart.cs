using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class gamestart : MonoBehaviour
{
    public void GameStart()
    {
        SceneManager.LoadScene(0);
    }

    public void GameQuit()
    {
        Application.Quit();
    }
}
