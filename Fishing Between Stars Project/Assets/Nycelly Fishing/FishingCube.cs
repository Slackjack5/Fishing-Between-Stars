
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class FishingCube : UdonSharpBehaviour
{
    public bool inWater = false;
    public Renderer myCubeRenderer;
    
    void Start()
    {
       
    }

    private void Update()
    {
        transform.Rotate(Vector3.up, 90f * Time.deltaTime);

        if (inWater==true)
        {
            myCubeRenderer.material.SetColor("_Color", Color.red);
        }

    }

    void OnCollisionEnter(Collision other)
    {
        //Change the Name to what the Water Is
        if(other.gameObject.name == "Test Water")
        {
            inWater = true;
            Debug.Log("In the Water");
        }

    }

    void OnCollisionExit(Collision other)
    {
        //Change the Name to what the Water Is
        if (other.gameObject.name == "Test Water")
        {
            inWater = false;
            Debug.Log("Out of Water");
        }

    }
}
