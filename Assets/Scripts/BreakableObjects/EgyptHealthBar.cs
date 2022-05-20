using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EgyptHealthBar : MonoBehaviour
{
    // Start is called before the first frame update
    Vector3 currentScale;
    float currentHealth; 

    void Start()
    {
        currentScale = transform.localScale; 
        
    
    }

    // Update is called once per frame
    void Update()
    {
        
        if(!GameObject.Find("EgyptFuseBox")) {
            Destroy(gameObject);
        }
        currentScale.x =  GameObject.Find("EgyptFuseBox").GetComponent<Security>().health/10000; 
        transform.localScale = currentScale; 
    }
}

