using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject[] enemyprefabs;
    public Transform[] spawnpoints;
    public float spawnrate;
    public AudioManager am;
    public GameObject spawneff;
    public  Transform player;
    public GameObject boss;

    void spawn()
    {
        int i = Random.Range(0, enemyprefabs.Length);
        foreach(Transform pos in spawnpoints)
        {
            if (boss == null)
                return;
            Instantiate(spawneff, pos.position, pos.rotation);
            GameObject enemy = Instantiate(enemyprefabs[i], pos.position, pos.rotation);
            enemy.GetComponent<TankMovement>().player = player;
            enemy.GetComponent<TankMovement>().am = am;
        }
    }
    void Start()
    {
        InvokeRepeating("spawn", 2f, spawnrate);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
