using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ItemSpawner : MonoBehaviour
{
    public Item itemToSpawn;
    public float speed = 0.01f;
    public List<Item> items = new List<Item>();
    public Transform parent;
    public float maxHorizontal;
    public float minHorizontal;
    public float maxVertical;
    public float minVertical;
    public float removeAtDistance;

    [Header("Spawning Settings")]
    public float minSpawnDelay = 0.1f; // Minimum spawn delay
    public float maxSpawnDelay = 0.5f; // Maximum spawn delay
    public float initialDelay = 1f; // Initial delay before spawning starts

    private void Start()
    {
        // Start spawning with initial delay
        Invoke(nameof(SpawnWithRandomDelay), initialDelay);
    }

    private void SpawnWithRandomDelay()
    {
        // Spawn an item
        BeginSpawn();

        // Schedule the next spawn with a random delay
        float nextDelay = Random.Range(minSpawnDelay, maxSpawnDelay);
        Invoke(nameof(SpawnWithRandomDelay), nextDelay);
    }

    private void BeginSpawn()
    {
        var spawnedItem = Instantiate(itemToSpawn, parent);
        spawnedItem.itemPosition = GetRandomLocation();
        items.Add(spawnedItem);
    }

    private Vector3 GetRandomLocation()
    {
        float xRand = Random.Range(minHorizontal, maxHorizontal);
        float yRand = Random.Range(minVertical, maxVertical);

        // Find the farthest item in the list
        float maxZ = items.Count > 0 ? items.Max(item => item.itemPosition.z) : 10f;

        // Spawn the new item behind the farthest item
        return new Vector3(xRand, yRand, maxZ + 100f); // Spawn far behind the farthest item
    }

    private void Update()
    {
        for (int i = items.Count - 1; i >= 0; i--)
        {
            Item item = items[i];
            ItemMover(item);

            if (item.itemPosition.z < removeAtDistance)
            {
                Destroy(item.gameObject);
                items.RemoveAt(i);
            }
        }
    }

    private void ItemMover(Item item)
    {
        item.itemPosition.z -= speed;
    }

    // Method to update the spawn delay range dynamically
    public void SetSpawnDelayRange(float newMinDelay, float newMaxDelay)
    {
        if (newMinDelay > 0 && newMaxDelay > newMinDelay)
        {
            minSpawnDelay = newMinDelay;
            maxSpawnDelay = newMaxDelay;
        }
    }
}