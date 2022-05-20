using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //Contains global variables for the game, updated by the options menu

    public float sound = 1;
    public float money = 0;
    public bool IsInputEnabled = false;
    private ArrayList moneySubs = new ArrayList();
    private ArrayList[] highscoreSubs;
    private static GameManager _instance;
    public GameObject vehicle;
    public float[] highscores;
    public bool[] purchasedVehicles = {true, false, false};
    public int selectedVehicle;


    private void Start() {
        DontDestroyOnLoad(gameObject);
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        } else {
            _instance = this;
        }

        //Initializing highscores
        highscoreSubs = new ArrayList[highscores.Length];
        for(int i=0;i<highscoreSubs.Length;i++){
            highscoreSubs[i] = new ArrayList();
        }

        SetVolume(sound);
    }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        } else {
            _instance = this;
        }
    }

    public void SetVolume(float volume){
        sound = volume;
        AudioSource audioSource = gameObject.GetComponent<AudioSource>();
        audioSource.volume = sound;

        AudioSource[] sources = GameObject.FindObjectsOfType<AudioSource>();

        for(int i=0;i<sources.Length;i++){
            sources[i].volume = sound;
        }
    }

    public void updateMoney(float amount){
        money = amount;
        publishMoney();
    }

    public void addMoneySubscriber(Subscriber sub){
        moneySubs.Add(sub);
    }
    private void publishMoney(){
        foreach(Subscriber sub in moneySubs){
            sub.update(money);
        }
    }

    public void updateHighscore(int index, float amount){
        if(amount>highscores[index]){
            highscores[index] = amount;
            publishHighscore(index);
        } 
    }

    public void addHighscoreSubscriber(int index, Subscriber sub){
        highscoreSubs[index].Add(sub);
    }
    private void publishHighscore(int index){
        foreach(Subscriber sub in highscoreSubs[index]){
            sub.update(highscores[index]);
        }
    }
    
}
