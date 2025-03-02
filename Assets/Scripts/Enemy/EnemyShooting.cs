using System;
using UnityEngine;

public class EnemyShooting : MonoBehaviour
{
    public GameObject bullet;
    public Transform bulletPos;
    private GameObject player;
    private float timer;
    public float detectionRange = 10f;
    public AudioSource shootSound;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }
    void Update()
    {
        float distance = Vector2.Distance(transform.position, player.transform.position);

        if(distance < detectionRange)
        {
            LookAtPlayer();
            timer += Time.deltaTime;
           
            if (timer > 2)
            {
                timer = 0;
                Shoot();
            }
        }

    }

    private void LookAtPlayer()
    {
        Vector3 direction = player.transform.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    void Shoot()
    {
        Instantiate(bullet, bulletPos.position, Quaternion.identity);
        if(shootSound != null)
        {
            shootSound.Play();
        }
    }
}
