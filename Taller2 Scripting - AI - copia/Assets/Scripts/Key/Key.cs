using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Key : MonoBehaviour {

    SpawnKey spawnKey;

    private void Start()
    {
        spawnKey = FindObjectOfType<SpawnKey>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 9)
        {
            spawnKey.Spawn();
        }
    }
}
