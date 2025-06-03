using UnityEngine;
using System.Collections.Generic;

public static class GameConstants
{
    public const int MaxGuesses = 6;
    public const int WordLength = 5;

    public static readonly List<string> Words = new List<string>
    {
        "apple", "drift", "brave", "cloud", "sword"
    };

    public static readonly Color CorrectColor = new Color32(106, 170, 100, 255);
    public static readonly Color PresentColor = new Color32(201, 180, 88, 255);
    public static readonly Color AbsentColor  = new Color32(120, 124, 126, 255);
}