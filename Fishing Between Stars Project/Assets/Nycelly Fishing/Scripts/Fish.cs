
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
    
    void Start()
    {


    }

    private void Update()
    {
        
        if(onHook)
        {
            gameObject.transform.position = myHook.transform.position;
        }
        else
        {
            
            //transform.DetachChildren();
            //gameObject.transform.parent = null;
            //gameObject.transform.position = fishSpawn.transform.position;
        }
        
        if(fishTouched)
        {
            myFishRenderer.material.SetColor("_Color", Color.green);
            //gameObject.GetComponentInParent<FishingCube>().tier1Fish = false;
            //gameObject.GetComponentInParent<FishingCube>().tier2Fish = false;
            //gameObject.GetComponentInParent<FishingCube>().hardReset = true;
            fishTouched = false;
            //gameObject.SetActive(false);
        }
    }


    public virtual void OnPickup() 
    {
        //If your a fish , then reset the fishing rod if taken off the hook.
        //fishTouched = true;

    }
    /*
    public virtual void Interact() 
    {
        
    }
    */
    public virtual void OnPickupUseDown() 
    {
        //myFishRenderer.material.SetColor("_Color", Color.green);
    }

    public virtual void OnPickupUseUp() 
    {
       // myFishRenderer.material.SetColor("_Color", Color.blue);
    }

    public void detach()
    {
        Debug.Log("Detached Fish");

    }

    public void SendtoSpawn()
    {
        Debug.Log("Send to Spawn");
        gameObject.transform.position = fishSpawn.transform.position;
        transform.DetachChildren();
        gameObject.transform.parent = null;
    }
}
