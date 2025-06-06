using System.Collections.Generic;
using UnityEngine;

public class WordListManager : MonoBehaviour
{
    public static WordListManager Instance;

    private HashSet<string> validWords;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadWordLists();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void LoadWordLists()
    {
        validWords = new HashSet<string>();

        // Load guesses
        TextAsset guessList = Resources.Load<TextAsset>("wordlist");
        AddWordsFromTextAsset(guessList);

        // Load valid solutions
        TextAsset solutionList = Resources.Load<TextAsset>("solutionlist");
        AddWordsFromTextAsset(solutionList);
    }

    private void AddWordsFromTextAsset(TextAsset textAsset)
    {
        if (textAsset != null)
        {
            string[] words = textAsset.text.Split(new[] { '\n', '\r' }, System.StringSplitOptions.RemoveEmptyEntries);
            foreach (string word in words)
            {
                validWords.Add(word.Trim().ToLower());
            }
        }
        else
        {
            Debug.LogError("Missing word list file.");
        }
    }

    public bool IsValidWord(string word)
    {
        return validWords.Contains(word.ToLower());
    }
}
