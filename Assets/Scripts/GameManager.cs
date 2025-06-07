using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class GameManager : MonoBehaviour
{
    [Header("Managers")]
    public WordListManager wordListManager;
    public CloserWordsListManager closerWordsListManager;
    public KeyboardController keyboardController;
    public MessageBoxController messageBox; // 💬 new

    [Header("Top Row Letter Slots")]
    public List<TextMeshProUGUI> letterSlots; // Must have 5 elements
    private bool[] revealedLetters = new bool[5];

    private string hiddenWord = "";

    void Start()
    {
        if (wordListManager == null)
        {
            Debug.LogError("❌ WordListManager is not assigned.");
            return;
        }

        hiddenWord = wordListManager.GetRandomAnswer();
        Debug.Log($"[GameManager] Hidden word selected: {hiddenWord}");

        // 💬 Show initial message
        if (messageBox != null)
        {
            Debug.Log("[GameManager] Showing startup message.");
            messageBox.ShowMessage("Guess the five-letter word!");
        }
        else
        {
            Debug.LogWarning("[GameManager] messageBox is NULL.");
        }
    }

    public void SubmitGuess(string playerGuess)
    {
        playerGuess = playerGuess.ToUpper();

        if (playerGuess.Length != 5)
        {
            Debug.LogWarning("Guess must be exactly 5 letters.");
            messageBox?.ShowMessage("Enter 5 letters.");
            return;
        }

        if (!wordListManager.IsValidGuess(playerGuess))
        {
            Debug.LogWarning($"'{playerGuess}' is not a valid word.");
            messageBox?.ShowMessage("That's not a valid word!");
            return;
        }

        List<int> feedback = GenerateFeedback(playerGuess, hiddenWord);

        // 💬 Optional motivational message
        if (feedback.Any(f => f == 2))
        {
            messageBox?.ShowMessage("You're drifting closer!");
        }
        else if (feedback.Any(f => f == 1))
        {
            messageBox?.ShowMessage("You guessed a letter!");
        }
        else
        {
            messageBox?.ShowMessage("Keep trying!");
        }

        // Reveal letters in top row
        for (int i = 0; i < 5; i++)
        {
            if (feedback[i] == 2 && !revealedLetters[i] && i < letterSlots.Count)
            {
                revealedLetters[i] = true;
                StartCoroutine(FlipRevealLetter(i, playerGuess[i].ToString()));
            }
        }

        // Add to closer words list
        if (closerWordsListManager != null)
        {
            closerWordsListManager.AddGuess(playerGuess, feedback);
        }

        // Update keyboard
        if (keyboardController != null)
        {
            for (int i = 0; i < 5; i++)
            {
                keyboardController.UpdateKeyColor(playerGuess[i], feedback[i]);
            }
        }

        // Win condition
        if (playerGuess == hiddenWord)
        {
            Debug.Log("🎉 You guessed the hidden word!");
            messageBox?.ShowMessage("You reached the word!");
        }
    }

    private List<int> GenerateFeedback(string guess, string target)
    {
        List<int> result = new List<int> { 0, 0, 0, 0, 0 };
        bool[] targetUsed = new bool[5];

        // First pass: correct positions
        for (int i = 0; i < 5; i++)
        {
            if (guess[i] == target[i])
            {
                result[i] = 2;
                targetUsed[i] = true;
            }
        }

        // Second pass: correct letter, wrong position
        for (int i = 0; i < 5; i++)
        {
            if (result[i] == 0)
            {
                for (int j = 0; j < 5; j++)
                {
                    if (!targetUsed[j] && guess[i] == target[j])
                    {
                        result[i] = 1;
                        targetUsed[j] = true;
                        break;
                    }
                }
            }
        }

        return result;
    }

    private IEnumerator FlipRevealLetter(int index, string letter)
    {
        Transform tile = letterSlots[index].transform.parent;
        Image bg = tile.GetComponent<Image>();
        TextMeshProUGUI text = letterSlots[index];

        float flipTime = 0.2f;
        float elapsed = 0f;

        Vector3 startScale = tile.localScale;
        Vector3 midScale = new Vector3(1, 0, 1);

        // Flip out
        while (elapsed < flipTime)
        {
            tile.localScale = Vector3.Lerp(startScale, midScale, elapsed / flipTime);
            elapsed += Time.deltaTime;
            yield return null;
        }

        tile.localScale = midScale;
        text.text = letter;
        if (bg != null)
            bg.color = closerWordsListManager.correctPositionColor;

        // Flip in
        elapsed = 0f;
        while (elapsed < flipTime)
        {
            tile.localScale = Vector3.Lerp(midScale, startScale, elapsed / flipTime);
            elapsed += Time.deltaTime;
            yield return null;
        }

        tile.localScale = startScale;
    }
}
