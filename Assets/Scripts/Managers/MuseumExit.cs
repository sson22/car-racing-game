using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuseumExit : MonoBehaviour
{
    public LevelManager levelManager;
    // Start is called before the first frame update
    void Start()
    {
        levelManager = GameObject.Find("LevelManager").GetComponent<LevelManager>();
    }

    private void OnTriggerEnter(Collider other) {
        if(other.tag == "car")
            levelManager.Escape();
        
    }

}
