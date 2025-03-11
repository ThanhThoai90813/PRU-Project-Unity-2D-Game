using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooting : MonoBehaviour
{
    public GameObject bullet;
    public Transform bulletPos;
    private GameObject player;
    private float timer;
    public float detectionRange = 20f;
    public AudioSource shootSound;

    public List<Transform> waypoints; 
    private int waypointIndex = 0;
    public float moveSpeed = 2f;
    private float waypointThreshold = 0.2f;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }
    void Update()
    {
        float distance = Vector2.Distance(transform.position, player.transform.position);

        if(distance < detectionRange)
        {
            //LookAtPlayer();
            timer += Time.deltaTime;
           
            if (timer > 2)
            {
                timer = 0;
                Shoot();
            }
        }
        else
        {
            Patrol();
        }

    }
    private void Patrol()
    {
        if (waypoints.Count == 0) return;

        Transform targetWaypoint = waypoints[waypointIndex];
        transform.position = Vector2.MoveTowards(transform.position, targetWaypoint.position, moveSpeed * Time.deltaTime);

        if (Vector2.Distance(transform.position, targetWaypoint.position) < waypointThreshold)
        {
            waypointIndex++;
            if (waypointIndex >= waypoints.Count)
            {
                waypointIndex = 0;
            }
        }
    }
    //private void LookAtPlayer()
    //{
    //    Vector3 direction = player.transform.position - transform.position;
    //    float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

    //    transform.rotation = Quaternion.Euler(0, 0, angle);
    //}

    void Shoot()
    {
        GameObject newBullet = Instantiate(bullet, bulletPos.position, Quaternion.identity);
        EnemyBullet bulletScript = newBullet.GetComponent<EnemyBullet>();
        if (bulletScript != null)
        {
            bulletScript.force = 7f; // Thay đổi tốc độ đạn tại đây
            bulletScript.damage = 10; // Thay đổi damage tại đây
        }

        if (shootSound != null)
        {
            shootSound.Play();
        }

    }
}
