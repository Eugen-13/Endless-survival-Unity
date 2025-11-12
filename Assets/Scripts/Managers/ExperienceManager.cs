using UnityEngine;
﻿using Unity.VisualScripting.FullSerializer;
public class ExperienceManager : MonoBehaviour
{
    public static ExperienceManager Instance;

    [SerializeField] private ExperienceCrystal _crystalPrefab;
    [SerializeField] private float _chance = 0.3f;
    [SerializeField] private int _poolSize = 20;

    private string _crystalPoolName = "CrystalPool1";

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void TrySpawnCrystal(int exp, Vector3 position, Vector2 scale)
    {
        float random = Random.Range(0.0f, 1.0f);
        if (random <= _chance)
        {
            var crystal = PoolManager.Instance.Get(_crystalPoolName, position, Quaternion.identity);
            crystal.GetComponent<ExperienceCrystal>().SetExpCount(exp);
        }
    }

    private void Start()
    {
        PoolManager.Instance.CreatePool(_crystalPoolName, _crystalPrefab.gameObject, _poolSize);
    }
}