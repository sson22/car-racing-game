using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Buy : MonoBehaviour
{
    public float price;
    public GameManager gameManager;
    public GameObject selectButton;
    public TMP_Text text;
    public int vehicleIndex;
    private void Awake() {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        text.text = string.Format("${0}", price);
        gameObject.SetActive(!gameManager.purchasedVehicles[vehicleIndex]);
    }

    public void Purchase(){
        if(gameManager.money >= price){
            gameManager.updateMoney(gameManager.money-price);
            gameObject.SetActive(false);
            gameManager.purchasedVehicles[vehicleIndex] = true;
            selectButton.SetActive(true);
        }
    }
}
