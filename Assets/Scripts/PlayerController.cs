using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float Speed = 10f;
    public float LimitX = 8.5f; 
    public GameObject ProjectilePrefab;
    public Transform FirePoint;

    public float TimeBetweenShots = 0.5f; 
    private float NextShotTime = 0f; 

    void Update()
    {
        float HorizontalMovement = Input.GetAxisRaw("Horizontal");
        transform.Translate(Vector3.right * HorizontalMovement * Speed * Time.deltaTime);

        float ClampedX = Mathf.Clamp(transform.position.x, -LimitX, LimitX);
        transform.position = new Vector3(ClampedX, transform.position.y, transform.position.z);

        if (Input.GetKeyDown(KeyCode.Space) && Time.time >= NextShotTime)
        {
            Shoot();
            NextShotTime = Time.time + TimeBetweenShots;
        }
    }

    void Shoot()
    {
        Instantiate(ProjectilePrefab, FirePoint.position, Quaternion.identity);
    }
}
