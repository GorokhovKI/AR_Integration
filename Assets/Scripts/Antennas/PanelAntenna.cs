using UnityEngine;

public class PanelAntenna : MonoBehaviour
{
    public float frequency = 2.4f;  // Частота в ГГц
    public float antennaGain = 15f;  // Коэффициент усиления антенны в дБи
    public float verticalBeamwidth = 15f;  // Вертикальный угол диаграммы направленности в градусах
    public float horizontalBeamwidth = 60f;  // Горизонтальный угол диаграммы направленности в градусах
    public float signalQualityReduction = 0.5f;  // Уменьшение качества сигнала при столкновении с коллайдером "Wall"
    public float signalRadius = 10f;  // Радиус действия антенны
    public GameObject signalVisualizationPrefab;  // Префаб визуализации сигнала
    private GameObject signalVisualization;  // Объект визуализации сигнала

    void Start()
    {
        // Создаем визуализацию сигнала
        if (signalVisualizationPrefab != null)
        {
            signalVisualization = Instantiate(signalVisualizationPrefab, transform.position, Quaternion.identity);
            signalVisualization.transform.localScale = new Vector3(signalRadius * 2, signalRadius * 2, signalRadius * 2);
        }
    }

    void Update()
    {
        // Обновляем позицию визуализации сигнала
        if (signalVisualization != null)
        {
            signalVisualization.transform.position = transform.position;
        }

        // Рассчитываем максимальное расстояние от антенны
        float maxDistance = Mathf.Pow(10, antennaGain / 20); // Преобразуем коэффициент усиления из дБи в линейное усиление

        // Начинаем линию от антенны
        Vector3[] linePositions = new Vector3[2];
        linePositions[0] = transform.position;

        // Рассчитываем конечную точку линии
        Vector3 direction = transform.forward * signalRadius;
        RaycastHit hit;
        if (Physics.Raycast(transform.position, direction, out hit, signalRadius))
        {
            linePositions[1] = hit.point;
        }
        else
        {
            linePositions[1] = transform.position + direction;
        }

        // Рисуем линию для визуализации сигнала
        if (signalVisualization != null)
        {
            LineRenderer lineRenderer = signalVisualization.GetComponent<LineRenderer>();
            if (lineRenderer != null)
            {
                lineRenderer.SetPositions(linePositions);
            }
        }
    }
}
