using UnityEngine;
using UnityEngine.InputSystem;

namespace Jam.DodgeGame.Player
{
    public class ShieldController : MonoBehaviour
    {
        [SerializeField] private GameObject _shieldRotate;

        private void Update()
        {
            Vector2 mousePos = Mouse.current.position.ReadValue();
            mousePos = Camera.main.ScreenToWorldPoint(mousePos);
            Vector3 direction = new Vector3(mousePos.x, mousePos.y, 0) - _shieldRotate.transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            _shieldRotate.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }
}
