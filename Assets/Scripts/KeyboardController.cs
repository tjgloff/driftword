using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class KeyboardController : MonoBehaviour
{
    public Color correctPositionColor;
    public Color correctLetterColor;
    public Color incorrectColor;

    private Dictionary<char, Button> keyButtons = new();
    private Dictionary<char, int> keyStates = new(); // 0: gray, 1: light blue, 2: dark blue

    void Start()
    {
        Button[] buttons = GetComponentsInChildren<Button>();
        foreach (Button button in buttons)
        {
            TextMeshProUGUI tmp = button.GetComponentInChildren<TextMeshProUGUI>();
            if (tmp == null) continue;

            char c = tmp.text.ToUpper()[0];
            if (char.IsLetter(c) && !keyButtons.ContainsKey(c))
            {
                keyButtons[c] = button;
                keyStates[c] = -1;
            }
        }
    }

    public void UpdateKeyColor(char letter, int feedback)
    {
        char c = char.ToUpper(letter);

        if (!keyButtons.ContainsKey(c)) return;

        if (feedback <= keyStates[c]) return;

        keyStates[c] = feedback;
        Color color = feedback switch
        {
            2 => correctPositionColor,
            1 => correctLetterColor,
            _ => incorrectColor
        };

        keyButtons[c].GetComponent<Image>().color = color;
    }
}
