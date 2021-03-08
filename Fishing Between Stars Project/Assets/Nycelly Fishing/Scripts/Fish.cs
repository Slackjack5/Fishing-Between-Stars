
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class Fish : UdonSharpBehaviour
{
    //Change Color
    public Renderer myFishRenderer;

    void Start()
    {


    }

    private void Update()
    {

    }

    //On Pickup un-parent the fish
    private void OnPickup()
    {
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
}
