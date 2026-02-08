using UnityEngine;
using System.Collections.Generic;

namespace Jam.DodgeGame
{
    public class BulletPool : MonoBehaviour
    {
        [SerializeField] private Bullet _bulletPrefab;
        [SerializeField] private int _initialSize = 50;

        private readonly Queue<Bullet> _pool = new();

        private void Awake()
        {
            // Создаём стартовый пул
            for (int i = 0; i < _initialSize; i++)
                CreateBullet();
        }

        private Bullet CreateBullet()
        {
            Bullet bullet = Instantiate(_bulletPrefab, transform);
            bullet.gameObject.SetActive(false);
            bullet.SetPool(this);
            _pool.Enqueue(bullet);
            return bullet;
        }

        public Bullet Get()
        {
            // Если пул пуст — создаём новую пулю автоматически
            if (_pool.Count == 0)
            {
                CreateBullet();
            }

            Bullet bullet = _pool.Dequeue();
            bullet.gameObject.SetActive(true);
            return bullet;
        }

        public void Return(Bullet bullet)
        {
            bullet.gameObject.SetActive(false);
            _pool.Enqueue(bullet);
        }

        /// Позволяет расширить пул на нужное количество заранее
        public void Expand(int amount)
        {
            for (int i = 0; i < amount; i++)
            {
                CreateBullet();
            }
        }
    }
}
