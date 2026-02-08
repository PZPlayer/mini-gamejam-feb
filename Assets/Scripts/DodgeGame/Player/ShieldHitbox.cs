using UnityEngine;

namespace Jam.DodgeGame.Player
{
    public class ShieldHitbox : MonoBehaviour
    {
        [SerializeField] private ShieldController _shieldController;

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.TryGetComponent(out Jam.DodgeGame.Bullet bullet))
            {
                bullet.ReturnToPool();
                _shieldController.InstantRecharge();
            }
        }
    }
}
