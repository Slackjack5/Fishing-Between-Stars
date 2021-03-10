
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;


public class ResistanceText : UdonSharpBehaviour
{

    //Text
    public Text playerResistance;
    public Text fishResistance;
    public Text fishExhaustion;

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
    public float fishDecayMax = 200;
    //Variables for Counting Resistance
    private float timerDelay = 0;
    public float timerDelayMax = 10;
    //Varaibles for Fights
    public float timeBetweenFights = 300;
    private float endFight = 0;
    public float fightDuration = 400;
    //Reactions
    public bool hookBite = false;
    private bool buildingFight=false;
    public int fightTimer = 0;

    //Types of fish
    int randFight = Random.Range(0, 2);

    //Exhaustion
    public float exhaustionScore = 0;
    private float exhaustionDelay = 0;
    public float exhaustionDelayMax = 10;

    //Gameobjects
    public GameObject myHook;

    //UI
    public RectTransform canvas;
    public RectTransform resistanceTarget;
    public RectTransform fish;
    private Vector3 startingPosition;
    public float speed;
    public Image fishExhaustionBar;

    //New Variables
    private float deltaResistanceScore = 1f;
    public float deltaResistanceScoreMaxRise = 4f;
    public float deltaResistanceScoreMaxFall = 4f;
    private float fishMovement = 1;
    public float fishMovementMax = 8;
    void Start()
    {
        resistanceTarget = resistanceTarget.GetComponent<RectTransform>();
        fish = fish.GetComponent<RectTransform>();
        canvas = canvas.GetComponent<RectTransform>();
        startingPosition = resistanceTarget.transform.position;
        speed = .1f;
    }

    private void FixedUpdate()
    {


        

        //Player Resistance
        //If The Trigger is being held , increase player resistance
        if (triggerHeld)
        {

            //Slow down how fast we count player resistance
            reistanceRate += 1;
            
            if (reistanceRate>= reistanceRateMax)
            {
                //Resistance
                reistanceRate = 0;

                //Go to positive if negative
                if (deltaResistanceScore == -1)
                {
                    deltaResistanceScore *= -1;
                }
                else
                {
                    if(deltaResistanceScore<=-1)
                    {
                        deltaResistanceScore /= 2;
                    }
                    else
                    {
                        deltaResistanceScore *= 2;
                    }
                    
                }
            }
        }
        else
        {
            //Slow down how fast we decay player resistance
            reistanceDecay += 1;
            if(reistanceDecay>= reistanceRateMax/2)
            {
                reistanceDecay = 0;

                if (deltaResistanceScore == 1)
                {
                    deltaResistanceScore *= -1;
                }
                else
                {
                    if(deltaResistanceScore<=-1)
                    {
                        deltaResistanceScore *= 2;
                    }
                    else
                    {
                        deltaResistanceScore /= 2;
                    }
                }
            }
        }

        //Set Maximum Values
        if (deltaResistanceScore >= deltaResistanceScoreMaxRise)
        {
            deltaResistanceScore = deltaResistanceScoreMaxRise;
        }
        else if (deltaResistanceScore <= -deltaResistanceScoreMaxFall)
        {
            deltaResistanceScore = -deltaResistanceScoreMaxFall;
        }
        //Update Resistance Score
        resistanceScore += deltaResistanceScore;
        
        //Resistance Calculator
        
        //Ui
        float squarePosition = Mathf.Lerp(-40f, 40f, resistanceScore / 1000f);
        float fishPosition = Mathf.Lerp(-40f, 40f, fishResistanceScore / 1000f);
        resistanceTarget.anchoredPosition = new Vector3(0f, squarePosition, 0f);
        fish.anchoredPosition = new Vector3(0f, fishPosition, 0f);
        fishExhaustionBar.fillAmount = exhaustionScore / 100;
        //Fish Resistance
        if (hookBite==true)
        {
                //Decay Resistance if not currently fighting
                fishDecay += 1;
                if (fishDecay >= fishDecayMax)
                {
                    float newFishMovement = Random.Range(0, fishMovementMax);
                    

                   
                    if(fishMovement<=0)
                    {
                        fishMovement = newFishMovement;
                    }
                    else
                    {
                        fishMovement = -newFishMovement;
                    }
                    fishDecay = 0;
                }
            
            //Move our fish
            fishResistanceScore += fishMovement;

            //When our resistance is close to fish resistance, give the fish exhaustion
            if (resistanceScore<= fishResistanceScore+200)
            {
                if(resistanceScore >= fishResistanceScore - 200)
                {
                    exhaustionDelay += 1;
                    if(exhaustionDelay>=exhaustionDelayMax)
                    {
                        exhaustionScore += 1;
                        exhaustionDelay = 0;
                    }
                }
            }
        }
        //Don't Let Values Drop Below 0
        if (resistanceScore < 0)
        {
            resistanceScore = 0;
        }
        if (resistanceScore > 1000)
        {
            resistanceScore = 1000;
        }
        if (fishResistanceScore < 0)
        {
            fishResistanceScore = 0;
        }
        if (fishResistanceScore > 1000)
        {
            fishResistanceScore = 1000;
        }
        //If we meet our goal, the fish is exhausted!
        if (exhaustionScore>=100)
        {
            myHook.GetComponent<FishingCube>().fishExhausted = true;
        }
    }
    private void Update()
    {
        //Convert Player Resistance to a seeable string
        playerResistance.text = "Me:" + " " + resistanceScore.ToString();
        //Convert Fish Resistance to a seeable string
        fishResistance.text = "Fish:" + " " + fishResistanceScore.ToString();
        //Convert Fish Resistance to a seeable string
        fishExhaustion.text = "Goal:" + " " + exhaustionScore.ToString();
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

    public void resetVariables()
    {
        //Player
         resistanceScore = 0;
         triggerHeld = false;
         reistanceRate = 0;
         reistanceDecay = 0;
    //Fish
         fishResistanceScore = 0;
         fishFight = true;
         fishFlee = false;
         fishType = 0;
         fishDecay = 0;
    //Variables for Counting Resistance
         timerDelay = 0;
    //Varaibles for Fights
         endFight = 0;
    //Reactions
         hookBite = false;
         buildingFight = false;
         fightTimer = 0;
    //Types of fish
         randFight = Random.Range(0, 2);
    //Exhaustion
         exhaustionScore = 0;
         exhaustionDelay = 0;
}
}
