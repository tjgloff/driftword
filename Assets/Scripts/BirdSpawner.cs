using UnityEngine;

public class BirdSpawner : MonoBehaviour
{
    public GameObject birdPrefab;
    public int numberOfBirds = 10;
    public Vector2 xRange = new Vector2(-5f, 5f);
    public Vector2 yRange = new Vector2(3.5f, 5f);
    public Vector2 scaleRange = new Vector2(0.05f, 0.15f);
    public Vector2 speedRange = new Vector2(0.3f, 0.8f);

    void Start()
    {
        for (int i = 0; i < numberOfBirds; i++)
        {
            SpawnBird();
        }
    }

    void SpawnBird()
    {
        Vector3 spawnPos = new Vector3(
            Random.Range(xRange.x, xRange.y),
            Random.Range(yRange.x, yRange.y),
            0
        );

        GameObject bird = Instantiate(birdPrefab, spawnPos, Quaternion.identity, transform);

        float randomScale = Random.Range(scaleRange.x, scaleRange.y);
        bird.transform.localScale = Vector3.one * randomScale;

        BirdController controller = bird.GetComponent<BirdController>();
        if (controller != null)
        {
            controller.speed = Random.Range(speedRange.x, speedRange.y);
            controller.bobAmount = 0.01f * randomScale;
        }
    }
}
