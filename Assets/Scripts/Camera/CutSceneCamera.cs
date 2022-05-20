using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CutSceneCamera : MonoBehaviour
{
    public Camera cam;
    public TMP_Text cutsceneText;
    public Image cutsceneFlash;
    public Cutscene[] cutscene;
    
    private int index;
    private float timeLeft;
    private Cutscene current;
    [HideInInspector]
    public bool finished = false;


    // Start is called before the first frame update
    void Start()
    {
        if(!cam) cam = gameObject.GetComponent<Camera>();
        index = 0;
        current = cutscene[index];
        timeLeft = current.time;
        cutsceneText.text = current.text;
    }

    // Update is called once per frame
    void Update()
    {
        if(timeLeft>0){
            cam.transform.position = Vector3.Lerp(cam.transform.position, current.CamPos.position, current.speed);
            cam.transform.rotation = Quaternion.Lerp(cam.transform.rotation, current.CamPos.rotation, current.speed);
            timeLeft -= Time.deltaTime;
            cutsceneFlash.color = new Color(255,255,255, timeLeft/current.time);
        } else {
            index++;
            if(index<cutscene.Length){
                current = cutscene[index];
                timeLeft = current.time;
                cutsceneText.text = current.text;
            } else {
                finished = true;
            }
        }
    }

    public void Finish()
    {
        this.enabled = false;
        cutsceneText.enabled = false;
    }

    [System.Serializable]
    public class Cutscene{
        public Transform CamPos;
        public float time;
        public float speed;
        public string text;
    } 
}
