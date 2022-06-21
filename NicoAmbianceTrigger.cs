using UnityEngine;

public class NicoAmbianceTrigger : MonoBehaviour
{
    public Object nicoAmbManagerObj;
    public NicoAmbianceManager nAM;

    // Trigger options
    [Header("Destroy once triggered")]
    public bool destroyTrigger;

    [Header("Toggle audio on/off on trigger enter")]
    public bool audioToggle;

    [Header("Event follow player")]
    public bool followPlayer;

    [Header("Acts like OnTriggerStay, reverts to previous values on exit")]
    public bool revert;

    // Trigger params
    [Header("0: Outdoor 1: Indoor")]
    [Range(0f, 1)] public int newZone;

    [Header("Set new ambiance volume")]
    [Range(0f, 1f)] public float newVolume;

    [Header("Set new wind strength")]
    [Range(0f, 1f)] public float newWindStrength;

    [Header("<--- Outdoor [0.5] Indoor --->")]
    [Range(0f, 1f)] public float newReverbSetting;

    [Header("Set new outdoor ambiance intensity")]
    [Range(0f, 1f)] public float newOutdoorAmbianceIntensity;

    [Header("Set new indoor ambiance intensity")]
    [Range(0f, 1f)] public float newIndoorAmbianceIntensity;

    [Header("New hall ambiance (reverb)")]
    [Range(0, 1)] public float newHallAmbiance;

    // Store last used values (cba array/list parsing XD)
    private int lastZone;
    private float lastVol;
    private float lastWind;
    private float lastReverbSetting;
    private float lastOutdoorIntensity;
    private float lastIndoorIntensity;
    private float lastHallAmbiance;



    private void Awake()
    {
        if (FindObjectOfType<NicoAmbianceManager>() == null)
        {
            Instantiate(nicoAmbManagerObj);
            Debug.Log("Instantiating new AmbianceManager...");
        }
    }


    private void Start()
    {
        nAM = FindObjectOfType<NicoAmbianceManager>();
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            lastZone = nAM.environmentSetting;
            lastVol = nAM.volume;
            lastWind = nAM.windStrength;
            lastReverbSetting = nAM.reverbZoneSetting;
            lastOutdoorIntensity = nAM.outdoorAmbianceIntensity;
            lastIndoorIntensity = nAM.indoorAmbianceIntensity;
            lastHallAmbiance = nAM.hallAmbiance;

            nAM.environmentSetting = newZone;
            nAM.volume = newVolume;
            nAM.windStrength = newWindStrength;
            nAM.reverbZoneSetting = newReverbSetting;
            nAM.outdoorAmbianceIntensity = newOutdoorAmbianceIntensity;
            nAM.indoorAmbianceIntensity = newIndoorAmbianceIntensity;
            nAM.hallAmbiance = newHallAmbiance;

            if (audioToggle && nAM.IsPlaying())
            {
                nAM.ambianceManagerInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            }
            else if (!audioToggle && !nAM.IsPlaying())
            {
                nAM.ambianceManagerInstance.start();
            }

            if (destroyTrigger)
            {
                Destroy(gameObject, 0.3f);
            }

            if (followPlayer)
            {
                nAM.followPlayer = followPlayer;
            }
        }
    }


    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player" && revert)
        {
            nAM.environmentSetting = lastZone;
            nAM.volume = lastVol;
            nAM.windStrength = lastWind;
            nAM.reverbZoneSetting = lastReverbSetting;
            nAM.outdoorAmbianceIntensity = lastOutdoorIntensity;
            nAM.indoorAmbianceIntensity = lastIndoorIntensity;
            nAM.hallAmbiance = lastHallAmbiance;
        }
    }
}
