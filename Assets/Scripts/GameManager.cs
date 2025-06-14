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
    public MessageBoxController messageBox;
    public HintButtonController hintButtonController;

    [Header("Top Row Letter Slots")]
    public List<TextMeshProUGUI> letterSlots; // Must have 5 elements
    private bool[] revealedLetters = new bool[5];

    [Header("Boat")]
    public BoatController boatController;

    private string hiddenWord = "";

    // NEW: Track which letters and positions have already been credited
    private HashSet<char> globallyCreditedLetters = new HashSet<char>();
    private bool[] globallyCreditedPositions = new bool[5];

    void Start()
    {
        if (wordListManager == null)
        {
            Debug.LogError("❌ WordListManager is not assigned.");
            return;
        }

        hiddenWord = wordListManager.GetRandomAnswer();
        Debug.Log($"[GameManager] Hidden word selected: {hiddenWord}");

        if (messageBox != null)
        {
            messageBox.ShowMessage("Guess the five-letter word!");
        }

        if (hintButtonController != null)
        {
            hintButtonController.ResetHints();
        }

        // Reset credit trackers
        globallyCreditedLetters.Clear();
        for (int i = 0; i < 5; i++) globallyCreditedPositions[i] = false;
        for (int i = 0; i < 5; i++) revealedLetters[i] = false;
    }

    public void SubmitGuess(string playerGuess)
    {
        playerGuess = playerGuess.ToUpper();

        if (playerGuess.Length != 5)
        {
            messageBox?.ShowMessage("Enter 5 letters.");
            return;
        }

        if (!wordListManager.IsValidGuess(playerGuess))
        {
            messageBox?.ShowMessage("That's not a valid word!");
            return;
        }

        List<int> feedback = GenerateFeedback(playerGuess, hiddenWord);

        // Show motivational message
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

        // NEW: Accurate one-time credit for each correct letter and position
        float progressScore = 0f;

        for (int i = 0; i < 5; i++)
        {
            char guessedLetter = playerGuess[i];

            if (feedback[i] == 2)
            {
                // Position credit (only once)
                if (!globallyCreditedPositions[i])
                {
                    globallyCreditedPositions[i] = true;
                    progressScore += 0.1f;
                }

                // Letter credit (only once)
                if (!globallyCreditedLetters.Contains(guessedLetter))
                {
                    globallyCreditedLetters.Add(guessedLetter);
                    progressScore += 0.1f;
                }

                // Reveal correct-position letter
                if (!revealedLetters[i])
                {
                    revealedLetters[i] = true;
                    if (i < letterSlots.Count)
                    {
                        StartCoroutine(FlipRevealLetter(i, guessedLetter.ToString()));
                    }
                }
            }
            else if (feedback[i] == 1)
            {
                // Letter credit (only once)
                if (!globallyCreditedLetters.Contains(guessedLetter))
                {
                    globallyCreditedLetters.Add(guessedLetter);
                    progressScore += 0.1f;
                }
            }
        }

        // Move the boat only if any new credit was earned
        if (boatController != null && progressScore > 0f)
        {
            boatController.MoveBoat(progressScore);
        }

        // Update closer words list
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
            messageBox?.ShowMessage("You reached the word!");
        }
    }

    public string GetHiddenWord()
    {
        return hiddenWord;
    }

    private List<int> GenerateFeedback(string guess, string target)
    {
        List<int> result = new List<int> { 0, 0, 0, 0, 0 };
        bool[] targetUsed = new bool[5];

        // First pass: correct letters in correct positions
        for (int i = 0; i < 5; i++)
        {
            if (guess[i] == target[i])
            {
                result[i] = 2;
                targetUsed[i] = true;
            }
        }

        // Second pass: correct letters in wrong positions
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
