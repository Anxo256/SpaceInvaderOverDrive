using UnityEngine;

public class Missile : MonoBehaviour
{
    public float StartSpeed = 5f;
    public float MaxSpeed = 16f;
    public float Acceleration = 14f;
    public float UpperLimit = 10f;

    public float ExplosionRadius = 1.6f;
    public int PointsPerAlien = 10;
    public GameObject ExplosionPrefab;

    private float CurrentSpeed;
    private bool Exploded = false;

    void Start()
    {
        CurrentSpeed = StartSpeed;
    }

    void Update()
    {
        CurrentSpeed = Mathf.MoveTowards(CurrentSpeed, MaxSpeed, Acceleration * Time.deltaTime);
        transform.Translate(Vector3.up * CurrentSpeed * Time.deltaTime);

        if (transform.position.y > UpperLimit)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D Collision)
    {
        if (Exploded) return;

        if (Collision.CompareTag("Alien"))
        {
            Explode();
        }
    }

    void Explode()
    {
        Exploded = true;
        
        Collider2D[] Hits = Physics2D.OverlapCircleAll(transform.position, ExplosionRadius);

        foreach (Collider2D Hit in Hits)
        {
            if (Hit != null && Hit.CompareTag("Alien"))
            {
                InterfaceManager.Instance.AddPoints(PointsPerAlien);
                Destroy(Hit.gameObject);
            }
        }

        if (ExplosionPrefab != null)
        {
            GameObject Fx = Instantiate(ExplosionPrefab, transform.position, Quaternion.identity);
            ExplosionEffect Effect = Fx.GetComponent<ExplosionEffect>();

            if (Effect != null)
            {
                Effect.Setup(ExplosionRadius * 2f);
            }
        }

        DetachParticles();
        Destroy(gameObject);
    }
    
    void DetachParticles()
    {
        foreach (ParticleSystem Ps in GetComponentsInChildren<ParticleSystem>())
        {
            Ps.transform.SetParent(null);
            ParticleSystem.EmissionModule Emission = Ps.emission;
            Emission.enabled = false;
            Destroy(Ps.gameObject, 1.5f);
        }
    }
}
