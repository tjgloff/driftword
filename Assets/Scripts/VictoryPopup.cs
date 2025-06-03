using UnityEngine;
using UnityEngine.UI;

public class VictoryPopup : MonoBehaviour
{
    public GameObject popupPanel;

    public void Show()
    {
        if (popupPanel != null)
            popupPanel.SetActive(true);
    }

    public void Hide()
    {
        if (popupPanel != null)
            popupPanel.SetActive(false);
    }
}