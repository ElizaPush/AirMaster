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

            //  Отправляем данные в капсулу 
            _controller.Pitch = -rot.x; //_controller.Pitch = -rot.x;
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