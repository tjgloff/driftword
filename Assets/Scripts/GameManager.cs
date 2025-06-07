using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public CloserWordsListManager closerWordsListManager;
<<<<<<< HEAD
    public BoatController boatController;

    private string hiddenWord = "CRANE"; // You can change this to any 5-letter word
=======
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
>>>>>>> parent of 1d5a57e (Message box added)

    public void SubmitGuess(string playerGuess)
    {
        string guess = playerGuess.ToUpper();
        string target = hiddenWord.ToUpper();

<<<<<<< HEAD
        // Generate feedback for each letter (0 = wrong, 1 = right letter, 2 = right place)
        List<int> feedback = GenerateFeedback(guess, target);

        // Update the Closer Words List
=======
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
<<<<<<< HEAD
<<<<<<< HEAD
>>>>>>> parent of 1d5a57e (Message box added)
=======
>>>>>>> parent of 1d5a57e (Message box added)
=======
>>>>>>> parent of 1d5a57e (Message box added)
        if (closerWordsListManager != null)
        {
            closerWordsListManager.AddGuess(guess, feedback);
        }

<<<<<<< HEAD
<<<<<<< HEAD
<<<<<<< HEAD
        // Move the boat based on feedback
        float movePercent = CalculateBoatMovement(guess, target);
        if (boatController != null)
=======
=======
>>>>>>> parent of 1d5a57e (Message box added)
=======
>>>>>>> parent of 1d5a57e (Message box added)
        // Update keyboard colors
        if (keyboardController != null)
>>>>>>> parent of 1d5a57e (Message box added)
        {
            boatController.MoveBoatByPercent(movePercent);
        }

<<<<<<< HEAD
<<<<<<< HEAD
<<<<<<< HEAD
        // Check for win
        if (guess == target)
        {
            Debug.Log("🎉 You guessed it!");
            // TODO: Add victory animation or message
=======
=======
>>>>>>> parent of 1d5a57e (Message box added)
=======
>>>>>>> parent of 1d5a57e (Message box added)
        if (playerGuess == hiddenWord)
        {
            Debug.Log("🎉 You guessed the hidden word!");
            // TODO: Trigger victory animation
<<<<<<< HEAD
<<<<<<< HEAD
>>>>>>> parent of 1d5a57e (Message box added)
=======
>>>>>>> parent of 1d5a57e (Message box added)
=======
>>>>>>> parent of 1d5a57e (Message box added)
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

<<<<<<< HEAD
<<<<<<< HEAD
<<<<<<< HEAD
        for (int i = 0; i < guess.Length; i++)
=======
=======
>>>>>>> parent of 1d5a57e (Message box added)
=======
>>>>>>> parent of 1d5a57e (Message box added)
        // First pass: correct letter & position
        for (int i = 0; i < 5; i++)
>>>>>>> parent of 1d5a57e (Message box added)
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
<<<<<<< HEAD
=======

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
>>>>>>> parent of 1d5a57e (Message box added)
}
