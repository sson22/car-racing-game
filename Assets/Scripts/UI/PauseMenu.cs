using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public bool paused = false;
    public KeyCode pauseKey = KeyCode.Escape;
    public GameObject pauseGUI;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(pauseKey)){
            togglePause();
            pauseGUI.SetActive(paused);
        }
    }

    void togglePause(){
        paused = !paused;
        if(paused){
            Time.timeScale = 0f;
        } else {
            Time.timeScale = 1f;
        }
    }

    public void loadMainMenu(){
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}
