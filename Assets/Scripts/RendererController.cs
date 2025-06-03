using UnityEngine;

public class RendererController : MonoBehaviour
{
    public void SetTileColor(GameObject tile, Color color)
    {
        var renderer = tile.GetComponent<SpriteRenderer>();
        if (renderer != null)
            renderer.color = color;
    }
}