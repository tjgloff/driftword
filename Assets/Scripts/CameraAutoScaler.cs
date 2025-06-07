using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraAutoScaler : MonoBehaviour
{
    [Header("Reference Resolution (your design resolution)")]
    public float targetWidth = 720f;
    public float targetHeight = 1280f;

    [Header("Scaling Options")]
    public bool matchWidth = false; // true = match width, false = match height

    private Camera cam;

    void Start()
    {
        cam = GetComponent<Camera>();

        float targetAspect = targetWidth / targetHeight;
        float windowAspect = (float)Screen.width / (float)Screen.height;

        float scaleFactor = matchWidth
            ? targetAspect / windowAspect
            : windowAspect / targetAspect;

        cam.orthographicSize = (targetHeight / 200f) * (matchWidth ? scaleFactor : 1f);
    }
}
