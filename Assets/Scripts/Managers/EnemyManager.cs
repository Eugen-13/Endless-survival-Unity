using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static readonly List<Transform> Enemies = new();
    public static readonly List<EnemyBase> Behaviours = new();

    public static void Register(EnemyBase enemy)
    {
        if (!Enemies.Contains(enemy.transform))
            Enemies.Add(enemy.transform);

        if (!Behaviours.Contains(enemy))
            Behaviours.Add(enemy);
    }

    public static void Unregister(EnemyBase enemy)
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