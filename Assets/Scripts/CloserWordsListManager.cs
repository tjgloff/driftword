using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CloserWordsListManager : MonoBehaviour
{
    public GameObject guessEntryPrefab; // The GuessEntry prefab to instantiate
    public Transform listContent;       // The Content object inside the ScrollView
    public Color correctPositionColor;  // Color for correct letter & correct position
    public Color correctLetterColor;    // Color for correct letter but wrong position
    public Color incorrectColor;        // Color for incorrect letter

    // Call this to add a new guess with feedback
    public void AddGuess(string guess, List<int> feedback)
    {
        GameObject entry = Instantiate(guessEntryPrefab, listContent);
        entry.transform.SetAsFirstSibling(); // Add new entries to the top

        for (int i = 0; i < 5; i++)
        {
            char guessChar = guess[i];

            // Find child named LetterBox1, LetterBox2, etc.
            Transform box = entry.transform.Find($"LetterBox{i + 1}");
            if (box == null) continue;

            // Set the letter text
            TextMeshProUGUI letterText = box.GetComponentInChildren<TextMeshProUGUI>();
            if (letterText != null)
                letterText.text = guessChar.ToString().ToUpper();

            // Set the background color based on feedback value
            Image boxImage = box.GetComponent<Image>();
            if (boxImage != null)
            {
                switch (feedback[i])
                {
                    case 2:
                        boxImage.color = correctPositionColor;
                        break;
                    case 1:
                        boxImage.color = correctLetterColor;
                        break;
                    default:
                        boxImage.color = incorrectColor;
                        break;
                }
            }
        }
    }
}
