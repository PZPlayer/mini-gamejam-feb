using UnityEngine;

namespace Jam.DodgeGame
{
    public class EnemyAttackController : MonoBehaviour
    {
        [SerializeField] private Transform _targetTransform;
        [SerializeField, Range(1, 5)] private int _difficulty = 1;
        [SerializeField] private EnemyAttackBase[] _attacks;

        public int Difficulty
        {
            get => _difficulty;
            set => _difficulty = Mathf.Clamp(value, 1, 5);
        }

        private void Update()
        {
            TryUseRandomAttack();
        }

        private void TryUseRandomAttack()
        {
            if (_attacks.Length == 0)
                return;

            EnemyAttackBase attack = _attacks[Random.Range(0, _attacks.Length)];

            if (!attack.IsReady)
                return;

            Vector2 targetPosition = _targetTransform.position;
            attack.TryExecute(targetPosition, _difficulty);
        }
    }
}
