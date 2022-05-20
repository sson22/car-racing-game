using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootPickup : Pickup
{   
    public string pickupAction =  "ADD_SCORE";
    public float objectValue;
    private void Start() {
        if(!manager){
            manager = GameObject.Find("LevelManager").GetComponent<LevelManager>();
        }
    }
    public override void notify(LevelManager manager)
    {
        manager.update(pickupAction, objectValue);
    }
}
