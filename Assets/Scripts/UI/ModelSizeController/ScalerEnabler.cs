using UnityEngine.UI;
using UnityEngine;

public class ScalerEnabler : MonoBehaviour
{

    [SerializeField] private Slider sizeSlider;


    private void Start()
    {
        sizeSlider.gameObject.SetActive(false);
    }
    void OnEnable()
    {
        FindObjectOfType<ModelSpawner>().OnModelSpawned += OnModelSpawned;
        FindObjectOfType<ModelSpawner>().OnModelDeleted += OnModelDeleted;
    }

    void OnDisable()
    {
        FindObjectOfType<ModelSpawner>().OnModelSpawned -= OnModelSpawned;
        FindObjectOfType<ModelSpawner>().OnModelDeleted -= OnModelDeleted;
    }

    private void OnModelSpawned(GameObject model)
    {
        sizeSlider.gameObject.SetActive(true); 
    }

    private void OnModelDeleted()
    {
        sizeSlider.gameObject.SetActive(false); 
    }
}
