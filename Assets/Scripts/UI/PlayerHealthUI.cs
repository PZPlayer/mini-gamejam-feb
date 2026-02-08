using UnityEngine;
using UnityEngine.UI;

namespace Jam.Scripting.UI
{
    using UnityEngine;
    using UnityEngine.UI;

    public class PlayerHealthUI : MonoBehaviour
    {
        [SerializeField] private Slider _slider;

        public void UpdateHealth(int current, int max)
        {
            if (_slider != null)
            {
                _slider.maxValue = max;
                _slider.value = current;
            }
        }
    }
}

