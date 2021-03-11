
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class Velocimeter : UdonSharpBehaviour
{
    public Rigidbody rb; // Add the rigidbody of your Player to your script
    public Vector3 oldPos;
    public Quaternion oldRot;

    public bool jerkRod = false;
    public bool positionRecorded = false;

    public Vector3 oldVelocity;
    public Vector3 tetherPosition;
    public Vector3 oldAngleVector;
    public GameObject centerObject;
    public bool tetherBroken = false;
    private int timer = 0;
    private int timermax = 60;

    public bool jerkLeft = false;
    public bool jerkRight = false;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        oldAngleVector = transform.position - centerObject.transform.position;
    }

    private void Update()
    {
        oldVelocity = rb.velocity;
        
        //Detect if Moving Left or Right
        //Vector3 movement = oldRot * (transform.position - oldPos);
        oldPos = transform.position;
        oldRot = transform.rotation;
        if (jerkRod)
        {



            if (timer>=timermax)
            {
                Vector3 currentVector = transform.position - centerObject.transform.position;
                float vectorAngle = Vector3.SignedAngle(oldAngleVector, currentVector, centerObject.transform.up);
                oldAngleVector = currentVector;
                Debug.Log(vectorAngle);
                if (vectorAngle <= -25 && jerkLeft)
                {
                    tetherBroken = true;
                    jerkLeft = false;
                }
                else if(vectorAngle <= 25 && jerkRight)
                {
                    tetherBroken = true;
                    jerkRight = false;
                }
                timer = 0;
            }

            timer += 1;
        }
    }
}
