using Futurift.DataSenders;
using Futurift.Options;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


namespace Futurift
{
    public class SimpleController : MonoBehaviour
    {
        [SerializeField] private string ipAddress = "127.0.0.1";
        [SerializeField] private int port = 6065;

        

        private FutuRiftController _controller;

        private void Awake()
        {
            var udpOptions = new UdpOptions
            {
                ip = ipAddress,
                port = port
            };

            _controller = new FutuRiftController(new UdpPortSender(udpOptions));
        }

        [Header("Tuning")]
        [Tooltip("Максимальный угол наклона по тангажу и крену, при котором капсула достигает максимального отклонения")]
        public float maxAngle = 30f;
        [Tooltip("Чувствительность капсулы к наклонам")]
        public float sensitivity = 6f;
        [Tooltip("Порог турбулентности (изменения скорости углов) для вибрации)")]
        public float turbulenceThreshold = 10f;
        [Tooltip("Максимальная сила вибрации")]
        public float maxVibration = 1.0f;
        private Vector3 lastRotation;

        private float lastSpeed;
        private float accelPitchOffset;
        private float smoothedAccelTilt;

        public float accelerationSensitivity = 1.5f; //0.6f;
        public float maxAccelerationTilt = 12f; //6f;
        [Range(0f, 1f)] public float accelerationSmooth = 0.2f;  // 0.8f;

        public float collisionTiltForce = 18f; //8f;  //сила(угол) наклона капсулы при столкновении (вперед)
        public float collisionRecoverySpeed = 4f;
        public float collisionThreshold = 1.0f; //минимальная сила столкновения

        private float collisionTilt;
        private float targetCollisionTilt;

