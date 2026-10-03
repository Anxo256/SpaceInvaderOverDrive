using UnityEngine;

public class AliensController : MonoBehaviour
{
    public float Speed = 2f;
    public float DescentDistance = 0.5f;
    public float ScreenLimitX = 5f;
    
    public GameObject AlienProjectilePrefab;
    public float ShootInterval = 1.5f; 
    private float NextShootTime = 0f;

    private Vector3 Direction = Vector3.right;

    void Start()
    {
        NextShootTime = Time.time + ShootInterval;
    }

    void Update()
    {
        transform.Translate(Direction * Speed * Time.deltaTime);

        foreach (Transform Alien in transform)
        {
            if (Direction == Vector3.right && Alien.position.x >= ScreenLimitX)
            {
                ChangeDirectionAndDescend();
                break;
            }
            else if (Direction == Vector3.left && Alien.position.x <= -ScreenLimitX)
            {
                ChangeDirectionAndDescend();
                break;
            }
        }
        
        if (Time.time >= NextShootTime && transform.childCount > 0)
        {
            NextShootTime = Time.time + ShootInterval;
            AlienShoot();
        }
    }

    void ChangeDirectionAndDescend()
    {
        Direction = (Direction == Vector3.right) ? Vector3.left : Vector3.right;
        transform.position += Vector3.down * DescentDistance;
    }

    void AlienShoot()
    {
        int RandomIndex = Random.Range(0, transform.childCount);
        Transform RandomAlien = transform.GetChild(RandomIndex);

        if (AlienProjectilePrefab != null)
        {
            Instantiate(AlienProjectilePrefab, RandomAlien.position, Quaternion.identity);
        }
    }
}