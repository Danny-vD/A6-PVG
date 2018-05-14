﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingScript : MonoBehaviour
{
    public enum WeaponMode
    {
        AIR,            //Light
        WATER,          //Light
        FIRE,           //Light
        SUCTION,         //Dark
        ICE,            //Dark
        LIGHTNING,      //Dark
    }

    [Header("References")]
    public GameObject IceCone;
    public GameObject Lightning;
    public GameObject AirBall;
    public GameObject FireBall;
    public GameObject WaterBall;
    public Transform ShootingPoint;

    WeaponMode _weaponMode = WeaponMode.SUCTION;

    [SerializeField] private float _suctionPower = 1000f;

    private bool _inLight = false;
    private int _lightLevel;

    private bool DEBUGING = false;


    private KeyCode[] _actionButtons = new KeyCode[] { KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3, KeyCode.Alpha4, KeyCode.Alpha5, KeyCode.Alpha6 };

    private void Start()
    {
        //ToggleMode();
    }

    private void FixedUpdate()
    {
        _lightLevel = 0;
    }

    // Update is called once per frame
    void Update()
    {
        // check if the player is shooting
        if (Input.GetMouseButtonDown(0))
        {
            Shoot();
        }

        if (Input.GetKeyDown(KeyCode.F1))
        {
            DEBUGING = !DEBUGING;
        }

        // check if the projectile is being changed
        foreach (KeyCode button in _actionButtons)
        {
            if (Input.GetKeyDown(button))
            {
                ChangeMode(button);
            }
        }
    }

    private void LateUpdate()
    {
        if (_lightLevel <= 0)
        {
            _inLight = false;
        }
        else
        {
            _inLight = true;
        }
    }


    // check if the player is standing in light
    void OnTriggerStay(Collider other)
    {
        if (other.tag == "Light")
        {
            RaycastHit hit;
            if (Physics.Raycast(other.transform.position, transform.position - other.transform.position, out hit))
            {
                if (hit.collider.tag == "Player")
                {
                    _lightLevel++;
                }
                else
                {
                    _lightLevel--;
                }
            }
            ToggleMode();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Light")
        {
            _inLight = false;
            ToggleMode();
        }
    }

    private void Shoot()
    {
        // shoot projectiles
        switch (_weaponMode)
        {
            case WeaponMode.WATER:
                GameObject newWaterball = Instantiate(WaterBall, ShootingPoint.position, Quaternion.FromToRotation(Vector3.forward, Vector3.forward));
                newWaterball.GetComponent<Rigidbody>().AddForce(ShootingPoint.transform.forward * 1000f);
                Destroy(newWaterball, 10f);
                break;
            case WeaponMode.ICE:
                RaycastHit hitz;
                Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
                Vector3 IceDirection = Vector3.zero;
                if (Physics.Raycast(ray, out hitz))
                {
                    Debug.Log(hitz.transform.name);
                    IceDirection = hitz.point - ShootingPoint.position;
                }
                else
                {
                    IceDirection = ray.GetPoint(100f) - ShootingPoint.position;
                }
                GameObject newIceCone = Instantiate(IceCone, ShootingPoint.position, Quaternion.FromToRotation(Vector3.up, IceDirection));
                newIceCone.GetComponent<Rigidbody>().AddForce(IceDirection.normalized * 1000f);
                Destroy(newIceCone, 10f);
                break;
            case WeaponMode.FIRE:
                GameObject newFireball = Instantiate(FireBall, ShootingPoint.position, Quaternion.FromToRotation(Vector3.forward, Vector3.forward));
                newFireball.GetComponent<Rigidbody>().AddForce(ShootingPoint.transform.forward * 1000f);
                Destroy(newFireball, 10f);
                break;
            case WeaponMode.LIGHTNING:
                RaycastHit hit;
                Ray LightningRay = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
                if (Physics.Raycast(LightningRay, out hit, 100f))
                {
                    Vector3 direction = (hit.point - ShootingPoint.position);
                    int distance = 2;
                    for (int i = 0; i < direction.magnitude; i += distance)
                    {
                        GameObject newLightning = Instantiate(Lightning, ShootingPoint.position + direction.normalized * i, Quaternion.FromToRotation(Vector3.up, direction));
                        Destroy(newLightning, 0.5f);
                    }

                    if(hit.transform.tag == "water")
                    {
                        WaterScript wScript = hit.transform.GetComponent<WaterScript>();
                        if (wScript != null)
                            wScript.Electrocute();
                    }
                }
                break;
            case WeaponMode.AIR:
                GameObject newAirball = Instantiate(AirBall, ShootingPoint.position, Quaternion.FromToRotation(Vector3.forward, Vector3.forward));
                newAirball.GetComponent<Rigidbody>().AddForce(ShootingPoint.transform.forward * 1000f);
                Destroy(newAirball, 10f);
                break;
            case WeaponMode.SUCTION:
                RaycastHit suctionHit;
                Ray suctionRay = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
                if (Physics.Raycast(suctionRay, out suctionHit, 50f))
                {
                    Vector3 retractionDirection = (transform.position - suctionHit.point);
                    retractionDirection.y = 0; // remove upwards/downwards force
                    if (suctionHit.transform.tag == "Cube")
                    {
                        suctionHit.rigidbody.AddForce(retractionDirection.normalized * _suctionPower);
                    }
                }
                break;
        }
    }

    // Check if the player is in the light and change the weapons mode accordingly
    private void ToggleMode()
    {
        ///Light:
        ///WATER
        ///FIRE
        ///AIR
        ///
        ///Dark:
        ///ICE
        ///LIGHTNING
        ///SUCTION
        ///
        if (DEBUGING)
            return;

        if (_inLight)
        {
            switch (_weaponMode)
            {
                case WeaponMode.ICE:
                    _weaponMode = WeaponMode.WATER;
                    break;
                case WeaponMode.LIGHTNING:
                    _weaponMode = WeaponMode.FIRE;
                    break;
                case WeaponMode.SUCTION:
                    _weaponMode = WeaponMode.AIR;
                    break;
            }
        }
        else
        {
            switch (_weaponMode)
            {
                case WeaponMode.WATER:
                    _weaponMode = WeaponMode.ICE;
                    break;
                case WeaponMode.FIRE:
                    _weaponMode = WeaponMode.LIGHTNING;
                    break;
                case WeaponMode.AIR:
                    _weaponMode = WeaponMode.SUCTION;
                    break;
            }
        }
    }

    private void ChangeMode(KeyCode button)
    {
        switch (button)
        {
            case KeyCode.Alpha1:
                _weaponMode = WeaponMode.AIR;
                break;
            case KeyCode.Alpha2:
                _weaponMode = WeaponMode.WATER;
                break;
            case KeyCode.Alpha3:
                _weaponMode = WeaponMode.FIRE;
                break;
            case KeyCode.Alpha4:
                _weaponMode = WeaponMode.SUCTION;
                break;
            case KeyCode.Alpha5:
                _weaponMode = WeaponMode.ICE;
                break;
            case KeyCode.Alpha6:
                _weaponMode = WeaponMode.LIGHTNING;
                break;
        }
    }
}