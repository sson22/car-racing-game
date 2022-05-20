using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MenuHighscore : Subscriber
{
    public TMP_Text text;
    public GameManager gameManager;
    public int level;
    private float score = 0;
    public string displayText;
    // Start is called before the first frame update
    void Start()
    {
        text = gameObject.GetComponent<TMP_Text>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        if(gameManager){
            gameManager.addHighscoreSubscriber(level, this);
            score = gameManager.highscores[level];
        }
        updateText();
    }

    // Update is called once per frame
    
    override public void update(params float[] vals)
    {
        score = vals[0];
        updateText();
    }

    public void updateText(){
        text.text = string.Format("{0} {1}", displayText, score);
    }
}
