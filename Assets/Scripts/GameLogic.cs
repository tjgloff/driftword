using UnityEngine;
using System.Collections.Generic;

public class GameLogic : MonoBehaviour
{
    private string secretWord;
    private int guessesLeft;

    void Start()
    {
        secretWord = GameConstants.Words[Random.Range(0, GameConstants.Words.Count)];
        guessesLeft = GameConstants.MaxGuesses;
    }

    public bool SubmitGuess(string guess)
    {
        if (guess.Length != GameConstants.WordLength)
            return false;

        guessesLeft--;
        return guess.ToLower() == secretWord.ToLower();
    }
}