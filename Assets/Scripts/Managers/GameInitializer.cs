using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInitializer : MonoBehaviour
{
    // Start is called before the first frame update
    public GameManager gameManager;
    public GameObject gameManagerObject;
    void Start()
    {
        if(GameObject.Find("GameManager")){

        } else {
            GameObject o = GameObject.Instantiate(gameManagerObject);
            o.name = "GameManager";
            gameManager = o.GetComponent<GameManager>();
        }
        
    }
}
