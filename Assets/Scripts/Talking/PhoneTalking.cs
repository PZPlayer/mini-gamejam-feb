using TMPro;
using UnityEngine;

namespace Jam.Talking
{
    public class PhoneTalking : MonoBehaviour
    {
        [SerializeField] private Transform _spawnPlace;
        [SerializeField] private GameObject _messagePrefab;
        [SerializeField] private TextMeshProUGUI _messageCountText;
        [SerializeField] private Animator _anmtr;
        private int _messageCount = 0;

        public void WriteAMessage(string message)
        {
            _messageCount++;
            _messageCountText.text = _messageCount.ToString();
            _anmtr.SetTrigger("Notify");
            TextMeshProUGUI buttonText = Instantiate(_messagePrefab, _spawnPlace).GetComponentInChildren<TextMeshProUGUI>();
            buttonText.text = message;
        }
    }
}
