using UnityEngine;
using UnityEngine.UI;
using FMODUnity;
using FMOD.Studio;

public class NicoVCAManager : MonoBehaviour
{
    /* pastacode */

    private VCA vCA;

    public string vcaName;

    [SerializeField][Range(0f, 1f)] private float vcaValue;


    private void Start()
    {
        vCA = RuntimeManager.GetVCA("vca:/" + vcaName);
        vCA.getVolume(out vcaValue);
        gameObject.GetComponent<Slider>().value = vcaValue;
    }

    public void GlobalVolumeControl(float vol)
    {
        vCA.setVolume(vol);
        vCA.getVolume(out vcaValue);
    }
}
