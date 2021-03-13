
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

    void Start()
    {


    }

    private void Update()
    {

    }


    public virtual void OnPickup() 
    {
        //If your a fish , then reset the fishing rod if taken off the hook.

            gameObject.transform.parent.gameObject.GetComponent<FishingCube>().tier1Fish = false;
            gameObject.transform.parent.gameObject.GetComponent<FishingCube>().resetEverything();
            gameObject.transform.parent.gameObject.GetComponent<FishingCube>().fishPulledReset();
        
        
        transform.DetachChildren();
        gameObject.transform.parent = null;
        myFishRenderer.material.SetColor("_Color", Color.blue);
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
