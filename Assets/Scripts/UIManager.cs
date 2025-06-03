using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Text feedbackText;

    public void ShowMessage(string message)
    {
        if (feedbackText != null)
            feedbackText.text = message;
    }
}