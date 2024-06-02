using UnityEngine;
using UnityEngine.UIElements;

public class AntennaPlacement : MonoBehaviour
{

    [SerializeField] private GameObject omniAntennaPrefab;  
    [SerializeField] private GameObject directionalAntennaPrefab;  

     public GameObject[] corridors;  
     public GameObject[] rooms;  

    void Start()
    {
        
        PlaceAntennas();
    }

    void PlaceAntennas()
    {
        
        foreach (GameObject corridor in corridors)
        {
            Vector3 position = corridor.transform.position;
            Instantiate(omniAntennaPrefab, position, Quaternion.identity);
        }

        
        foreach (GameObject room in rooms)
        {
            Vector3 position = room.transform.position;
            Instantiate(directionalAntennaPrefab, position, Quaternion.identity);
        }
    }
}
