using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using Gamekit2D;

public class NicoDesaturateLowHp : MonoBehaviour
{
    private PostProcessVolume ppv;
    private ColorGrading cg;
    private Grain gr;
    private Vignette vn;
    private Damageable dmg;

    [Range(-100f, 38f)] public float saturation = 38;
    [Range(0.2f, 1f)] public float grain = 0.2f;
    [Range(0f, 0.5f)] public float vignette = 0f;

    private void Start()
    {
        ppv = GetComponent<PostProcessVolume>();
        ppv.profile.TryGetSettings(out cg);
        ppv.profile.TryGetSettings(out gr);
        ppv.profile.TryGetSettings(out vn);
        dmg = GameObject.FindGameObjectWithTag("Player").GetComponent<Damageable>();
    }

    private void Update()
    {
        cg.saturation.value = saturation;
        gr.intensity.value = grain;
        vn.intensity.value = vignette;

        switch (dmg.CurrentHealth)
        {
            case 5:
                saturation = 38f;
                grain = 0.2f;
                vignette = 0f;
                break;
            case 4:
                saturation = 29f;
                grain = 0.35f;
                vignette = 0.1f;
                break;
            case 3:
                saturation = 10f;
                grain = 0.5f;
                vignette = 0.2f;
                break;
            case 2:
                saturation = -30f;
                grain = 0.65f;
                vignette = 0.3f;
                break;
            case 1:
                saturation = -60f;
                grain = 0.8f;
                vignette = 0.4f;
                break;
            case 0:
                saturation = -100f;
                grain = 0.95f;
                vignette = 0.5f;
                break;
        }
    }
}
