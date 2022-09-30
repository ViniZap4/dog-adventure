using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Monster : MonoBehaviour
{
	protected GameManager _GameManager;

	protected Animator anim;

	public enum monsterState
	{
		IDLE, PATROL, ALERT, FOLLOW, FURY, DIE
	}

	public monsterState selfState;

	//IA variables
	protected NavMeshAgent agent;
	protected int idWaypoint;
	protected Vector3 destination;
	protected FieldOfView fieldOfView;

	// is something
	public bool isDie = false;
	public bool isAlert = false;
	public bool isAttack = false;
	public bool isWalk = false;

	//monster attributes
	public int HP;
	public float lookSpeed;
	public float alertTime;
	public float followPersist;
	public float rangeAttack;
	public float attackDelay;

	//attack
	public Collider[] hitInfo;
	public Transform hitStart;
	public Transform hitEnd;
	public float hitRadius;
	public LayerMask hitMask;
	public int amountDmg = 1;

	public GameObject playerRef;


	protected void Awake()
	{
		_GameManager = FindObjectOfType(typeof(GameManager)) as GameManager;
		anim = GetComponent<Animator>();
		//hitMask = LayerMask.GetMask("Target");
		
	}

	protected int Rand()
	{
		int rand = Random.Range(0, 100); //0 ... 99

		return rand;
	}


}