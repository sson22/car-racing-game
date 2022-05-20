using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShaderSwitch : MonoBehaviour
{   
    GameManager gameManager;
    public Shader shader1;
    public Shader shader2;
    public bool isShader1;
    public string tag;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
         if (Input.GetKeyDown(KeyCode.N) && gameManager.IsInputEnabled)
        {   
            Shader shaderToChange2 = shader1;
            
            
            if (isShader1)
            {
                shaderToChange2 = shader2;
                
            }

            foreach (var item in GameObject.FindGameObjectsWithTag(tag)){
              foreach (Renderer rend in item.GetComponentsInChildren<Renderer>())
                {
                    rend.material.shader = shaderToChange2;
                }  
            }
            
            isShader1 = !isShader1;
        }
        
    }
}