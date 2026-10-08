using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float Speed = 14f;
    [Header("Zona de movimiento (se calcula con la camara)")]
    public float MarginX = 0.8f;
    public float MarginBottom = 0.8f;
    [Range(0.1f, 1f)] public float PlayableHeight = 0.5f;
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
    
    private float[] NextShotTimes = new float[3];

    [Header("Particulas de disparo (destello en la nave)")]
    public GameObject BaseMuzzleFx;
    public GameObject ShotgunMuzzleFx;
    public GameObject MissileMuzzleFx;

    private int CurrentWeapon = 0;
    private int LastReadyMask = 7;
    private float NextHudRefresh = 0f;
    private float[] RemainingCache = new float[3];
    private float[] CooldownCache = new float[3];

    void Start()
    {
        RefreshHud();
    }

    void Update()
    {
        if (Time.timeScale == 0f) return;

        float HorizontalMovement = Input.GetAxisRaw("Horizontal");
        float VerticalMovement = 0f;
        Vector3 Move = new Vector3(HorizontalMovement, VerticalMovement, 0f);
        if (Move.sqrMagnitude > 1f) Move.Normalize();
        transform.Translate(Move * Speed * Time.deltaTime, Space.World);

        float ClampedX = Mathf.Clamp(transform.position.x, ScreenBounds.Left + MarginX, ScreenBounds.Right - MarginX);
        float MaxY = ScreenBounds.Bottom + (ScreenBounds.Top - ScreenBounds.Bottom) * PlayableHeight;
        float ClampedY = Mathf.Clamp(transform.position.y, ScreenBounds.Bottom + MarginBottom, MaxY);
        transform.position = new Vector3(ClampedX, ClampedY, transform.position.z);

        if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1)) SelectWeapon(0);
        if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2)) SelectWeapon(1);
        if (Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Keypad3)) SelectWeapon(2);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            TryShoot();
        }
        
        int ReadyMask = GetReadyMask();
        if (ReadyMask != LastReadyMask || (ReadyMask != 7 && Time.time >= NextHudRefresh))
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
        if (GetRemaining(CurrentWeapon) > 0f) return false;

        switch (CurrentWeapon)
        {
            case 1:
                ShootShotgun();
                NextShotTimes[1] = Time.time + ShotgunCooldown;
                break;
            case 2:
                ShootMissile();
                NextShotTimes[2] = Time.time + MissileCooldown;
                break;
            default:
                Shoot();
                NextShotTimes[0] = Time.time + TimeBetweenShots;
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
    
    public float GetRemaining(int Weapon)
    {
        return Mathf.Max(0f, NextShotTimes[Mathf.Clamp(Weapon, 0, 2)] - Time.time);
    }

    int GetReadyMask()
    {
        int Mask = 0;

        for (int i = 0; i < 3; i++)
        {
            if (GetRemaining(i) <= 0f) Mask |= (1 << i);
        }

        return Mask;
    }

    void RefreshHud()
    {
        LastReadyMask = GetReadyMask();
        NextHudRefresh = Time.time;

        if (InterfaceManager.Instance != null)
        {
            for (int i = 0; i < 3; i++)
            {
                RemainingCache[i] = GetRemaining(i);
            }

            CooldownCache[0] = TimeBetweenShots;
            CooldownCache[1] = ShotgunCooldown;
            CooldownCache[2] = MissileCooldown;

            InterfaceManager.Instance.UpdateWeaponHud(CurrentWeapon, RemainingCache, CooldownCache);
        }
    }
}
