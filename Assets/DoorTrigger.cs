using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DoorTrigger : MonoBehaviour
{
    Animator anim;
    AudioSource doorAS;
    public AudioClip openAC, closeAC;
    public bool isOpen;
    public bool isLooked;
    [SerializeField]
    Text doorText;
    string doorString;

    private void Start()
    {
        doorText.enabled = false;
        doorAS = GetComponent<AudioSource>();
        anim = GetComponent<Animator>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            if (!isLooked && !isOpen)
            {
                doorString = "Open Door";
                UpdateDoorUI();

                if (Input.GetKeyDown(KeyCode.E))
                {
                    OpenDoor();
                }
            }
            else if (isOpen)
            {
                doorString = "Close Door";
                UpdateDoorUI();

                if (Input.GetKeyDown(KeyCode.E))
                {
                    CloseDoor();
                }
            }
            else
            {
                doorString = "Door Is Locked";
                UpdateDoorUI();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            doorText.enabled = false;
        }
    }

    public void OpenDoor()
    {
        isOpen = true;
        doorText.enabled = false;
        anim.SetTrigger("Open");
        doorAS.PlayOneShot(openAC);
    }

    void CloseDoor()
    {
        isOpen = false;
        anim.SetTrigger("Close");
        doorAS.PlayOneShot(closeAC);
    }

    void UpdateDoorUI()
    {
        doorText.enabled = true;
        doorText.text = doorString.ToString();
    }
}
