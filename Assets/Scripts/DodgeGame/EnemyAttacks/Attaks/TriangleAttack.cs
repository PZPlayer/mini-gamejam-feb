using UnityEngine;
using System.Collections;

namespace Jam.DodgeGame
{
    public class TriangleAttack : EnemyAttackBase
    {
        [SerializeField] private BulletPool _bulletPool;
        [SerializeField] private int _count = 6;
        [SerializeField] private float _minRadius = 2f;
        [SerializeField] private float _maxRadius = 4f;
        [SerializeField] private float _spawnDelay = 0.2f;
        [SerializeField] private float _launchDelay = 0.2f;
        [SerializeField] private float _baseSpeed = 4f;
        [SerializeField] private int _baseDamage = 1;

        protected override void Execute(Vector2 targetPosition, int difficulty)
        {
            StartCoroutine(AttackCoroutine(targetPosition, difficulty));
        }

        private IEnumerator AttackCoroutine(Vector2 center, int difficulty)
        {
            float radius = Random.Range(_minRadius, _maxRadius);
            float startAngle = Random.Range(0f, 360f);
            bool clockwise = Random.value > 0.5f;
            float angleStep = 360f / _count * (clockwise ? 1 : -1);

            Bullet[] bullets = new Bullet[_count];

            for (int i = 0; i < _count; i++)
            {
                float angle = startAngle + i * angleStep;
                Vector2 pos = center + new Vector2(
                    Mathf.Cos(angle * Mathf.Deg2Rad),
                    Mathf.Sin(angle * Mathf.Deg2Rad)
                ) * radius;

                Bullet bullet = _bulletPool.Get();
                bullet.transform.position = pos;
                bullet.transform.up = (center - pos).normalized;

                bullets[i] = bullet;
                yield return new WaitForSeconds(_spawnDelay);
            }

            float speed = CalculateSpeed(_baseSpeed, difficulty);
            int damage = CalculateDamage(_baseDamage, difficulty);

            foreach (var bullet in bullets)
            {
                Vector2 dir = (center - (Vector2)bullet.transform.position).normalized;
                bullet.Init(dir, speed, damage);
                yield return new WaitForSeconds(_launchDelay);
            }
        }
    }
}
