using UnityEngine;
using UnityEngine.Rendering;

public class GoblinGateTrigger : MonoBehaviour
{
    public Transform[] spawnPoints;
    private bool hasSpawned = false;
    private float speed = 3f;
    public GameObject[] monsters; 

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !hasSpawned)
        {
            hasSpawned = true;
            foreach (Transform t in spawnPoints)
            {
                GameObject randomMonster = monsters[Random.Range(0, monsters.Length)];
                GameObject spawnedMonster = Instantiate(randomMonster, t.position, Quaternion.identity);

                if (spawnedMonster.CompareTag("Bat")){
                    Rigidbody2D rb = spawnedMonster.GetComponent<Rigidbody2D>();
                    if (rb != null)
                    {
                        rb.linearVelocity = new Vector2(0, speed);
                    }
                }
            }
        }
    }


}
