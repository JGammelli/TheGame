using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Movement : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey("right"))
        {
            transform.position += Vector3.right * 5f * Time.deltaTime;

        }
        if (Input.GetKey("left"))
        {
            transform.position += Vector3.left * 5f * Time.deltaTime;

        }

    }

}
