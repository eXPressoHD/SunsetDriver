using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float movementSpeed;

    private GameObject _gameplayManager;
    private Camera _cam;

    // Start is called before the first frame update
    void Start()
    {
        _cam = Camera.main;
        _gameplayManager = GameObject.Find("GameplayManager");        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            _cam.GetComponent<Animator>().SetTrigger("LeftPressed");
            _gameplayManager.GetComponent<LaneManager>().ChangeLane(this.gameObject, ELaneDirection.Right);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            _cam.GetComponent<Animator>().SetTrigger("RightPressed");
            _gameplayManager.GetComponent<LaneManager>().ChangeLane(this.gameObject, ELaneDirection.Left);
        }
    }
}
