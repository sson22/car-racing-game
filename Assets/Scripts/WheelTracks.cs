using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelTracks : MonoBehaviour
{
    public Shader _drawShader;
    private RenderTexture _splatmap;
    private Material _snowMaterial;
    private Material  _drawMaterial;
    public GameObject _terrain;
    public WheelCollider[] _wheel;
    RaycastHit _groundHit;
    int _layerMask;
    public float drawStrength;
    public float drawSize;
    public float slipThreshold = 0.5f;
    public float threshold = 0.5f;
    // Start is called before the first frame update
    void Start()
    {
        _layerMask = LayerMask.GetMask("Ground");
        _drawMaterial = new Material(_drawShader);
        _drawMaterial.SetFloat("_Size", drawSize);
        _snowMaterial  = _terrain.GetComponent<MeshRenderer>().material;
        _splatmap = new RenderTexture(1024,1024,0,RenderTextureFormat.ARGBFloat);
        _snowMaterial.SetTexture("_Splat", _splatmap);     
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        for(int i = 0; i < _wheel.Length; i++){
            Vector3 _currPosition = _wheel[i].transform.position;

            WheelHit hit;     

            //Store last coordinate, check if they are too close
            //Check if the car is moving, only run the script while the car moves
            if(_wheel[i].GetGroundHit(out hit) && hit.collider.Equals(gameObject.GetComponent<Collider>())){
                float slip = Mathf.Abs(hit.forwardSlip)+Mathf.Abs(hit.sidewaysSlip);
                if(slip<threshold){
                    continue;
                }else{
                    Physics.Raycast(_wheel[i].transform.position, -Vector3.up, out _groundHit, 1f, _layerMask);
                    Debug.Log(string.Format("Slip {0} threshhold {1}", slip, threshold));
                    _drawMaterial.SetVector("_Coordinate", new Vector4(_groundHit.textureCoord.x, _groundHit.textureCoord.y, 0, 0));

                    RenderTexture temp = RenderTexture.GetTemporary(_splatmap.width, _splatmap.height, 0, RenderTextureFormat.ARGBFloat);

                    Graphics.Blit(_splatmap, temp);
                    Graphics.Blit(temp, _splatmap, _drawMaterial); 

                    RenderTexture.ReleaseTemporary(temp);
                }

                
            }
        }


    }
}
