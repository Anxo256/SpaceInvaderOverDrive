using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float ProjectileSpeed = 15f;
    public float UpperLimit = 10f;

    void Update()
    {
        transform.Translate(Vector3.up * ProjectileSpeed * Time.deltaTime);

        if (transform.position.y > UpperLimit)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D Collision)
    {
        if (Collision.CompareTag("Alien"))
        {
            InterfaceManager.Instance.AddPoints(10); 
            
            Destroy(Collision.gameObject); 
            Destroy(gameObject);           
        }
    }
}
