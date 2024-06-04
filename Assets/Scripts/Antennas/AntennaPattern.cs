using UnityEngine;

public class AntennaPattern : MonoBehaviour
{
    public GameObject antenna;
    public GameObject patternSphere;
    public float gain = 1.0f;
    public float beamwidth = 45.0f;
    public float reductionFactor = 0.5f; // Фактор уменьшения размера при столкновении

    private Vector3 originalScale;

    void Start()
    {
        CreatePattern();
    }

    void CreatePattern()
    {
        // Создание полупрозрачной сферы
        GameObject pattern = Instantiate(patternSphere, antenna.transform.position, Quaternion.identity);
        pattern.transform.parent = antenna.transform;

        // Настройка масштаба сферы в зависимости от характеристик направленности
        float scaleX = Mathf.Sin(beamwidth * Mathf.Deg2Rad / 2) * gain;
        float scaleY = gain;
        float scaleZ = Mathf.Sin(beamwidth * Mathf.Deg2Rad / 2) * gain;

        pattern.transform.localScale = new Vector3(scaleX, scaleY, scaleZ);

        // Настройка материала для полупрозрачной сферы
        Material patternMaterial = pattern.GetComponent<Renderer>().material;
        Color color = patternMaterial.color;
        color.a = 0.5f; // Устанавливаем прозрачность
        patternMaterial.color = color;

        // Сохранение исходного масштаба
        originalScale = pattern.transform.localScale;

        // Добавление коллайдера и скрипта обработки триггера
        SphereCollider sphereCollider = pattern.AddComponent<SphereCollider>();
        sphereCollider.isTrigger = true;

        PatternColliderHandler handler = pattern.AddComponent<PatternColliderHandler>();
        handler.reductionFactor = reductionFactor;
        handler.originalScale = originalScale;
    }
}

public class PatternColliderHandler : MonoBehaviour
{
    public float reductionFactor;
    public Vector3 originalScale;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Wall"))
        {
            // Уменьшение размера сферы при столкновении с объектом с тегом Wall
            transform.localScale = originalScale * reductionFactor;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Wall"))
        {
            // Восстановление исходного размера при выходе из коллайдера
            transform.localScale = originalScale;
        }
    }
}