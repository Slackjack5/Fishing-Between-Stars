
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class Fish : UdonSharpBehaviour
{
    //Change Color
    public Renderer myFishRenderer;
    public bool tier1Fishy;
    public bool tier2Fishy;
    public bool tier3Fishy;
    public bool tier4Fishy;
    public bool variantFish;
    public bool corruptedFish;
    public bool crimsonFish;

    public Transform fishSpawn;
    //Game Object
    public GameObject myHook;
    [UdonSynced] public bool onHook = false;
    [UdonSynced] public bool fishTouched = false;
    public float fishCharge = 0;
    [UdonSynced] public bool fishCaptured = false;
    [UdonSynced] public bool toSpawn = false;
    public bool triggerHeld=false;
    
    void Start()
    {


    }

    private void Update()
    {
        /*
        if(onHook)
        {
            gameObject.transform.position = myHook.transform.position;
        }
        else
        {
            
            transform.DetachChildren();
            gameObject.transform.parent = null;
            //gameObject.transform.position = fishSpawn.transform.position;
        }
        */
        if(toSpawn)
        {
            SendtoSpawn();
            toSpawn = false;
        }

        if(triggerHeld)
        {
            fishCharge += 1;
        }

        if(fishCaptured)
        {

            Networking.SetOwner(Networking.LocalPlayer, gameObject);
            Networking.SetOwner(Networking.LocalPlayer, myHook);
                fishCharge = 0;
            fishCaptured = false;
                SendtoSpawn();
            
        }
    }


    public virtual void OnPickup() 
    {
        Networking.SetOwner(Networking.LocalPlayer, gameObject);
        Networking.SetOwner(Networking.LocalPlayer, myHook);
        myHook.GetComponentInParent<FishingCube>().hardReset = true;
        onHook = false;
        myFishRenderer.material.SetColor("_Color", Color.white);
        gameObject.SetActive(false);
    }
    public virtual void OnDrop() 
    {
        myFishRenderer.material.SetColor("_Color", Color.white);
        gameObject.SetActive(false);
        if (fishCharge >= 100)
        {
            Networking.SetOwner(Networking.LocalPlayer, gameObject);
            fishCaptured = true;
        }

    }
    /*
    public virtual void Interact() 
    {
        Networking.SetOwner(Networking.LocalPlayer, gameObject);
        fishTouched = true;
    }
    */
    public virtual void OnPickupUseDown() 
    {
        //myFishRenderer.material.SetColor("_Color", Color.green);
        
        triggerHeld = true;

    }

    public virtual void OnPickupUseUp() 
    {
        // myFishRenderer.material.SetColor("_Color", Color.blue);
        //myFishRenderer.material.SetColor("_Color", Color.white);
        triggerHeld = false;

    }

    public void detach()
    {
        Debug.Log("Detached Fish");

    }

    public void SendtoSpawn()
    {
        Debug.Log("Send to Spawn");
        onHook = false;
        gameObject.transform.position = fishSpawn.transform.position;
        transform.DetachChildren();
        gameObject.transform.parent = null;
    }
}
