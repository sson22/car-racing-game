using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetPositon : MonoBehaviour
{
    private Rigidbody rigidbody;
    public Vector3 resetOffset;
    public float maxResetVelocity;
    public float resetTime;
    private float resetTimer;
    private bool resetTimerStarted;
    private bool resetPublished;
    private GameManager gameManager;
    public KeyCode resetKey = KeyCode.R;
    private bool isInputEnabled;
    private Subscriber resetSub;
    
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        rigidbody = GetComponent<Rigidbody>();
    }
    void Update()
    {
        isInputEnabled = gameManager.isActiveAndEnabled;
        if(rigidbody.velocity.magnitude < maxResetVelocity && isInputEnabled){
            if(resetTimerStarted){
                if(resetTimer > 0){
                    resetTimer -= Time.deltaTime;
                } else {
                    if(!resetPublished){
                        PublishReset(0);
                        resetPublished = true;
                    }

                    if(Input.GetKeyDown(resetKey))
                        resetPosition();
                }
            } else {
                resetTimer = resetTime;
                resetTimerStarted = true;
            }
        } else {
            if(resetPublished){
                PublishReset(1);
                resetPublished = false;
                resetTimerStarted = false;
            }
        }
    }
    
    public void ResetSubscribe(Subscriber sub){
        resetSub = sub;
    }

    public void PublishReset(float reset){
        if(resetSub){
            resetSub.update(reset);
        }
    }

    public void resetPosition(){
        transform.position = transform.position + resetOffset;
        transform.eulerAngles = new Vector3(0,transform.eulerAngles.y,0);
    }
}
