using System.ComponentModel;
using UnityEngine;
using UnityEngine.UI;

public class ModelScaleConttroller : MonoBehaviour
{
    [Description ("Can be usable in any object")]
    [Header ("Referens to model")]
    [SerializeField] private Slider sizeSlider; // —сылка на слайдер
    [SerializeField] private GameObject model; // —сылка на модель
    
    void Start()
    {
        if (sizeSlider != null)
        {
            sizeSlider.onValueChanged.AddListener(OnSliderValueChanged);
        }
    }

    // Update is called once per frame
    void OnSliderValueChanged(float value)
    {
        if (model != null)
        {
            model.transform.localScale = new Vector3(value, value, value);
        }
    }
}
