using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectVehicle : MonoBehaviour
{
    public GameManager gameManager;
    public GameObject vehicle;
    public int vehicleIndex;


    void Awake()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        gameObject.SetActive(gameManager.purchasedVehicles[vehicleIndex]);
    }

    public void Select(){
        gameManager.vehicle = this.vehicle.gameObject;
        gameManager.selectedVehicle = vehicleIndex;
    }
}
