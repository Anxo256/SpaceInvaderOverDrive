using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float Speed = 10f;
    public float LimitX = 8.5f; 
    public Transform FirePoint;

    [Header("Arma 1: disparo base (tecla 1)")]
    public GameObject ProjectilePrefab;
    public float TimeBetweenShots = 0.5f; 

    [Header("Arma 2: escopeta (tecla 2)")]
    public GameObject ShotgunPelletPrefab;
    public int PelletCount = 3;
    public float SpreadAngle = 12f;
    public float ShotgunCooldown = 1.2f;

    [Header("Arma 3: misil (tecla 3)")]
    public GameObject MissilePrefab;
    public float MissileCooldown = 2.5f;

    private float NextShotTime = 0f; 
    [Header("Particulas de disparo (destello en la nave)")]
    public GameObject BaseMuzzleFx;
    public GameObject ShotgunMuzzleFx;
    public GameObject MissileMuzzleFx;

    private int CurrentWeapon = 0;
    private bool WasReady = true;

    void Start()
    {
        RefreshHud();
    }

    void Update()
    {
        if (Time.timeScale == 0f) return;

        float HorizontalMovement = Input.GetAxisRaw("Horizontal");
        transform.Translate(Vector3.right * HorizontalMovement * Speed * Time.deltaTime);

        float ClampedX = Mathf.Clamp(transform.position.x, -LimitX, LimitX);
        transform.position = new Vector3(ClampedX, transform.position.y, transform.position.z);

        if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1)) SelectWeapon(0);
        if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2)) SelectWeapon(1);
        if (Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Keypad3)) SelectWeapon(2);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            TryShoot();
        }

        bool Ready = Time.time >= NextShotTime;
        if (Ready != WasReady)
        {
            RefreshHud();
        }
    }

    public void SelectWeapon(int Index)
    {
        CurrentWeapon = Mathf.Clamp(Index, 0, 2);
        RefreshHud();
    }

    public bool TryShoot()
    {
        if (Time.time < NextShotTime) return false;

        switch (CurrentWeapon)
        {
            case 1:
                ShootShotgun();
                NextShotTime = Time.time + ShotgunCooldown;
                break;
            case 2:
                ShootMissile();
                NextShotTime = Time.time + MissileCooldown;
                break;
            default:
                Shoot();
                NextShotTime = Time.time + TimeBetweenShots;
                break;
        }

        RefreshHud();
        return true;
    }

    void Shoot()
    {
        if (ProjectilePrefab == null) return;
        Instantiate(ProjectilePrefab, FirePoint.position, Quaternion.identity);
        SpawnFx(BaseMuzzleFx);
    }

    void ShootShotgun()
    {
        if (ShotgunPelletPrefab == null) return;

        for (int i = 0; i < PelletCount; i++)
        {
            float Angle = (PelletCount == 1) ? 0f : Mathf.Lerp(-SpreadAngle, SpreadAngle, i / (PelletCount - 1f));
            Instantiate(ShotgunPelletPrefab, FirePoint.position, Quaternion.Euler(0f, 0f, Angle));
        }

        SpawnFx(ShotgunMuzzleFx);
    }

    void ShootMissile()
    {
        if (MissilePrefab == null) return;
        Instantiate(MissilePrefab, FirePoint.position, Quaternion.identity);
        SpawnFx(MissileMuzzleFx);
    }

    void SpawnFx(GameObject FxPrefab)
    {
        if (FxPrefab != null)
        {
            Instantiate(FxPrefab, FirePoint.position, Quaternion.identity);
        }
    }

    void RefreshHud()
    {
        WasReady = Time.time >= NextShotTime;

        if (InterfaceManager.Instance != null)
        {
            InterfaceManager.Instance.UpdateWeaponHud(CurrentWeapon, WasReady);
        }
    }
}
