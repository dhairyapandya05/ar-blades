using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;

public class PlayerSelectionManager : MonoBehaviour
{
    public Transform playerSwitcherTransform;


    public GameObject[] spinnerTopModles;
    public int playerselectionNumber;

    [Header("UI")]
    public TextMeshProUGUI playerModelType_Text;

    public Button nextButton;
    public Button previousButton;

    public GameObject ui_Selection;
    public GameObject ui_After_Selection;
    #region unity methods
    // Start is called before the first frame update
    void Start()
    {
        ui_Selection.SetActive(true);
        ui_After_Selection.SetActive(false);
        playerselectionNumber = 0;

    }

    // Update is called once per frame
    void Update()
    {


        // Debug.Log("Bahut zyada Chal raha hai marae bhai");

    }
    #endregion


    #region ui callback region
    public void NextPlayyyyer()
    {
        playerselectionNumber += 1;
        if (playerselectionNumber > 3)
            playerselectionNumber = 0;
        nextButton.enabled = false;
        previousButton.enabled = false;
        Debug.Log("Next Player is clicked");
        Debug.Log("PLayer Selected : " + playerselectionNumber);
        StartCoroutine(Rotate(Vector3.up, playerSwitcherTransform, 90, 1.0f));
        if (playerselectionNumber == 0 || playerselectionNumber == 1)
        {
            // the type of the spinner is of atackeker type
            playerModelType_Text.text = "Attack";
        }
        else
        {
            playerModelType_Text.text = "Defend";

        }
    }

    public void PreviousPlayyyyer()
    {
        playerselectionNumber -= 1;
        if (playerselectionNumber < 0)
            playerselectionNumber = 3;
        nextButton.enabled = false;
        previousButton.enabled = false;
        Debug.Log("Previous Player is clicked");
        Debug.Log("PLayer Selected : " + playerselectionNumber);
        StartCoroutine(Rotate(Vector3.up, playerSwitcherTransform, -90, 1.0f));
        if (playerselectionNumber == 0 || playerselectionNumber == 1)
        {
            // the type of the spinner is of atackeker type
            playerModelType_Text.text = "Attack";
        }
        else
        {
            playerModelType_Text.text = "Defend";

        }

    }


    public void OnSelectButtonClicked()
    {
        ui_Selection.SetActive(false);
        ui_After_Selection.SetActive(true);
        ExitGames.Client.Photon.Hashtable playerselecctionprop = new ExitGames.Client.Photon.Hashtable { { MultiplayerARSpinnerTopGame.PLAYER_SELECTION_NUMBER, playerselectionNumber } };
        PhotonNetwork.LocalPlayer.SetCustomProperties(playerselecctionprop);

    }


    public void onReselectButtonClicked()
    {
        ui_Selection.SetActive(true);
        ui_After_Selection.SetActive(false);
    }

    public void onBackbuttonclicked()
    {
        SceneLoader.Instance.LoadScene("Scene_Lobby");

    }

    public void onBattleButtonClicked()
    {
        SceneLoader.Instance.LoadScene("Scene_Gameplay");
    }
    #endregion

    #region private methods

    IEnumerator Rotate(Vector3 axis, Transform transformToRotate, float angle, float duration = 1.0f)
    {

        Quaternion originalRotation = transformToRotate.rotation;
        Quaternion finalRotation = transformToRotate.rotation * Quaternion.Euler(axis * angle);

        float elapsedTime = 0.0f;
        while (elapsedTime < duration)
        {
            transformToRotate.rotation = Quaternion.Slerp(originalRotation, finalRotation, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transformToRotate.rotation = finalRotation;
        nextButton.enabled = true;
        previousButton.enabled = true;



    }


    #endregion
}


