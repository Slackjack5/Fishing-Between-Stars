
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;
using System.Collections;

public class ResistanceText : UdonSharpBehaviour
{

    //Text
    public Text playerResistance;
    public Text fishResistance;

    //Player
    public float resistanceScore = 0;
    private bool triggerHeld = false;
    private float reistanceRate = 0;
    private float reistanceDecay = 0;
    public float reistanceRateMax = 30;

    //Fish
    public float fishResistanceScore = 0;
    public bool fishFight = true;
    public bool fishFlee = false;
    public int fishType=0;
    private float fishDecay = 0;
    public float fishDecayMax = 20;
    private float timerDelay = 0;
    public float timerDelayMax = 20;
    public float timeBetweenFights = 1000;
    //Reactions
    public bool hookBite = false;
    private bool buildingFight=false;
    public int fightTimer = 0;

    void Start()
    {
        
    }

    private void FixedUpdate()
    {
        //Player Resistance
        //If The Trigger is being held , increase player resistance
        if (triggerHeld)
        {
            //Slow down how fast we count player resistance
            reistanceRate += 1;
            if(reistanceRate>= reistanceRateMax)
            {
                resistanceScore += 1;
                reistanceRate = 0;
            }
            
        }
        else
        {
            //Slow down how fast we decay player resistance
            reistanceDecay += 1;
            if(reistanceDecay>= reistanceRateMax/2)
            {
                resistanceScore -= 1;
                reistanceDecay = 0;
            }
        }

   

        //Fish Resistance
        if (hookBite==true)
        {

            if (fishFight)
            {
                fightSmall();
            }
            else if (fishFlee)
            {

            }
            else
            {
                fightTimer += 1;
                if (fightTimer>= timeBetweenFights)
                {
                    fishFight = true;
                    fightTimer = 0;
                }
                
                    
                //Decay Resistance if not currently fighting
                fishDecay += 1;
                if (fishDecay >= fishDecayMax)
                {
                    fishResistanceScore -= 1;
                    fishDecay = 0;
                }
            }

        }
        //Don't Let Values Drop Below 0
        if (resistanceScore < 0)
        {
            resistanceScore = 0;
        }
        if (resistanceScore > 100)
        {
            resistanceScore = 100;
        }
        if (fishResistanceScore < 0)
        {
            fishResistanceScore = 0;
        }
        if (fishResistanceScore > 100)
        {
            fishResistanceScore = 100;
        }
    }
    private void Update()
    {
        //Convert Player Resistance to a seeable string
        playerResistance.text = "Me:" + " " + resistanceScore.ToString();
        //Convert Fish Resistance to a seeable string
        fishResistance.text = "Fish:" + " " + fishResistanceScore.ToString();
    }

    public virtual void OnPickupUseDown()
    {
        triggerHeld = true;
    }

    public virtual void OnPickupUseUp()
    {
        //myFishRenderer.material.SetColor("_Color", Color.blue);
        triggerHeld = false;
    }
    //Fish Fights
    private void fightSmall()
    {
        timerDelay += 1;
        if(timerDelay>=timerDelayMax/2)
        {
            fishResistanceScore += 1;
            timerDelay = 0;
        }
        
        /*
        if(buildingFight)
        {
           EndFight(3f);
        }
        */
    }

    private void fleeSmall()
    {

    }


}
