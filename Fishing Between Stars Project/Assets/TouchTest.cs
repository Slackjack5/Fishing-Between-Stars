
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
    public int arrayPos;

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

        Debug.Log("Right Hand:" + " " + r);
        Debug.Log("Left Hand:" + " " + l);

        if (Vector3.Distance(r, myCircles[arrayPos].transform.position) <= range)
        {
            Debug.Log("Player in Range");
            myCircles[arrayPos].GetComponent<Renderer>().material.SetColor("_Color", Color.blue);

            //Increment Array Position
            arrayPos = (arrayPos+1) % myCircles.Length;
            if(arrayPos==0)
            {
                for(int x=0;x<myCircles.Length-1;x++)
                {
                    myCircles[x].GetComponent<Renderer>().material.SetColor("_Color", Color.white);
                }
            }

            //Add Resistance
            

            if (myFishingRod.GetComponent<ResistanceText>().triggerHeld==true)
            {

                //Slow down how fast we count player resistance
                myFishingRod.GetComponent<ResistanceText>().reistanceRate += 30;

                if (myFishingRod.GetComponent<ResistanceText>().reistanceRate >= myFishingRod.GetComponent<ResistanceText>().reistanceRateMax)
                {
                    //Resistance
                    myFishingRod.GetComponent<ResistanceText>().reistanceRate = 0;

                    //Go to positive if negative
                    if (myFishingRod.GetComponent<ResistanceText>().deltaResistanceScore == -1)
                    {
                        myFishingRod.GetComponent<ResistanceText>().deltaResistanceScore *= -1;
                    }
                    else
                    {
                        if (myFishingRod.GetComponent<ResistanceText>().deltaResistanceScore <= -1)
                        {
                            myFishingRod.GetComponent<ResistanceText>().deltaResistanceScore /= 2;
                        }
                        else
                        {
                            myFishingRod.GetComponent<ResistanceText>().deltaResistanceScore *= 2;
                        }

                    }
                }
            }
            //myFishingRod.GetComponent<ResistanceText>().wasTouched = true;

        }



    }


}
