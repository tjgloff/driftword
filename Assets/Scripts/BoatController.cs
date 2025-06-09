using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BoatController : MonoBehaviour
{
    [Header("References")]
    public RectTransform startPoint;
    public RectTransform endPoint;
    public RectTransform boatImage; // Assign the boat image here

    [Header("Movement Settings")]
    [Range(0f, 1f)]
    public float currentProgress = 0f;
    public float moveDuration = 0.5f;

    private Vector3 targetPosition;

    void Start()
    {
        if (boatImage == null) boatImage = GetComponent<RectTransform>();
        UpdateBoatPositionInstant();
    }

    public void MoveBoat(float progress)
    {
        currentProgress = Mathf.Clamp01(progress);
        targetPosition = Vector3.Lerp(startPoint.position, endPoint.position, currentProgress);
        StopAllCoroutines();
        StartCoroutine(SmoothMove());
    }

    private IEnumerator SmoothMove()
    {
        Vector3 initialPosition = boatImage.position;
        float elapsed = 0f;

        while (elapsed < moveDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / moveDuration);

            // Easing: ease in/out (smoother acceleration)
            float easedT = Mathf.SmoothStep(0f, 1f, t);

            boatImage.position = Vector3.Lerp(initialPosition, targetPosition, easedT);
            yield return null;
        }

        boatImage.position = targetPosition;
    }

    private void UpdateBoatPositionInstant()
    {
        if (startPoint != null && endPoint != null && boatImage != null)
        {
            boatImage.position = Vector3.Lerp(startPoint.position, endPoint.position, currentProgress);
        }
    }
}
