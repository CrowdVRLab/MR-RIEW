using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExchangeControls : MonoBehaviour
{
    GameObject player;

    void Start()
    {
        player = GameObject.Find("OVRPlayerController");
        player.GetComponent<CharacterController>().enabled = false;
        player.GetComponent<OVRPlayerController>().enabled = false;

    }


    private void OnDisable()
    {
        if (player)
        {
            player.GetComponent<CharacterController>().enabled = true;
            player.GetComponent<OVRPlayerController>().enabled = true;
        }
    }
}
