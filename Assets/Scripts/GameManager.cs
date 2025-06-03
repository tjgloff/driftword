using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private WordListLoader wordListLoader;
    private string hiddenWord;

    [SerializeField] private GuessInput guessInput;           // Your input script (linked in inspector)
    [SerializeField] private GuessListManager guessList;      // Manages the list of recent guesses

    void Start()
    {
        wordListLoader = Object.FindFirstObjectByType<WordListLoader>();



        if (wordListLoader == null)
        {
            Debug.LogError("WordListLoader not found in scene!");
            return;
        }

        hiddenWord = wordListLoader.GetRandomSolution();
        Debug.Log("Hidden word is: " + hiddenWord);  // For testing
    }

    public void SubmitGuess(string guess)
    {
        guess = guess.ToLower().Trim();

        if (guess.Length != 5)
        {
            Debug.Log("Guess must be 5 letters.");
            return;
        }

        if (!wordListLoader.IsValidGuess(guess))
        {
            Debug.Log("Not a valid word.");
            return;
        }

        // Add to guess list
        if (guessList != null)
        {
            guessList.AddGuess(guess, GetFeedbackColors(guess));
        }

        if (guess == hiddenWord)
        {
            Debug.Log("Correct! You win!");
            // TODO: Trigger Victory UI or animation
        }
        else
        {
            Debug.Log("Keep trying!");
        }
    }

    /// <summary>
    /// Returns a list of color feedback codes: 
    /// 0 = gray (wrong letter), 
    /// 1 = light blue (right letter, wrong place), 
    /// 2 = dark blue (correct letter and position)
    /// </summary>
    private List<int> GetFeedbackColors(string guess)
    {
        List<int> result = new List<int>() { 0, 0, 0, 0, 0 };
        char[] answerChars = hiddenWord.ToCharArray();
        char[] guessChars = guess.ToCharArray();
        bool[] used = new bool[5];

        // Pass 1: Check for exact matches
        for (int i = 0; i < 5; i++)
        {
            if (guessChars[i] == answerChars[i])
            {
                result[i] = 2; // dark blue
                used[i] = true;
            }
        }

        // Pass 2: Check for correct letters in wrong positions
        for (int i = 0; i < 5; i++)
        {
            if (result[i] == 2) continue; // already matched

            for (int j = 0; j < 5; j++)
            {
                if (!used[j] && guessChars[i] == answerChars[j])
                {
                    result[i] = 1; // light blue
                    used[j] = true;
                    break;
                }
            }
        }

        return result;
    }
}
