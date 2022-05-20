using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MenuFundsText : Subscriber
{
    public TMP_Text text;
    public GameManager gameManager;
    private float money = 0;
    public string displayText;
    // Start is called before the first frame update
    void Start()
    {
        text = gameObject.GetComponent<TMP_Text>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        if(gameManager){
            gameManager.addMoneySubscriber(this);
            money = gameManager.money;
        }
        updateText();
    }

    // Update is called once per frame
    
    override public void update(params float[] vals)
    {
        money = vals[0];
        updateText();
    }

    public void updateText(){
        text.text = string.Format("{0} ${1}", displayText, money);
    }
}
