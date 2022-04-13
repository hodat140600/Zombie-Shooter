﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pickup : MonoBehaviour
{
    Ray ray;
    RaycastHit hit;
    [SerializeField]
    float pickupDistance = 5f;
    
    Camera mainCam;
    public Text pickupText;
    SimpleShoot pistolScript;
    public LayerMask layer;
    string pickupInfo;
    [Header("Audio")]
    public AudioClip pickupAmmoAC;
    public AudioClip pickupHealthAC;
    AudioSource pickupAS;

    private void Start()
    {
        pickupAS = GetComponent<AudioSource>();
        mainCam = Camera.main;
        pistolScript = GetComponentInChildren<SimpleShoot>();
    }

    private void Update()
    {
        ray = mainCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        if (Physics.Raycast(ray, out hit, pickupDistance, layer))
        {
            pickupText.enabled = true;
            pickupText.text = hit.transform.name.ToString();
            if (hit.transform.tag == "PistolAmmo")
            {
                if (pistolScript.currentCarriedAmmo < pistolScript.maxCarriedAmmo)
                {
                    PickupPistolAmmo();
                }
                else
                {
                    pickupInfo = "Pistol Ammo Full";
                    pickupText.text = pickupInfo;
                }
            }
            else if (hit.transform.tag == "HealthPack")
            {
                if (PlayerHealth.singleton.currentHealth < PlayerHealth.singleton.maxHealth)
                {
                    PickupHealth();
                }
                else
                {
                    pickupInfo = "Health Full";
                    pickupText.text = pickupInfo;
                }
            }
        }
        else
        {
            pickupText.enabled = false;
        }
    }

    void PickupPistolAmmo()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Destroy(hit.transform.gameObject);
            pistolScript.currentCarriedAmmo = pistolScript.maxCarriedAmmo;
            pistolScript.UpdateAmmoUI();
            pickupAS.PlayOneShot(pickupAmmoAC);
            pickupText.enabled = false;

        }

    }

    void PickupHealth()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            HealthPack healthPackScript = hit.transform.GetComponent<HealthPack>();
            float healthAmmount = healthPackScript.healthAmmount;
            if (PlayerHealth.singleton.currentHealth + healthAmmount > PlayerHealth.singleton.maxHealth)
            {
                PlayerHealth.singleton.currentHealth = PlayerHealth.singleton.maxHealth;
                PlayerHealth.singleton.UpdateHealthUI();
            }
            else
            {
                PlayerHealth.singleton.AddHealth(healthAmmount);
                PlayerHealth.singleton.UpdateHealthUI();
            }
            Destroy(hit.transform.gameObject);
            pickupAS.PlayOneShot(pickupHealthAC);
            pickupText.enabled = false;

        }

    }
}
