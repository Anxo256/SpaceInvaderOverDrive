using UnityEngine;

public class ExplosionEffect : MonoBehaviour
{
    public SpriteRenderer Flash;
    public float Duration = 0.35f;
    public float LifeTime = 1.3f;

    private Color BaseColor;
    private float TargetScale = 1f;
    private float Age = 0f;
    private bool IsSetUp = false;

    public void Setup(float Diameter)
    {
        if (Flash == null) Flash = GetComponent<SpriteRenderer>();

        BaseColor = Flash.color;

        float SpriteSize = (Flash.sprite != null) ? Flash.sprite.bounds.size.x : 1f;
        TargetScale = Diameter / SpriteSize;

        Flash.transform.localScale = Vector3.one * TargetScale * 0.2f;
        IsSetUp = true;
    }

    void Update()
    {
        if (!IsSetUp) Setup(1f);

        Age += Time.deltaTime;
        float T = Mathf.Clamp01(Age / Duration);

        Flash.transform.localScale = Vector3.one * Mathf.Lerp(TargetScale * 0.2f, TargetScale, T);

        Color C = BaseColor;
        C.a = 1f - T;
        Flash.color = C;

        if (Age >= Mathf.Max(Duration, LifeTime))
        {
            Destroy(gameObject);
        }
    }
}
