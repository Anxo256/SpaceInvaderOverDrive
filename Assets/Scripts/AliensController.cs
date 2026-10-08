using UnityEngine;

public class AliensController : MonoBehaviour
{
    public float Speed = 2f;
    public float DescentDistance = 0.5f;
    public float ScreenLimitX = 8.2f;
    public float InvasionLimitY = -3.4f;
    
    public GameObject AlienProjectilePrefab;
    public float ShootInterval = 1.5f; 
    private float NextShootTime = 0f;

    private bool HadAliens = false;
    private bool GameFinished = false;

    private Vector3 Direction = Vector3.right;

    void Start()
    {
        ScreenLimitX = ScreenBounds.Right - 1.0f;
        InvasionLimitY = ScreenBounds.Bottom + 2.5f;

        NextShootTime = Time.time + ShootInterval;
    }

    void Update()
    {
        if (GameFinished) return;

        if (transform.childCount > 0)
        {
            HadAliens = true;
        }
        
        if (HadAliens && transform.childCount == 0)
        {
            GameFinished = true;
            InterfaceManager.Instance.Win();
            return;
        }

        transform.Translate(Direction * Speed * Time.deltaTime);

        bool MustTurn = false;

        foreach (Transform Alien in transform)
        {
            if (Alien.position.y <= InvasionLimitY)
            {
                GameFinished = true;
                InterfaceManager.Instance.GameOver();
                return;
            }

            if (Direction == Vector3.right && Alien.position.x >= ScreenLimitX)
            {
                MustTurn = true;
            }
            else if (Direction == Vector3.left && Alien.position.x <= -ScreenLimitX)
            {
                MustTurn = true;
            }
        }

        if (MustTurn)
        {
            ChangeDirectionAndDescend();
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
