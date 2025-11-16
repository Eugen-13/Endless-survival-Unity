using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private void FixedUpdate()
    {
        var enemies = EnemyManager.Behaviours;
        for (int i = 0; i < enemies.Count; i++)
        {
            var e1 = enemies[i];
            e1.Behavior();

            for (int j = i + 1; j < enemies.Count; j++)
            {
                var e2 = enemies[j];
                Vector2 diff = e1.transform.position - e2.transform.position;
                float dist = diff.magnitude;
                if (dist < e1.RepelRadius && dist > 0.01f)
                {
                    diff.Normalize();
                    float force = (1f - dist / e1.RepelRadius) * e1.RepelForce * Time.fixedDeltaTime;
                    e1.Agent.Move(diff * force);
                    e2.Agent.Move(-diff * force);
                }
            }
        }
    }
}