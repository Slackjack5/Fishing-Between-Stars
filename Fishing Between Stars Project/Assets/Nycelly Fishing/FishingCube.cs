
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

    public GameObject[] Tier1Fishes;
    public GameObject[] Tier2Fishes;

    public bool fishSpawned = false;

    private int resetTimer = 0;
    public  int resetTimerMax = 100;
    //Test
    private bool testBool = false;

    //Fishing Rod
    public GameObject myFishingRod;

    //Fishes
    public bool tier1Fish = false;
    private GameObject previousFish;
    public bool fishPulledOut = false;

    //Water Type
    public bool Corruption = false;
    public bool Crimson = false;
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
                    myFishingRod.GetComponent<ResistanceText>().fishResistanceScore = 500;
                    myFishingRod.GetComponent<ResistanceText>().resistanceScore = 500;
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
                        
                        //Randomly generate a tier 1 fish
                        if(!tier1Fish)
                        {
                            //Put the Fish on the hook and parent it
                            var myNewFish = Tier1Fishes[Random.Range(0, Tier1Fishes.Length)];//VRCInstantiate(testFish);
                            myNewFish.transform.position = gameObject.transform.position;
                            myNewFish.transform.parent = gameObject.transform;
                            previousFish = myNewFish;
                            //Say that we caught a tier 1 fish
                            tier1Fish = true;
                        }
                        else if(tier1Fish && Corruption) //Randomly Generate Tier 2 Fish
                        {
                            //Put the Fish on the hook and parent it
                            var myNewFish2 = Tier2Fishes[Random.Range(0,Tier2Fishes.Length)];//VRCInstantiate(testFish);
                            myNewFish2.transform.position = gameObject.transform.position;
                            myNewFish2.transform.parent = gameObject.transform;

                            //Send the Tier 1 Fish back to Spawn
                            previousFish.GetComponent<Fish>().SendtoSpawn();
                            previousFish = myNewFish2;
                            //Say that we caught a tier 2 fish
                        }
                        fishSpawned = true;
                        hookBite = false;
                        
                    }
                    if(fishSpawned)
                    {

                    }
                    myCubeRenderer.material.SetColor("_Color", Color.blue);

                    //Reset Variables
                    fishExhausted = false;
                    myFishingRod.GetComponent<ResistanceText>().fishResistanceScore = 0;
                    myFishingRod.GetComponent<ResistanceText>().resistanceScore = 0;
                    myFishingRod.GetComponent<ResistanceText>().exhaustionScore = 0;
                    resetEverything();
                }
                else
                {
                    //Reset Everything After Some Timer
                    if (resetTimer >= resetTimerMax)
                    {
                        //Reset all Variables
                        resetTimer = 0;
                        resetEverything();
                        myFishingRod.GetComponent<ResistanceText>().resetVariables();
                    }
                    else
                    {
                        resetTimer += 1;
                    }
                }

            }
            else
            {
                    //Reset all Variables
                    resetTimer = 0;
                    resetEverything();
                    myFishingRod.GetComponent<ResistanceText>().resetVariables();
           
                
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


    }

    private void OnTriggerEnter(Collider other)
    {
        //Change the Name to what the Water Is
        if (other.gameObject.name == "CorruptedWater")
        {
            inWater = true;
            Corruption = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //Change the Name to what the Water Is
        if (other.gameObject.name == "CorruptedWater")
        {
            inWater = false;
        }
    }
    void OnCollisionExit(Collision other)
    {


    }

    public void resetEverything()
    {
        myCubeRenderer.material.SetColor("_Color", Color.white);
        //Bool
        inWater = false;
        hookBite = false;
        fishExhausted = false;
        //Fishing Timer
        timer = 0;
        timerBig = 0;
        fishSpawned = false;
        testBool = false;
        Corruption = false;
        Crimson = false;
        myFishingRod.GetComponent<ResistanceText>().resetVariables();
    }
}
