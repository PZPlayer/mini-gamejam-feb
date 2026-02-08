using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

namespace Jam.DodgeGame.Player
{
    public class ShieldController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private GameObject _shieldObject;
        [SerializeField] private Transform _shieldRotate;

        [Header("Settings")]
        [SerializeField] private float _shieldDuration = 1.5f;
        [SerializeField] private float _cooldownTime = 2f;

        [Header("Shield UI")]
        [SerializeField] private Transform _shieldBarFill; // объект Fill
        [SerializeField] private float _shieldBarMaxWidth = 1f; // максимальная ширина полоски


        private bool _canUseShield = true;
        private Coroutine _shieldCoroutine;
        private float _cooldownProgress = 1f;

        public float CooldownProgress => _cooldownProgress;
        public bool IsReady => _canUseShield;

        private void Awake()
        {
            _shieldObject.SetActive(false);
            _cooldownProgress = 1f;
        }

        public void OnShield(InputAction.CallbackContext context)
        {
            if (!context.started) return;
            if (!_canUseShield) return;

            ActivateShield();
        }

        private void ActivateShield()
        {
            _canUseShield = false;
            _cooldownProgress = 0f;
            _shieldObject.SetActive(true);

            if (_shieldCoroutine != null)
                StopCoroutine(_shieldCoroutine);

            _shieldCoroutine = StartCoroutine(ShieldRoutine());
        }

        private IEnumerator ShieldRoutine()
        {
            yield return new WaitForSeconds(_shieldDuration);

            _shieldObject.SetActive(false);

            float timer = 0f;
            while (timer < _cooldownTime)
            {
                timer += Time.deltaTime;
                _cooldownProgress = timer / _cooldownTime;
                yield return null;
            }

            _cooldownProgress = 1f;
            _canUseShield = true;
        }

        public void InstantRecharge()
        {
            if (_shieldCoroutine != null)
                StopCoroutine(_shieldCoroutine);

            _shieldObject.SetActive(false);
            _cooldownProgress = 1f;
            _canUseShield = true;
        }

        private void Update()
        {
            RotateShieldToMouse();
            UpdateShieldBar();
        }

        private void RotateShieldToMouse()
        {
            Vector2 mousePos = Mouse.current.position.ReadValue();
            Vector2 worldMouse = Camera.main.ScreenToWorldPoint(mousePos);

            Vector2 dir = worldMouse - (Vector2)_shieldRotate.position;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

            _shieldRotate.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }

        private void UpdateShieldBar()
        {
            if (_shieldBarFill == null) return;

            // Меняем локальный масштаб по X
            Vector3 scale = _shieldBarFill.localScale;
            scale.x = _cooldownProgress * _shieldBarMaxWidth;
            _shieldBarFill.localScale = scale;
        }
    }

}

