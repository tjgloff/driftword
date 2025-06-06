using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class KeyboardInput : MonoBehaviour
{
    public TMP_Text guessInputField;
    public List<TextMeshProUGUI> letterSlots;
    public CloserWordsListManager guessListManager;
    public KeyboardController keyboardController;

    private string currentGuess = "";
    private string targetWord = "CRANE"; // change this as needed

    private bool[] revealedLetters = new bool[5];

    public void OnLetterButtonClick(string letter)
    {
        if (currentGuess.Length < 5)
        {
            currentGuess += letter.ToUpper();
            guessInputField.text = currentGuess;
        }
    }

    public void OnBackspace()
    {
        if (currentGuess.Length > 0)
        {
            currentGuess = currentGuess.Substring(0, currentGuess.Length - 1);
            guessInputField.text = currentGuess;
        }
    }

    public void OnSubmit()
    {
        if (currentGuess.Length != 5) return;

        List<int> feedback = new List<int> { 0, 0, 0, 0, 0 };
        char[] targetChars = targetWord.ToCharArray();
        bool[] matched = new bool[5];

        // First pass: correct position
        for (int i = 0; i < 5; i++)
        {
            if (currentGuess[i] == targetWord[i])
            {
                feedback[i] = 2;
                matched[i] = true;
                targetChars[i] = '*';
            }
        }

        // Second pass: correct letter, wrong position
        for (int i = 0; i < 5; i++)
        {
            if (feedback[i] == 0)
            {
                for (int j = 0; j < 5; j++)
                {
                    if (!matched[j] && currentGuess[i] == targetChars[j])
                    {
                        feedback[i] = 1;
                        matched[j] = true;
                        break;
                    }
                }
            }
        }

        // Update guess row with flip animation
        for (int i = 0; i < letterSlots.Count; i++)
        {
            if (feedback[i] == 2 && !revealedLetters[i])
            {
                revealedLetters[i] = true;
                StartCoroutine(FlipRevealLetter(i, currentGuess[i].ToString(), guessListManager.correctPositionColor));
            }
        }

        // Add to scrolling list
        guessListManager?.AddGuess(currentGuess, feedback);

        // Update keyboard
        if (keyboardController != null)
        {
            for (int i = 0; i < 5; i++)
            {
                keyboardController.UpdateKeyColor(currentGuess[i], feedback[i]);
            }
        }

        currentGuess = "";
        guessInputField.text = "";
    }

    public void OnHint()
    {
        // Placeholder for future hint logic
    }

    private IEnumerator FlipRevealLetter(int index, string letter, Color color)
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
        if (bg != null) bg.color = color;

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
