using UnityEngine;
using UnityEngine.InputSystem;

namespace Assembly.IBX.Main
{
    [RequireComponent(typeof(Rigidbody))]
    public class DroneController : MonoBehaviour
    {

        private Rigidbody rb;


        /*Speed*/
        public int ForwardBackward = 50;
        /*Speed*/
        public int Tilt = 50;
        /*Speed*/
        public int FlyLeftRight = 50;
        /*Speed*/
        public int UpDown = 50;

        [SerializeField] private bool enableDroneControl;

        private Vector3 DroneRotation;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
        }

        void FixedUpdate()
        {

            rb.AddForce(0, 9, 0);//drone not lose height very fast, if you want not to lose height al all change 9 into 9.80665 

            if (enableDroneControl == false) return;

            DroneRotation = rb.transform.localEulerAngles;
            if (DroneRotation.z > 10 && DroneRotation.z <= 180) { rb.AddRelativeTorque(0, 0, -10); }//if tilt too big(stabilizes drone on z-axis)
            if (DroneRotation.z > 180 && DroneRotation.z <= 350) { rb.AddRelativeTorque(0, 0, 10); }//if tilt too big(stabilizes drone on z-axis)
            if (DroneRotation.z > 1 && DroneRotation.z <= 10) { rb.AddRelativeTorque(0, 0, -3); }//if tilt not very big(stabilizes drone on z-axis)
            if (DroneRotation.z > 350 && DroneRotation.z < 359) { rb.AddRelativeTorque(0, 0, 3); }//if tilt not very big(stabilizes drone on z-axis)


            if (Keyboard.current.aKey.isPressed) { rb.AddRelativeTorque(0, Tilt / -1, 0); }//tilt drone left
            if (Keyboard.current.dKey.isPressed) { rb.AddRelativeTorque(0, Tilt, 0); }//tilt drone right


            if (DroneRotation.x > 10 && DroneRotation.x <= 180) { rb.AddRelativeTorque(-10, 0, 0); }//if tilt too big(stabilizes drone on x-axis)
            if (DroneRotation.x > 180 && DroneRotation.x <= 350) { rb.AddRelativeTorque(10, 0, 0); }//if tilt too big(stabilizes drone on x-axis)
            if (DroneRotation.x > 1 && DroneRotation.x <= 10) { rb.AddRelativeTorque(-3, 0, 0); }//if tilt not very big(stabilizes drone on x-axis)
            if (DroneRotation.x > 350 && DroneRotation.x < 359) { rb.AddRelativeTorque(3, 0, 0); }//if tilt not very big(stabilizes drone on x-axis)


            if (Keyboard.current.wKey.isPressed) { rb.AddRelativeForce(0, 0, ForwardBackward); rb.AddRelativeTorque(10, 0, 0); }//drone fly forward

            if (Keyboard.current.leftArrowKey.isPressed) { rb.AddRelativeForce(FlyLeftRight / -1, 0, 0); rb.AddRelativeTorque(0, 0, 10); }//rotate drone left

            if (Keyboard.current.rightArrowKey.isPressed) { rb.AddRelativeForce(FlyLeftRight, 0, 0); rb.AddRelativeTorque(0, 0, -10); }//rotate drone right

            if (Keyboard.current.sKey.isPressed) { rb.AddRelativeForce(0, 0, ForwardBackward / -1); rb.AddRelativeTorque(-10, 0, 0); }// drone fly backward

            if (Keyboard.current.upArrowKey.isPressed) { rb.AddRelativeForce(0, UpDown, 0); }//drone fly up

            if (Keyboard.current.downArrowKey.isPressed) { rb.AddRelativeForce(0, UpDown / -1, 0); }//drone fly down
        }
    }
}