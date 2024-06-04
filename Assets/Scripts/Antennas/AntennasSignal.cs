using UnityEngine;

public class AntennaSignal : MonoBehaviour
{
    public float signalRadius = 10f;
    public LayerMask wallLayer;  // Слой, к которому принадлежат стены
    public float wallAttenuation = 10f;  // Коэффициент ослабления сигнала на одну стену (10 дБ)
    public float floorAttenuation = 10f;  // Коэффициент ослабления сигнала на один этаж (10 дБ)
    public float gain = 14.0f; // Усреднённое усиление антенны
    public float beamwidth = 60.0f; // Усреднённая ширина диаграммы направленности
    [SerializeField] private Material signalMaterial;
    [SerializeField] private bool showInEdit;

    private string wallTag = "Wall";  // Тег для стен
    private string floorTag = "Floor";  // Тег для пола
    private SphereCollider signalCollider;
    private GameObject signalSphere;

    void Start()
    {
        // Создание зоны покрытия сигнала
        signalCollider = gameObject.AddComponent<SphereCollider>();
        signalCollider.isTrigger = true;
        signalCollider.radius = signalRadius;

        // Визуализация зоны покрытия (опционально)
        signalSphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        signalSphere.transform.SetParent(transform);
        signalSphere.transform.localPosition = Vector3.zero;
        UpdateSignalSphereScale(signalRadius);
        Destroy(signalSphere.GetComponent<Collider>());
        signalSphere.GetComponent<MeshRenderer>().material = signalMaterial;
    }

    void Update()
    {
        // Проверка силы сигнала в целевой точке каждый кадр
        if (signalSphere != null)
        {
            float signalStrength = CalculateSignalStrength(signalSphere.transform.localPosition);
            UpdateSignalSphereSize(signalStrength);
        }
    }

    void UpdateSignalSphereScale(float radius)
    {
        // Настройка масштаба сферы в зависимости от характеристик направленности
        float scaleX = Mathf.Sin(beamwidth * Mathf.Deg2Rad / 2) * gain * radius * 2;
        float scaleY = radius * 2;
        float scaleZ = Mathf.Sin(beamwidth * Mathf.Deg2Rad / 2) * gain * radius * 2;
        signalSphere.transform.localScale = new Vector3(scaleX, scaleY, scaleZ);
    }

    void UpdateSignalSphereSize(float signalStrength)
    {
        // Расчет новых размеров сферы сигнала с учетом влияния стен и полов
        int wallCount = CountWallsOnPath(transform.position, signalSphere.transform.position);
        int floorCount = CountFloorOnPath(transform.position, signalSphere.transform.position);

        // Обновление размеров сферы сигнала
        float wallAttenuationFactor = Mathf.Pow(10f, -wallCount * wallAttenuation / 10f);
        float floorAttenuationFactor = Mathf.Pow(10f, -floorCount * floorAttenuation / 10f);

        float newRadius = signalRadius * Mathf.Sqrt(signalStrength);  // Корень квадратный для учета области
        float scaleX = Mathf.Sin(beamwidth * Mathf.Deg2Rad / 2) * gain * newRadius * 2 * wallAttenuationFactor;
        float scaleY = newRadius * 2 * floorAttenuationFactor;
        float scaleZ = Mathf.Sin(beamwidth * Mathf.Deg2Rad / 2) * gain * newRadius * 2 * wallAttenuationFactor;

        signalSphere.transform.localScale = new Vector3(scaleX, scaleY, scaleZ);
    }

    public float CalculateSignalStrength(Vector3 targetPosition)
    {
        float distance = Vector3.Distance(transform.position, targetPosition);

        // Normalize distance for signal strength calculation
        float normalizedDistance = distance / signalRadius;
        float baseSignalStrength = Mathf.Clamp01(1f - normalizedDistance);

        // Если целевая точка находится вне зоны покрытия, сила сигнала будет 0
        if (distance > signalRadius)
        {
            return 0f;
        }

        // Определение количества стен и полов на пути сигнала
        int wallCount = CountWallsOnPath(transform.position, targetPosition);
        int floorCount = CountFloorOnPath(transform.position, targetPosition);

        // Расчет итоговой силы сигнала с учетом ослабления на стенах и полу
        float signalStrength = baseSignalStrength * Mathf.Pow(10f, -(wallCount * wallAttenuation + floorCount * floorAttenuation) / 10f);

        return signalStrength;
    }

    int CountFloorOnPath(Vector3 start, Vector3 end)
    {
        int floorCount = 0;
        RaycastHit[] hits = Physics.RaycastAll(start, (end - start).normalized, Vector3.Distance(start, end));
        foreach (RaycastHit hit in hits)
        {
            if (hit.collider.gameObject.CompareTag(floorTag))
            {
                floorCount++;
            }
        }
        return floorCount;
    }

    int CountWallsOnPath(Vector3 start, Vector3 end)
    {
        int wallCount = 0;
        RaycastHit[] hits = Physics.RaycastAll(start, (end - start).normalized, Vector3.Distance(start, end));
        foreach (RaycastHit hit in hits)
        {
            if (hit.collider.gameObject.CompareTag(wallTag))
            {
                wallCount++;
            }
        }
        return wallCount;
    }

    void OnDrawGizmos()
    {
        if (showInEdit)
        {
            Gizmos.color = new Color(0, 1, 0, 0.2f);
            float scaleX = Mathf.Sin(beamwidth * Mathf.Deg2Rad / 2) * gain * signalRadius;
            float scaleY = signalRadius;
            float scaleZ = Mathf.Sin(beamwidth * Mathf.Deg2Rad / 2) * gain * signalRadius;
            Gizmos.DrawWireMesh(CreateEllipsoidMesh(), transform.position, transform.rotation, new Vector3(scaleX, scaleY, scaleZ));
        }
    }

    Mesh CreateEllipsoidMesh()
    {
        GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        Mesh mesh = sphere.GetComponent<MeshFilter>().sharedMesh;
        DestroyImmediate(sphere);
        return mesh;
    }
}
