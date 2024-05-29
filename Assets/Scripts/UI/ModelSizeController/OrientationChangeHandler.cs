using UnityEngine;
using UnityEngine.UI;

public class OrientationChangeHandler : MonoBehaviour
{
    private ScreenOrientation lastOrientation;
    [SerializeField] private RectTransform sliderRectTransform; 

    void Start()
    {
        lastOrientation = Screen.orientation;
        UpdateUIForOrientation();
    }

    void Update()
    {
        if (Screen.orientation != lastOrientation)
        {
            OnOrientationChange();
            lastOrientation = Screen.orientation;
        }
    }

    void OnOrientationChange()
    {

        UpdateUIForOrientation();
    }

    void UpdateUIForOrientation()
    {

        if (Screen.orientation == ScreenOrientation.Portrait || Screen.orientation == ScreenOrientation.PortraitUpsideDown)
        {

            sliderRectTransform.anchorMin = new Vector2(0.5f, 0);
            sliderRectTransform.anchorMax = new Vector2(0.5f, 0);
            sliderRectTransform.anchoredPosition = new Vector2(0, 100); // Позиция от нижней границы
            sliderRectTransform.sizeDelta = new Vector2(400, 40); // Размер слайдера
        }
        else if (Screen.orientation == ScreenOrientation.LandscapeLeft || Screen.orientation == ScreenOrientation.LandscapeRight)
        {

            sliderRectTransform.anchorMin = new Vector2(0.5f, 0);
            sliderRectTransform.anchorMax = new Vector2(0.5f, 0);
            sliderRectTransform.anchoredPosition = new Vector2(0, 50); // Позиция от нижней границы
            sliderRectTransform.sizeDelta = new Vector2(600, 40); // Размер слайдера
        }
    }
}