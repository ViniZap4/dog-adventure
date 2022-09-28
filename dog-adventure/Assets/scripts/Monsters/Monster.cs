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
		IDLE, ALERT, PATROL, FOLLOW, FURY, DIE
	}

	public monsterState selfState;

	//IA variables
	protected NavMeshAgent agent;
	protected int idWaypoint;
	protected Vector3 destination;
	protected bool isWalk;

	//monster attributes
	public bool isDie = false;
	public bool isAlert = false;
	public int HP = 100;


	protected void Awake()
	{
		_GameManager = FindObjectOfType(typeof(GameManager)) as GameManager;
		anim = GetComponent<Animator>();
	}

	protected int Rand()
	{
		int rand = Random.Range(0, 100); //0 ... 99

		return rand;
	}
}