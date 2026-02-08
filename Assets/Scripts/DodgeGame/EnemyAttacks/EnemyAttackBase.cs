using UnityEngine;

namespace Jam.DodgeGame
{
    public abstract class EnemyAttackBase : MonoBehaviour
    {
        [Header("Attack Settings")]
        [SerializeField] protected float _cooldown = 2f;

        protected float _lastUseTime = -999f;

        public bool IsReady => Time.time >= _lastUseTime + _cooldown;

        public void TryExecute(Vector2 targetPosition, int difficulty)
        {
            if (!IsReady)
                return;

            _lastUseTime = Time.time;
            Execute(targetPosition, difficulty);
        }

        protected abstract void Execute(Vector2 targetPosition, int difficulty);

        protected float CalculateSpeed(float baseSpeed, int difficulty)
        {
            // difficulty 1 → x1
            // difficulty 5 → x2
            float t = (difficulty - 1) / 4f;
            return Mathf.Lerp(baseSpeed, baseSpeed * 2f, t);
        }

        protected int CalculateDamage(int baseDamage, int difficulty)
        {
            // difficulty 1 → x1
            // difficulty 5 → x3
            float t = (difficulty - 1) / 4f;
            return Mathf.RoundToInt(Mathf.Lerp(baseDamage, baseDamage * 3f, t));
        }
    }
}
