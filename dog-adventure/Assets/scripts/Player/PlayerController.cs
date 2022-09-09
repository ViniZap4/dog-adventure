using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Moviment Variables")]
    private Vector3 direction;
    public float MovementSpeed = 3f;
    private bool isWalking;


    //input variables
    private float horizontal;
    private float vertical;

    //components
    private CharacterController controller;


    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();

    }

    // Update is called once per frame
    void Update()
    {
        Inputs();
        MoveCharacter();

    }

    //geting and managing inputs
    void Inputs()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

    }

    void MoveCharacter()
    {
        // set direction in axis x and z and normalized
        direction = new Vector3(horizontal, 0f, vertical).normalized;

        // if player is in moving 
        if (direction.magnitude > 0.1f)
        {

            //geting angle of direction and converting for deg
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;

            // set transform rotation of player
            transform.rotation = Quaternion.Euler(0, targetAngle, 0);

            isWalking = true;

        }
        else
        {
            isWalking = false;
        }

        //move Character setting direaction and speed
        controller.Move(direction * MovementSpeed * Time.deltaTime);

    }
}
