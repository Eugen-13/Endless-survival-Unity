using UnityEngine;
using Cinemachine;

public class Camera : MonoBehaviour
{
    private CinemachineVirtualCamera _vcam;
    void Start()
    {
        _vcam = GetComponent<CinemachineVirtualCamera>();
        _vcam.Follow = Player.Instance.transform    ;
    }

}
