
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
    public GameObject[] Tier2CorruptedFishes;
    public GameObject[] Tier2CrimsonFishes;
    public GameObject[] Tier3CorruptedFishes;
    public GameObject[] Tier3CrimsonFishes;
    //Variant Fishes
    public GameObject[] Tier2CorruptedVariant;
    public GameObject[] Tier2CrimsonVariant;
    public bool fishSpawned = false;

    private int resetTimer = 0;
    public  int resetTimerMax = 100;
    //Test
    private bool testBool = false;

    //Fishing Rod
    public GameObject myFishingRod;

    //Fishes
    public bool tier1Fish = false;
    public bool tier2Fish = false;
    public bool tier3Fish = false;
    private GameObject previousFish;
    public bool previousFishCorrupted;
    public bool previousFishCrimson;
    public bool fishPulledOut = false;

    //Water Type
    public bool Corruption = false;
    public bool Crimson = false;

    //Difficulty

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

                    if(!tier1Fish)//If currently fishing for tier 1 fish
                    {
                        myFishingRod.GetComponent<ResistanceText>().jerkAdder = 0;
                        myFishingRod.GetComponent<ResistanceText>().jerkNumber = 50;
                        //Generate Exhaustion Easily
                        myFishingRod.GetComponent<ResistanceText>().exhaustionDelayMax = 10;
                        //Make Fish Faster
                        myFishingRod.GetComponent<ResistanceText>().fishMovementMax = 5;
                    }
                    else if (tier1Fish && !tier2Fish)//If currently fishing for tier 2 fish
                    {
                        myFishingRod.GetComponent<ResistanceText>().jerkAdder = 35;
                        myFishingRod.GetComponent<ResistanceText>().jerkNumber = 35;
                        //Generate Exhaustion Moderately
                        myFishingRod.GetComponent<ResistanceText>().exhaustionDelayMax = 20;
                        //Make Fish Faster
                        myFishingRod.GetComponent<ResistanceText>().fishMovementMax = 8;
                    }
                    else if (tier2Fish)//If currently fishing for tier 3 fish
                    {
                        myFishingRod.GetComponent<ResistanceText>().jerkAdder = 26;
                        myFishingRod.GetComponent<ResistanceText>().jerkNumber = 25;
                        //Generate Exhaustion Moderately
                        myFishingRod.GetComponent<ResistanceText>().exhaustionDelayMax = 30;
                        //Make Fish Faster
                        myFishingRod.GetComponent<ResistanceText>().fishMovementMax = 12;
                    }
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
                    //Generate Difficulty
                    if (fishSpawned == false)
                    {

                        //Randomly generate a tier 1 fish
                        if (!tier1Fish)
                        {
                            //Put the Fish on the hook and parent it
                            var myNewFish = Tier1Fishes[Random.Range(0, Tier1Fishes.Length)];//VRCInstantiate(testFish);
                            myNewFish.transform.position = gameObject.transform.position;
                            myNewFish.transform.parent = gameObject.transform;
                            previousFish = myNewFish;
                            //Say that we caught a tier 1 fish
                            tier1Fish = true;
                        }
                        else if (tier1Fish && Corruption && !tier2Fish) //Randomly Generate Tier 2 Corruption Fish
                        {
                            //Put the Fish on the hook and parent it
                            var myNewFish2 = Tier2CorruptedFishes[Random.Range(0, Tier2CorruptedFishes.Length)];//VRCInstantiate(testFish);
                                //If Not a Crimson Fish, try again
                                myNewFish2 = Tier2CorruptedFishes[Random.Range(0, Tier2CorruptedFishes.Length)];

                            myNewFish2.transform.position = gameObject.transform.position;
                            myNewFish2.transform.parent = gameObject.transform;

                            //Send the Tier 1 Fish back to Spawn
                            previousFish.GetComponent<Fish>().SendtoSpawn();
                            previousFish = myNewFish2;
                            //Say that we caught a tier 2 fish
                            tier2Fish = true;
                            //tier1Fish = false;
                        }
                        else if (tier1Fish && Crimson && !tier2Fish) //Randomly Generate Tier 2 Crimson Fish
                        {
                            //Put the Fish on the hook and parent it
                            var myNewFish2 = Tier2CrimsonFishes[Random.Range(0, Tier2CrimsonFishes.Length)];//VRCInstantiate(testFish);

                            //If Not a Crimson Fish, try again
                            myNewFish2 = Tier2CrimsonFishes[Random.Range(0, Tier2CrimsonFishes.Length)];

                            myNewFish2.transform.position = gameObject.transform.position;
                            myNewFish2.transform.parent = gameObject.transform;

                            //Send the Tier 1 Fish back to Spawn
                            previousFish.GetComponent<Fish>().SendtoSpawn();
                            previousFish = myNewFish2;
                            //Say that we caught a tier 2 fish
                            tier2Fish = true;
                            //t
                        }
                        else if (tier2Fish) //If we put a tier 2 fish in the water,
                        {
                            //Put the Fish on the hook and parent it, If fish is corrupted and dipped into Crimson spawn variant fish
                            if (previousFish.GetComponent<Fish>().corruptedFish == true && Crimson)
                            {
                                var myNewFish2 = Tier2CrimsonVariant[Random.Range(0, Tier2CrimsonVariant.Length)];//VRCInstantiate(testFish);
                                myNewFish2.transform.position = gameObject.transform.position;
                                myNewFish2.transform.parent = gameObject.transform;

                                //Send the Tier 1 Fish back to Spawn
                                previousFish.GetComponent<Fish>().SendtoSpawn();
                                previousFish = myNewFish2;
                                //Say that we caught a tier 2 fish
                                tier2Fish = true;
                                Debug.Log("Spawning Variant");
                            }
                            else if (previousFish.GetComponent<Fish>().corruptedFish == true && Corruption == true) //Else Generate Tier 3 Corrupted Fish
                            {
                                var myNewFish2 = Tier3CorruptedFishes[Random.Range(0, Tier3CorruptedFishes.Length)];//VRCInstantiate(testFish);
                                myNewFish2.transform.position = gameObject.transform.position;
                                myNewFish2.transform.parent = gameObject.transform;

                                //Send the Tier 1 Fish back to Spawn
                                previousFish.GetComponent<Fish>().SendtoSpawn();
                                previousFish = myNewFish2;
                                //Say that we caught a tier 2 fish
                                tier3Fish = true;
                                Debug.Log("Spawning Tier 3");
                            }
                            else if (previousFish.GetComponent<Fish>().corruptedFish == false && Corruption == false) //Else Generate Tier 3 Crimson Fish
                            {
                                var myNewFish2 = Tier3CrimsonFishes[Random.Range(0, Tier3CrimsonFishes.Length)];//VRCInstantiate(testFish);
                                myNewFish2.transform.position = gameObject.transform.position;
                                myNewFish2.transform.parent = gameObject.transform;

                                //Send the Tier 1 Fish back to Spawn
                                previousFish.GetComponent<Fish>().SendtoSpawn();
                                previousFish = myNewFish2;
                                //Say that we caught a tier 2 fish
                                tier3Fish = true;
                                Debug.Log("Spawning Tier 3");
                            }
                            else if (previousFish.GetComponent<Fish>().corruptedFish == false && Corruption) //Generate Variant
                            {
                                var myNewFish2 = Tier2CorruptedVariant[Random.Range(0, Tier2CorruptedVariant.Length)];//VRCInstantiate(testFish);
                                myNewFish2.transform.position = gameObject.transform.position;
                                myNewFish2.transform.parent = gameObject.transform;

                                //Send the Tier 1 Fish back to Spawn
                                previousFish.GetComponent<Fish>().SendtoSpawn();
                                previousFish = myNewFish2;
                                //Say that we caught a tier 2 fish
                                tier2Fish = true;
                                Debug.Log("Spawning Variant");
                            }
                            Debug.Log("Chicken Strips with man Aise");
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
            Crimson = false;
        }
        else if(other.gameObject.name == "CrimsonWater")
        {
            inWater = true;
            Crimson = true;
            Corruption = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //Change the Name to what the Water Is
        if (other.gameObject.name == "CorruptedWater")
        {
            inWater = false;
        }
        else if(other.gameObject.name == "CrimsonWater")
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

    public void fishPulledReset()
    {
        //Fishes
        tier1Fish = false;
        tier2Fish = false;
        tier3Fish = false;

        previousFishCorrupted = false;
        previousFishCrimson = false;
        fishPulledOut = false;
    }
}
