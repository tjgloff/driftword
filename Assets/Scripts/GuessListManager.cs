using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GuessListManager : MonoBehaviour
{
    public Transform listParent;          // The container where guess entries appear
    public GameObject guessEntryPrefab;   // The prefab that represents one guess row

    private const int maxGuesses = 4;

    public void AddGuess(string word, List<int> feedbackColors)
    {
        // Create a new guess entry
        GameObject newEntry = Instantiate(guessEntryPrefab, listParent);

        // Populate the entry with letters and colors
        for (int i = 0; i < 5; i++)
        {
            Transform letterTile = newEntry.transform.GetChild(i);
            Text letterText = letterTile.GetComponentInChildren<Text>();
            Image background = letterTile.GetComponent<Image>();

            if (letterText != null) letterText.text = word[i].ToString().ToUpper();

            if (background != null)
            {
                switch (feedbackColors[i])
                {
                    case 0:
                        background.color = Color.gray;
                        break;
                    case 1:
                        background.color = new Color(0.6f, 0.8f, 1f); // light blue
                        break;
                    case 2:
                        background.color = new Color(0.2f, 0.4f, 0.8f); // dark blue
                        break;
                }
            }
        }

        // If we exceed max guesses, delete the oldest one
        if (listParent.childCount > maxGuesses)
        {
            Destroy(listParent.GetChild(0).gameObject);
        }
    }
}
