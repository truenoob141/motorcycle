using System;
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

        public float Distance { get; private set; }

        public event Action onRestart;

        private const float fallHeight = 6;
        
        private Transform camTransform;
        private Vector3 camOffset;

        private Vector3 spawnPos;
        private Vector3 lastPos;
        private int moveDir;

        private readonly ContactPoint2D[] contacts = new ContactPoint2D[1];

        private void Start()
        {
            var cam = Camera.main;
            if (cam == null)
                throw new Exception("No camera");

            var position = transform.position;
            spawnPos = position;
            camTransform = cam.transform;
            camOffset = camTransform.position - position;
        }

        private void Update()
        {
            var pos = transform.position;
            camTransform.position = pos + camOffset;

            Distance += (pos - lastPos).magnitude;
            lastPos = pos;

            if (pos.y < -fallHeight)
                Restart();
        }

        private void FixedUpdate()
        {
            var torque = -moveDir * speed * Time.fixedDeltaTime;
            leftWheel.AddTorque(torque);
            rightWheel.AddTorque(torque);
            
            body.AddTorque(-moveDir * carTorque * Time.fixedDeltaTime);
        }

        public void EnableMove(bool isRight)
        {
            moveDir = isRight ? 1 : -1;
        }

        public void DisableMove()
        {
            moveDir = 0;
        }

        public bool IsLeftContactRightNot()
        {
            return leftWheel.GetContacts(contacts) > 0 && rightWheel.GetContacts(contacts) == 0;
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            Restart();
        }

        private void Restart()
        {
            Debug.Log("Restart");
            var trans = this.transform;
            trans.position = spawnPos;
            trans.rotation = Quaternion.identity;

            leftWheel.Reset();
            rightWheel.Reset();
            body.Reset();
            
            Distance = 0;
            lastPos = spawnPos;

            onRestart?.Invoke();
        }
    }
}