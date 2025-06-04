using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteInEditMode]
public class KeyboardAutoLinker : MonoBehaviour
{
    public KeyboardController keyboardController;

#if UNITY_EDITOR
    [ContextMenu("Auto-Fill Keyboard Keys")]
    public void AutoFillKeys()
    {
        if (keyboardController == null)
        {
            Debug.LogError("KeyboardController not assigned.");
            return;
        }

        List<KeyboardController.KeyButton> foundKeys = new List<KeyboardController.KeyButton>();

        Button[] buttons = GetComponentsInChildren<Button>(true);
        foreach (Button button in buttons)
        {
            TextMeshProUGUI tmpText = button.GetComponentInChildren<TextMeshProUGUI>();
            if (tmpText == null) continue;

            string label = tmpText.text.Trim().ToUpper();

            // Skip non-letter keys
            if (label.Length != 1 || label[0] < 'A' || label[0] > 'Z') continue;

            KeyboardController.KeyButton newKey = new KeyboardController.KeyButton
            {
                letter = label,
                keyImage = button.GetComponent<Image>(),
                keyText = tmpText,
                currentFeedback = -1
            };

            foundKeys.Add(newKey);
        }

        keyboardController.keys = foundKeys;
        EditorUtility.SetDirty(keyboardController);
        Debug.Log($"Auto-filled {foundKeys.Count} keyboard keys.");
    }
#endif
}
