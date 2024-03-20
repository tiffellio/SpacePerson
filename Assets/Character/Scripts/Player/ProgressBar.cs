using UnityEngine.UI;
using UnityEngine;

namespace csci485.Management
{
    public class ProgressBar : MonoBehaviour
    {
        private Slider slider;

        private void Awake()
        {
            slider = GetComponent<Slider>();
        }

        public void UpdateCurrentValue(float _value)
        {
            slider.value = _value;
        }

        public void UpdateMaxValue(float _value)
        {
            slider.maxValue = _value;
        }

        public void UpdateMinValue(float _value)
        {
            slider.minValue = _value;
        }
    }

}
