using UnityEngine;
using UnityEngine.InputSystem;

namespace Jam.DodgeGame.Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Movement : MonoBehaviour
    {
        [SerializeField] private float speed;

        private Rigidbody2D rb;
        private Vector2 moveDirection;

        private void Start()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate()
        {
            rb.linearVelocity = moveDirection * speed;
        }

        private void OnMove(InputValue value)
        {
            moveDirection = value.Get<Vector2>();
        }
    }
}
