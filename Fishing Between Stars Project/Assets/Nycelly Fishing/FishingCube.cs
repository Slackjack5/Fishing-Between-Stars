
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class FishingCube : UdonSharpBehaviour
{
    void Start()
    {
        
    }

    private void Update()
    {
        transform.Rotate(Vector3.up, 90f * Time.deltaTime);
    }
}
