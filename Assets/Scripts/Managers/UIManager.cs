using UnityEngine;
public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [SerializeField] private UIXPPanel uIXPPanel;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }
    public void UpdateXP(float current, float required)
    {
        uIXPPanel.UpdateXP(current, required);
    }

    public void UpdateLevel(int level)
    {
        uIXPPanel.UpdateLevel(level);
    }
}