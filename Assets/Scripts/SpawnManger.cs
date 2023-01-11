using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SpawnManger : MonoBehaviourPunCallbacks
{

    public GameObject[] PlayerPrefabs;
    public Transform[] spawnpositions;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }


    #region Photon CallBack Methods


    public override void OnJoinedRoom()
    {
        if (PhotonNetwork.IsConnectedAndReady)
        {
            object playerSelectionNumber;
            if (PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue(MultiplayerARSpinnerTopGame.PLAYER_SELECTION_NUMBER, out playerSelectionNumber))
            {
                Debug.Log("Player Selection Number :" + (int)playerSelectionNumber);

                int randomSpownPoint = Random.Range(0, spawnpositions.Length - 1);
                Vector3 instantiatePositions = spawnpositions[randomSpownPoint].position;

                PhotonNetwork.Instantiate(PlayerPrefabs[(int)playerSelectionNumber].name, instantiatePositions, Quaternion.identity);

            }
        }

    }

    #endregion
}
