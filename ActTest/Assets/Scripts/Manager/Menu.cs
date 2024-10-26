using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public void Open_Settings()
    {

    }
    public void Start_Game()
    {
        //暂时跳战斗后面改酒馆
        SceneManager.LoadScene("CCZ_Scene_Fight");
    }
    public void Exit_Game()
    {
        Application.Quit();
    }
}
