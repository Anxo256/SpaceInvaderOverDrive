using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float ProjectileSpeed = 15f;
    public float UpperLimit = 10f;
    public float SideLimit = 12f;
    public float MaxLifetime = 0f;

    public GameObject HitEffectPrefab;

    private float Age = 0f;
    private bool HasHit = false;

    void Update()
    {
        transform.Translate(Vector3.up * ProjectileSpeed * Time.deltaTime);

        Age += Time.deltaTime;

        if (transform.position.y > UpperLimit || Mathf.Abs(transform.position.x) > SideLimit)
        {
            Destroy(gameObject);
        }
        else if (MaxLifetime > 0f && Age >= MaxLifetime)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D Collision)
    {
        if (HasHit) return;

        if (Collision.CompareTag("Alien"))
        {
            HasHit = true;

            if (HitEffectPrefab != null)
            {
                Instantiate(HitEffectPrefab, Collision.transform.position, Quaternion.identity);
            }

            InterfaceManager.Instance.AddPoints(10); 
            
            Destroy(Collision.gameObject); 
            Destroy(gameObject);           
        }
    }
}
