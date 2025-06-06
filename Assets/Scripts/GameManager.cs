using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GameManager : MonoBehaviour
{
    public CloserWordsListManager closerWordsListManager;

    private List<string> answerWords = new List<string>();
    private HashSet<string> validGuesses = new HashSet<string>();
    private string hiddenWord = "";

    void Start()
    {
        LoadWordLists();
        SelectRandomHiddenWord();
    }

    private void LoadWordLists()
    {
        // Load answer words
        TextAsset answersFile = Resources.Load<TextAsset>("answerlist");
        answerWords = answersFile.text
            .Split(new[] { '\r', '\n' }, System.StringSplitOptions.RemoveEmptyEntries)
            .Select(word => word.ToUpper())
            .ToList();

        // Load all valid guesses
        TextAsset guessesFile = Resources.Load<TextAsset>("wordlist");
        validGuesses = new HashSet<string>(
            guessesFile.text
            .Split(new[] { '\r', '\n' }, System.StringSplitOptions.RemoveEmptyEntries)
            .Select(word => word.ToUpper())
        );
    }

    private void SelectRandomHiddenWord()
    {
        int randomIndex = Random.Range(0, answerWords.Count);
        hiddenWord = answerWords[randomIndex];
        Debug.Log($"[Driftword] Hidden Word: {hiddenWord}"); // TEMP — remove in production
    }

    public void SubmitGuess(string playerGuess)
    {
        playerGuess = playerGuess.ToUpper();

        if (!validGuesses.Contains(playerGuess))
        {
            Debug.LogWarning($"'{playerGuess}' is not a valid word.");
            return;
        }

        List<int> feedback = GenerateFeedback(playerGuess, hiddenWord);

        if (closerWordsListManager != null)
        {
            closerWordsListManager.AddGuess(playerGuess, feedback);
        }

        // TODO: Add boat movement logic based on feedback

        if (playerGuess == hiddenWord)
        {
            Debug.Log("🎉 You guessed it!");
            // TODO: Trigger game victory logic
        }
    }

    private List<int> GenerateFeedback(string guess, string target)
    {
        List<int> result = new List<int>();

        for (int i = 0; i < guess.Length; i++)
        {
            if (guess[i] == target[i])
                result.Add(2); // correct letter & position
            else if (target.Contains(guess[i]))
                result.Add(1); // correct letter, wrong position
            else
                result.Add(0); // letter not in word
        }

        return result;
    }
}
