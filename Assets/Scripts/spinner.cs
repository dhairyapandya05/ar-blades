using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spinner : MonoBehaviour
{
    public float spinspeed = 3600;
    public bool dospin = false;
    private Rigidbody rb;
    public GameObject playerGraphics;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (dospin)
        {
            playerGraphics.transform.Rotate(new Vector3(0, spinspeed * Time.deltaTime, 0));
        }
    }
}
