using UnityEngine;

public class BirdController : MonoBehaviour
{
    public float speed = 0.5f;
    public float bobAmount = 0.02f;
    public float bobSpeed = 1f;

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        // Drift sideways
        transform.Translate(Vector3.right * speed * Time.deltaTime);

        // Bobbing effect
        float bobY = startPos.y + Mathf.Sin(Time.time * bobSpeed) * bobAmount;
        transform.position = new Vector3(transform.position.x, bobY, transform.position.z);

        // Destroy if offscreen
        if (transform.position.x > 10f) Destroy(gameObject); // Adjust if needed
    }
}
