using UnityEngine;

public class AlienProjectile : MonoBehaviour
{
    public float ProjectileSpeed = 8f;
    public float LowerLimit = -10f;

    void Update()
    {
        transform.Translate(Vector3.down * ProjectileSpeed * Time.deltaTime);

        if (transform.position.y < LowerLimit)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D Collision)
    {
        if (Collision.CompareTag("Player"))
        {
            InterfaceManager.Instance.LoseLife();
            
            Destroy(gameObject);
        }
    }
}