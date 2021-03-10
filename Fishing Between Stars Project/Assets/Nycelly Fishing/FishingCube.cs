
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class FishingCube : UdonSharpBehaviour
{
    //Bool
    public bool inWater = false;
    public bool hookBite = false;
    public bool fishExhausted = false;

    public Renderer myCubeRenderer;
    //Fishing Timer
    private int timer = 0;
    private int timerBig = 0;
    public int timerMin = 4;
    public int timerMax = 10;

    public GameObject testFish;

    public bool fishSpawned = false;
    //Fighting The Fish
    private int resistance = 0;
    private int playerStrength = 100;

    //Test
    private bool testBool = false;

    //Fishing Rod
    public GameObject myFishingRod;

    void Start()
    {
       
    }

    private void Update()
    {
        //transform.Rotate(Vector3.up, 90f * Time.deltaTime);

        //If the hook is in the water , Run the Code for Catching a Fish
        if (inWater==true)
        {

            //Set a random number for how long it will take to catch this fish
            var chosenNumber = Random.Range(timerMin, timerMax);
            if (timerBig >= chosenNumber)
            {
                hookBite = true;
            }


            //If the Fish Bites the Hook
            if (hookBite == true)
            {
                if(!fishExhausted)
                {
                    myCubeRenderer.material.SetColor("_Color", Color.green);
                }
                else
                {
                    myCubeRenderer.material.SetColor("_Color", Color.yellow);
                }
                
                //Tell the rod we caught a fish
                myFishingRod.GetComponent<ResistanceText>().hookBite = true;
                if(testBool==false)
                {
                    myFishingRod.GetComponent<ResistanceText>().fishResistanceScore = 50;
                    myFishingRod.GetComponent<ResistanceText>().fishType = Random.Range(0,1);
                    testBool = true;
                }
                
            }
            else
            {
                myCubeRenderer.material.SetColor("_Color", Color.red);
            }
            
        }
        else //If not in the water
        {

            //If player pulls fish out the water, else reset everything
            if (hookBite == true)
            {
               
                if (fishExhausted)
                {
                    if (fishSpawned == false)
                    {
                        var myNewSmoke = testFish;//VRCInstantiate(testFish);
                        myNewSmoke.transform.position = gameObject.transform.position;
                        myNewSmoke.transform.parent = gameObject.transform;

                        fishSpawned = true;
                        hookBite = false;
                    }
                    myCubeRenderer.material.SetColor("_Color", Color.blue);

                    //Reset Variables
                    fishExhausted = false;
                    myFishingRod.GetComponent<ResistanceText>().fishResistanceScore = 0;
                    myFishingRod.GetComponent<ResistanceText>().resistanceScore = 0;
                    myFishingRod.GetComponent<ResistanceText>().exhaustionScore = 0;

                }

            }
            else
            {
                //Reset all Variables
                hookBite = false;
                timer = 0;
                timerBig = 0;
                myCubeRenderer.material.SetColor("_Color", Color.white);
            }
            
        }

    }
    
    public void biteHook()
    {

    }

    private void FixedUpdate()
    {
        //Check if the Fish has bit the hook, if not continue the wait timer
        if (hookBite==false && inWater==true)
        {
            if (timer <= 30)
            {
                timer += 1;
            }
            else
            {
                timerBig += 1;
                timer = 0;
            }
        }
       
    }

    void OnCollisionEnter(Collision other)
    {
        //Change the Name to what the Water Is
        if(other.gameObject.name == "Test Water")
        {
            inWater = true;
            Debug.Log("In the Water");
        }

    }

    void OnCollisionExit(Collision other)
    {
        //Change the Name to what the Water Is
        if (other.gameObject.name == "Test Water")
        {
            inWater = false;
            Debug.Log("Out of Water");
        }

    }
}
