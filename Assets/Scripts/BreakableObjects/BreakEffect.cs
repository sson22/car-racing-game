using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakEffect : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject brokenVersion; 
    public float breakForce = 10;
    private bool isBroken = false;
    

    public AudioClip soundClipPlay;

    void OnCollisionEnter(Collision collision) {
        if(Mathf.Abs(collision.impulse.magnitude) > breakForce ) {
            if(!isBroken){
                isBroken = true;
                AudioSource.PlayClipAtPoint(soundClipPlay,transform.position);
                Destroy(Instantiate(brokenVersion, transform.position, transform.rotation), 3); 
                Destroy(gameObject); 
                
            }
            
        }
       
    }
}
 