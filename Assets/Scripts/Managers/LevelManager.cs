using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    //Controls level specific parameters and tracks level specific
    
    private GameManager gameManager;
    public int levelIndex;
    public GameObject inGameUI;
    public CutSceneCamera cutscene;
    public Timer timer;
    public Text timerText;
    public Text scoreText;
    public GameObject camera;
    private GameObject playerVehicle;
    public Transform spawnTransform;
    public Color UIColor;
    public Color scoreColor;
    public float scoreTime;
    private float scoreTimeRem;
    private bool scored = false;
    private float score = 0;
    public GameObject gameOverUI;
    public Text gameOverText;
    public Text gameOverScoreText;
    public float gameOverDelay = 2;
    public float gameOverTime = 10;
    private bool gameOver = false;
    private bool escaped = false;
    public GameObject vehicle;
    

    private void Start() {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        if(cutscene){
            gameManager.IsInputEnabled = false;
            camera.GetComponent<CameraFollowController>().enabled = false;
            timer.enabled = false;
            cutscene.enabled = true;
            inGameUI.SetActive(false);
        } else {
            gameManager.IsInputEnabled = true;
        }

        scoreText.text = string.Format("{0}$", score);
        
        playerVehicle = gameManager.vehicle;
        vehicle = Instantiate(playerVehicle, spawnTransform);
        camera.GetComponent<CameraFollowController>().objectToFollow = vehicle.transform;
    }

    private void Update() {
        if(cutscene){
            if(cutscene.enabled){
                Cutscene();
                return;
            }
        }
        
        

        if(gameOver){
            GameOver();
            return;
        }

        Game();
    }

    void Scored(){
        scoreTimeRem -= Time.deltaTime;
        scoreText.color = Color.Lerp(UIColor, scoreColor, (scoreTimeRem/scoreTime)) ;
        if(scoreTimeRem<0){
            scored = false;
        }
    }

    void Cutscene(){
        if(cutscene.finished){
            camera.GetComponent<CameraFollowController>().enabled = true;
            cutscene.Finish();
            timer.enabled = true;
            gameManager.IsInputEnabled = true;
            inGameUI.SetActive(true);
        }
    }

    void Game(){
        if(timer.time<0){
            gameOver = true;
        } else {
            int minutes = Mathf.FloorToInt(timer.time/60);
            int seconds = Mathf.FloorToInt(timer.time - (minutes*60));
            int miliSeconds = Mathf.FloorToInt((timer.time - (minutes*60) - seconds)*100);

            if(timer.time<30){
                timerText.color = Color.red;
            }

            timerText.text = string.Format("{0}:{1}.{2,1:D2}", minutes, seconds, miliSeconds);

            if(scored){
                Scored();
            }
        } 
    }

    void GameOver(){
        inGameUI.SetActive(false);
        gameManager.IsInputEnabled = false;
        
        if(gameOverDelay > 0){
            gameOverDelay -= Time.deltaTime;
        } else {
            if(gameOverTime > 0){
                gameOverUI.SetActive(true);
                if(escaped){
                    gameOverScoreText.text = string.Format("Loot Stolen: ${0}", score);
                }
                gameOverTime -= Time.deltaTime;

            } else {
                if(escaped){
                    gameManager.updateMoney(gameManager.money+score);
                    gameManager.updateHighscore(levelIndex, score);
                }
                SceneManager.LoadScene("MainMenu");
            }
        }
    }

    public void Escape(){
        gameOverText.text = "Escaped!";
        gameOver = true;
        escaped = true;

    }

    public void update(string action, float data){
        switch (action){
            case "ADD_SCORE":
                score += data;
                scoreText.text = string.Format("{0}$", score);
                scored = true;
                scoreTimeRem = scoreTime;
                break;
        }
    }
}
