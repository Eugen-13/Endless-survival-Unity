using UnityEngine;
using Zenject;

namespace PlayerSystem
{
    public class PlayerMovement : IInitializable, ITickable, IFixedTickable
    {
        private InputSystem _inputSystem;
        private Vector2 _direction;
        private Rigidbody2D _rb;

        private Player _player;

        [Inject]
        private void Construct(Player player)
        {
            _player = player;
        }

        public void Initialize()
        {
            _inputSystem = new InputSystem();
            _inputSystem.Enable();

            _rb = _player.GetComponent<Rigidbody2D>();
            _rb.interpolation = RigidbodyInterpolation2D.Interpolate;
        }

        public void Tick()
        {
             _direction = _inputSystem.Player.Move.ReadValue<Vector2>();
        }

        public void FixedTick()
        {
            Move();
        }

        private void Move()
        {
            Vector2 newPos = _rb.position + (_direction * (_player.Speed * Time.fixedDeltaTime));
            _rb.MovePosition(newPos);

            if (!(_direction.sqrMagnitude > 0.01f))
            {
                return;
            }

            float angle = (Mathf.Atan2(_direction.y, _direction.x) * Mathf.Rad2Deg) - 90f;
            _rb.MoveRotation(angle);
        }
    }
}
