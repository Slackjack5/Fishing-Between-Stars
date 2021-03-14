
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class LeverRotation : UdonSharpBehaviour
{
    public VRCPlayerApi player;
    public Transform Target;
    public float RotationSpeed;

    //values for internal use
    private Quaternion _lookRotation;
    private Vector3 _direction;
    void Start()
    {
        player = null;
    }

    void FixedUpdate()
    {
        if(player!=null)
        {
            Vector3 r = player.GetTrackingData(VRCPlayerApi.TrackingDataType.RightHand).position;// - player.GetPosition();
            //public static Action<VRCPlayerApi, VRC_Pickup.PickupHand, float, float, float> _PlayHapticEventInHand;
            

            //find the vector pointing from our position to the target
            _direction = (r - transform.position).normalized;
            _direction.x = 0;
            //_direction.z = 0;
            //_direction.y = 0;
            //create the rotation we need to be in to look at the target
            _lookRotation = Quaternion.LookRotation(Quaternion.Euler(-90, 0, 0) * _direction * -1, Vector3.right);

            //rotate us over time according to speed until we are in the required rotation
            transform.rotation = Quaternion.Slerp(transform.rotation, _lookRotation, Time.deltaTime * RotationSpeed);
        }
       
    }
}
