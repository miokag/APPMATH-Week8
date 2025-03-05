using UnityEngine;

public class Item : MonoBehaviour
{
    public Vector3 itemPosition;
    private SpriteRenderer spriteRenderer;
    public float scaleThreshold = 0.5f;
    public float laneDistance = 4f; // Match with Player script

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            spriteRenderer.color = GetRandomColor();
        }
    }

    private void Update()
    {
        var perspective = CameraComponent.focalLength / (CameraComponent.focalLength + itemPosition.z);
        transform.localScale = Vector3.one * perspective;
        transform.position = new Vector2(itemPosition.x, itemPosition.y) * perspective;

        if (spriteRenderer != null)
        {
            spriteRenderer.sortingOrder = Mathf.RoundToInt(-itemPosition.z);
        }

        CheckCollisionWithPlayer();
    }

    private Color GetRandomColor()
    {
        return new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
    }

    private void CheckCollisionWithPlayer()
    {
        GameObject player = GameObject.FindWithTag("Player");
        if (player == null) return;

        Vector3 playerPos = player.transform.position;
        Vector3 playerScale = player.transform.localScale;
        Vector3 itemScale = transform.localScale;

        // Scale-based collision check
        if (Mathf.Abs(playerScale.x - itemScale.x) < scaleThreshold &&
            Mathf.Abs(playerScale.y - itemScale.y) < scaleThreshold &&
            Mathf.Abs(playerScale.z - itemScale.z) < scaleThreshold)
        {
            // Lane-based collision check
            float itemLaneX = Mathf.Round(transform.position.x / laneDistance) * laneDistance;
            float playerLaneX = Mathf.Round(playerPos.x / laneDistance) * laneDistance;

            if (Mathf.Abs(playerLaneX - itemLaneX) < 0.1f) // Small margin for accuracy
            {
                Debug.Log("Collision detected with Player!");
                Destroy(gameObject);
            }
        }
    }
}
