using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    public int healthRestore = 20;
    public Vector3 spinRotationSpeed = new Vector3(0, 180, 0);


    private void OnTriggerEnter2D(Collider2D collision)
    {
        Damageable damageable = collision.GetComponent<Damageable>();

        //kiểm tra nếu đầy máu thì ko ăn item
        if (damageable)
        {
            bool wasHealed = damageable.Heal(healthRestore);

            if (wasHealed)
                Destroy(gameObject);
        }
    }


    private void Update()
    {
        //giúp item xoay 180 độ
        transform.eulerAngles += spinRotationSpeed * Time.deltaTime;
    }
}
