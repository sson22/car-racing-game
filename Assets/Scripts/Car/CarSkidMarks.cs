using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSkidMarks : MonoBehaviour
{
    public float sideSlipThreshold;
    public float frontSlipThreshold;
    public float yDisplacement;
    public WheelSkid[] wheelSkids;


    [System.Serializable]
    public class WheelSkid{
        public WheelCollider wheel;
        public TrailRenderer skid;
    }


    private void Start() {
        foreach(WheelSkid wheelSkid in wheelSkids){
            wheelSkid.skid.emitting = false;
        }
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        foreach(WheelSkid wheelSkid in wheelSkids){
            WheelCollider wheel = wheelSkid.wheel;
            TrailRenderer skid = wheelSkid.skid;

            WheelHit hit;
            if(wheelSkid.wheel.GetGroundHit(out hit)){
                if(hit.sidewaysSlip > sideSlipThreshold || hit.forwardSlip > frontSlipThreshold){
                    wheelSkid.skid.gameObject.transform.position = hit.point+new Vector3(0,yDisplacement,0);
                    wheelSkid.skid.emitting = true;
                }else{
                    wheelSkid.skid.emitting = false;
                }
            }else{
                wheelSkid.skid.emitting = false;
            }
        }
        
    }
}
