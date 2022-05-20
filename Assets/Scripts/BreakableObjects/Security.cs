using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Security : MonoBehaviour
{
    public GameObject glassCase; 
    public GameObject brokenVersion; 

    public float health = 50000; 


    void OnCollisionEnter(Collision collision) {
        Debug.Log(collision.impulse.sqrMagnitude); 
        health -= collision.impulse.sqrMagnitude;
        if(health <= 0) { 
        Destroy(glassCase); 
        Destroy(Instantiate(brokenVersion, transform.position, transform.rotation), 3);  
        Destroy(gameObject); 
        }
       
    }
}
