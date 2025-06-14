using System.Collections.Generic;
using UnityEngine;

public class HintButtonController : MonoBehaviour
{
    [Header("Game References")]
    public GameManager gameManager;
    public MessageBoxController messageBox;

    private string hiddenWord;
    private List<Hint> hintList = new List<Hint>();
    private int nextHintIndex = 0;

    private class Hint
    {
        public char letter;
        public int position; // -1 = no position
        public Hint(char letter, int position)
        {
            this.letter = letter;
            this.position = position;
        }
    }

    void Start()
    {
        InitializeHints();
    }

    public void OnHintButtonClicked()
    {
        if (nextHintIndex >= hintList.Count)
        {
            messageBox.ShowMessage("No more hints. You're on your own now!");
            return;
        }

        Hint currentHint = hintList[nextHintIndex];
        string message;

        if (currentHint.position == -1)
        {
            message = $"One of the letters is \"{currentHint.letter}\".";
        }
        else
        {
            int displayPos = currentHint.position + 1;
            message = $"The letter \"{currentHint.letter}\" is in position {displayPos}.";
        }

        messageBox.ShowMessage(message);
        nextHintIndex++;
    }

    public void ResetHints()
    {
        InitializeHints();
    }

    private void InitializeHints()
    {
        if (gameManager == null || messageBox == null)
        {
            Debug.LogError("HintButtonController: Missing reference to GameManager or MessageBoxController.");
            return;
        }

        hiddenWord = gameManager.GetHiddenWord().ToUpper();
        nextHintIndex = 0;
        hintList.Clear();

        HashSet<char> uniqueLetters = new HashSet<char>();

        // Add vague letter hints
        for (int i = 0; i < hiddenWord.Length; i++)
        {
            char c = hiddenWord[i];
            if (!uniqueLetters.Contains(c))
            {
                uniqueLetters.Add(c);
                hintList.Add(new Hint(c, -1));
            }
        }

        // Add positional hints
        for (int i = 0; i < hiddenWord.Length; i++)
        {
            hintList.Add(new Hint(hiddenWord[i], i));
        }
    }
}
