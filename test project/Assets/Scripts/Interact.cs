﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;
using UnityEngine.UI;

public class Interact : MonoBehaviour
{
    [SerializeField] float SlowedWalkingSpeed = 3;
    [SerializeField] float SlowedRunningSpeed = 4;

    [SerializeField] Image Crosshair;
    [SerializeField] Text InteractableUI;

    private GameObject _heldObject;
    private FirstPersonController _playerController;

    void Start()
    {
        _playerController = GetComponent<FirstPersonController>();
    }

    void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, 2f))
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
 
                // LEVER
                if (hit.collider.tag == "Lever")
                {
                    Activate lever = hit.collider.GetComponent<Activate>();

                    lever.Action();

                    lever.Animation();
                }

                // CUBE
                if (hit.collider.tag == "Cube")
                {
                    _heldObject = hit.collider.gameObject;
                    hit.transform.parent = transform;
                    _playerController.SetSpeed(SlowedWalkingSpeed, SlowedRunningSpeed);
                }
            }

            if(hit.transform.gameObject.layer == LayerMask.NameToLayer("Interactable"))
            {
                InteractableUI.gameObject.SetActive(true);
                Crosshair.gameObject.SetActive(false);
            }
            else
            {
                ResetUI();
            }

        }
        else
        {
            ResetUI();
        }

        // Release object
        if (Input.GetKeyUp(KeyCode.E) || Input.GetKeyDown(KeyCode.Space))
        {
            if(_heldObject != null)
            {
                _heldObject.transform.parent = null;
                _heldObject = null;
                _playerController.ResetSpeed();
            }
        }

    }

    private void ResetUI()
    {
        InteractableUI.gameObject.SetActive(false);
        Crosshair.gameObject.SetActive(true);
    }
}
