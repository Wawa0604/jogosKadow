using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PowerUpSpawner : MonoBehaviour
{
    public GameObject powerUpPrefab;
    public float spawnInterval = 5f;
    public Vector2Int mapaTamanho = new Vector2Int(3, 3); // Tamanho do mapa
    public float alturaSpawn = 1f; // Altura acima do plano para spawnar

    private float proximoSpawnTempo;

    void Start()
    {
        proximoSpawnTempo = Time.time + spawnInterval;
        // Spawn inicial de alguns power-ups (opcional)
        SpawnPowerUp();
        SpawnPowerUp();
    }

    void Update()
    {
        if (Time.time >= proximoSpawnTempo)
        {
            SpawnPowerUp();
            proximoSpawnTempo = Time.time + spawnInterval;
        }
    }

    void SpawnPowerUp()
    {
        // Calcula uma posição aleatória dentro do mapa
        float x = Random.Range(-mapaTamanho.x / 2f + 0.5f, mapaTamanho.x / 2f - 0.5f);
        float z = Random.Range(-mapaTamanho.y / 2f + 0.5f, mapaTamanho.y / 2f - 0.5f);
        Vector3 spawnPosicao = new Vector3(transform.position.x + x, transform.position.y + alturaSpawn, transform.position.z + z);

        // Instancia o power-up
        if (powerUpPrefab != null)
        {
            GameObject novoPowerUp = Instantiate(powerUpPrefab, spawnPosicao, Quaternion.identity);
            novoPowerUp.AddComponent<PowerUp>(); // Adiciona o script de comportamento ao power-up instanciado
            novoPowerUp.GetComponent<Collider>().isTrigger = true; // Garante que seja um trigger
            novoPowerUp.tag = "PowerUp"; // Garante a tag correta
        }
        else
        {
            Debug.LogError("O Power Up Prefab não foi atribuído no PowerUpSpawner!");
        }
    }
}