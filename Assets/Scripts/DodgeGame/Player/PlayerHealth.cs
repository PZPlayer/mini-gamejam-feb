using UnityEngine;
using UnityEngine.Events;
using System.Collections;

namespace Jam.DodgeGame.Player
{
    public class PlayerHealth : MonoBehaviour, IDamageable
    {
        [SerializeField] private int _maxHP = 20;
        [SerializeField] private int _currentHP;
        [SerializeField] private float _invulDuration = 0.5f;
        [SerializeField] private SpriteRenderer _spriteRenderer;

        [Header("Events")]
        public UnityEvent OnDeath; // Событие смерти игрока
        public UnityEvent<int, int> OnHealthChanged; // Событие изменения здоровья (текущее, максимум)

        private bool _isInvulnerable = false;

        private void Start()
        {
            _currentHP = _maxHP;

            if (_spriteRenderer == null)
                _spriteRenderer = GetComponent<SpriteRenderer>();

            OnHealthChanged?.Invoke(_currentHP, _maxHP); // обновим UI сразу при старте
        }

        public void TakeDamage(int damage)
        {
            if (_isInvulnerable) return;

            _currentHP -= damage;
            _currentHP = Mathf.Max(_currentHP, 0);

            Debug.Log($"HP: {_currentHP}");

            OnHealthChanged?.Invoke(_currentHP, _maxHP); // уведомляем UI

            if (_currentHP <= 0)
            {
                Die();
            }
            else
            {
                StartCoroutine(InvulCoroutine());
            }
        }

        private IEnumerator InvulCoroutine()
        {
            _isInvulnerable = true;
            float timer = 0f;
            bool toggle = false;

            while (timer < _invulDuration)
            {
                toggle = !toggle;
                _spriteRenderer.enabled = toggle;

                timer += 0.1f;
                yield return new WaitForSeconds(0.1f);
            }

            _spriteRenderer.enabled = true;
            _isInvulnerable = false;
        }

        private void Die()
        {
            Debug.Log("Player dead");
            OnDeath?.Invoke(); // вызываем событие смерти
        }
    }
}
