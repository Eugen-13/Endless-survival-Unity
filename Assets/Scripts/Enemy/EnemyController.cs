using System.Collections.Generic;
using EnemyTypes;
using Managers;
using UnityEngine;
using Zenject;

public class EnemyController : ITickable
{
    private readonly EnemyManager _enemyManager;

    public EnemyController(EnemyManager enemyManager)
    {
        _enemyManager = enemyManager;
    }

    public void Tick()
    {
        List<BaseEnemy> enemies = _enemyManager.Behaviours;

        for (int i = 0; i < enemies.Count; i++)
        {
            BaseEnemy e1 = enemies[i];
            e1.Behavior();

            Vector3 pos1 = e1.transform.position;

            for (int j = i + 1; j < enemies.Count; j++)
            {
                BaseEnemy e2 = enemies[j];
                Vector2 diff = pos1 - e2.transform.position;
                float sqrDist = diff.sqrMagnitude;
                float radius = Mathf.Max(e1.RepelRadius, e2.RepelRadius);
                float sqrRadius = radius * radius;
                if (sqrDist < sqrRadius || sqrDist < 0.0001f)
                {
                    diff.Normalize();
                    float force = (1f - (Mathf.Sqrt(sqrDist) / e1.RepelRadius)) * e1.RepelForce * Time.fixedDeltaTime;
                    e1.Agent.Move(diff * force);
                    e2.Agent.Move(-diff * force);
                }
            }
        }
    }
}
