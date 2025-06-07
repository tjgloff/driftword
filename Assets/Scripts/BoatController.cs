using UnityEngine;

public class BoatController : MonoBehaviour
{
    [SerializeField] private Transform startPoint;
    [SerializeField] private Transform endPoint;

    private float currentProgress = 0f; // Value between 0.0 and 1.0

    public void MoveBoatByPercent(float percent)
    {
        currentProgress += percent;
        currentProgress = Mathf.Clamp01(currentProgress); // Keep between 0 and 1

        transform.position = Vector3.Lerp(startPoint.position, endPoint.position, currentProgress);
    }
}
