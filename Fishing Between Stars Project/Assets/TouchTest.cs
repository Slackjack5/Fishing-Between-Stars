﻿
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class TouchTest : UdonSharpBehaviour
{

    private VRCPlayerApi player;
    public GameObject testBall;
    public float range = 50;
    public Renderer myRenderer;
    public GameObject myFishingRod;

    public GameObject[] myCircles;
    public GameObject myHandle;
    public int arrayPos;
    public int lastPos;
    public bool resistanceAdded = false;



    void Start()
    {
        player = Networking.LocalPlayer;
        Debug.Log(player);
        arrayPos = 1;
    }

    private void Update()
    {
        Vector3 r = player.GetTrackingData(VRCPlayerApi.TrackingDataType.RightHand).position;// - player.GetPosition();
        Vector3 l = player.GetTrackingData(VRCPlayerApi.TrackingDataType.LeftHand).position;

       // Debug.Log("Right Hand:" + " " + r);
        //Debug.Log("Left Hand:" + " " + l);

        if (Vector3.Distance(r, myCircles[arrayPos].transform.position) <= range)
        {




            //Slow down how fast we count player resistance
            if(myCircles[arrayPos].GetComponent<TouchTest>().resistanceAdded==false)
            {

                if (myHandle.GetComponent<Handle>().handleHeld==true)
                {
                    Debug.Log("adding resistance by Hand");
                    //Add Vibration
                    Networking.LocalPlayer.PlayHapticEventInHand(VRC_Pickup.PickupHand.Right, 0.1f, 50, 50);
                    //myFishingRod.GetComponent<ResistanceText>().triggerHeld = true;
                    myFishingRod.GetComponent<ResistanceText>().resistanceScore += 100;
                    myFishingRod.GetComponent<ResistanceText>().wasTouched = true;
                    myFishingRod.GetComponent<ResistanceText>().deltaResistanceScore = 16;
                    myCircles[arrayPos].GetComponent<TouchTest>().resistanceAdded = true;
                    //Debug.Log("Player in Range");
                    myCircles[arrayPos].GetComponent<Renderer>().enabled = false;

                    if (arrayPos == 0)
                    {
                        for (int x = 0; x <= myCircles.Length - 1; x++)
                        {
                            myCircles[x].GetComponent<Renderer>().enabled = true;
                            myCircles[x].GetComponent<TouchTest>().resistanceAdded = false;
                        }
                    }

                    //Increment Array Position
                    arrayPos = (arrayPos + 1) % myCircles.Length;
                }

            }



            //myFishingRod.GetComponent<ResistanceText>().wasTouched = true;


        }





        if (myHandle.GetComponent<Handle>().handleHeld == false)
        {
            myFishingRod.GetComponent<ResistanceText>().handleBeingHeld = false;
        }
        else
        {
            myFishingRod.GetComponent<ResistanceText>().handleBeingHeld = true;
        }
    }


}
