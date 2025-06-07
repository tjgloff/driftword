using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public CloserWordsListManager closerWordsListManager;
    public BoatController boatController;

    private string hiddenWord = "CRANE"; // You can change this to any 5-letter word

    public void SubmitGuess(string playerGuess)
    {
        string guess = playerGuess.ToUpper();
        string target = hiddenWord.ToUpper();

        // Generate feedback for each letter (0 = wrong, 1 = right letter, 2 = right place)
        List<int> feedback = GenerateFeedback(guess, target);

        // Update the Closer Words List
        if (closerWordsListManager != null)
        {
            closerWordsListManager.AddGuess(guess, feedback);
        }

        // Move the boat based on feedback
        float movePercent = CalculateBoatMovement(guess, target);
        if (boatController != null)
        {
            boatController.MoveBoatByPercent(movePercent);
        }

        // Check for win
        if (guess == target)
        {
            Debug.Log("🎉 You guessed it!");
            // TODO: Add victory animation or message
        }
    }

    private float CalculateBoatMovement(string guess, string target)
    {
        float percent = 0f;

        for (int i = 0; i < guess.Length; i++)
        {
            if (target.Contains(guess[i]))
                percent += 0.1f; // +10% for correct letter

            if (guess[i] == target[i])
                percent += 0.1f; // +10% more for correct position
        }

        return percent; // e.g. 0.4f = 40% movement
    }

    private List<int> GenerateFeedback(string guess, string target)
    {
        List<int> result = new List<int>();

        for (int i = 0; i < guess.Length; i++)
        {
            if (guess[i] == target[i])
                result.Add(2); // correct position
            else if (target.Contains(guess[i]))
                result.Add(1); // correct letter wrong position
            else
                result.Add(0); // incorrect letter
        }

        return result;
    }
}
