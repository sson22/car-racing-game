using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuCamera : MonoBehaviour
{
    public Vector3[] positions;
    public int posIndex = 0;
    public Vector3 destination;
    public float speed;
    // Start is called before the first frame update
    void Start()
    {
        destination = positions[posIndex];
    }

    // Update is called once per frame
    void Update()
    {
        MoveToDest();
    }

    void MoveToDest(){
        transform.position = Vector3.Lerp(transform.position, destination, speed);
    }

    public void MoveLeft(){
      if(posIndex>=0){
          posIndex--;
          destination = positions[posIndex];
      }
    }

    public void MoveRight(){
        if(posIndex<positions.Length){
          posIndex++;
          destination = positions[posIndex];
      }
    }
}
