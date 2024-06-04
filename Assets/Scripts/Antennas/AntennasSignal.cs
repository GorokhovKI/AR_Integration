using UnityEngine;

public class AntennaSignal : MonoBehaviour
{
    public float signalRadius = 10f;
    public LayerMask wallLayer;  // ����, � �������� ����������� �����
    public float wallAttenuation = 10f;  // ����������� ���������� ������� �� ���� ����� (10 ��)
    public float floorAttenuation = 10f;  // ����������� ���������� ������� �� ���� ���� (10 ��)
    public float gain = 14.0f; // ���������� �������� �������
    public float beamwidth = 60.0f; // ���������� ������ ��������� ��������������
    [SerializeField] private Material signalMaterial;
    [SerializeField] private bool showInEdit;

    private string wallTag = "Wall";  // ��� ��� ����
    private string floorTag = "Floor";  // ��� ��� ����
    private SphereCollider signalCollider;
    private GameObject signalSphere;

    void Start()
    {
        // �������� ���� �������� �������
        signalCollider = gameObject.AddComponent<SphereCollider>();
        signalCollider.isTrigger = true;
        signalCollider.radius = signalRadius;

        // ������������ ���� �������� (�����������)
        signalSphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        signalSphere.transform.SetParent(transform);
        signalSphere.transform.localPosition = Vector3.zero;
        UpdateSignalSphereScale(signalRadius);
        Destroy(signalSphere.GetComponent<Collider>());
        signalSphere.GetComponent<MeshRenderer>().material = signalMaterial;
    }

    void Update()
    {
        // �������� ���� ������� � ������� ����� ������ ����
        if (signalSphere != null)
        {
            float signalStrength = CalculateSignalStrength(signalSphere.transform.localPosition);
            UpdateSignalSphereSize(signalStrength);
        }
    }

    void UpdateSignalSphereScale(float radius)
    {
        // ��������� �������� ����� � ����������� �� ������������� ��������������
        float scaleX = Mathf.Sin(beamwidth * Mathf.Deg2Rad / 2) * gain * radius * 2;
        float scaleY = radius * 2;
        float scaleZ = Mathf.Sin(beamwidth * Mathf.Deg2Rad / 2) * gain * radius * 2;
        signalSphere.transform.localScale = new Vector3(scaleX, scaleY, scaleZ);
    }

    void UpdateSignalSphereSize(float signalStrength)
    {
        // ������ ����� �������� ����� ������� � ������ ������� ���� � �����
        int wallCount = CountWallsOnPath(transform.position, signalSphere.transform.position);
        int floorCount = CountFloorOnPath(transform.position, signalSphere.transform.position);

        // ���������� �������� ����� �������
        float wallAttenuationFactor = Mathf.Pow(10f, -wallCount * wallAttenuation / 10f);
        float floorAttenuationFactor = Mathf.Pow(10f, -floorCount * floorAttenuation / 10f);

        float newRadius = signalRadius * Mathf.Sqrt(signalStrength);  // ������ ���������� ��� ����� �������
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

        // ���� ������� ����� ��������� ��� ���� ��������, ���� ������� ����� 0
        if (distance > signalRadius)
        {
            return 0f;
        }

        // ����������� ���������� ���� � ����� �� ���� �������
        int wallCount = CountWallsOnPath(transform.position, targetPosition);
        int floorCount = CountFloorOnPath(transform.position, targetPosition);

        // ������ �������� ���� ������� � ������ ���������� �� ������ � ����
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
