using UnityEngine;
using UnityEngine.InputSystem;

namespace Jam.DodgeGame.Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Movement : MonoBehaviour
    {
        [SerializeField] private float _speed = 5f;

        private Rigidbody2D _rb;
        private Vector2 _moveDirection;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate()
        {
            _rb.linearVelocity = _moveDirection * _speed;
        }

        // Ё“ќ“ метод мы вручную подключим в PlayerInput
        public void OnMove(InputAction.CallbackContext context)
        {
            _moveDirection = context.ReadValue<Vector2>();
        }
    }
}
