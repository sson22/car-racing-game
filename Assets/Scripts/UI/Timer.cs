using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public float time = 1000;
    void Update()
    {
        if(time>0){
            time -= Time.deltaTime;
        }
    }
}
