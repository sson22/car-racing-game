using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour {
    public List<AxleInfo> axleInfos; // the information about each individual axle
    public CarStats stats; // the game stats of the car
    private GameManager gameManager;
    private Subscriber resetSub;
    public Rigidbody rb;
    public Vector3 centerOfMass;
    private float maxMotorTorque; // maximum torque the motor can apply to wheel
    public float maxBrakeTorque;
    public float maxSteeringAngle; // maximum steer angle the wheel can have
    public float antiRollStiffness;
    public float downForceCoeff;
    public Vector3 downForceOffset;
    [HideInInspector]
    public KeyCode driftKey = KeyCode.LeftShift;
    public KeyCode brakeKey = KeyCode.Space;
    public float phantomForceCoef = 0.1f;
    private bool isInputEnabled = true;
    public AudioClip soundClipPlay;
    public AudioSource audioSource;
    private float pitch = 0;
    public float pitchRamp = 0.001f;


    private void Awake() {
       AudioSource.PlayClipAtPoint(soundClipPlay,transform.position,1);

    }
    private void Start() {
        rb.centerOfMass = centerOfMass;
        
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        audioSource.volume = gameManager.sound;

    }

    public void FixedUpdate()
    {
        if(gameManager) isInputEnabled =  gameManager.IsInputEnabled;

        applyStats(stats);
        int inputEnabled = (isInputEnabled?1:0);

        float motor = 
            inputEnabled *                                              //Input Enabled
            Input.GetAxis("Vertical") *                                 //Input
            maxMotorTorque *                                            //Acceleration
            ((Mathf.Abs(rb.velocity.magnitude)  < stats.topSpeed)?1:0); //Top Speed

        //Audio
        pitch += Mathf.Abs(pitchRamp*(motor)) - pitchRamp*maxMotorTorque*0.5f;
        pitch = Mathf.Clamp(pitch, 0, maxMotorTorque);
        audioSource.pitch = pitch/(maxMotorTorque+(maxMotorTorque*0.8f))+0.2f;


        float steering = 
            inputEnabled *                          
            Input.GetAxis("Horizontal")*
            maxSteeringAngle;
            
        
        float brake = 
            inputEnabled*
            (Input.GetKey(brakeKey)?1:0) *
            maxBrakeTorque +
            (isInputEnabled?0:1)*maxBrakeTorque;
            
        foreach (AxleInfo axleInfo in axleInfos) {
            if (axleInfo.steering) {
                axleInfo.leftWheel.steerAngle = steering;
                axleInfo.rightWheel.steerAngle = steering;
            }
            if (axleInfo.motor) {
                axleInfo.leftWheel.motorTorque = motor;
                axleInfo.rightWheel.motorTorque = motor;
            }
            if (axleInfo.brake) {
                axleInfo.leftWheel.brakeTorque = brake;
                axleInfo.rightWheel.brakeTorque = brake;
            }

            ApplyLocalPositionToVisuals(axleInfo.leftWheel, axleInfo.leftWheelModel);
            ApplyLocalPositionToVisuals(axleInfo.rightWheel, axleInfo.rightWheelModel);
            ApplyAntiRoll(axleInfo.leftWheel, axleInfo.rightWheel);
        }

        //Drifitng
        if(Input.GetKey(driftKey) && isInputEnabled){
            applySlipExt(stats.drifting * stats.sideSlipExt, stats.drifting * stats.frontSlipExt);
            applyStiffness(stats.handling * (1/stats.drifting), stats.grip * (1/stats.drifting));
        } else {
            applySlipExt(stats.sideSlipExt, stats.frontSlipExt);
            applyStiffness(stats.handling, stats.grip);
        }

        
        ApplyDownForce();
        ApplyPhantomForce(motor);
        
    }
    
    
    // finds the corresponding visual wheel
    // correctly applies the transform
    public void ApplyLocalPositionToVisuals(WheelCollider collider, Transform visWheel)
    {
          
        Vector3 position;
        Quaternion rotation;
        collider.GetWorldPose(out position, out rotation);
     
        visWheel.position = position;
        visWheel.rotation = rotation;
    }

    public void ApplyDownForce(){
        float lift = -downForceCoeff * rb.velocity.sqrMagnitude;
        rb.AddForceAtPosition(lift * transform.up, transform.position);
    }

    public void ApplyPhantomForce(float force){
        rb.AddForce(transform.forward*phantomForceCoef*force);
    }

    public void ApplyAntiRoll(WheelCollider _wheelL, WheelCollider _wheelR){
        WheelHit hitL, hitR;
        float _travelL, _travelR;        
        bool _groundedL = _wheelL.GetGroundHit(out hitL);
        bool _groundedR = _wheelR.GetGroundHit(out hitR);
        
        if (_groundedL)
            _travelL = (-_wheelL.transform.InverseTransformPoint(hitL.point).y - _wheelL.radius) / _wheelL.suspensionDistance;
        else
            _travelL = 1.0f;
        
        if (_groundedR)
            _travelR = (-_wheelR.transform.InverseTransformPoint(hitR.point).y - _wheelR.radius) / _wheelR.suspensionDistance;
        else
            _travelR = 1.0f;

        float _springForce = (_wheelL.suspensionSpring.spring+_wheelR.suspensionSpring.spring)/2;
        float _antiRollForce = (_travelL-_travelR)*antiRollStiffness*_springForce;

        if (_groundedL)
            rb.AddForceAtPosition(_wheelL.transform.up * -_antiRollForce, _wheelL.transform.position);  
        if (_groundedR)
            rb.AddForceAtPosition(_wheelR.transform.up * _antiRollForce, _wheelR.transform.position);
    }

    

    public void applyStats(CarStats stats){
        foreach(AxleInfo axle in axleInfos){
            WheelCollider[] wheels = {axle.leftWheel, axle.rightWheel};
            foreach(WheelCollider wheel in wheels){

                //Friction settings
                WheelFrictionCurve sFriction = wheel.sidewaysFriction;
                WheelFrictionCurve fFriction = wheel.forwardFriction;
                sFriction.stiffness = stats.handling;
                fFriction.stiffness = stats.grip;

                sFriction.extremumSlip = stats.sideSlipExt;
                fFriction.extremumSlip = stats.frontSlipExt;

                wheel.sidewaysFriction = sFriction;
                wheel.forwardFriction = fFriction;

                //Suspension settings
                JointSpring suspension = wheel.suspensionSpring;
                suspension.spring = stats.weight * (300f/20f);
                suspension.damper = stats.weight * (30f/20f);
                wheel.suspensionSpring = suspension;
            }
        }

        rb.mass = stats.weight;
        maxMotorTorque = stats.acceleration;
    }

    public void applySlipExt(float sideSlipExt, float frontSlipExt){
        foreach(AxleInfo axle in axleInfos){
             WheelCollider[] wheels = {axle.leftWheel, axle.rightWheel};
            foreach(WheelCollider wheel in wheels){
                WheelFrictionCurve sFriction = wheel.sidewaysFriction;
                WheelFrictionCurve fFriction = wheel.forwardFriction;
                sFriction.extremumSlip = sideSlipExt;
                fFriction.extremumSlip = frontSlipExt;
                wheel.sidewaysFriction = sFriction;
                wheel.forwardFriction = fFriction;
            }  
        }
    }

    public void applyStiffness(float sideSlipExt, float frontSlipExt){
        foreach(AxleInfo axle in axleInfos){
             WheelCollider[] wheels = {axle.leftWheel, axle.rightWheel};
            foreach(WheelCollider wheel in wheels){
                WheelFrictionCurve sFriction = wheel.sidewaysFriction;
                WheelFrictionCurve fFriction = wheel.forwardFriction;
                sFriction.stiffness = sideSlipExt;
                fFriction.stiffness = frontSlipExt;
                wheel.sidewaysFriction = sFriction;
                wheel.forwardFriction = fFriction;
            }  
        }
    }

    
}

[System.Serializable]
public class CarStats {
    public float handling;
    public float grip;
    public float topSpeed;
    public float acceleration;
    public float weight;
    public float drifting;
    public float sideSlipExt;
    public float frontSlipExt;
}
    
[System.Serializable]
public class AxleInfo {
    public WheelCollider leftWheel;
    public Transform leftWheelModel;
    public WheelCollider rightWheel;
    public Transform rightWheelModel;
    public bool motor; // is this wheel attached to motor?
    public bool brake; //does this wheel apply breaks?
    public bool steering; // does this wheel apply steer angle?
}
