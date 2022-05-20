using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinematicCamera : MonoBehaviour
{
    public Transform objectToFollow;
    public float followSpeed = 0;
    public float lookSpeed = 10;
    public Vector3 offset;
    public float fovCoef = 60;
    public float maxFOV = 90;
    public float maxSpeed = 3;
    public float minSpeed = 0;
    
    
    void lookAtTarget(){
        Vector3 _lookDirection = objectToFollow.position - transform.position; 
        Quaternion _quat = Quaternion.LookRotation(_lookDirection, Vector3.up);
        transform.rotation = Quaternion.Lerp(transform.rotation, _quat, lookSpeed);
    }

    void followTarget(){
        Vector3 _targetPosition = objectToFollow.position + offset;
        transform.position = Vector3.Lerp(transform.position, _targetPosition, followSpeed);                          
    }

    void setFOV(){
        float speed = objectToFollow.GetComponent<Rigidbody>().velocity.magnitude;
        float fov = (Mathf.Sqrt(speed/maxSpeed))*fovCoef;
        fov = Mathf.Clamp(fov, fovCoef, maxFOV);
        this.GetComponent<Camera>().fieldOfView = fov;
        
    }

    void FixedUpdate()
    {
        lookAtTarget();
        followTarget();
    }
}
