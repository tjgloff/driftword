using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class WordListManager : MonoBehaviour
{
    public List<string> answerWords { get; private set; } = new List<string>();
    public HashSet<string> validGuesses { get; private set; } = new HashSet<string>();

    void Awake()
    {
        LoadWordLists();
    }

    private void LoadWordLists()
    {
        Debug.Log("[WordListManager] Loading word lists...");

        // Load answers
        TextAsset answersAsset = Resources.Load<TextAsset>("answerlist");
        if (answersAsset == null)
        {
            Debug.LogError("Missing file: answerlist.txt in Resources folder.");
        }
        else
        {
            answerWords = ProcessTextAsset(answersAsset);
            Debug.Log($"[WordListManager] Loaded {answerWords.Count} answer words.");
        }

        // Load guesses
        TextAsset guessesAsset = Resources.Load<TextAsset>("wordlist");
        if (guessesAsset == null)
        {
            Debug.LogError("Missing file: wordlist.txt in Resources folder.");
        }
        else
        {
            validGuesses = new HashSet<string>(ProcessTextAsset(guessesAsset));
            Debug.Log($"[WordListManager] Loaded {validGuesses.Count} valid guess words.");
        }
    }

    private List<string> ProcessTextAsset(TextAsset textAsset)
    {
        return textAsset.text
            .Split(new[] { '\r', '\n' }, System.StringSplitOptions.RemoveEmptyEntries)
            .Select(word => word.Trim().ToUpper())
            .ToList();
    }

    public bool IsValidGuess(string guess)
    {
        return validGuesses.Contains(guess.ToUpper());
    }

    public string GetRandomAnswer()
    {
        if (answerWords.Count == 0)
        {
            Debug.LogError("Answer list is empty!");
            return "CRANE"; // fallback
        }

        int index = Random.Range(0, answerWords.Count);
        return answerWords[index];
    }
}
