using UnityEngine;
using TMPro;
using System.Collections;

[RequireComponent(typeof(CanvasGroup))]
public class MessageBoxController : MonoBehaviour
{
    public TextMeshProUGUI messageText;
    public float fadeDuration = 0.5f;
    public float displayDuration = 2f;

    private CanvasGroup canvasGroup;

    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        if (messageText == null)
            messageText = GetComponent<TextMeshProUGUI>();
    }

    public void ShowMessage(string message)
    {
        StopAllCoroutines();
        StartCoroutine(FadeMessage(message));
    }

    private IEnumerator FadeMessage(string message)
    {
        messageText.text = message;

        float t = 0f;
        while (t < fadeDuration)
        {
            canvasGroup.alpha = Mathf.Lerp(0, 1, t / fadeDuration);
            t += Time.deltaTime;
            yield return null;
        }
        canvasGroup.alpha = 1;

        yield return new WaitForSeconds(displayDuration);

        t = 0f;
        while (t < fadeDuration)
        {
            canvasGroup.alpha = Mathf.Lerp(1, 0, t / fadeDuration);
            t += Time.deltaTime;
            yield return null;
        }
        canvasGroup.alpha = 0;
    }
}
