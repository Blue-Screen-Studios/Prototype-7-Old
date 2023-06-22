using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

using Assembly.IBX.Main.Input;
using static UnityEngine.InputSystem.InputAction;
using UnityEngine.UIElements;

namespace Assembly.IBX.Main
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private float maxThrottleVelocity;
        [SerializeField] private float throttleForceMultiplier;
        [SerializeField] private float maxRotationalVelocity;

        [SerializeField] private float tiltMultiplier;
        [SerializeField] private float tiltLimit;

        [SerializeField] private Transform[] props;

        private Controls.GameActions gameControls;

        private void Awake()
        {
            gameControls = InputManager.GetGameActions();
        }
        private void FixedUpdate()
        {
            float throttleAxis = gameControls.Throttle.ReadValue<float>();

            Rigidbody rb = GetComponent<Rigidbody>();

            if(rb.velocity.y < 0)
            {
                rb.AddForce(Vector3.up * Physics.gravity.magnitude, ForceMode.Force);
            }

            rb.AddForce(transform.up * throttleForceMultiplier * throttleAxis, ForceMode.Force);

            Vector2 tiltDirection = gameControls.Tilt.ReadValue<Vector2>();

            float angleX = transform.localEulerAngles.x;
            angleX = (angleX > 180) ? angleX - 360 : angleX;

            float angleZ = transform.localEulerAngles.z;
            angleZ = (angleZ > 180) ? angleZ - 360 : angleZ;

            //Debug.Log($"{(int)angleX}, {(int)angleZ}");

            bool exceedsPositiveTiltLimitX = angleX > tiltLimit;
            bool exceedsNegativeTiltLimitX = angleX < -tiltLimit;
            bool exceedsPositiveTiltLimitZ = angleZ > tiltLimit;
            bool exceedsNegativeTiltLimitZ = angleZ < -tiltLimit;

            Debug.ClearDeveloperConsole();
            
            float tiltZ = tiltDirection.x;
            float tiltX = tiltDirection.y;

            if(exceedsNegativeTiltLimitX)
            {
                Debug.Log("Exceeds negative X");
                rb.angularVelocity = Vector3.Lerp(rb.angularVelocity, Vector3.zero, .25f);
            }

            if(exceedsPositiveTiltLimitX)
            {
                Debug.Log("Exceeds positive X");
                rb.angularVelocity = Vector3.Lerp(rb.angularVelocity, Vector3.zero, .25f);
            }

            if(exceedsNegativeTiltLimitZ)
            {

                Debug.Log("Exceeds negative Z");
                rb.angularVelocity = Vector3.Lerp(rb.angularVelocity, Vector3.zero, .25f);
            }

            if(exceedsPositiveTiltLimitZ)
            {

                Debug.Log("Exceeds positive Z");
                rb.angularVelocity = Vector3.Lerp(rb.angularVelocity, Vector3.zero, .25f);
            }

            //Debug.Log($"{tiltX}, {tiltZ}");
            
            if(tiltX > 0)
            {
                float xAxisForce = Mathf.Abs(tiltX);

                rb.AddForceAtPosition(xAxisForce * transform.up * tiltMultiplier, props[0].localPosition);
                rb.AddForceAtPosition(xAxisForce * transform.up * tiltMultiplier, props[2].localPosition);

                Debug.DrawLine(props[0].transform.localPosition, props[0].transform.up + Vector3.up, Color.magenta);
                Debug.DrawLine(props[2].transform.localPosition, props[2].transform.up + Vector3.up, Color.magenta);
            }

            if (rb.velocity.magnitude < maxThrottleVelocity)
            {
                foreach (Transform prop in props)
                {
                    rb.AddForceAtPosition(throttleAxis * Vector3.up, prop.localPosition, ForceMode.Force);
                }
            }

            //transform.eulerAngles = new Vector3(tiltDirection.x * tiltMultiplier, 0f, tiltDirection.y * tiltMultiplier);
        }

        
        private void temp()
        {
            /*
             * Vector2 direction =  gameControls.Move.ReadValue<Vector2>();
            Vector3 threeDimensionalDirection = new Vector3(direction.x, 0f, direction.y);

            transform.LookAt(threeDimensionalDirection, Vector3.up);

            Vector3 moveVector = threeDimensionalDirection;

            Rigidbody body = GetComponent<Rigidbody>();

            if(body.velocity.magnitude < maxVelocity)
            {
                body.AddForce(moveVector * moveForce);
            }
            */
        }
    }
}
