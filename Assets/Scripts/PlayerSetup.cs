using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class PlayerSetup : MonoBehaviourPun
{
    public TextMeshProUGUI playerNameText;
    // Start is called before the first frame update
    void Start()
    {
        if (photonView.IsMine)
        {
            //player is local player
            transform.GetComponent<MovementController>().enabled = true;// to access the values present in movement controller script
            transform.GetComponent<MovementController>().joystick.gameObject.SetActive(true);
        }

        else
        {
            // player is opponent
            transform.GetComponent<MovementController>().enabled = false;// to access the values present in movement controller script
            transform.GetComponent<MovementController>().joystick.gameObject.SetActive(false);
        }

        setPlayername();
    }

    void setPlayername()
    {
        if (playerNameText != null)
        {

            if (photonView.IsMine)
            {
                playerNameText.text = "You";
                playerNameText.color = Color.red;
            }
            else
            {
                playerNameText.text = photonView.Owner.NickName;
            }
        }
    }
}
