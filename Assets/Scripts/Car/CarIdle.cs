using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarIdle : MonoBehaviour
{
    private Rigidbody rb;
    private float rumbleTime;
    private float timer;
    public float maxIdleSpeed;
    public float rpm;
    public float force;
    public float maxDist;
    // Start is called before the first frame update
    void Start()
    {

        rb = gameObject.GetComponent<Rigidbody>();
        rumbleTime = 60/rpm;
        timer = 0;
    }

    // Update is called once per frame
    private void FixedUpdate() {
        rumbleTime = 60/rpm;
        if(Mathf.Abs(rb.velocity.magnitude)<maxIdleSpeed){
            timer += Time.fixedDeltaTime;
            if(timer>rumbleTime){
                Rumble();
                timer = 0;
                
            }

        }
    }

    void Rumble(){
        float x = Random.Range(-maxDist, maxDist);
        float z = Random.Range(-maxDist, maxDist);
        rb.AddForceAtPosition(Vector3.up*force, rb.transform.position+new Vector3(x,0,z));

    }
}
