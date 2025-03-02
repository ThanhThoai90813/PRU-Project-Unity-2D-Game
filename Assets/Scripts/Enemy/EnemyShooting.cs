using UnityEngine;

public class EnemyShooting : MonoBehaviour
{
    public GameObject bullet;
    public Transform bulletPos;
    private GameObject player;
    private float timer;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }
    void Update()
    {
        float distance = Vector2.Distance(transform.position, player.transform.position);

        if(distance < 10)
        {
            timer += Time.deltaTime;
           
            if (timer > 2)
            {
                timer = 0;
                Shoot();
            }
        }

    }

    void Shoot()
    {
        Instantiate(bullet, bulletPos.position, Quaternion.identity);
    }
}
