using Cinemachine;
using PlayerSystem;
using UnityEngine;
using Zenject;

namespace Camera
{
    public class Camera : MonoBehaviour
    {
        [SerializeField] private CinemachineVirtualCamera _virtualCamera;

        [Inject] private Player _player;

        private void Start()
        {
            _virtualCamera.Follow = _player.transform;
        }
    }
}
