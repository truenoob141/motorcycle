using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Motorcycle
{
    public class VehicleBehaviour : MonoBehaviour
    {
        [SerializeField] private float speed = 100;
        [SerializeField] private float carTorque = 10;
        [Header("Refs")] [SerializeField] private Rigidbody2D body;
        [SerializeField] private Rigidbody2D leftWheel;
        [SerializeField] private Rigidbody2D rightWheel;
        [SerializeField] private InputActionReference moveAction;

        private Transform camTransform;
        private Vector3 camOffset; 
        
        private float move;

        private void Start()
        {
            var cam = Camera.main;
            if (cam == null)
                throw new Exception("No camera");

            camTransform = cam.transform;
            camOffset = camTransform.position - transform.position;
        }

        private void OnEnable()
        {
            moveAction.action.performed += OnMoveAction;
        }

        private void Update()
        {
            camTransform.position = transform.position + camOffset;
        }

        private void FixedUpdate()
        {
            var torque = -move * speed * Time.fixedDeltaTime;
            leftWheel.AddTorque(torque);
            rightWheel.AddTorque(torque);
            
            body.AddTorque(-move * carTorque * Time.fixedDeltaTime);
        }

        private void OnMoveAction(InputAction.CallbackContext ctx)
        {
            var axis = ctx.ReadValue<Vector2>();
            move = axis.x;
        }
    }
}