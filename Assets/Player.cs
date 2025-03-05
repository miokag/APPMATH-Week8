using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float jumpForce = 5f;
    public float laneSwitchSpeed = 5f;
    public int currentLane = 1; // 0: left, 1: middle, 2: right
    public float laneDistance = 3f; // Increased lane distance
    public bool isJumping = false;
    public float scaleThreshhold = 0.5f;

    private Vector3 targetPosition;

    private void Start()
    {
        targetPosition = transform.position;
    }

    private void Update()
    {
        HandleInput();
        MoveToTarget();
        CheckCollisions();
    }

    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow) && !isJumping)
        {
            StartCoroutine(Jump());
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            MoveLane(-1);
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            MoveLane(1);
        }
    }

    private IEnumerator Jump()
    {
        isJumping = true;
        float jumpTime = 0.5f;
        float elapsedTime = 0f;
        Vector3 startPosition = transform.position;

        while (elapsedTime < jumpTime)
        {
            float yOffset = Mathf.Sin(elapsedTime / jumpTime * Mathf.PI) * jumpForce;
            transform.position = startPosition + new Vector3(0, yOffset, 0); // Manual Y movement
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = startPosition;
        isJumping = false;
    }

    private void MoveLane(int direction)
    {
        currentLane = Mathf.Clamp(currentLane + direction, 0, 2);
        targetPosition = new Vector3((currentLane - 1) * laneDistance, transform.position.y, transform.position.z);
    }

    private void MoveToTarget()
    {
        // Manual movement towards target position
        Vector3 direction = targetPosition - transform.position;
        float distance = Mathf.Sqrt(direction.x * direction.x + direction.y * direction.y + direction.z * direction.z); // Manual distance calculation
        if (distance > 0.01f)
        {
            Vector3 movement = direction / distance * laneSwitchSpeed * Time.deltaTime; // Normalize and scale
            transform.position = transform.position + movement;
        }
    }

    private void CheckCollisions()
    {
        foreach (Item item in FindObjectOfType<ItemSpawner>().items)
        {
            // Get item's scale using the same perspective formula
            float itemScale = CameraComponent.focalLength / (CameraComponent.focalLength + item.itemPosition.z);
            float playerScale = transform.localScale.x; // Assuming uniform scaling

            // Check if the player's scale is close to the item's scale
            if (Mathf.Abs(playerScale - itemScale) < scaleThreshhold)
            {
                // Check if the player and item are in the same lane
                float itemLaneX = Mathf.Round(item.itemPosition.x / laneDistance) * laneDistance;
                float playerLaneX = transform.position.x;

                if (Mathf.Abs(playerLaneX - itemLaneX) < 0.1f) // Allow small margin
                {
                    Debug.Log("Collision detected!");
                    // Handle collision (e.g., destroy item, trigger event)
                    Destroy(item.gameObject);
                }
            }
        }
    }

}