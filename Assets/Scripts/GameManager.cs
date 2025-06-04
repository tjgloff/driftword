using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public CloserWordsListManager closerWordsListManager;

    private string hiddenWord = "CRANE"; // Example hidden word — replace with your actual logic

    public void SubmitGuess(string playerGuess)
    {
        List<int> feedback = GenerateFeedback(playerGuess.ToUpper(), hiddenWord.ToUpper());

        // Send the guess and feedback to the Closer Words List
        if (closerWordsListManager != null)
        {
            closerWordsListManager.AddGuess(playerGuess.ToUpper(), feedback);
        }

        // TODO: Trigger boat movement here based on feedback, if desired

        // Check for win
        if (playerGuess.ToUpper() == hiddenWord.ToUpper())
        {
            Debug.Log("🎉 You guessed it!");
            // TODO: Trigger game victory logic here
        }
    }

    private List<int> GenerateFeedback(string guess, string target)
    {
        List<int> result = new List<int>();

        for (int i = 0; i < guess.Length; i++)
        {
            if (guess[i] == target[i])
                result.Add(2); // Correct letter and position
            else if (target.Contains(guess[i].ToString()))
                result.Add(1); // Correct letter, wrong position
            else
                result.Add(0); // Incorrect letter
        }

        return result;
    }
}
