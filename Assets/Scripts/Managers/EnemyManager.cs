using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static readonly List<Transform> Enemies = new();
    public static readonly List<TriangleEnemyBehaviour> Behaviours = new();

    public static void Register(TriangleEnemyBehaviour enemy)
    {
        if (!Enemies.Contains(enemy.transform))
            Enemies.Add(enemy.transform);

        if (!Behaviours.Contains(enemy))
            Behaviours.Add(enemy);
    }

    public static void Unregister(TriangleEnemyBehaviour enemy)
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