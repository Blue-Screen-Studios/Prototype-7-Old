using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class DroneControlC : MonoBehaviour {

		
	public float movmentX = 50; 
	public float Tilt = 50; 
	public float MovementZ = 50;  
	public float UpDown = 50;

	[SerializeField] private float appliedTorque;
	[SerializeField] private float stableizationRetaliationForceMajor = 10;
	[SerializeField] private float stableizationRetaliationForceMinor = 3;

	private Vector3 droneRotation;

	private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

	void FixedUpdate() {
		droneRotation = rb.transform.localEulerAngles;

		if (droneRotation.z > 10 && droneRotation.z <= 180) {rb.AddRelativeTorque (0, 0, -stableizationRetaliationForceMajor);}//if tilt too big(stabilizes drone on z-axis)
		if(droneRotation.z>180 && droneRotation.z<=350){rb.AddRelativeTorque (0, 0, stableizationRetaliationForceMajor);}//if tilt too big(stabilizes drone on z-axis)
		if(droneRotation.z>1 && droneRotation.z<=10){rb.AddRelativeTorque (0, 0, -stableizationRetaliationForceMinor);}//if tilt not very big(stabilizes drone on z-axis)
		if(droneRotation.z>350 && droneRotation.z<359){rb.AddRelativeTorque (0, 0, stableizationRetaliationForceMinor);}//if tilt not very big(stabilizes drone on z-axis)


		if(Keyboard.current.aKey.isPressed) {rb.AddRelativeTorque(0,-appliedTorque / 10,0);}//tilt drone left
		if(Keyboard.current.dKey.isPressed) {rb.AddRelativeTorque(0,appliedTorque / 10,0);}//tilt drone right
		

		if(droneRotation.x>10 && droneRotation.x<=180){rb.AddRelativeTorque (-stableizationRetaliationForceMajor, 0, 0);}//if tilt too big(stabilizes drone on x-axis)
		if(droneRotation.x>180 && droneRotation.x<=350){rb.AddRelativeTorque (stableizationRetaliationForceMajor, 0, 0);}//if tilt too big(stabilizes drone on x-axis)
		if(droneRotation.x>1 && droneRotation.x<=10){rb.AddRelativeTorque (-stableizationRetaliationForceMinor, 0, 0);}//if tilt not very big(stabilizes drone on x-axis)
		if(droneRotation.x>350 && droneRotation.x<359){rb.AddRelativeTorque (stableizationRetaliationForceMinor, 0, 0);}//if tilt not very big(stabilizes drone on x-axis)


		rb.AddForce(0,9,0);//drone not lose height very fast, if you want not to lose height al all change 9 into 9.80665 
		

		if(Keyboard.current.wKey.isPressed){rb.AddRelativeForce(0,0,movmentX);rb.AddRelativeTorque (appliedTorque, 0, 0);}//drone fly forward

		if(Keyboard.current.leftArrowKey.isPressed){rb.AddRelativeForce(MovementZ/-1,0,0);rb.AddRelativeTorque (0, 0, appliedTorque);}//rotate drone left

		if(Keyboard.current.rightArrowKey.isPressed){rb.AddRelativeForce(MovementZ,0,0);rb.AddRelativeTorque (0, 0, -appliedTorque);}//rotate drone right

		if(Keyboard.current.sKey.isPressed){rb.AddRelativeForce(0,0,movmentX/-1);rb.AddRelativeTorque (-appliedTorque, 0, 0);}// drone fly backward

		if(Keyboard.current.upArrowKey.isPressed){rb.AddRelativeForce(0,UpDown,0);}//drone fly up

		if(Keyboard.current.downArrowKey.isPressed){rb.AddRelativeForce(0,UpDown/-1,0);}//drone fly down
	}

}