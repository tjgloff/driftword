using UnityEngine;
// Test change for GitHub Desktop

public class BirdController : MonoBehaviour
{
    public float speed = 1.0f;

    void Update()
    {
        transform.position += Vector3.left * speed * Time.deltaTime;
    }
}