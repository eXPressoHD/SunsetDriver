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
   [SerializeField]
   private GameObject _enemyCar;

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

      SpawnCarsOnLanes(newObj.Item3.transform);

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

   private void SpawnCarsOnLanes(Transform newSnippetTransform)
   {
      int numLanes = 0;
      for (int i = 0; i < newSnippetTransform.childCount; i++)
      {
         if (newSnippetTransform.GetChild(i).CompareTag("Lane"))
         {
            numLanes++;
         }
      }

      float[] laneProbabilities = new float[numLanes];
      for (int i = 0; i < laneProbabilities.Length; i++)
      {
         laneProbabilities[i] = 1.0f / numLanes; 
      }

      List<Transform> shuffledLanes = new List<Transform>();
      for (int i = 0; i < newSnippetTransform.childCount; i++)
      {
         Transform laneTransform = newSnippetTransform.GetChild(i);
         if (laneTransform.CompareTag("Lane"))
         {
            shuffledLanes.Add(laneTransform);
         }
      }
      shuffledLanes.Shuffle();

      foreach (Transform laneTransform in shuffledLanes)
      {
         int laneIndex = shuffledLanes.IndexOf(laneTransform);

         if (UnityEngine.Random.value < laneProbabilities[laneIndex])
         {
            GameObject car = Instantiate(_enemyCar, laneTransform.position, Quaternion.Euler(0, 90, 0));
            car.transform.parent = newSnippetTransform;
         }
      }
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

public static class ListExtensions
{
   private static System.Random rng = new System.Random();

   public static void Shuffle<T>(this IList<T> list)
   {
      int n = list.Count;
      while (n > 1)
      {
         n--;
         int k = rng.Next(n + 1);
         T value = list[k];
         list[k] = list[n];
         list[n] = value;
      }
   }
}