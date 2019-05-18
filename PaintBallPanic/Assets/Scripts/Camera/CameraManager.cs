using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SA.TB
{
    public class CameraManager : MonoBehaviour
    {
        public float speed = 0.1f;
        public float rotationSpeed = 0.5f;

        public void Init()
        {

            Vector3 tp = GridBase.singleton.GetWorldCoordinatedFromNode(10,-3,2);
            transform.position = tp;

        }


        private void Update()
        {

            cameraMove();
            Rotation();
        }


        void cameraMove()
        {
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");

            Vector3 targetPosition = Vector3.zero;

            if (horizontal != 0)
                targetPosition += horizontal * transform.right;

            if (vertical != 0)
                targetPosition += vertical * transform.forward;

            transform.position += targetPosition * speed * Time.deltaTime;
        }


        void Rotation()
        {
            bool rotateLeft = Input.GetKey(KeyCode.Q);
            bool rotateRight = Input.GetKey(KeyCode.E);

           

            if(rotateLeft || rotateRight)
            {
                float value = rotationSpeed;

                if (rotateLeft)
                { value = -value; }

                Vector3 euler = transform.localEulerAngles;
                euler.y += value;
                transform.localEulerAngles = euler; 
              

            }

        }

        public static CameraManager singleton;

        private void Awake()
        {
            singleton = this; 
        }
    }
}
