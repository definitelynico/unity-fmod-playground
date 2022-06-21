using System.Collections;
using UnityEngine;
using Gamekit2D;
using FMODUnity;
using FMOD.Studio;

public class NicoAmbianceManager : MonoBehaviour
{
    // Event cache
    [EventRef] public string ambianceManager;
    public EventInstance ambianceManagerInstance;

    private GameObject player;

    private bool isPlaying = false;

    // Options
    [Header("If true, the instance will follow the player")]
    public bool followPlayer;

    [Header("Fade audio between scene transitions")]
    public bool fadeTransitions;

    // Parameters
    [Header("0: Outdoor 1: Indoor")]
    [Range(0f, 1)] public int environmentSetting;

    [Header("Global ambiance volume control")]
    [Range(0f, 1f)] public float volume;

    [Header("Wind intensity slider")]
    [Range(0f, 1f)] public float windStrength;

    [Header("<--- Outdoor [0.5] Indoor --->")]
    [Range(0f, 1f)] public float reverbZoneSetting;

    [Header("Sets the intensity of the outdoor ambiance")]
    [Range(0f, 1f)] public float outdoorAmbianceIntensity;

    [Header("Sets the intensity of the indoor ambiance")]
    [Range(0f, 1f)] public float indoorAmbianceIntensity;

    [Header("Extra hall ambiance (reverb)")]
    [Range(0, 1)] public float hallAmbiance;



    private void Awake()
    {
        DontDestroyOnLoad(this);
    }


    private void Start()
    {
        FindPlayerObject();
        isPlaying = true;
        ambianceManagerInstance = RuntimeManager.CreateInstance(ambianceManager);
        RuntimeManager.AttachInstanceToGameObject(ambianceManagerInstance, player.transform, player.GetComponent<Rigidbody2D>());
        ambianceManagerInstance.start();
    }


    private void Update()
    {
        if (SceneController.Transitioning && player == null)
        {
            if (fadeTransitions)
            {
                StartCoroutine(FadeAudioWhileLoading());
            }

            FindPlayerObject();
        }

        if (followPlayer)
        {
            transform.position = player.transform.position;
        }

        // Update params
        ambianceManagerInstance.setParameterByName("AmbianceSelector", environmentSetting);
        ManInst("AmbianceVolume", volume);
        ManInst("WindStrength", windStrength);
        ManInst("AmbianceReverbFader", reverbZoneSetting);
        ManInst("AmbianceOutdoorIntensity", outdoorAmbianceIntensity);
        ManInst("AmbianceIndoorIntensity", indoorAmbianceIntensity);
        ManInst("HallReverb", hallAmbiance);
    }


    private void FindPlayerObject()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }
    }


    private void ManInst(string name, float value)
    {
        ambianceManagerInstance.setParameterByName(name, value);
    }


    public bool IsPlaying()
    {
        PLAYBACK_STATE playbackState;
        ambianceManagerInstance.getPlaybackState(out playbackState);
        return playbackState != PLAYBACK_STATE.STOPPED;
    }


    IEnumerator FadeAudioWhileLoading()
    {
        volume = 0.3f;
        yield return new WaitForSeconds(0.5f);

        volume = 1f;
        yield break;
    }


    private void OnDrawGizmos()
    {
        if (followPlayer && isPlaying)
        {
            Gizmos.DrawWireSphere(player.transform.position, 0.5f);
        }
    }
}
