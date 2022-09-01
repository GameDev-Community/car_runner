using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace ZombieAI
{
    public class ZombieAIManager : MonoBehaviour
    {
        [SerializeField] private ZombieAI zombieAIPrefab;
        [SerializeField] private Transform zombieHolder;
        [SerializeField] private int zombieCount;
        [SerializeField] private Transform[] spawnPoints;
        [SerializeField] private Transform[] targetPoints;

        private void Start()
        {
            for (int i = 0; i < zombieCount; i++)
            {
                SpawnZombie();
            }
        }

        private void SpawnZombie()
        {
            Vector3 spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)].position;
            ZombieAI zombieAI = Instantiate(zombieAIPrefab, spawnPoint, Quaternion.identity, zombieHolder);
            zombieAI.Initialize(this);
        }

        public Vector3 GenerateTargetPosition() => targetPoints[Random.Range(0, targetPoints.Length)].position;
    }
}