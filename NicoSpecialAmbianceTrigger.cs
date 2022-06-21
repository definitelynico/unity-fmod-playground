using UnityEngine;
using FMODUnity;
using FMOD.Studio;
using System.Collections;

/* alt0169definitelynico2022 */

public class NicoSpecialAmbianceTrigger : MonoBehaviour
{
    [Header("Event to play when triggered.")]
    [EventRef] public string selectedEvent;
    EventInstance selectedEventInstance;

    [Space(30)]
    [Range(0, 1)] public float volume;
    [Tooltip("General amount value, for droplets etc.")][Range(0, 1)] public float generalAmount;

    public enum SpecialAmbiances { Spaceship, Crystals, Acid, Droplets };
    public SpecialAmbiances selectAmbianceToPlay;

    [Space(30)]
    [Tooltip("The object will follow the players blabla... This is useful for larger areas.")]
    public bool advancedObjectDistance;
    [Space(30)]
    public Vector3 followFrom;
    public Vector3 followTo;

    private float pos1;
    private float pos2;
    private GameObject player;
    private Transform distance;
    private GameObject emitterObj;
    private float scaledDistance;
    private bool isInsideTrigger = false;
    private bool isPlaying = false;
    [Space(30)]
    public bool showGizmos = true;



    private void Start()
    {
        isPlaying = true;

        if (this.emitterObj == null)
        {
            CreateAudioEmitter();
        }

        player = GameObject.FindGameObjectWithTag("Player");
        gameObject.GetComponent<SpriteRenderer>().enabled = false;

        pos1 = this.followFrom.x;
        pos2 = this.followTo.x;
    }


    private void CreateAudioEmitter()
    {
        emitterObj = new GameObject("emitterObj_" + gameObject.name.ToString());
        emitterObj.transform.position = this.followFrom;
    }


    IEnumerator UpdateVolume()
    {
        while (this.isInsideTrigger)
        {
            scaledDistance = 1 / Vector3.Distance(player.transform.position, emitterObj.transform.position);
            scaledDistance = Mathf.Clamp(scaledDistance, 0f, 1f);
            //Debug.Log(scaledDistance);

            if (advancedObjectDistance)
            {
                float ppX = player.transform.position.x;
                if (ppX > pos1 && ppX < pos2)
                {
                    emitterObj.transform.position = new Vector3(ppX, this.followFrom.y, 0);
                }
            }

            selectedEventInstance.setParameterByName("GeneralVolumeControl", scaledDistance * volume);
            selectedEventInstance.setParameterByName("GeneralAmount", generalAmount);
            yield return new WaitForSeconds(0.1f);
        }

        if (!this.isInsideTrigger)
        {
            yield break;
        }
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            if (advancedObjectDistance)
            {
                if (Vector3.Distance(player.transform.position, this.followFrom) >
                    Vector3.Distance(player.transform.position, this.followTo))
                {
                    emitterObj.transform.position = this.followTo;
                }
                else
                {
                    emitterObj.transform.position = this.followFrom;
                }
            }

            isInsideTrigger = true;
            StartCoroutine(UpdateVolume());
            selectedEventInstance = RuntimeManager.CreateInstance(selectedEvent);
            selectedEventInstance.set3DAttributes(RuntimeUtils.To3DAttributes(emitterObj));
            selectedEventInstance.setParameterByName("SpecialTrigger", ((int)selectAmbianceToPlay));
            selectedEventInstance.start();
        }
    }


    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            isInsideTrigger = false;
            selectedEventInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            selectedEventInstance.release();
        }
    }


    private void OnDrawGizmos()
    {
        // TODO: Draw attenuation min/max values. Handles for Vector3 start- and end positions.
        //       Handles for vector positions on start- and end points. (Custom editor).

        if (showGizmos)
        {
            Gizmos.DrawLine(followFrom, followTo);

            if (isPlaying)
            {
                Gizmos.DrawWireSphere(emitterObj.transform.position, 0.5f);
            }
        }
    }
}
