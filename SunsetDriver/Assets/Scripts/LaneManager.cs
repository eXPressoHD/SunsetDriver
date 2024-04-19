using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LaneManager : MonoBehaviour
{

    [SerializeField]
    private GameObject _player;
    private GameObject _currentLane;
    private int _laneIndex;

    [SerializeField]
    private Dictionary<int, GameObject> _lanes;

   [SerializeField]
   private int lanes;

    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _lanes = new Dictionary<int, GameObject>();
        _lanes = FetchAllLanes(); //Get all lanes 
        _laneIndex = 0; //starts at zero!           
        _currentLane = _lanes[_laneIndex]; //Get current lane with startIndex
    }

   private void Update()
   {
      _lanes = FetchAllLanes();
   }

   public void ChangeLane(GameObject objectToMove, ELaneDirection laneDirection)
    {
        var checkTuple = CheckForBesideLane(laneDirection);

        if (checkTuple.laneExists)
        {
            MoveObjectToLane(objectToMove, checkTuple.nextLaneToMove, laneDirection);            
        }
    }

    private void MoveObjectToLane(GameObject objectToMove, GameObject newLane, ELaneDirection laneDirection)
    {
        Debug.Log($"Moved {objectToMove.name} to {newLane.name}");

        var currentPosOfMoveableObject = objectToMove.transform.position;
        var newLanePosition = newLane.transform.position;

        StartCoroutine(MoveFromTo(objectToMove.transform, currentPosOfMoveableObject, newLanePosition, 40f));

        if (laneDirection == ELaneDirection.Right)
        {
            _laneIndex++;
        }
        else
        {
            _laneIndex--;
        }
    }

   IEnumerator MoveFromTo(Transform objectToMove, Vector3 a, Vector3 b, float speed)
   {
      float step = (speed / (a - b).magnitude) * Time.fixedDeltaTime;
      float t = 0;
      while (t <= 1.0f)
      {
         t += step;

         Vector3 newPosition = new Vector3(objectToMove.position.x, objectToMove.position.y, Mathf.Lerp(a.z, b.z, t));

         objectToMove.position = newPosition; 
         yield return new WaitForFixedUpdate();
      }
      objectToMove.position = new Vector3(objectToMove.position.x, objectToMove.position.y, b.z); 
   }

   private (bool laneExists, GameObject nextLaneToMove) CheckForBesideLane(ELaneDirection laneDirection)
    {
        int localLaneIndex = _laneIndex;

        switch (laneDirection)
        {
            case ELaneDirection.Left:
                localLaneIndex--;
                if (localLaneIndex >= 0)
                {
                    if (_lanes[localLaneIndex] != null)
                    {
                        return (true, _lanes[localLaneIndex]);
                    }
                }
                break;

            case ELaneDirection.Right:
                localLaneIndex++;
                if (localLaneIndex <= _lanes.Count)
                {
                    if (_lanes[localLaneIndex] != null)
                    {
                        return (true, _lanes[localLaneIndex]);
                    }
                }
                break;
        }

        return (false, null);
    }

   private Dictionary<int, GameObject> FetchAllLanes()
   {
      var dic = new Dictionary<int, GameObject>();
      var lanes = GameObject.FindGameObjectsWithTag("Lane");

      if (lanes.Length > 4)
      {
         lanes = lanes.Skip(4).Take(4).ToArray();
      }

      int counter = 0;
      foreach (GameObject lane in lanes.OrderBy(key => key.name))
      {
         dic.Add(counter, lane);
         counter++;
      }

      return dic;
   }
}
