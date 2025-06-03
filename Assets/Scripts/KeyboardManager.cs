using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class KeyboardManager : MonoBehaviour
{
    public KeyboardInput keyboardInput;

    void Start()
    {
        Button[] allButtons = GetComponentsInChildren<Button>();

        foreach (Button button in allButtons)
        {
            TextMeshProUGUI tmpText = button.GetComponentInChildren<TextMeshProUGUI>();

            if (tmpText != null)
            {
                string label = tmpText.text.Trim().ToUpper();

                // Skip special keys
                if (label == "←" || label == "⌫" || label == "SUBMIT" || label == "HINT" || label == "EXIT")
                    continue;

                // Capture correct value in lambda
                string letter = label;
                button.onClick.AddListener(() => keyboardInput.OnLetterButtonClick(letter));
            }
        }
    }
}
