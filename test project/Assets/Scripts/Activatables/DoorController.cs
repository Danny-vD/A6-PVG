﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : Activatable
{
    private bool _closed = true;
    [HideInInspector] public bool InProgress = false;

    void Update()
    {
        if (_closed)
        {
            if (transform.localPosition.y > 0)
            {
                transform.position = transform.position - new Vector3(0, 0.05f, 0);
                InProgress = true;
            }
            else
            {
                InProgress = false;
            }
        }
        else
        {
            if (transform.localPosition.y < 4)
            {
                transform.position = transform.position + new Vector3(0, 0.05f, 0);
                InProgress = true;
            }
            else
            {
                InProgress = false;
            }
        }
    }

    override public void ToggleActive()
    {
        _closed = !_closed;
    }

    override public void ToggleActive(bool pActive)
    {
        _closed = pActive;
    }
}