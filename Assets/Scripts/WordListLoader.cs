using System.Collections.Generic;
using UnityEngine;

public class WordListLoader : MonoBehaviour
{
    public List<string> solutionWords;
    public HashSet<string> allowedGuesses;

    void Awake()
    {
        LoadWordLists();
    }

    void LoadWordLists()
    {
        TextAsset solutionFile = Resources.Load<TextAsset>("wordle_solutions");
        TextAsset guessFile = Resources.Load<TextAsset>("wordle_guesses");

        solutionWords = new List<string>(solutionFile.text.Split('\n'));
        allowedGuesses = new HashSet<string>(guessFile.text.Split('\n'));

        // Trim whitespace
        for (int i = 0; i < solutionWords.Count; i++)
            solutionWords[i] = solutionWords[i].Trim().ToLower();

        var cleanedGuesses = new List<string>();
        foreach (var word in allowedGuesses)
            cleanedGuesses.Add(word.Trim().ToLower());

        allowedGuesses = new HashSet<string>(cleanedGuesses);
    }

    public bool IsValidGuess(string word)
    {
        return allowedGuesses.Contains(word.ToLower());
    }

    public string GetRandomSolution()
    {
        return solutionWords[Random.Range(0, solutionWords.Count)];
    }
}
