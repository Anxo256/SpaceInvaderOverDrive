using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WeaponSlotUI : MonoBehaviour
{
    public Image Fill;                  // circulo con fill radial: se llena mientras recarga
    public Image Border;                // aro del circulo
    public TextMeshProUGUI NameText;    // nombre del arma
    public TextMeshProUGUI StatusText;  // READY o segundos que faltan
    public Color WeaponColor = Color.white;

    private string LastStatus = "";

    // Remaining = segundos que faltan, Cooldown = duracion total de la recarga del arma
    public void SetState(bool Selected, float Remaining, float Cooldown)
    {
        bool Ready = Remaining <= 0f;

        // El circulo se llena de 0 a 1 mientras recarga; lleno = lista
        float Progress = (Cooldown > 0f) ? Mathf.Clamp01(1f - Remaining / Cooldown) : 1f;
        Fill.fillAmount = Ready ? 1f : Progress;
        Fill.color = Ready ? WeaponColor : Color.Lerp(Color.black, WeaponColor, 0.6f);

        Border.color = Selected ? Color.white : new Color(1f, 1f, 1f, 0.35f);
        transform.localScale = Vector3.one * (Selected ? 1.08f : 1f);

        NameText.fontStyle = Selected ? FontStyles.Bold : FontStyles.Normal;
        NameText.color = Selected ? Color.white : new Color(1f, 1f, 1f, 0.65f);

        string Status = Ready ? "READY" : Remaining.ToString("0.0") + "s";

        if (Status != LastStatus)
        {
            LastStatus = Status;
            StatusText.text = Status;
            StatusText.color = Ready ? WeaponColor : new Color(0.85f, 0.85f, 0.85f);
        }
    }
}
