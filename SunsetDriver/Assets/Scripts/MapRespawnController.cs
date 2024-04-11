using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MapRespawnController : MonoBehaviour
{
    private GameObject _mapManager;

    private void Start()
    {
        _mapManager = GameObject.Find("MapManager");
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "RoadPart")
        {
            _mapManager.GetComponent<MapManager>().SpawnNewRoadSnippet();
        }
    }
}
