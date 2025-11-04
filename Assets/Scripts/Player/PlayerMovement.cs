using UnityEngine;
public class PlayerMovement : MonoBehaviour
{
    private InputSystem _inputSystem;
    private Vector2 _direction;
    private Rigidbody2D _rb;


    void Start()
    {
        _inputSystem = new InputSystem();
        _inputSystem.Enable();

        _rb = GetComponent<Rigidbody2D>();
        _rb.interpolation = RigidbodyInterpolation2D.Interpolate;

    }

    void Update()
    {
        _direction = _inputSystem.Player.Move.ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {
        Move();
    }
    void Move()
    {
        Vector2 newPos = _rb.position + _direction * Player.Instance.Speed * Time.fixedDeltaTime;
        _rb.MovePosition(newPos);
        if (_direction.sqrMagnitude > 0.01f)
        {
            float angle = Mathf.Atan2(_direction.y, _direction.x) * Mathf.Rad2Deg - 90f;
            _rb.MoveRotation(angle);
        }
    }
}
