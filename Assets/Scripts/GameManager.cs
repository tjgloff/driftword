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

    [Header("Top Row Letter Slots")]
    public List<TextMeshProUGUI> letterSlots; // Must be 5 elements
    private bool[] revealedLetters = new bool[5]; // Tracks which letters have been revealed

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
    }

    public void SubmitGuess(string playerGuess)
    {
        playerGuess = playerGuess.ToUpper();

        if (playerGuess.Length != 5)
        {
            Debug.LogWarning("Guess must be exactly 5 letters.");
            return;
        }

        if (!wordListManager.IsValidGuess(playerGuess))
        {
            Debug.LogWarning($"'{playerGuess}' is not a valid word. Guess rejected.");
            return;
        }

        List<int> feedback = GenerateFeedback(playerGuess, hiddenWord);

        // Reveal correct letters in guess row
        for (int i = 0; i < 5; i++)
        {
            if (feedback[i] == 2 && !revealedLetters[i])
            {
                revealedLetters[i] = true;
                StartCoroutine(FlipRevealLetter(i, playerGuess[i].ToString()));
            }
        }

        // Update the closer words list
        if (closerWordsListManager != null)
        {
            closerWordsListManager.AddGuess(playerGuess, feedback);
        }

        // Update keyboard colors
        if (keyboardController != null)
        {
            for (int i = 0; i < 5; i++)
            {
                keyboardController.UpdateKeyColor(playerGuess[i], feedback[i]);
            }
        }

        if (playerGuess == hiddenWord)
        {
            Debug.Log("🎉 You guessed the hidden word!");
            // TODO: Trigger victory animation
        }
    }

    private List<int> GenerateFeedback(string guess, string target)
    {
        List<int> result = new List<int> { 0, 0, 0, 0, 0 };
        bool[] targetUsed = new bool[5];

        // First pass: correct letter & position
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

        // Shrink (flip out)
        while (elapsed < flipTime)
        {
            tile.localScale = Vector3.Lerp(startScale, midScale, elapsed / flipTime);
            elapsed += Time.deltaTime;
            yield return null;
        }

        tile.localScale = midScale;
        text.text = letter;
        if (bg != null) bg.color = closerWordsListManager.correctPositionColor;

        // Expand (flip in)
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
