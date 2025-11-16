using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static readonly List<Transform> Enemies = new();
    public static readonly List<BaseEnemy> Behaviours = new();

    public static void Register(BaseEnemy enemy)
    {
        if (!Enemies.Contains(enemy.transform))
            Enemies.Add(enemy.transform);

        if (!Behaviours.Contains(enemy))
            Behaviours.Add(enemy);
    }

    public static void Unregister(BaseEnemy enemy)
    {
        Enemies.Remove(enemy.transform);
        Behaviours.Remove(enemy);
    }

    public static void Clear()
    {
        Enemies.Clear();
        Behaviours.Clear();
    }
}