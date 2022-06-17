using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace LastCard
{
    public class SliderValueCapturer : MonoBehaviour
    {
        public Slider slider;
        public Text text;

        public void MoveValueIntoText()
        {
            text.text = slider.value.ToString();
        }
    }
}