using UnityEngine;
using UnityEngine.EventSystems;

public class NicoUISoundManager : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler
{
    /* definitelynico eventsystems abuse */

    AudioManager audioManager;

    private void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
    }

    public void OnPointerClick(PointerEventData pointerEventData)
    {
        if (pointerEventData.button == PointerEventData.InputButton.Left)
        {
            switch (name)
            {
                case "UIButton":
                    audioManager.PlayUIButton();
                    break;
                case "UICloseButton":
                    audioManager.PlayUICancel();
                    break;
                case "StartButton":
                    audioManager.PlayUIStart();
                    break;
                case "OptionsButton":
                    audioManager.PlayUIButton();
                    break;
                case "ExitButton":
                    audioManager.PlayUICancel();
                    break;
                default:
                    audioManager.PlayUIButton();
                    break;
            }
        }
    }

    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        if (pointerEventData.IsPointerMoving())
        {
            switch (name)
            {
                case "UIButton":
                    audioManager.PlayUIHover();
                    break;
                case "UICloseButton":
                    // play close sound
                    break;
                case "StartButton":
                    audioManager.PlayUIHover();
                    break;
                case "OptionsButton":
                    audioManager.PlayUIHover();
                    break;
                case "ExitButton":
                    audioManager.PlayUIHover();
                    break;
                default:
                    audioManager.PlayUIHover();
                    break;
            }
        }
    }
}
