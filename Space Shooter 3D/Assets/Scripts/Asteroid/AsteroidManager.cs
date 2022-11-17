using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidManager : MonoBehaviour
{
    public static AsteroidManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        else Destroy(this);
    }

    public GameObject[] asteroidPrefabs;
    public float asteroideSpawnDistance = 50f;

    public float spawnTime = 2f;
    private float timer = 0f;

    [HideInInspector]
    public float minX, maxX, minY, maxY;

    public List<GameObject> aliveAsteroids = new List<GameObject>();

    void Start()
    {
        timer = spawnTime;
    }

    void Update()
    {
        timer += Time.deltaTime;
        if(timer >= spawnTime)
        {
            SpawnNewAsteroids();
            timer = 0f;
        }
    }

    public void SpawnNewAsteroids()
    {
        float newX = Random.Range(minX, maxX);
        float newY = Random.Range(minY, maxY);

        Vector3 spawnPos = new Vector3(newX, newY, asteroideSpawnDistance);

        GameObject GO = Instantiate(asteroidPrefabs[Random.Range(0, asteroidPrefabs.Length)], spawnPos, Quaternion.identity);
        aliveAsteroids.Add(GO);
    }

    public void UpdateAsteroids(List <GameObject> targetAsteroids)
    {
        foreach(GameObject asteroid in aliveAsteroids)
        {
            if (targetAsteroids.Contains(asteroid))
            {
                //Chamando função do outro script para deixar asteroide vermelho
                asteroid.GetComponent<AsteroidController>().SetTargetMaterial();

            }
            else
            {
                //resetar cor
                asteroid.GetComponent<AsteroidController>().ResetMaterial();
            }
        }
    }

}