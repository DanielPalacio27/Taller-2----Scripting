using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnKey : MonoBehaviour {

    [SerializeField] GameObject key;
    [SerializeField] TerrainCollider terrainCollider;
    Vector3 offset;

    void Start()
    {
        offset = new Vector3(key.transform.localScale.x / 2f, 0f, key.transform.localScale.z / 2f);
        Spawn();
    }	

    public void Spawn()
    {
        Vector3 min = terrainCollider.bounds.min + offset;
        Vector3 max = terrainCollider.bounds.max - offset;
        Vector3 randomPosition = new Vector3(Random.Range(min.x, max.x), max.y, Random.Range(min.z, max.z));
        key.transform.position = randomPosition;
    }

}
