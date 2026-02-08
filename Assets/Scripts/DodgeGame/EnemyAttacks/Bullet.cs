using UnityEngine;
using System.Collections;

namespace Jam.DodgeGame
{
    public class Bullet : MonoBehaviour
    {
        [SerializeField] private float _lifeTime = 5f;

        private float _speed;
        private int _damage;
        private Vector2 _direction;

        private BulletPool _pool;
        private Coroutine _moveCoroutine;

        public void SetPool(BulletPool pool)
        {
            _pool = pool;
        }

        public void Init(Vector2 direction, float speed, int damage)
        {
            if (!gameObject.activeSelf)
                gameObject.SetActive(true);

            _direction = direction.normalized;
            _speed = speed;
            _damage = damage;

            if (_moveCoroutine != null)
                StopCoroutine(_moveCoroutine);

            _moveCoroutine = StartCoroutine(MoveCoroutine());
        }


        private IEnumerator MoveCoroutine()
        {
            float timer = 0f;

            while (timer < _lifeTime)
            {
                transform.Translate(_direction * _speed * Time.deltaTime, Space.World);
                timer += Time.deltaTime;
                yield return null;
            }

            ReturnToPool();
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.TryGetComponent(out IDamageable damageable))
            {
                damageable.TakeDamage(_damage);
                ReturnToPool();
            }
        }

        public void ReturnToPool()
        {
            if (_moveCoroutine != null)
            {
                StopCoroutine(_moveCoroutine);
                _moveCoroutine = null;
            }

            _pool.Return(this);
        }
    }
}
