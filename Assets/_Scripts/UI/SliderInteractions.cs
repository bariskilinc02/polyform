using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderInteractions : MonoBehaviour
{
    public Slider Slider;

    //change slider value when box clicked
    public void SetSliderValue(float value, float maxValue)
    {

        Slider.maxValue = maxValue;
        Slider.value = value;

    }

    //change slider value when object is already selected
    public void GetSliderValue(GroundUnit groundUnit ,LevelController levelController,Box box)
    {
        if(groundUnit.IsAccesableHeight(levelController, groundUnit, (int)Slider.value))
        {
            box.Effect((int)Slider.value);
        }
    }
}
