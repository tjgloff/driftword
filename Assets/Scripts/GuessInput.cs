using UnityEngine;

public class GuessInput : MonoBehaviour
{
    public GameManager gameManager;

    public void SubmitGuess(string guess)
    {
        if (gameManager != null)
        {
            gameManager.SubmitGuess(guess);
        }
    }
}
