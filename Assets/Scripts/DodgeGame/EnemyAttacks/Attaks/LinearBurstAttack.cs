using UnityEngine;
using System.Collections;

namespace Jam.DodgeGame
{
    public class LinearBurstAttack : EnemyAttackBase
    {
        [Header("References")]
        [SerializeField] private BulletPool _bulletPool;
        [SerializeField] private Transform _firePoint;

        [Header("Burst Settings")]
        [SerializeField] private int _burstsCount = 3;          // сколько очередей
        [SerializeField] private int _bulletsPerBurst = 3;      // сколько пуль в очереди
        [SerializeField] private float _timeBetweenBullets = 0.1f;
        [SerializeField] private float _timeBetweenBursts = 0.4f;

        [Header("Bullet Settings")]
        [SerializeField] private float _baseSpeed = 6f;
        [SerializeField] private int _baseDamage = 1;

        protected override void Execute(Vector2 targetPosition, int difficulty)
        {
            StartCoroutine(BurstCoroutine(targetPosition, difficulty));
        }

        private IEnumerator BurstCoroutine(Vector2 targetPosition, int difficulty)
        {
            Vector2 startPosition = _firePoint != null
                ? (Vector2)_firePoint.position
                : (Vector2)transform.position;

            Vector2 direction = (targetPosition - startPosition).normalized;

            float speed = CalculateSpeed(_baseSpeed, difficulty);
            int damage = CalculateDamage(_baseDamage, difficulty);

            for (int burst = 0; burst < _burstsCount; burst++)
            {
                for (int i = 0; i < _bulletsPerBurst; i++)
                {
                    Bullet bullet = _bulletPool.Get();
                    bullet.transform.position = startPosition;
                    bullet.transform.up = direction;
                    bullet.Init(direction, speed, damage);

                    yield return new WaitForSeconds(_timeBetweenBullets);
                }

                yield return new WaitForSeconds(_timeBetweenBursts);
            }
        }
    }
}
