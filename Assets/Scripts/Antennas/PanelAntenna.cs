using UnityEngine;

public class PanelAntenna : MonoBehaviour
{
    public float frequency = 2.4f;  // ������� � ���
    public float antennaGain = 15f;  // ����������� �������� ������� � ���
    public float verticalBeamwidth = 15f;  // ������������ ���� ��������� �������������� � ��������
    public float horizontalBeamwidth = 60f;  // �������������� ���� ��������� �������������� � ��������
    public float signalQualityReduction = 0.5f;  // ���������� �������� ������� ��� ������������ � ����������� "Wall"
    public float signalRadius = 10f;  // ������ �������� �������
    public GameObject signalVisualizationPrefab;  // ������ ������������ �������
    private GameObject signalVisualization;  // ������ ������������ �������

    void Start()
    {
        // ������� ������������ �������
        if (signalVisualizationPrefab != null)
        {
            signalVisualization = Instantiate(signalVisualizationPrefab, transform.position, Quaternion.identity);
            signalVisualization.transform.localScale = new Vector3(signalRadius * 2, signalRadius * 2, signalRadius * 2);
        }
    }

    void Update()
    {
        // ��������� ������� ������������ �������
        if (signalVisualization != null)
        {
            signalVisualization.transform.position = transform.position;
        }

        // ������������ ������������ ���������� �� �������
        float maxDistance = Mathf.Pow(10, antennaGain / 20); // ����������� ����������� �������� �� ��� � �������� ��������

        // �������� ����� �� �������
        Vector3[] linePositions = new Vector3[2];
        linePositions[0] = transform.position;

        // ������������ �������� ����� �����
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

        // ������ ����� ��� ������������ �������
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
