using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class XRPlane : MonoBehaviour
{
    public float pitchSensitivity = 30f;
    public float rollSensitivity = 30f;
    public float yawSensitivity = 20f;
    public float thrustPower = 50f;
    public float maxSpeed = 100f;
    public float acceleration = 10f;
    public float moveSpeed;

    public float boostMultiplier = 2f;
    public float boostDuration = 2f;

    private Rigidbody rb;
    private Vector2 moveInput;
    private Vector2 yawInput;
    //private bool isFiring;
    private bool isBoosting;
    private float boostTimer;
    private float currentSpeed;

    //private bool engineStarted = false; // Добавляем флаг запуска двигателя



    //[SerializeField] private float lift = 135f;
    //[SerializeField] private float maxThrust = 200f;
    //private float throttle;
    //[SerializeField] private float throttleIncrement = 0.1f;


    public float verticalSpeed = 20f;
    public float verticalAcceleration = 5f;
    public float maxVerticalSpeed = 30f;
    private float currentVerticalSpeed;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void OnMoveInput(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();

    }
    public void OnYawInput(InputAction.CallbackContext context)
    {
        yawInput = context.ReadValue<Vector2>();

    }
    // Добавляем метод для запуска/остановки двигателя
    //public void OnEngineToggle(InputAction.CallbackContext context)
    //{
    //    if (context.performed)
    //    {
    //        engineStarted = !engineStarted; // Переключаем состояние двигателя
    //        if (!engineStarted)
    //        {
    //            currentSpeed = 0f; // При выключении сбрасываем скорость
    //            rb.velocity = Vector3.zero; // Останавливаем самолет
    //        }
    //    }
    //}
    public void OnBoostInput(InputAction.CallbackContext context)
    {
        if (context.performed && !isBoosting)
        {
            isBoosting = true;
            boostTimer = boostDuration;
        }
    }
    //public void OnThrottleIncrease(InputAction.CallbackContext context) //type - button
    //{
    //    if (context.performed)
    //    {
    //        throttle += throttleIncrement;
    //        throttle = Mathf.Clamp(throttle, 0f, 1f);



    //    }
    //}

    //public void OnThrottleDecrease(InputAction.CallbackContext context)  //type - button
    //{
    //    if (context.performed)
    //    {
    //        throttle -= throttleIncrement;
    //        throttle = Mathf.Clamp(throttle, 0f, 1f);

    //    }
    //}

    void FixedUpdate()
    {
        float pitch = -moveInput.y * pitchSensitivity * Time.fixedDeltaTime; //наклон вверх,вниз
        float roll = -moveInput.x * rollSensitivity * Time.fixedDeltaTime; // (наклон крыльев)
        float yaw = yawInput.x * yawSensitivity * Time.fixedDeltaTime; //Поворот самолета вокруг вертикальной оси
        Quaternion rotationChange = Quaternion.Euler(pitch, yaw, roll);
        rb.MoveRotation(rb.rotation * rotationChange);

        //rb.AddForce(transform.up * maxThrust * throttle);
        //rb.AddForce(Vector3.up * rb.velocity.magnitude * lift);

        if (isBoosting)
        {
            boostTimer -= Time.fixedDeltaTime;
            if (boostTimer <= 0f)
            {
                isBoosting = false;
            }
        }

        //if (engineStarted)
        //{
        //    float effectiveMaxSpeed = isBoosting ? maxSpeed * boostMultiplier : maxSpeed;

        //    // Регулировка скорости вперед (yawInput.y от -1 до 1)
        //    // -1 = медленно, 0 = нормально, +1 = быстро
        //    float speedMultiplier = Mathf.Clamp(yawInput.y + 1f, 0f, 2f) * 0.5f;
        //    float targetSpeed = speedMultiplier * effectiveMaxSpeed;

        //    currentSpeed = Mathf.MoveTowards(currentSpeed, targetSpeed, acceleration * Time.fixedDeltaTime);
        //    rb.velocity = transform.forward * currentSpeed;
        //}
        //else
        //{
        //    // Двигатель выключен - останавливаем самолет
        //    rb.velocity = Vector3.zero;
        //    currentSpeed = 0f;
        //}




        float effectiveMaxSpeed = isBoosting ? maxSpeed * boostMultiplier : maxSpeed;  //*

        float targetSpeed = Mathf.Clamp(yawInput.y, -1f, 1f) * maxSpeed;  //* (ось X правого стика геймпада или стрелки влево/вправо)
        currentSpeed = Mathf.MoveTowards(currentSpeed, targetSpeed, acceleration * Time.fixedDeltaTime); //*




        //float targetVerticalSpeed = moveInput.y * maxVerticalSpeed;
        //currentVerticalSpeed = Mathf.MoveTowards(currentVerticalSpeed, targetVerticalSpeed, verticalAcceleration * Time.fixedDeltaTime);

        ////// Комбинируем горизонтальное и вертикальное движение
        //Vector3 forwardMovement = transform.forward * (currentSpeed * Time.fixedDeltaTime * thrustPower);
        //Vector3 verticalMovement = transform.up * (currentVerticalSpeed * Time.fixedDeltaTime);

        ////// Применяем общее движение
        //rb.velocity = forwardMovement + verticalMovement;
        rb.velocity = transform.forward * (currentSpeed * Time.fixedDeltaTime * thrustPower); //*

    }



    void Update()
    {

    }
}
