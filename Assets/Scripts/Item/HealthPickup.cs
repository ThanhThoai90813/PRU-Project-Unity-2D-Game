using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    public int healthRestore = 20;
    public bool isHealing = true;
    public Vector3 spinRotationSpeed = new Vector3(0, 180, 0);

    AudioSource pickupSource;

	private void Awake()
	{
		pickupSource = GetComponent<AudioSource>();
	}

	private void OnTriggerEnter2D(Collider2D collision)
    {
        Damageable damageable = collision.GetComponent<Damageable>();


        if (damageable)
        {
            bool effetApplied = false;
            if(isHealing)
            {
                    effetApplied = damageable.Heal(healthRestore);       
            }
            else
            {
                effetApplied = damageable.Hit(healthRestore, Vector2.zero);
            }
            if (effetApplied)
            {
                if(pickupSource)
                    AudioSource.PlayClipAtPoint(pickupSource.clip, transform.position, pickupSource.volume);
                Destroy(gameObject);
            }
        }

    }


    private void Update()
    {
        //giúp item xoay 180 độ
        //transform.eulerAngles += spinRotationSpeed * Time.deltaTime;
    }
}
