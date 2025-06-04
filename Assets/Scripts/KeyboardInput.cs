using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class KeyboardInput : MonoBehaviour
{
    public TMP_Text guessInputField;
    public List<TextMeshProUGUI> letterSlots;
    public CloserWordsListManager guessListManager;
    public KeyboardController keyboardController;

    private string currentGuess = "";
    private string targetWord = "CRANE";

    public void OnLetterButtonClick(string letter)
    {
        if (currentGuess.Length < 5)
        {
            currentGuess += letter.ToUpper();
            guessInputField.text = currentGuess;
        }
    }

    public void OnBackspace()
    {
        if (currentGuess.Length > 0)
        {
            currentGuess = currentGuess.Substring(0, currentGuess.Length - 1);
            guessInputField.text = currentGuess;
        }
    }

    public void OnSubmit()
    {
        if (currentGuess.Length != 5)
        {
            Debug.Log("Guess must be 5 letters.");
            return;
        }

        Debug.Log("Submitted Guess: " + currentGuess);

        List<int> feedback = new List<int>();
        bool[] matchedInTarget = new bool[5];

        for (int i = 0; i < 5; i++)
        {
            if (currentGuess[i] == targetWord[i])
            {
                feedback.Add(2);
                matchedInTarget[i] = true;
            }
            else
            {
                feedback.Add(0);
            }
        }

        for (int i = 0; i < 5; i++)
        {
            if (feedback[i] == 0)
            {
                for (int j = 0; j < 5; j++)
                {
                    if (!matchedInTarget[j] && currentGuess[i] == targetWord[j])
                    {
                        feedback[i] = 1;
                        matchedInTarget[j] = true;
                        break;
                    }
                }
            }
        }

        for (int i = 0; i < letterSlots.Count; i++)
        {
            if (feedback[i] == 2)
            {
                letterSlots[i].text = currentGuess[i].ToString();

                Image tileImage = letterSlots[i].transform.parent.GetComponent<Image>();
                if (tileImage != null && guessListManager != null)
                {
                    tileImage.color = guessListManager.correctPositionColor;
                }
            }
        }

        if (guessListManager != null)
        {
            guessListManager.AddGuess(currentGuess, feedback);
        }

        // 🔑 Update keyboard key colors
        if (keyboardController != null)
        {
            for (int i = 0; i < currentGuess.Length; i++)
            {
                keyboardController.UpdateKeyColor(currentGuess[i], feedback[i]);
            }
        }

        currentGuess = "";
        guessInputField.text = "";
    }

    public void OnHint()
    {
        Debug.Log("Hint requested.");
    }
}
