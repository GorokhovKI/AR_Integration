using UnityEngine;

public class AntennaPattern : MonoBehaviour
{
    public GameObject antenna;
    public GameObject patternSphere;
    public float gain = 1.0f;
    public float beamwidth = 45.0f;
    public float reductionFactor = 0.5f; // ������ ���������� ������� ��� ������������

    private Vector3 originalScale;

    void Start()
    {
        CreatePattern();
    }

    void CreatePattern()
    {
        // �������� �������������� �����
        GameObject pattern = Instantiate(patternSphere, antenna.transform.position, Quaternion.identity);
        pattern.transform.parent = antenna.transform;

        // ��������� �������� ����� � ����������� �� ������������� ��������������
        float scaleX = Mathf.Sin(beamwidth * Mathf.Deg2Rad / 2) * gain;
        float scaleY = gain;
        float scaleZ = Mathf.Sin(beamwidth * Mathf.Deg2Rad / 2) * gain;

        pattern.transform.localScale = new Vector3(scaleX, scaleY, scaleZ);

        // ��������� ��������� ��� �������������� �����
        Material patternMaterial = pattern.GetComponent<Renderer>().material;
        Color color = patternMaterial.color;
        color.a = 0.5f; // ������������� ������������
        patternMaterial.color = color;

        // ���������� ��������� ��������
        originalScale = pattern.transform.localScale;

        // ���������� ���������� � ������� ��������� ��������
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
            // ���������� ������� ����� ��� ������������ � �������� � ����� Wall
            transform.localScale = originalScale * reductionFactor;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Wall"))
        {
            // �������������� ��������� ������� ��� ������ �� ����������
            transform.localScale = originalScale;
        }
    }
}