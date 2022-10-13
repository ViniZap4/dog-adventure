using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Moviment Variables")]
    [Space]
    private Vector3 direction;
    public float MovementSpeed = 3f;
    private bool isWalking;

    //input variables
    private float horizontal;
    private float vertical;

    [Header("Attack Variables")]
    [Space] private bool isAttacking;
    public Transform HitArea;
    public float hitRange = 1f;
    public Collider[] hitInfo;
    public LayerMask hitMask;

    public int amountDmg = 10;

    
    private bool isDefending;
    public float defendAngle = 4;

    public int maxHP = 100;
    public int HP = 100;

    public int maxStamina = 100;
    public float stamina = 100f;

    //components
    private CharacterController controller;
    private Animator anim;


    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Inputs();
        MoveCharacter();
        UpdateAnimator();
        UpdateStamina();
    }

    void UpdateStamina()
    {
        if (maxStamina > stamina)
        {

            stamina += 2 * Time.deltaTime;
        }
        else if(stamina != maxStamina)
        {
            stamina = maxStamina;
        }else if (stamina < 0){
            stamina = 0;
        }
    }

    //geting and managing inputs
    void Inputs()
    {
        //mobiment input
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        if(Input.GetButtonDown("Fire1") && !isAttacking && !isDefending && stamina > 5)
        {
            stamina -= 5;
            Attack();
        }

        if (Input.GetButtonDown("Defend") && !isDefending && stamina > 5)
        {
            isDefending = true;
        }

        if (Input.GetButtonUp("Defend"))
        {
            isDefending = false;
        }

        

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

    
    void UpdateAnimator()
    {
        anim.SetBool("isWalking", isWalking);
        anim.SetBool("isDefending", isDefending);
    }

    void Attack()
    {
        isAttacking = true;
        anim.SetTrigger("Attack");

        //hitInfo = Physics.OverlapSphere(HitArea.position, hitRange, hitMask);
        //hitInfo = Physics.OverlapSphere(HitArea.position, hitRange, LayerMask.GetMask("NPC"));

        //foreach (Collider itemCollided in hitInfo)
        //{
            //itemCollided.gameObject.SendMessage("GetHit", amountDmg, SendMessageOptions.DontRequireReceiver);

            //if (itemCollided.tag == "monster" || itemCollided.tag == "DMG")
            //{
            //}
        //}
    }

    void AttackIsDone()
    {
        isAttacking = false;
    }

    void GetHit(int amountDmg)
    {


        if (isDefending)
        {
            if (stamina < 3 * amountDmg)
            {
                isDefending = false;
            }

            stamina -= 3 * amountDmg;
        }
        else
        {
            HP -= amountDmg;

            if (HP > 0)
            {
                anim.SetTrigger("Hit");
            }
            else
            {
                //_GameManager.ChangeGameState(GameState.DIE);
                anim.SetTrigger("Die");
            }
        }
  
    }

    private void OnTriggerEnter(Collider other)
    {
        

        if (other.gameObject.tag == "DMG" && other.transform.parent.tag == "monster")
        {
            GetHit(other.transform.parent.GetComponent<Monster>().amountDmg);
        }
    }


    /*
    private void OnDrawGizmos()
    {

        //(Slime.hitStart.position, Slime.hitEnd.position, Slime.hitRadius);

        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(HitArea.position, hitRange);
    }
    */

}