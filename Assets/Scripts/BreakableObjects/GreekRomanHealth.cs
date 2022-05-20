using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreekRomanHealth : MonoBehaviour
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
        
        if(!GameObject.Find("GreekRomanFuseBox")) {
            Destroy(gameObject);
        }
        currentScale.x =  GameObject.Find("GreekRomanFuseBox").GetComponent<Security>().health/10000; 
        transform.localScale = currentScale; 
    }
}
