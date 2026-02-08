using UnityEngine;
using System.Collections;

namespace Jam.DodgeGame
{
    public class CircleAttack : EnemyAttackBase
    {
        [SerializeField] private BulletPool _bulletPool;
        [SerializeField] private int _bulletCount = 8;          // сколько пуль разлетается во все стороны
        [SerializeField] private float _minRadius = 1f;         // минимальный радиус окружности
        [SerializeField] private float _maxRadius = 3f;         // максимальный радиус окружности
        [SerializeField] private float _spawnDelay = 0.1f;      // задержка между созданием пуль
        [SerializeField] private float _baseSpeed = 5f;
        [SerializeField] private int _baseDamage = 1;

        protected override void Execute(Vector2 targetPosition, int difficulty)
        {
            StartCoroutine(AttackCoroutine(targetPosition, difficulty));
        }

        private IEnumerator AttackCoroutine(Vector2 center, int difficulty)
        {
            // 1. Случайный радиус
            float radius = Random.Range(_minRadius, _maxRadius);

            // 2. Случайная точка на окружности
            float angle = Random.Range(0f, 360f);
            Vector2 spawnPoint = center + new Vector2(
                Mathf.Cos(angle * Mathf.Deg2Rad),
                Mathf.Sin(angle * Mathf.Deg2Rad)
            ) * radius;

            // 3. Вычисляем скорость и урон с учётом сложности
            float speed = CalculateSpeed(_baseSpeed, difficulty);
            int damage = CalculateDamage(_baseDamage, difficulty);

            // 4. Разлетаем пули во все стороны равномерно
            float angleStep = 360f / _bulletCount;
            for (int i = 0; i < _bulletCount; i++)
            {
                float bulletAngle = i * angleStep;
                Vector2 dir = new Vector2(
                    Mathf.Cos(bulletAngle * Mathf.Deg2Rad),
                    Mathf.Sin(bulletAngle * Mathf.Deg2Rad)
                );

                Bullet b = _bulletPool.Get();
                b.transform.position = spawnPoint;
                b.transform.up = dir;   // чтобы пуля смотрела по направлению движения
                b.Init(dir, speed, damage);

                yield return new WaitForSeconds(_spawnDelay);
            }
        }
    }
}

