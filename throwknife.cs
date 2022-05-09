using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class throwknife : MonoBehaviour
{
    public Rigidbody weaponRb;
    private knife weaponScript;
    private ThirdPersonController th;


    public Transform weapon;
    public Transform hand;
    public float throwForce = 50;
    public Transform curve_point;
    public TrailRenderer Trailrender;

    private Vector3 origLocPos;
    private Vector3 origLocRot;
    private Vector3 old_pos;
    public float time = 0.0f;
    private bool isReturing = false;
    // Start is called before the first frame update
    void Start()
    {
        Trailrender.emitting = false;
        weaponRb = weapon.GetComponent<Rigidbody>();
        weaponScript = weapon.GetComponent<knife>();
        th = GetComponent<ThirdPersonController>();
        origLocPos = weapon.localPosition;
        origLocRot = weapon.localEulerAngles;
    }

    // Update is called once per frame
    void Update()
    {
       
        if (isReturing)
        {
            
            if (time < 1.0f)
            {
                
                
                weapon.position = getBezierQuadraticCurvePoint(time, old_pos, curve_point.position, hand.position);
                time += Time.deltaTime * 1.5f;
            }
            else
            {
                ResetKnife();
                th.catching();
            }
        }
    }

   

    //throw knf
    public void Throwknife()
    {
       
        Trailrender.emitting = true;
        th.aiming(false);
        th.hasWeapon = false;
        weaponScript.activated = true;
        weapon.parent = null;
        weaponRb.isKinematic = false;
        weaponRb.collisionDetectionMode = CollisionDetectionMode.Continuous;
        //weapon.eulerAngles = new Vector3(-90 + transform.eulerAngles.x, 0, 0);
        weapon.eulerAngles = new Vector3(0, -90 + transform.eulerAngles.y, 0);
        weapon.transform.position += transform.right / 5;
        weaponRb.AddForce(Camera.main.transform.TransformDirection(Vector3.forward) * throwForce, ForceMode.Impulse);
        
    }
    //return knf
    public void ReturnKnife()
    {
        
        old_pos = weapon.position;
        
        weaponRb.Sleep();
        weaponRb.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
        isReturing = true;
        weaponRb.isKinematic = true;
        weaponScript.activated = true;

    }

    //rest knif
    public void ResetKnife()
    {
        Trailrender.emitting = false;
        time = 0.0f;
        weaponRb.isKinematic = true;
        isReturing = false;
        weaponScript.activated = false;
        weapon.parent = hand;
        weapon.localEulerAngles = origLocRot;
        weapon.localPosition = origLocPos;
        
    }


    Vector3 getBezierQuadraticCurvePoint(float t, Vector3 p0, Vector3 p1, Vector3 p2)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;
        Vector3 p = (uu * p0) + (2 * u * t * p1) + (tt * p2);
        return p;
        
    }

    
}
