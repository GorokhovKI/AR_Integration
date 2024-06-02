using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class AntennaSignal : MonoBehaviour
{
    public float signalRadius = 10f;  
    public LayerMask wallLayer;  // ����, � �������� ����������� �����
    public float wallAttenuation = 2f;  // ����������� ���������� ������� �� ���� �����
    [SerializeField] private Material signalMaterial;
    [SerializeField] private GameObject wallObject;
    [SerializeField] private bool showInEdit;

    private string wallTag = "Wall";  // ��� ��� ����

    private SphereCollider signalCollider;
    private GameObject signalSphere;

    void Start()
    {
        // �������� ����� �������� �������
        signalCollider = gameObject.AddComponent<SphereCollider>();
        signalCollider.isTrigger = true;
        signalCollider.radius = signalRadius;

        // ������������ ���� �������� (�����������)
        signalSphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        signalSphere.transform.SetParent(transform);
        signalSphere.transform.localPosition = Vector3.zero;
        signalSphere.transform.localScale = new Vector3(signalRadius * 2, signalRadius * 2, signalRadius * 2);
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

    void UpdateSignalSphereSize(float signalStrength)
    {
        // ���������� ������� ����� ������� �� ������ ���� �������
        float newRadius = signalRadius * signalStrength;
        signalSphere.transform.localScale = new Vector3(newRadius * 2, newRadius * 2, newRadius * 2);
    }

    public float CalculateSignalStrength(Vector3 targetPosition)
    {
        // ������ ������� ���� �������
        float distance = Vector3.Distance(transform.position, targetPosition);
        if (distance > signalRadius)
        {
            return 0f;  // ��� ���� ��������
        }

        // ����������� ���������� ���� �� ���� �������
        int wallCount = CountWallsOnPath(transform.position, targetPosition);
        float signalStrength = 1f / (1f + wallAttenuation * wallCount);
        return signalStrength;
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
                Debug.Log(wallCount);
            }
        }
        return wallCount;
    }

    void OnDrawGizmos()
    {
        if (showInEdit)
        {
            Gizmos.color = new Color(0, 1, 0, 0.2f);
            Gizmos.DrawSphere(transform.position, signalRadius);
        }
    }
}
