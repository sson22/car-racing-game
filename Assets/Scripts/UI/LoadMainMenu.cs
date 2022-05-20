using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadMainMenu : MonoBehaviour
{
    public void Load(){
        GameObject.Find("GameManager").GetComponent<GameManager>().IsInputEnabled = false;
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");

    }
}
