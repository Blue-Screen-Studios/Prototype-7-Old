using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

using Assembly.IBX.Main.Input;
using static UnityEngine.InputSystem.InputAction;

namespace Assembly.IBX.Main
{
    public class MovePlayer : MonoBehaviour
    {
        [SerializeField] private float moveForce;
        [SerializeField] private float maxVelocity;

        private Controls.GameActions gameControls;

        private void Awake()
        {
            gameControls = InputManager.GetGameActions();
        }

        private void FixedUpdate()
        {
            Vector2 direction =  gameControls.Move.ReadValue<Vector2>();
            Vector3 threeDimensionalDirection = new Vector3(direction.x, 0f, direction.y);

            transform.LookAt(threeDimensionalDirection, Vector3.up);

            Vector3 moveVector = threeDimensionalDirection;

            Rigidbody body = GetComponent<Rigidbody>();

            body.AddForce(moveVector * moveForce);
        }
    }
}
