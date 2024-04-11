using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    [SerializeField]
    private float _movementSpeed;

    public GameObject[] MapSnippet
    {
        get { return _map; }
        set { _map = value; }
    }

    public List<GameObject> MapSnippetDecoration
    {
        get { return _mapDecoration; }
        set { _mapDecoration = value; }
    }

    public bool NewSnippedSpawned { get; set; }

    private int _snipCount;

    [SerializeField]
    private GameObject[] _map;
    [SerializeField]
    private List<GameObject> _mapDecoration;

    private void Start()
    {
        _map = new GameObject[3];
        _map[0] = GameObject.Find("RoadBasicObject");
        _snipCount = 0;
    }

    void Update()
    {
        MoveMap();
    }

    public void SpawnNewRoadSnippet()
    {
        var newObj = CreateNewGameObjectTile();
        
        _snipCount++;
        _map[_snipCount] = (newObj.Item3);
        
        if (_snipCount == 2)
        {
            _snipCount--;
            Destroy(_map[0]);
            var newArray = new GameObject[_map.Length];
            Array.Copy(_map, 1, newArray, 0, _map.Length - 1);
            _map = newArray;
        }
    }

    private (Vector3, Quaternion, GameObject) CreateNewGameObjectTile()
    {
        var tempGameObject = _map[_snipCount]; //Get Road Object
        var pos = tempGameObject.transform.GetChild(3).gameObject.transform.position;
        var rotation = tempGameObject.transform.rotation;
        var newObj = Instantiate(tempGameObject, pos, rotation);

        return (pos, rotation, newObj);
    }


    private void MoveMap()
    {
        foreach (GameObject mapItem in _map)
        {
            if (mapItem != null)
            {
                mapItem.transform.Translate(Vector3.left * Time.deltaTime * _movementSpeed);
            }
        }
    }
}
