
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
    [UdonSynced] public float resistanceScore = 0;
    public bool triggerHeld = false;
    public float reistanceRate = 0;
    private float reistanceDecay = 0;
    public float reistanceRateMax = 30;
    public float resistanceRuns = 0;

    //Fish
    [UdonSynced] public float fishResistanceScore = 0;
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
    //int randFight = Random.Range(0, 2);

    //Exhaustion
    [UdonSynced] public float exhaustionScore = 0;
    private float exhaustionDelay = 0;
    /*[UdonSynced]*/ public float exhaustionDelayMax = 10;

    //Gameobjects
    public GameObject myHook;

    //UI
    public RectTransform canvas;
    public RectTransform resistanceTarget;
    public RectTransform fish;
    private Vector3 startingPosition;
    public float speed;
    public Image fishExhaustionBar;
    public GameObject dangerRod;
    public GameObject leftArrow;
    public GameObject rightArrow;
    public GameObject exhaustionCompletetion;

    //New Variables
    public float deltaResistanceScore = 1f;
    public float deltaResistanceScoreMaxRise = 4f;
    public float deltaResistanceScoreMaxFall = 4f;
     private float fishMovement = 1;
   /*[UdonSynced]*/ public float fishMovementMax = 8;
    public GameObject velocityMeter;
    //Jerk Event
    [UdonSynced] public bool jerkEvent = false;
    /*[UdonSynced]*/ public int jerkNumber = 1000;
    /*[UdonSynced]*/ public int jerkAdder = 1000;

    public Animator myAnimator;
    //Failure Timer
    private int failureTimer = 0;
    public int failureTimerMax = 150;
    private bool failing = false;

    //Player
    public VRCPlayerApi player;

    //Animation
    public bool wasTouched = false;

    //
    private int animStopTimer = 0;
    public int animStopTimerMax = 60;

    //Mode
    [UdonSynced] public bool VR = false;

    //handle
    public bool handleBeingHeld=false;

    //Unsync Player
    public GameObject rodLever;

    //Sync
    public bool playerHolding = false;
    [UdonSynced] public int whichDirection = 5;

    //Vibration
    private int vibrationTimer=0;
    private int vibrationTimerMax = 15;

    //All Players Synced?
    public int syncTimer = 0;
    public int syncTimerMax = 100;
    void Start()
    {
        resistanceTarget = resistanceTarget.GetComponent<RectTransform>();
        fish = fish.GetComponent<RectTransform>();
        canvas = canvas.GetComponent<RectTransform>();
        startingPosition = resistanceTarget.transform.position;
        speed = .1f;
        player = Networking.LocalPlayer;
        
    }

    private void FixedUpdate()
    {


        if (playerHolding)
        {


            //If we are in Desktop
            if (VR == false)
            {

                //Player Resistance
                //If The Trigger is being held , increase player resistance
                if (triggerHeld)
                {

                    //For VR
                    //Slow down how fast we count player resistance
                    reistanceRate += 1;

                    if (reistanceRate >= reistanceRateMax)
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
                            if (deltaResistanceScore <= -1)
                            {
                                deltaResistanceScore /= 2;
                            }
                            else
                            {
                                deltaResistanceScore *= 2;
                            }

                        }
                    }

                    //For Desktop

                }
                else
                {

                    //Slow down how fast we decay player resistance
                    reistanceDecay += 1;
                    if (reistanceDecay >= reistanceRateMax / 2)
                    {
                        reistanceDecay = 0;

                        if (deltaResistanceScore == 1)
                        {
                            deltaResistanceScore *= -1;
                        }
                        else
                        {
                            if (deltaResistanceScore <= -1)
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


                //Animations
                if (triggerHeld)
                {

                    myAnimator.enabled = true;
                    myAnimator.SetBool("Reeling", true);
                    myAnimator.SetFloat("AnimSpeed", 1.5f);


                }
                else
                {

                    myAnimator.enabled = true;
                    myAnimator.SetFloat("AnimSpeed", .5f);
                    myAnimator.SetBool("Reeling", true);


                }

            }
            else //If in VR
            {
                //Disable Animations for VR
                myAnimator.enabled = false;

                if (handleBeingHeld == false)
                {
                    //Slow down how fast we decay player resistance
                    reistanceDecay += 1;
                    if (reistanceDecay >= reistanceRateMax / 2)
                    {
                        reistanceDecay = 0;
                        if (deltaResistanceScore == 0)
                        {
                            deltaResistanceScore = 1;
                        }

                        if (deltaResistanceScore == 1)
                        {
                            deltaResistanceScore *= -1;
                        }
                        else
                        {
                            if (deltaResistanceScore <= -1)
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
                else if (handleBeingHeld == true)
                {
                    deltaResistanceScore = 0;
                }

            }

            //Animations
            myAnimator.enabled = true;
            myAnimator.SetFloat("AnimSpeed", .5f);
            myAnimator.SetBool("Reeling", true);

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
            //Networking.SetOwner(player, gameObject);
            resistanceScore += deltaResistanceScore;


            //Resistance Calculator



            //Jerk Event
            if (exhaustionScore == jerkNumber)
            {
                //Set New Instance Owner
                Networking.SetOwner(player, gameObject);
                whichDirection = Random.Range(0, 2);
                //Turn on Jerk Event
                velocityMeter.GetComponent<Velocimeter>().jerkRod = true;
                Networking.SetOwner(player, gameObject);
                jerkEvent = true;
                if (whichDirection == 0)
                {
                    velocityMeter.GetComponent<Velocimeter>().jerkLeft = true;
                }
                else
                {
                    velocityMeter.GetComponent<Velocimeter>().jerkRight = true;
                }
                //Add one exhuastion so previous code only occurs once
                Networking.SetOwner(player, gameObject);
                exhaustionScore += 1;
                //Networking.SetOwner(player, gameObject);
                jerkNumber += jerkAdder;
            }
            //Vibration
            if (jerkEvent == true)
            {

                Networking.LocalPlayer.PlayHapticEventInHand(VRC_Pickup.PickupHand.Left, 0.1f, 30, 30);
            }
            if (jerkEvent == false)
            {
                velocityMeter.GetComponent<Velocimeter>().jerkLeft = false;
                velocityMeter.GetComponent<Velocimeter>().jerkRight = false;
            }
                //Fish Resistance
                if (hookBite == true)
            {
                //Decay Resistance if not currently fighting
                fishDecay += 1;
                if (fishDecay >= fishDecayMax)
                {
                    float newFishMovement = Random.Range(0, fishMovementMax);



                    if (fishMovement <= 0)
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
                //Networking.SetOwner(player, gameObject);
                fishResistanceScore += fishMovement;

                //When our resistance is close to fish resistance, give the fish exhaustion
                if (resistanceScore <= fishResistanceScore + 200)
                {
                    dangerRod.SetActive(false);
                    failing = false;
                    if (resistanceScore >= fishResistanceScore - 200)
                    {
                        failing = false;
                        vibrationTimer = 0;
                        //If We are in a jerk event, don't give exhastion and warn the player
                        if (jerkEvent == false)
                        {
                            dangerRod.SetActive(false);
                            exhaustionDelay += 1;
                            if (exhaustionDelay >= exhaustionDelayMax)
                            {
                                //Networking.SetOwner(player, gameObject);
                                Networking.SetOwner(player, gameObject);
                                exhaustionScore += 1;
                                exhaustionDelay = 0;
                            }
                        }
                        else
                        {
                            dangerRod.SetActive(true);
                        }

                    }
                    else
                    {
                        dangerRod.SetActive(true);
                        failing = true;
                        //Vibrate Controller
                        if (!jerkEvent)
                        {
                            if (vibrationTimer >= vibrationTimerMax)
                            {
                                //Vibrate Controller
                                Networking.LocalPlayer.PlayHapticEventInHand(VRC_Pickup.PickupHand.Left, 0.2f, 100, 100);
                                vibrationTimer = 0;
                            }
                            else
                            {
                                vibrationTimer += 1;
                            }
                        }
                    }
                }
                else
                {
                    dangerRod.SetActive(true);
                    failing = true;
                    //Vibrate Controller
                    if (!jerkEvent)
                    {
                        if (vibrationTimer >= vibrationTimerMax)
                        {
                            //Vibrate Controller
                            Networking.LocalPlayer.PlayHapticEventInHand(VRC_Pickup.PickupHand.Left, 0.2f, 100, 100);
                            vibrationTimer = 0;
                        }
                        else
                        {
                            vibrationTimer += 1;
                        }
                    }
                }
            }
            //Don't Let Values Drop Below 0
            if (resistanceScore < 0)
            {
                resistanceScore = 0;
                //Stop Animations
                myAnimator.enabled = false;
                myAnimator.SetBool("Reeling", false);
            }
            if (resistanceScore > 1000)
            {
                resistanceScore = 1000;
                //Stop Animations
                myAnimator.enabled = false;
                myAnimator.SetBool("Reeling", false);
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
            if (exhaustionScore >= 100)
            {
                Networking.SetOwner(player, gameObject);
                //Do Not Set Owner Because it will bug out fishing rod position
                if (syncTimer >= syncTimerMax)
                {
                    myHook.GetComponent<FishingCube>().fishExhausted = true;
                    //Ui
                    exhaustionCompletetion.SetActive(true);
                    syncTimer = 0;

                }
                else
                {
                    syncTimer += 1;
                }
                exhaustionScore = 100;
            }



            

            Vector3 r = player.GetTrackingData(VRCPlayerApi.TrackingDataType.RightHand).position - player.GetPosition();

        }
        else
        {

        }
        //Sync

        
        if (exhaustionScore >= 100)
        {
            if(playerHolding==false)
            {
                myHook.GetComponent<FishingCube>().fishExhausted = true;
                //Ui
                exhaustionCompletetion.SetActive(true);
                exhaustionScore = 100;
                syncTimer = 0;
            }
        }

        //Animations

        if (wasTouched)
        {

            myAnimator.enabled = true;
            myAnimator.SetBool("Reeling", true);
            myAnimator.SetFloat("AnimSpeed", 1.5f);

            if (animStopTimer >= animStopTimerMax)
            {
                //Stop Animations
                myAnimator.enabled = false;
                myAnimator.SetBool("Reeling", false);
                wasTouched = false;
                Debug.Log("Animations Stopped");
                animStopTimer = 0;

            }
            else
            {
                animStopTimer += 1;
            }
            //Animation["Reeling"].time = 5.0;

        }
        //Failing
        if (failing == true && myHook.GetComponent<FishingCube>().hookBite == true)
        {
            if (failureTimer >= failureTimerMax)
            {
                resetVariables();
                myHook.GetComponent<FishingCube>().softReset = true;
                failureTimer = 0;
            }
            failureTimer += 1;
        }
        else
        {
            failureTimer = 0;
        }
        //Ui
        float squarePosition = Mathf.Lerp(-40f, 40f, resistanceScore / 1000f);
        float fishPosition = Mathf.Lerp(-40f, 40f, fishResistanceScore / 1000f);
        resistanceTarget.anchoredPosition = new Vector3(0f, squarePosition, 0f);
        fish.anchoredPosition = new Vector3(0f, fishPosition, 0f);
        fishExhaustionBar.fillAmount = exhaustionScore / 100;

        if (jerkEvent == false)
        {
            leftArrow.SetActive(false);
            rightArrow.SetActive(false);
        }
        if(jerkEvent)
        {
            if (whichDirection == 0)
            {
                leftArrow.SetActive(true);
            }
            else
            {
                rightArrow.SetActive(true);
            }
        }

    }
    
    private void Update()
    {
        //Sync Inportant Variables

        //Convert Player Resistance to a seeable string
        playerResistance.text = "Resistance Score:" + " " + resistanceScore.ToString() + " Exhaustion Score:" + " " + exhaustionScore.ToString() + " FishMovement Max:" + " " + fishMovementMax.ToString() + " jerkEvent" + " " + jerkEvent.ToString() + " jerkNumber" + " " + jerkNumber.ToString() + " jerkAdder" + " " + jerkAdder.ToString() + " VR" + " " + VR.ToString()
            + " whichDirection" + " " + whichDirection.ToString() + " playerHolding" + " " + playerHolding.ToString();
        fishResistance.text = "In Water:" + " " + myHook.GetComponent<FishingCube>().inWater.ToString() + " hookBite:" + " " + myHook.GetComponent<FishingCube>().hookBite.ToString() + " fishExhausted:" + " " + myHook.GetComponent<FishingCube>().fishExhausted.ToString()
            + " fishSpawned:" + " " + myHook.GetComponent<FishingCube>().fishSpawned.ToString() + " testBool:" + " " + myHook.GetComponent<FishingCube>().testBool.ToString() + " tier1Fish:" + " " + myHook.GetComponent<FishingCube>().tier1Fish.ToString()
            + " tier2Fish:" + " " + myHook.GetComponent<FishingCube>().tier2Fish.ToString() + " tier3Fish:" + " " + myHook.GetComponent<FishingCube>().tier3Fish.ToString() + " fishPulledOut:" + " " + myHook.GetComponent<FishingCube>().fishPulledOut.ToString()
            + " HardReset:" + " " + myHook.GetComponent<FishingCube>().hardReset.ToString() + " SoftReset:" + " " + myHook.GetComponent<FishingCube>().softReset.ToString() + " Collecting Fish:" + " " + myHook.GetComponent<FishingCube>().collectingFish.ToString();
        //Convert Fish Resistance to a seeable string
        //fishResistance.text = "Fish:" + " " + fishResistanceScore.ToString();
        //Convert Fish Resistance to a seeable string
        //fishExhaustion.text = "Goal:" + " " + exhaustionScore.ToString();
        //Hold Fish in Place

    }
    public virtual void OnPickup() 
    {
       
        gameObject.GetComponent<SphereCollider>().enabled = false;

        //Set this as the player
        rodLever.GetComponent<LeverRotation>().player = Networking.LocalPlayer;
        player = Networking.LocalPlayer;
        playerHolding = true;
    }

    public virtual void OnDrop() 
    {
        gameObject.GetComponent<SphereCollider>().enabled = true;
        rodLever.GetComponent<LeverRotation>().player = null;
        playerHolding = false;
    }

    public virtual void OnPickupUseDown()
    {
        triggerHeld = true;
        //wasTouched = true;
    }

    public virtual void OnPickupUseUp()
    {
        //myFishRenderer.material.SetColor("_Color", Color.blue);
        triggerHeld = false;
    }

    public void resetVariables()
    {
        //Networking.SetOwner(player, gameObject);
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
         //randFight = Random.Range(0, 2);
    //Exhaustion
         exhaustionScore = 0;
         exhaustionDelay = 0;
        //Jerk Event
        jerkEvent = false;
        jerkNumber = 1000;
        jerkAdder = 1000;

        //Difficulty
        exhaustionDelayMax = 10;
        fishMovementMax = 5;
        //Ui
        exhaustionCompletetion.SetActive(false);

        //Failing
        failing = false;

        //Sync
        syncTimer = 0;
    }


    void stopAnimations()
    {
        //Stop Animations
        myAnimator.enabled = false;
        myAnimator.SetBool("Reeling", false);
        wasTouched = false;
        Debug.Log("Animations Stopped");
    }
}
