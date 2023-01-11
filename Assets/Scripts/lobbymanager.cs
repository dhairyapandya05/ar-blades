using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class lobbymanager : MonoBehaviourPunCallbacks
{

    [Header("Login UI")]
    public InputField playerNameInputField;
    public GameObject uI_Login_Game_Object;

    [Header("Lobby UI")]
    public GameObject uI_Lobby_Game_Object;
    public GameObject uI_3D_Game_Object;

    [Header("Connection Status UI")]
    public GameObject uI_ConnectionStatus_Game_Object;
    public Text connectionStatusText;
    public bool showconncetionstatus = false;
    #region Unity Methods

    // Start is called before the first frame update
    void Start()
    {

        if (PhotonNetwork.IsConnected)
        {

            //aCTIVATING ONLY THE LOBBY UI
            uI_Lobby_Game_Object.SetActive(true);
            uI_3D_Game_Object.SetActive(true);
            uI_ConnectionStatus_Game_Object.SetActive(false);
            uI_Login_Game_Object.SetActive(false);
        }

        else
        {

            //Activating only the login UI since we did not want to connect to photon yet
            uI_Lobby_Game_Object.SetActive(false);
            uI_3D_Game_Object.SetActive(false);
            uI_ConnectionStatus_Game_Object.SetActive(false);
            uI_Login_Game_Object.SetActive(true);
        }


    }

    // Update is called once per frame
    void Update()
    {
        //it will show the condirtion of the network status only and only if the showconncetionstatus is set to true
        if (showconncetionstatus)
        {
            connectionStatusText.text = "Connection Status:" + PhotonNetwork.NetworkClientState;

        }
    }

    #endregion

    #region UI Callback Methods
    public void onEnteredGameButtonClicker()
    {

        string playerName = playerNameInputField.text;

        if (!string.IsNullOrEmpty(playerName))
        {


            uI_Lobby_Game_Object.SetActive(false);
            uI_3D_Game_Object.SetActive(false);
            uI_Login_Game_Object.SetActive(false);
            showconncetionstatus = true;
            uI_ConnectionStatus_Game_Object.SetActive(true);


            if (!PhotonNetwork.IsConnected)
            {
                PhotonNetwork.LocalPlayer.NickName = playerName;
                PhotonNetwork.ConnectUsingSettings();
            }

        }
        else
        {
            Debug.Log("Photon name is invalid or empty");

        }
    }


    public void onQuickMatchButtonClicked()
    {
        // SceneManager.LoadScene("Scene_Loading");
        SceneLoader.Instance.LoadScene("Scene_PlayerSelection");
    }
    #endregion


    #region Photon Callback Region
    public override void OnConnected()
    {
        Debug.Log("We have Connected to the internet");
    }

    public override void OnConnectedToMaster()
    {
        //this method is called when player is sucessfully connected to the server
        Debug.Log(PhotonNetwork.LocalPlayer.NickName + " is connected to Photon");

        uI_Lobby_Game_Object.SetActive(true);
        uI_3D_Game_Object.SetActive(true);
        uI_Login_Game_Object.SetActive(false);
        uI_ConnectionStatus_Game_Object.SetActive(false);

    }
    #endregion
}
