using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowController : MonoBehaviour
{
    public Transform objectToFollow;
    private Vector3 objectToFollowPrevPos;
    public float followSpeed = 1;
    public float lookSpeed = 1;
    public Vector3 offset =  (Vector3.up + -Vector3.forward)*10;

    public CollisionHandler collisionHandler = new CollisionHandler();
    private Camera cam;
    public float adjustDistance;
    public bool DebugDraw;
    public Vector3 destination = Vector3.zero;
    
    void Start(){
        cam = GetComponent<Camera>();
        collisionHandler.Init(cam);
        collisionHandler.UpdateClipPoints(transform.position, transform.rotation, ref collisionHandler.adjustedClipPoints);
        collisionHandler.UpdateClipPoints(destination, transform.rotation, ref collisionHandler.desiredClipPoints);
    }

    void lookAtTarget(){
        Vector3 _lookDirection = objectToFollow.position - transform.position; 
        Quaternion _quat = Quaternion.LookRotation(_lookDirection, Vector3.up);
        transform.rotation = Quaternion.Lerp(transform.rotation, _quat, lookSpeed);
    }

    void followTarget(){
        destination = objectToFollow.position + 
                      Vector3.up * offset.y +
                      objectToFollow.right * offset.x +
                      objectToFollow.forward.x * Vector3.right * offset.z +
                      objectToFollow.forward.z * Vector3.forward * offset.z;

        
        //Lock camera for drift
        // if(Input.GetKey(KeyCode.LeftShift)){
        //     Vector3 driftDestination = transform.position + 
        //                             ((objectToFollow.position-objectToFollowPrevPos)*
        //                             (1/followSpeed));
        //     destination = Vector3.MoveTowards(destination, driftDestination, followSpeed);
        // }
        


        if(collisionHandler.colliding){
            float minDistanceAdjust = Mathf.Clamp(
                collisionHandler.minDistance-((objectToFollow.transform.position - destination).magnitude-adjustDistance),
                0,
                collisionHandler.minDistance);

            Vector3 _adjustedTargetPosition = 
                destination +                                                                   //Original
                (objectToFollow.transform.position - destination).normalized * adjustDistance + //Pushed toward target
                new Vector3(0, minDistanceAdjust, 0);                                           //Pushed above if too close
            transform.position = Vector3.Lerp(transform.position, _adjustedTargetPosition, followSpeed);
        } else {
            transform.position = Vector3.Lerp(transform.position, destination, followSpeed);
        }

        objectToFollowPrevPos = objectToFollow.position;
    }

    void Update() {

        collisionHandler.UpdateClipPoints(transform.position, transform.rotation, ref collisionHandler.adjustedClipPoints);
        collisionHandler.UpdateClipPoints(destination, transform.rotation, ref collisionHandler.desiredClipPoints);
    }

    void FixedUpdate()
    {
        lookAtTarget();
        followTarget();

        if(DebugDraw){
            for(int i = 0; i< collisionHandler.desiredClipPoints.Length;i++){
                Debug.DrawLine(objectToFollow.position, collisionHandler.desiredClipPoints[i], Color.white);
                Debug.DrawLine(objectToFollow.position, collisionHandler.adjustedClipPoints[i], Color.green);

            }

        }
        

        collisionHandler.CheckColliding(objectToFollow.position);
        adjustDistance = Vector3.Distance(destination, objectToFollow.position) - collisionHandler.GetAdjustedDistanceByRay(objectToFollow.position);
    }

    [System.Serializable]
    public class CollisionHandler{
        public LayerMask collisionLayer;
        public bool colliding = false;
        [HideInInspector]
        public Vector3[] adjustedClipPoints;
        [HideInInspector]
        public Vector3[] desiredClipPoints;
        public float fovDiv = 2;
        public float minDistance = 5;
        Camera camera;

        public void Init(Camera cam){
            camera = cam;
            adjustedClipPoints = new Vector3[5];
            desiredClipPoints = new Vector3[5];
        }

        public bool CollisionAtClipPoint(Vector3[] clipPoints, Vector3 targetPos){
            for(int i = 0; i < clipPoints.Length ; i++){
                Ray ray = new Ray(targetPos, clipPoints[i]-targetPos);
                float distance = Vector3.Distance(clipPoints[i], targetPos);
                if (Physics.Raycast(ray, distance, collisionLayer)){
                    return true;
                }
            }
            return false;
        }

        public void UpdateClipPoints(Vector3 cameraPos, Quaternion cameraRot, ref Vector3[] intoArray){
            if(!camera){
                return;
            }

            intoArray = new Vector3[5];

            float z = camera.nearClipPlane;
            float x = Mathf.Tan(camera.fieldOfView/fovDiv) * z;
            float y = x/camera.aspect;

            //top left clip point
            intoArray[0] = (cameraRot * new Vector3 (-x,y,z)) + cameraPos;
            //top right clip point
            intoArray[1] = (cameraRot * new Vector3 (x,y,z)) + cameraPos;
            //bottom left clip point
            intoArray[2] = (cameraRot * new Vector3 (-x,-y,z)) + cameraPos;
            //bottom right clip point
            intoArray[3] = (cameraRot * new Vector3 (x,-y,z)) + cameraPos;
            //cameraPos
            intoArray[4] = cameraPos - camera.transform.forward;
        }

        public float GetAdjustedDistanceByRay(Vector3 from){
            float distance = -1;

            for(int i = 0; i < desiredClipPoints.Length; i++){
                Ray ray = new Ray(from, desiredClipPoints[i]-from);
                RaycastHit hit;

                if(Physics.Raycast(ray, out hit)){
                    if(distance == -1){
                        distance = hit.distance;
                    } else {
                        if (hit.distance < distance){
                            distance = hit.distance;
                        }
                    }
                }
            }

            if (distance == -1){
                return 0;
            } else {
                return distance;
            }

        }

        public void CheckColliding(Vector3 targetPos){
            if(CollisionAtClipPoint(desiredClipPoints, targetPos)){
                colliding = true;
            }else{
                colliding = false;
            }

        }
    }
}
