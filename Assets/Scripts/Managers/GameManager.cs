using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;


class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DOTween.SetTweensCapacity(1000, 200);
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        EnemyManager.Clear();
    }
    
}
