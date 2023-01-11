using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class SpinningTopsGameManager : MonoBehaviourPunCallbacks
{
    [Header("UI")]
    public GameObject uI_InformPanelGameobject;
    public TextMeshProUGUI uI_InformText;
    public GameObject searchforgamesbuttongameobject;
    // Start is called before the first frame update
    void Start()
    {
        uI_InformPanelGameobject.SetActive(true);
        uI_InformText.text = "Search for Games to Battle!";
    }

    // Update is called once per frame
    void Update()
    {

    }


    #region UI CALLBACK METHODS
    public void joinRandomRoom()
    {
        uI_InformText.text = "Searching for available Rooms";

        PhotonNetwork.JoinRandomRoom();
        searchforgamesbuttongameobject.SetActive(false);
    }


    public void OnQuitMatchclick()
    {
        if (PhotonNetwork.InRoom)
        {
            PhotonNetwork.LeaveRoom();
        }
        else
        {
            SceneLoader.Instance.LoadScene("Scene_Lobby");
        }
    }

    #endregion



    #region Photon Callbacks

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log(message);
        uI_InformText.text = message;

        CreateandJoinRoom();
    }


    public override void OnJoinedRoom()
    {
        if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
        {
            uI_InformText.text = "Joined to " + PhotonNetwork.CurrentRoom.Name + ". Waiting for other Players...";

        }
        else
        {
            uI_InformText.text = "Joined to " + PhotonNetwork.CurrentRoom.Name;
            StartCoroutine(Deactivateaftersomeseconds(uI_InformPanelGameobject, 2.0f));

        }
        Debug.Log(PhotonNetwork.NickName + " joined to " + PhotonNetwork.CurrentRoom.Name);
    }


    // when another player enter the room then this method will be triggered 

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log("New Player : " + PhotonNetwork.NickName + " joined to " + PhotonNetwork.CurrentRoom.Name + "Player Count : " + PhotonNetwork.CurrentRoom.PlayerCount);
        uI_InformText.text = "New Player : " + PhotonNetwork.NickName + " joined to " + PhotonNetwork.CurrentRoom.Name + "Player Count : " + PhotonNetwork.CurrentRoom.PlayerCount;
        StartCoroutine(Deactivateaftersomeseconds(uI_InformPanelGameobject, 2.0f));

    }


    public override void OnLeftRoom()
    {
       SceneLoader.Instance.LoadScene("Scene_Lobby");
    }
    #endregion


    void CreateandJoinRoom()
    {
        string randomRoomNmae = "Room " + Random.Range(0, 1000);
        Photon.Realtime.RoomOptions roomOptions = new Photon.Realtime.RoomOptions();
        roomOptions.MaxPlayers = 2;

        //Creating the room
        PhotonNetwork.CreateRoom(randomRoomNmae, roomOptions);

    }

    // to desable the ui of the information after some seconds

    IEnumerator Deactivateaftersomeseconds(GameObject _gameobjects, float seconds)
    {
        yield return new WaitForSeconds(seconds);
        _gameobjects.SetActive(false);
    }
}
