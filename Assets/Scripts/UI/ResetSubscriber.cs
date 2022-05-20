using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResetSubscriber : Subscriber
{
    public Text text;
    private LevelManager levelManager;
    private ResetPositon reset;
    private bool subscribed;
    private void Start() {
        levelManager = GameObject.Find("LevelManager").GetComponent<LevelManager>();
        
    }

    private void Update() {
        if(!subscribed){
            reset = levelManager.vehicle.GetComponent<ResetPositon>();
            reset.ResetSubscribe(this);
            subscribed = true;
        }
        
    }

    public override void update(params float[] p){
        if(p[0] == 0){
            text.text = "Press R to reset position";
        } else {
            text.text = "";
        }
    }

    
}
