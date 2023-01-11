using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;

public class Battlescript : MonoBehaviourPun
{
    public GameObject deathPanelPrefab;
    private GameObject deathPANELuigAMEobject;
    public GameObject ui_3d_gameobject;
    private Rigidbody rb;

    public bool isAttacker;
    public bool isDefender;
    private bool isDead;

    public spinner spinerScript;
    private float startSpinSpeed;
    private float currentSpinSpeed;
    public Image spinSpeedBar_Image;
    public TextMeshProUGUI spinspeedratio_Text;
    public float commondamage_coefficient = 0.04f;

    [Header("Player Type Damage Coefficient")]
    public float doDamage_Coefficient_Attacker = 10f;//do more damage then defender - advantage
    public float getDamage_Coefficient_Attacker = 1.2f;//gets more damage disadvantage

    public float doDamage_Coefficient_Defender = 0.75f;//do less damage - disadvantage
    public float getDamage_Coefficient_Defender = 0.2f;//gets less damage - advantage

    private void checkplayertype()
    {
        if (gameObject.name.Contains("Attacker"))
        {
            isAttacker = true;
            isDefender = false;
        }
        else if (gameObject.name.Contains("Defender"))
        {
            isAttacker = false;
            isDefender = true;
            spinerScript.spinspeed = 4400;
            startSpinSpeed = spinerScript.spinspeed;
            currentSpinSpeed = spinerScript.spinspeed;
            spinspeedratio_Text.text = currentSpinSpeed + "/" + startSpinSpeed;

        }
    }

    private void Awake()
    {
        startSpinSpeed = spinerScript.spinspeed;
        currentSpinSpeed = spinerScript.spinspeed;
        spinSpeedBar_Image.fillAmount = currentSpinSpeed / startSpinSpeed;
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            //Compare the speed of the SpinnerTops
            float mySpeed = gameObject.GetComponent<Rigidbody>().velocity.magnitude;
            float otherplayerSpeed = collision.collider.gameObject.GetComponent<Rigidbody>().velocity.magnitude;
            Debug.Log("My Speed : " + mySpeed + "__________________________" + "oTHER Player Speed : " + otherplayerSpeed);
            if (mySpeed > otherplayerSpeed)
            {

                float default_damage_amount = gameObject.GetComponent<Rigidbody>().velocity.magnitude * 3600f * commondamage_coefficient;

                if (isAttacker)
                {
                    default_damage_amount *= doDamage_Coefficient_Attacker;
                }
                else if (isDefender)
                {
                    default_damage_amount *= doDamage_Coefficient_Defender;

                }

                Debug.Log("You damage the other player");
                if (collision.collider.GetComponent<PhotonView>().IsMine)
                {




                    //Apply damage to the slower player
                    collision.collider.gameObject.GetComponent<PhotonView>().RPC("DoDamage", RpcTarget.AllBuffered, default_damage_amount);
                }
            }


        }
    }
    [PunRPC]
    public void DoDamage(float _damageAmount)
    {

        if (!isDead)
        {
            if (isAttacker)
            {
                _damageAmount *= getDamage_Coefficient_Attacker;

                if (_damageAmount > 1000)
                {
                    _damageAmount = 400f;
                }
            }
            else if (isDefender)
            {
                _damageAmount *= getDamage_Coefficient_Defender;

            }

            spinerScript.spinspeed -= _damageAmount;
            currentSpinSpeed = spinerScript.spinspeed;
            spinSpeedBar_Image.fillAmount = currentSpinSpeed / startSpinSpeed;
            spinspeedratio_Text.text = currentSpinSpeed.ToString("F0") + "/" + startSpinSpeed;

            if (currentSpinSpeed < 100)
            {
                //Die
                Die();
            }
        }


    }

    void Die()
    {
        isDead = true;
        GetComponent<MovementController>().enabled = false;
        rb.freezeRotation = false;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        spinerScript.spinspeed = 0f;
        ui_3d_gameobject.SetActive(false);

        if (photonView.IsMine)
        {
            //cOUNT DOWN FOR RESPOND
            StartCoroutine(ReSpawn());
        }


    }

    IEnumerator ReSpawn()
    {
        GameObject canvasgameobject = GameObject.Find("Canvas");
        if (deathPANELuigAMEobject == null)
        {
            deathPANELuigAMEobject = Instantiate(deathPanelPrefab, canvasgameobject.transform);

        }
        else
        {
            deathPANELuigAMEobject.SetActive(true);
        }
        Text respawnTimeText = deathPANELuigAMEobject.transform.Find("RespawnTimeText").GetComponent<Text>();
        float respawnTime = 8.0f;
        respawnTimeText.text = respawnTime.ToString(".00");

        while (respawnTime > 0.0f)
        {
            yield return new WaitForSeconds(1.0f);
            respawnTime -= 1.0f;
            respawnTimeText.text = respawnTime.ToString(".00");
            GetComponent<MovementController>().enabled = false;

        }
        deathPANELuigAMEobject.SetActive(false);
        GetComponent<MovementController>().enabled = true;
        photonView.RPC("ReBorn", RpcTarget.AllBuffered);
    }



    [PunRPC]
    public void Reborn()
    {
        spinerScript.spinspeed = startSpinSpeed; currentSpinSpeed = startSpinSpeed / startSpinSpeed;
        spinspeedratio_Text.text = currentSpinSpeed + "/" + startSpinSpeed;
        rb.freezeRotation = true;
        transform.rotation = Quaternion.Euler(Vector3.zero);
        ui_3d_gameobject.SetActive(true);
        isDead = false;
    }
    // Start is called before the first frame update
    void Start()
    {
        checkplayertype();
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
