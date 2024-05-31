//using System;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.XR.ARFoundation;

//public class ModelSpawner : MonoBehaviour
//{
//    // Start is called before the first frame update

//    [SerializeField] private GameObject ModelToSpawn;

//    private PlaneDetection planeDetection;
//    private GameObject spawnedModel;

//    public event Action<GameObject> OnModelSpawned;
//    public event Action OnModelDeleted;

//    void Start()
//    {
//        planeDetection = FindObjectOfType<PlaneDetection>();
//    }

//    // Update is called once per frame
//    void Update()
//    {
//        List<ARRaycastHit> hits = planeDetection.Hits;
        
//        if (Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began)
//        {
//            if (spawnedModel != null)
//            {
//                Destroy(spawnedModel);
//                OnModelDeleted?.Invoke();
//            }

//            spawnedModel = Instantiate(ModelToSpawn, hits[0].pose.position, ModelToSpawn.transform.rotation);
//            OnModelSpawned?.Invoke(spawnedModel);
//        }
//    }
//}
