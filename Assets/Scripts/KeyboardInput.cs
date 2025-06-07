using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class KeyboardInput : MonoBehaviour
{
    [Header("UI References")]
    public TMP_Text guessInputField;
    public GameManager gameManager;

    private string currentGuess = "";

    public void OnLetterButtonClick(string letter)
    {
        if (currentGuess.Length < 5)
        {
            currentGuess += letter.ToUpper();
            UpdateInputDisplay();
        }
    }

    public void OnBackspace()
    {
        if (currentGuess.Length > 0)
        {
            currentGuess = currentGuess.Substring(0, currentGuess.Length - 1);
            UpdateInputDisplay();
        }
    }

    public void OnSubmit()
    {
        if (currentGuess.Length != 5)
        {
            Debug.Log("Guess must be exactly 5 letters.");
            return;
        }

        gameManager.SubmitGuess(currentGuess);

        currentGuess = "";
        UpdateInputDisplay();
    }

    private void UpdateInputDisplay()
    {
        guessInputField.text = currentGuess;
    }

    public void OnHint()
    {
        // Placeholder for future hint logic
    }
}
