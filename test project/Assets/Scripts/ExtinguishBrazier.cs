﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtinguishBrazier : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "brazier")
        {
            collision.gameObject.GetComponent<Brazier>().Extinguish();
        }

        if (collision.gameObject.tag != "Player")
        {
            Destroy(gameObject);
        }
    }
}