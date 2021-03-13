
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class TouchTest : UdonSharpBehaviour
{

    private VRCPlayerApi player;
    public GameObject testBall;
    public float range = 50;
    public Renderer myRenderer;

    void Start()
    {
        player = Networking.LocalPlayer;
        Debug.Log(player);
    }

    private void Update()
    {
        Vector3 r = player.GetTrackingData(VRCPlayerApi.TrackingDataType.RightHand).position;// - player.GetPosition();
        Vector3 l = player.GetTrackingData(VRCPlayerApi.TrackingDataType.LeftHand).position;

        Debug.Log("Right Hand:" + " " + r);
        Debug.Log("Left Hand:" + " " + l);

        if (Vector3.Distance(r, transform.position) <= range)
        {
            Debug.Log("Player in Range");
            myRenderer.material.SetColor("_Color", Color.blue);
        }
        else
        {
            myRenderer.material.SetColor("_Color", Color.white);

        }


    }

}
