using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class maze_debug : MonoBehaviour
{
    public float rotY = 0f;
    public float forceZ = 0f;
    public bool move = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (move)
        {
            gameObject.transform.Rotate(Vector3.up, rotY);
            gameObject.transform.Translate(Vector3.forward * forceZ);
        }
    }
}
