using System.Collections;
using System.Collections.Generic;
using Bhaptics.SDK2;
using UnityEngine;

public class BhapticsController : MonoBehaviour
{
    private XRPlane airplane;
    private Rigidbody rb;
    private Vector3 lastVelocity;

    [Header("Thresholds")]
    public float rollThreshold = 25f;
    public float accelThreshold = 5f;
    public float brakeThreshold = -5f;
    public float turnAngleThreshold = 20f;

    [Header("Collision Settings")]
    public float maxCollisionForce = 30f; // нормализация силы вибрации

    //флаги состояний
    private bool rollTriggered = false;
    private bool accelTriggered = false;
    
    private bool sharpTurnTriggered = false;
    private bool boostTriggered = false;
    private bool initialized = false; //чтобы не было вибрации сразу после старта
    private bool attackTriggered = false;


    public float landingVelocityThreshold = 8f; // Минимальная скорость падения для активации вибрации
    public float groundCheckDistance = 2f;
    public LayerMask groundLayer;
    private bool isGrounded = false;
    private bool wasGrounded = false;




    void Start()
    {
        airplane = GetComponent<XRPlane>();
        rb = GetComponent<Rigidbody>();
        lastVelocity = rb.velocity;

        Invoke(nameof(EnableDetection), 1f);
    }

    void EnableDetection() => initialized = true;

    void FixedUpdate()
    {
        if (!initialized || rb == null) return;

        Vector3 acceleration = (rb.velocity - lastVelocity) / Time.fixedDeltaTime;
        float accelMagnitude = acceleration.magnitude;
        float deltaSpeed = rb.velocity.magnitude - lastVelocity.magnitude;

        // === 1. Разгон ===
        if (accelMagnitude > accelThreshold)
        {
            if (!accelTriggered)
            {
                TriggerHaptic("acceleration");
                accelTriggered = true;
            }
        }
        else accelTriggered = false;

        //DetectLanding();

       

        // === 2. Наклон (roll) ===
        float rollAngle = transform.eulerAngles.z;
        if (rollAngle > 180f) rollAngle -= 360f;

        if (Mathf.Abs(rollAngle) > rollThreshold)
        {
            if (!rollTriggered)
            {
                Roll("roll");
                rollTriggered = true;
            }
        }
        else rollTriggered = false;

        // === 4. Резкий манёвр ===
        //float turnAngle = Vector3.Angle(lastVelocity, rb.velocity);
        //if (turnAngle > turnAngleThreshold && rb.velocity.magnitude > 5f)
        //{
        //    if (!sharpTurnTriggered)
        //    {
        //        TriggerHaptic("sharp_turn");
        //        sharpTurnTriggered = true;
        //    }
        //}
        //else sharpTurnTriggered = false;

        // === 3. Boost ===
        if (airplane != null && airplane.IsBoosting())
        {
            if (!boostTriggered)
            {
                GetStressed("boost");
                boostTriggered = true;
            }
        }
        else boostTriggered = false;

        //lastVelocity = rb.velocity;


        // 4. attack
        if (airplane != null && airplane.isAttacking)
        {
            if (!attackTriggered)
            {
                Fire("attack");
                attackTriggered = true;
            }
        }
        else
            attackTriggered = false;

        lastVelocity = rb.velocity;

    }

    // === 5/6. Столкновение и посадка===
    void OnCollisionEnter(Collision collision)
    {
        if (!initialized) return;

        
         float impactForce = collision.relativeVelocity.magnitude;

         float intensity = Mathf.Clamp(impactForce / maxCollisionForce, 0.1f, 1f);

         Connect("collision", intensity);

            
    }
    private void DetectLanding() 
    {
        wasGrounded = isGrounded;
        isGrounded = Physics.Raycast(transform.position, Vector3.down, groundCheckDistance, groundLayer);
        if (!wasGrounded && isGrounded)
        {
            float verticalSpeed = Mathf.Abs(rb.velocity.y);
            if (verticalSpeed > landingVelocityThreshold)
            {
                float intensity = Mathf.Clamp01(verticalSpeed / 20f);
                TriggerHaptic("landing", intensity);
                //Debug.Log($"Landing: {verticalSpeed:F2}, Intensity: {intensity: F2}");
            }
        }
    }

    private void TriggerHaptic(string eventName, float intensity = 1f)
    {
        intensity = Mathf.Clamp01(intensity);
        BhapticsLibrary.Play(
             "all");  //gloves jacket
        //BhapticsLibrary.Play(
        //      "dash",  
        //       0,
        //       1.0f,
        //       intensity * 1.5f,   
        //       0.0f,
        //      0.0f
        //   );
        //BhapticsLibrary.Play("second", intensity);
        //Debug.Log($"Haptic Triggered: {eventName} (intensity: {intensity:F2})");
        Debug.Log($"Haptic Triggered: {eventName}");
    }

    private void Roll(string eventName, float intensity = 1f)
    {
        intensity = Mathf.Clamp01(intensity);
        BhapticsLibrary.Play(
             "first");   //my gloves and jacket
        //BhapticsLibrary.Play(
        //      "dash",  
        //       0,
        //       1.0f,
        //       intensity * 1.5f,   
        //       0.0f,
        //      0.0f
        //   );
        //BhapticsLibrary.Play("second", intensity);
        Debug.Log($"Haptic Triggered: {eventName} (intensity: {intensity:F2})");
    }

    private void GetStressed(string eventName, float intensity = 1f)
    {
        intensity = Mathf.Clamp01(intensity);
        BhapticsLibrary.Play("third");  //top-bottom jacket
        Debug.Log($"Haptic Triggered: {eventName}");

    }

    private void Connect(string eventName, float intensity = 1f)
    {
        intensity = Mathf.Clamp01(intensity);
        BhapticsLibrary.Play("dash"); //default jacket
        Debug.Log($"Haptic Triggered: {eventName} (intensity: {intensity:F2})");

    }
    private void Fire(string eventName, float intensity = 1f)
    {
        intensity = Mathf.Clamp01(intensity);
        BhapticsLibrary.Play("fire"); //my jacket left glove
        Debug.Log($"Haptic Triggered: {eventName} (intensity: {intensity:F2})");

    }
}
