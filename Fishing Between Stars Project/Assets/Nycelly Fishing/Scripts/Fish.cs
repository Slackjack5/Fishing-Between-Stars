
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
            
            transform.DetachChildren();
            gameObject.transform.parent = null;
            
        }
    }


    public virtual void OnPickup() 
    {
        //If your a fish , then reset the fishing rod if taken off the hook.
        onHook = false;
        gameObject.transform.parent.gameObject.GetComponent<FishingCube>().tier1Fish = false;
        gameObject.transform.parent.gameObject.GetComponent<FishingCube>().hardReset=true;
    }

    public virtual void OnPickupUseDown() 
    {
        myFishRenderer.material.SetColor("_Color", Color.green);
    }

    public virtual void OnPickupUseUp() 
    {
        myFishRenderer.material.SetColor("_Color", Color.blue);
    }

    public void detach()
    {
        Debug.Log("Detached Fish");

    }

    public void SendtoSpawn()
    {
        gameObject.transform.position = fishSpawn.transform.position;
        transform.DetachChildren();
        gameObject.transform.parent = null;
    }
}
