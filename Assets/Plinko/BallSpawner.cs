using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallSpawner : MonoBehaviour
{

    public GameObject ballPrefab;
    // Start is called before the first frame update
    void Start()
    {
        Vector3 spawnPos = GetComponent<Transform>().position;
        Instantiate(ballPrefab, spawnPos, Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Space)) {
            Vector3 spawnPos = GetComponent<Transform>().position;
            spawnPos.x = Random.Range(0,27);
            Instantiate(ballPrefab, spawnPos, Quaternion.identity);
        }
    }
}
