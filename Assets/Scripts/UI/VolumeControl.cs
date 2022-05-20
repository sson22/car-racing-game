using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeControl : MonoBehaviour
{
    GameManager gameManager;
    public Slider slider;
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        slider.value = gameManager.sound;
    }

    public void SetVolume(){
        gameManager.SetVolume(slider.value);
    }
}
