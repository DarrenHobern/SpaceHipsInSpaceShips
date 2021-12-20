using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class ObjectSpawner : MonoBehaviour
{
    [SerializeField] private GameObject prefab;
    [Range(0f, 1f)][SerializeField] private float spawnChance;
    [MinMaxSlider(0f, 60f)][SerializeField] private Vector2 spawnInterval;
    [SerializeField] private float spawnRadius = 10f;

    private float nextSpawnTime = 3f;
    private float nextSpawnDelay = 0f;
    private Transform container;

    private void Start() {
        container = new GameObject($"{prefab.name} Container").transform;
        container.parent = transform;
        nextSpawnTime = Random.Range(0, 5f);
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > nextSpawnTime) {
            SpawnObject();
        }
    }

    private void SpawnObject() {
        nextSpawnDelay = Random.Range(spawnInterval.x, spawnInterval.y);
        nextSpawnTime = Time.time + nextSpawnDelay;
        Vector3 position = Random.onUnitSphere * spawnRadius;
        position.y = 0;
        Quaternion rotation = Quaternion.Euler(-position); // Initial rotation is pointing to origin
        GameObject go = Instantiate(prefab, position, rotation, container);
        go.GetComponent<Rigidbody>().AddForce(-position); // This is unsafe
    }

    /// <summary>
    /// Visualise the range
    /// </summary>
    private void OnDrawGizmosSelected() {
        Gizmos.color = new Color(1,1,1, 0.1f);
        Gizmos.DrawSphere(transform.position, spawnRadius);
    }
}
