using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class PlaneDetection : MonoBehaviour
{
    // Start is called before the first frame update
    [Header ("Put your planeMarker here")]

    [SerializeField] private GameObject PlaneMarkerPrefab;
    [SerializeField] private GameObject objectToSpawn;

    private GameObject spawnedModel;
    private List<ARRaycastHit> hits = new List<ARRaycastHit>();
    private ARRaycastManager ARRaycastManagerScript;

    public event Action<GameObject> OnModelSpawned;
    public event Action OnModelDeleted;

    void Start()
    {
        ARRaycastManagerScript = FindObjectOfType<ARRaycastManager>();

        PlaneMarkerPrefab.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        ShowMarker();
    }

    void ShowMarker()
    {
        ARRaycastManagerScript.Raycast(new Vector2(Screen.width / 2, Screen.height / 2), hits, TrackableType.Planes);
        
        if (hits.Count > 0)
        {
            PlaneMarkerPrefab.transform.position = hits[0].pose.position;
            PlaneMarkerPrefab.SetActive(true);
        }

        if (Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began)
        {

            Instantiate(objectToSpawn, hits[0].pose.position, objectToSpawn.transform.rotation);

        }
    }
    public List<ARRaycastHit> Hits
    {
        get { return hits; }
    }
}
