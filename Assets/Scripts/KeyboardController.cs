using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class KeyboardController : MonoBehaviour
{
    [System.Serializable]
    public class KeyButton
    {
        public string letter;
        public Image keyImage;
        public TextMeshProUGUI keyText;
        public int currentFeedback = -1; // -1 = untouched, 0 = gray, 1 = light blue, 2 = dark blue
    }

    public List<KeyButton> keys;

    public Color correctPositionColor;  // dark blue
    public Color correctLetterColor;    // light blue
    public Color incorrectColor;        // gray

    public void UpdateKeyColor(char letterChar, int feedback)
    {
        string letter = letterChar.ToString().ToUpper();

        foreach (KeyButton key in keys)
        {
            if (key.letter == letter)
            {
                // Only upgrade the color if it's a stronger result
                if (feedback > key.currentFeedback)
                {
                    key.currentFeedback = feedback;

                    switch (feedback)
                    {
                        case 2:
                            key.keyImage.color = correctPositionColor;
                            break;
                        case 1:
                            key.keyImage.color = correctLetterColor;
                            break;
                        case 0:
                            key.keyImage.color = incorrectColor;
                            break;
                    }
                }
                break;
            }
        }
    }
}
