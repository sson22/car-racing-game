using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Pickup : MonoBehaviour
{
    public LevelManager manager;
    public GameObject createOnDestroy;
    private void OnTriggerEnter(Collider other) {
        notify(manager);
        GameObject destroyObject = GameObject.Instantiate<GameObject>(createOnDestroy);
        destroyObject.transform.position = transform.position + Vector3.up*1;
        Destroy(transform.parent.gameObject);
    }

    public abstract void notify(LevelManager Manager);
}
