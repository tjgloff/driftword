using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class KeyboardInput : MonoBehaviour
{
    public TMP_Text guessInputField; // The "GUESS..." input box
    public List<TextMeshProUGUI> letterSlots; // The top feedback tile row
    public GuessListManager guessListManager; // The scrollable list on the right

    private string currentGuess = "";

    // Called when a letter key is pressed
    public void OnLetterButtonClick(string letter)
    {
        if (currentGuess.Length < 5)
        {
            currentGuess += letter.ToUpper();
            guessInputField.text = currentGuess;
        }
    }

    // Called when backspace is pressed
    public void OnBackspace()
    {
        if (currentGuess.Length > 0)
        {
            currentGuess = currentGuess.Substring(0, currentGuess.Length - 1);
            guessInputField.text = currentGuess;
        }
    }

    // Called when submit is pressed
    public void OnSubmit()
    {
        Debug.Log("Submitted Guess: " + currentGuess);

        // Show guess in the top tile row
        for (int i = 0; i < letterSlots.Count; i++)
        {
            if (i < currentGuess.Length)
                letterSlots[i].text = currentGuess[i].ToString();
            else
                letterSlots[i].text = "";
        }

        // TEMP: Generate mock Wordle-style color feedback (cycling)
        List<string> feedback = new List<string>();
        for (int i = 0; i < currentGuess.Length; i++)
        {
            if (i % 3 == 0) feedback.Add("darkblue");
            else if (i % 3 == 1) feedback.Add("lightblue");
            else feedback.Add("gray");
        }

        // Add to the scrollable list on the right
        if (guessListManager != null)
        {
            guessListManager.AddGuess(currentGuess, feedback);
        }

        // Clear for next input
        currentGuess = "";
        guessInputField.text = "";
    }

    public void OnHint()
    {
        Debug.Log("Hint requested.");
        // Placeholder for future hint logic
    }
}
