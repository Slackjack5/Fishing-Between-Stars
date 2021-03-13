
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class Handle : UdonSharpBehaviour
{
    public GameObject handSpawn;
    private bool dropped = false;
    public bool handleHeld = false;
    void Start()
    {
        
    }

    public virtual void OnPickup() 
    { 
    
    }

    public virtual void OnDrop() 
    {
        Debug.Log("Dropping Handle");
        dropped = true;
        
    }

    public virtual void OnPickupUseDown()
    {
        handleHeld = true;
        gameObject.GetComponent<Renderer>().material.SetColor("_Color", Color.blue);
    }

    public virtual void OnPickupUseUp()
    {
        //myFishRenderer.material.SetColor("_Color", Color.blue);
        handleHeld = false;
        gameObject.GetComponent<Renderer>().material.SetColor("_Color", Color.white);
    }

    private void Update()
    {
        if(dropped)
        {
            gameObject.transform.position = handSpawn.transform.position;
            dropped = false;
        }
    }
}
