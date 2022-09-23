using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class mainMenu : MonoBehaviour
{
    void Update()
    {
        if(Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
    public void Play()
    {
        SceneManager.LoadScene(1);
    }
    public void Exit()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
        Application.Quit();
    }
}
