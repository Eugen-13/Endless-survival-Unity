using UnityEngine;

public class DamagePopupManager : MonoBehaviour
{
    public static DamagePopupManager Instance;

    [SerializeField] private DamagePopup _popupPrefab;
    [SerializeField] private float offset = 1.2f;
    [SerializeField] private int _poolSize = 20;

    private string _popupPoolName = "DamagePopupPool1";

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }
    private void Start()
    {
        PoolManager.Instance.CreatePool(_popupPoolName, _popupPrefab.gameObject, _poolSize);
    }

    public void ShowPopup(int damage, Vector3 position, Color color)
    {
        PoolManager.Instance.Get(_popupPoolName, position + Vector3.up * offset, Quaternion.identity).GetComponent<DamagePopup>().Show(damage, color);
    }
}