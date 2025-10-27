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

    public float boostMultiplier = 5f;
    public float boostDuration = 5f;

    private Rigidbody rb;
    private Vector2 moveInput;
    private Vector2 yawInput;
    
    //private Vector2 pitchroll;
    //private bool isFiring;
    private bool isBoosting;
    private float boostTimer;
    private float currentSpeed;

    //private bool engineStarted = false; // ��������� ���� ������� ���������



    //[SerializeField] private float lift = 135f;
    //[SerializeField] private float maxThrust = 200f;
    //private float throttle;
    //[SerializeField] private float throttleIncrement = 0.1f;


    public float verticalSpeed = 20f;
    public float verticalAcceleration = 5f;
    public float maxVerticalSpeed = 30f;
    private float currentVerticalSpeed;

    [SerializeField] AudioSource engineAudio;
    [SerializeField] AudioSource boostAudio;

    public float minPitch = 0.8f;
    public float maxPitch = 2.0f;
    public float minVolume = 0.1f;
    public float maxVolume = 0.6f;



    [SerializeField] InputActionReference pitchroll;
    [SerializeField] InputActionReference yaw;
    [SerializeField] InputActionReference speed;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        if (engineAudio == null)
            engineAudio = GetComponent<AudioSource>();
        if (engineAudio != null)
        {
            engineAudio.loop = true;
            engineAudio.playOnAwake = true;
            if (!engineAudio.isPlaying) engineAudio.Play();
        }
    }

    void OnEnable()
    {
        pitchroll.action.Enable();
        yaw.action.Enable();
        speed.action.Enable();
    }

    void OnDisable()
    {
        pitchroll.action.Disable();
        yaw.action.Disable();
        speed.action.Disable();
    }

    public void Move1()
    {
        Debug.Log(pitchroll.action.ReadValue<Vector2>());
        moveInput = pitchroll.action.ReadValue<Vector2>();
        
    }

    public void Yaw()
    {
        Debug.Log(yaw.action.ReadValue<Vector2>());
        yawInput = yaw.action.ReadValue<Vector2>();
        
    }

    public void Speed()
    {
        if (speed.action.WasPerformedThisFrame() && !isBoosting)
        {
            isBoosting = true;
            boostTimer = boostDuration;
            Debug.Log("Boost activated!");
            if (boostAudio != null) boostAudio.Play();
        }
        // if (pitchroll.action.performed && !isBoosting)
        // {
        //     isBoosting = true;
        //     boostTimer = boostDuration;
        //     Debug.Log("Boost activated!");
        //
        // }
    }

    public void OnMoveInput(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
        Debug.Log("move!");
        

    }
    public void OnYawInput(InputAction.CallbackContext context)
    {
        yawInput = context.ReadValue<Vector2>();
        Debug.Log("yaw!");

    }
    // ��������� ����� ��� �������/��������� ���������
    //public void OnEngineToggle(InputAction.CallbackContext context)
    //{
    //    if (context.performed)
    //    {
    //        engineStarted = !engineStarted; // ����������� ��������� ���������
    //        if (!engineStarted)
    //        {
    //            currentSpeed = 0f; // ��� ���������� ���������� ��������
    //            rb.velocity = Vector3.zero; // ������������� �������
    //        }
    //    }
    //}
    public void OnBoostInput(InputAction.CallbackContext context)
    {
        if (context.performed && !isBoosting)
        {
            isBoosting = true;
            boostTimer = boostDuration;
            Debug.Log("Boost activated!");

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
        Move1();
        Yaw();
        Speed();
        float pitch = -moveInput.y * pitchSensitivity * Time.fixedDeltaTime; //������ �����,����
        float roll = -moveInput.x * rollSensitivity * Time.fixedDeltaTime; // (������ �������)
        float yaw = yawInput.x * yawSensitivity * Time.fixedDeltaTime; //������� �������� ������ ������������ ���
        Quaternion rotationChange = Quaternion.Euler(pitch, yaw, roll);
        rb.MoveRotation(rb.rotation * rotationChange);

        //rb.AddForce(transform.up * maxThrust * throttle);
        //rb.AddForce(Vector3.up * rb.velocity.magnitude * lift);

        //ускорение
        if (isBoosting)
        {
            boostTimer -= Time.fixedDeltaTime;
            if (boostTimer <= 0f)
            {
                isBoosting = false;
                Debug.Log("Boost ended!");
            }
        }

        //if (engineStarted)
        //{
        //    float effectiveMaxSpeed = isBoosting ? maxSpeed * boostMultiplier : maxSpeed;

        //    // ����������� �������� ������ (yawInput.y �� -1 �� 1)
        //    // -1 = ��������, 0 = ���������, +1 = ������
        //    float speedMultiplier = Mathf.Clamp(yawInput.y + 1f, 0f, 2f) * 0.5f;
        //    float targetSpeed = speedMultiplier * effectiveMaxSpeed;

        //    currentSpeed = Mathf.MoveTowards(currentSpeed, targetSpeed, acceleration * Time.fixedDeltaTime);
        //    rb.velocity = transform.forward * currentSpeed;
        //}
        //else
        //{
        //    // ��������� �������� - ������������� �������
        //    rb.velocity = Vector3.zero;
        //    currentSpeed = 0f;
        //}




        float effectiveMaxSpeed = isBoosting ? maxSpeed * boostMultiplier : maxSpeed;  //*
        float targetSpeed = Mathf.Clamp(yawInput.y, -1f, 1f) * effectiveMaxSpeed;

        //float targetSpeed = Mathf.Clamp(yawInput.y, -1f, 1f) * maxSpeed;  //* (��� X ������� ����� �������� ��� ������� �����/������)
        currentSpeed = Mathf.MoveTowards(currentSpeed, targetSpeed, acceleration * Time.fixedDeltaTime); //*




        //float targetVerticalSpeed = moveInput.y * maxVerticalSpeed;
        //currentVerticalSpeed = Mathf.MoveTowards(currentVerticalSpeed, targetVerticalSpeed, verticalAcceleration * Time.fixedDeltaTime);

        ////// ����������� �������������� � ������������ ��������
        //Vector3 forwardMovement = transform.forward * (currentSpeed * Time.fixedDeltaTime * thrustPower);
        //Vector3 verticalMovement = transform.up * (currentVerticalSpeed * Time.fixedDeltaTime);

        ////// ��������� ����� ��������
        //rb.velocity = forwardMovement + verticalMovement;
        rb.velocity = transform.forward * (currentSpeed * Time.fixedDeltaTime * thrustPower); //*

        if (engineAudio)
        {
            float t = Mathf.InverseLerp(-effectiveMaxSpeed, effectiveMaxSpeed, currentSpeed);
            engineAudio.pitch = Mathf.Lerp(minPitch, maxPitch, t);
            engineAudio.volume = Mathf.Lerp(minVolume, maxVolume, t);
        }

    }



    void Update()
    {

    }
}