        private void Update()
        {

            var euler = transform.eulerAngles;
            var rot = new Vector3(
                euler.x > 180 ? euler.x - 360 : euler.x,
                euler.y,
                euler.z > 180 ? euler.z - 360 : euler.z
            );

            // Ограничение и сглаживание по pitch 
            if (Math.Abs(rot.x) > maxAngle)
            {
                if (rot.x < -maxAngle - 5f)
                    rot.x = -1 * (maxAngle - (Math.Abs(rot.x) / sensitivity));
                else if (rot.x > maxAngle + 5f)
                    rot.x = maxAngle - (rot.x / sensitivity);
                else
                    rot.x = Mathf.Clamp(rot.x, -maxAngle, maxAngle);
            }

            // Ограничение по roll 
            if (Math.Abs(rot.z) > maxAngle)
            {
                if (rot.z < -maxAngle - 5f)
                    rot.z = -1 * (maxAngle - (Math.Abs(rot.z) / sensitivity));
                else if (rot.z > maxAngle + 5f)
                    rot.z = maxAngle - (rot.z / sensitivity);
                else
                    rot.z = Mathf.Clamp(rot.z, -maxAngle, maxAngle);
            }

            // === вычисление ускорения ===
            float currentSpeed = GetComponent<Rigidbody>() ? GetComponent<Rigidbody>().velocity.magnitude : 0f;
            float acceleration = (currentSpeed - lastSpeed) / Time.deltaTime;
            lastSpeed = currentSpeed;

            float accelTilt = Mathf.Clamp(-Mathf.Pow(Mathf.Abs(acceleration), 0.9f) * Mathf.Sign(acceleration) * accelerationSensitivity, -maxAccelerationTilt, maxAccelerationTilt);

            float accelChange = Mathf.Abs(accelTilt - smoothedAccelTilt);
            if (accelChange > 0.1f)   //ускорение резко изменилось 
            {
                smoothedAccelTilt = Mathf.Lerp(smoothedAccelTilt, accelTilt, 0.5f);
            }
            else
            {
                smoothedAccelTilt = Mathf.Lerp(smoothedAccelTilt, accelTilt, 1f - accelerationSmooth);
            }

            //столкновение
            collisionTilt = Mathf.Lerp(collisionTilt, targetCollisionTilt, Time.deltaTime * collisionRecoverySpeed);
            if (Mathf.Abs(targetCollisionTilt) > 0.01f && Mathf.Abs(collisionTilt - targetCollisionTilt) < 0.1f)
                targetCollisionTilt = 0f;

            //угловая скорость
            var rb = GetComponent<Rigidbody>();
            if (rb != null)
            {
                Vector3 angularVelDeg = rb.angularVelocity * Mathf.Rad2Deg;

                float rollFromAngular = Mathf.Clamp(angularVelDeg.z / sensitivity, -maxAngle, maxAngle);
                float pitchFromAngular = Mathf.Clamp(-angularVelDeg.x / sensitivity, -maxAngle, maxAngle);

                rot.z += rollFromAngular * Time.deltaTime;
                rot.x += pitchFromAngular * Time.deltaTime;


            }


            float totalPitch = -rot.x - smoothedAccelTilt + collisionTilt;
            //float totalPitch = -rot.x - smoothedAccelTilt;
            //float totalPitch = -rot.x + smoothedAccelTilt;
            // === отклонение капсулы при ускорении ===
            //accelPitchOffset = Mathf.Lerp(accelPitchOffset, Mathf.Clamp(-acceleration * 0.2f, -10f, 10f), 0.1f);
            //float finalPitch = Mathf.Clamp(-rot.x + accelPitchOffset, -maxAngle, maxAngle);


            //  Отправляем данные в капсулу 
            _controller.Pitch = totalPitch;

            //  Отправляем данные в капсулу 
            //_controller.Pitch = -rot.x; //_controller.Pitch = -rot.x;
            _controller.Roll = rot.z;



            lastRotation = rot;


            //var euler = transform.eulerAngles;
            //var rot = transform.eulerAngles; ;
            //rot.x = euler.x > 180 ? euler.x - 360 : euler.x;
            //rot.z = euler.z > 180 ? euler.z - 360 : euler.z;
            //if (Math.Abs(rot.x) > 30f)
            //{
            //    if (rot.x < -35f)
            //    {
            //        rot.x = -1 * (30 - (Math.Abs(rot.x) / 6));
            //    }
            //    else if (rot.x > 35f)
            //    {
            //        rot.x = 30 - (rot.x / 6);
            //    }
            //    else if (rot.x < -30f)
            //    {
            //        rot.x = -30;
            //    }
            //    else if (rot.x > 30f)
            //    {
            //        rot.x = 30;
            //    }

            //}
            //if (Math.Abs(rot.z) > 30f)
            //{
            //    if (rot.z < -35f)
            //    {
            //        rot.z = -1 * (30 - (Math.Abs(rot.z) / 6));
            //    }
            //    else if (rot.z > 35f)
            //    {
            //        rot.z = 30 - (rot.z / 6);
            //    }
            //    else if (rot.z < -30f)
            //    {
            //        rot.z = -30;
            //    }
            //    else if (rot.z > 30f)
            //    {
            //        rot.z = 30;
            //    }

            //}
            //_controller.Pitch = rot.x;
            //_controller.Roll = rot.z;


            //var euler = transform.eulerAngles;
            //_controller.Pitch = (euler.x > 180 ? euler.x - 360 : euler.x);
            //_controller.Roll = (euler.z > 180 ? euler.z - 360 : euler.z);
        }

        //реакция на столкновение 
        private void OnCollisionEnter(Collision collision)
        {
            float impactForce = collision.relativeVelocity.magnitude;
            Debug.Log($"Столкновение с объектом: {collision.gameObject.name}, сила удара: {impactForce:F2}");

            targetCollisionTilt = -collisionTiltForce;
            Debug.Log($"Реакция на столкновение активирована, наклон: {targetCollisionTilt:F2}");

            //if (impactForce > collisionThreshold) 
            //{
            //    targetCollisionTilt = -Mathf.Clamp(impactForce, 0f, 10f) / 10f * collisionTiltForce;
            //    Debug.Log($"Реакция на столкновение активирована, наклон: {targetCollisionTilt:F2}");
            //}
        }


        private void OnEnable()
        {
            _controller?.Start();
        }

        private void OnDisable()
        {
            _controller?.Stop();
        }
    }
}