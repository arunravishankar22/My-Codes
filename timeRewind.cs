using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class timeRewind : MonoBehaviour
{
    public bool isRewinding = false;

    List<Vector3> position,rot;
    //List<Vector3> rot;
    //public Transform[] position;

    Rigidbody rb;


    // Start is called before the first frame update
    void Start()
    {
        position = new List<Vector3>();
        rot = new List<Vector3>();
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.R))
        {
            StartRewind();
        }
        if (Input.GetKeyUp(KeyCode.R))
            StopRewind();
    }

    private void FixedUpdate()
    {
        if (isRewinding)
             Rewind();
        else
             Record();
    }

    void Rewind()
    {
        if(position.Count>0)
        {
            transform.position =position[0];
            transform.eulerAngles = rot[0];
            position.RemoveAt(0);
            rot.RemoveAt(0);

        }
        else
        {
            StopRewind();
        }
        
    }

    void Record()
    {
        position.Insert(0, transform.position);
        rot.Insert(0, transform.rotation.eulerAngles);
    }

    void StartRewind()
    {
        rb.isKinematic = true;
        isRewinding = true;
    }

    void StopRewind()
    {
        isRewinding = false;
        rb.isKinematic = false;
    }
}
