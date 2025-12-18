using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemywithshoot : MonoBehaviour
{
    [Header("General")]
    //public float moveSpeed = 40f;
    public float turnSpeed = 2f;

    [Header("Detection")]
    public Transform player; // VR rig or airplane
    public float detectionRange = 200f; //обнаружение
    public float attackRange = 150f; //атака

    [Header("Patrol")]
    //public Transform[] patrolPoints;
    //private int currentPoint = 0;

    [Header("Shooting")]
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float fireCooldown = 0.5f; //время между выстрелами 0.5f
    private float fireTimer = 0f;

    private enum State { Idle, Chase, Attack }
    private State currentState = State.Idle;

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // ===== STATE TRANSITIONS =====
        if (distanceToPlayer < attackRange) currentState = State.Attack;
        else if (distanceToPlayer < detectionRange) currentState = State.Chase;
        else currentState = State.Idle;

        // ===== EXECUTE STATE =====
        switch (currentState)
        {
            case State.Idle:
                IdleLook();
                break;

            case State.Chase:
                LookAtPlayer();
                break;

            case State.Attack:
                AttackPlayer();
                break;
        }

        fireTimer -= Time.deltaTime;
    }

    // ========== PATROL ==========
    void IdleLook()
    {
        //Transform target = patrolPoints[currentPoint];

        //FlyTowards(target.position);

        //if (Vector3.Distance(transform.position, target.position) < 50f)
        //    currentPoint = (currentPoint + 1) % patrolPoints.Length;
    }

    // ========== CHASE ==========
    void LookAtPlayer()
    {
        //FlyTowards(player.position);
        Vector3 dir = (player.position - transform.position).normalized;
        Quaternion rot = Quaternion.LookRotation(dir);

        transform.rotation = Quaternion.Slerp(transform.rotation, rot, turnSpeed * Time.deltaTime);
    }

    // ========== ATTACK ==========
    void AttackPlayer()
    {
        //FlyTowards(player.position);
        LookAtPlayer();

        if (fireTimer <= 0f)
        {
            Debug.Log("Attacking");
            Fire();
            fireTimer = fireCooldown;
        }
    }

    

    // ========== SHOOTING ==========
    void Fire()
    {
        GameObject bulletj = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        Bulletmove bullet = bulletj.GetComponent<Bulletmove>();
        bullet.SetDirection(firePoint.forward);
       // GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation); //самая последняя версия
    //    //GameObject bulletObj = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);


    //    //Bulletmove bullet1 = bulletObj.GetComponent<Bulletmove>();

    //    //bullet.SetDirection(firePoint.forward);
    //    //GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
    //    //Instantiate(bulletPrefab, firePoint.position,
    //    //firePoint.rotation * Quaternion.Euler(90, 0, 0));

    //    //bullet.transform.rotation *= Quaternion.Euler(90, 0, 0);
    }
}
