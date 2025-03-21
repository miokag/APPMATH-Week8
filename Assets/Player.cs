using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float jumpForce = 5f;
    public float laneSwitchSpeed = 5f;
    public int currentLane = 1; // 0: left, 1: middle, 2: right
    public float laneDistance = 3f; // Increased lane distance
    public bool isJumping = false;
    
    

    private Vector3 targetPosition;

    private void Start()
    {
        targetPosition = transform.position;
    }

    private void Update()
    {
        HandleInput();
        MoveToTarget();
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

    

}