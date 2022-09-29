using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class Slime : Monster
{
	public ParticleSystem blood;

    void Start()
    {
		// get Components
		agent = GetComponent<NavMeshAgent>();
		fieldOfView = GetComponent<FieldOfView>();
		playerRef = fieldOfView.playerRef;

		//start state
		StayStill(50);

		//atributes
		HP = 30;
		lookSpeed = 1.5f;
		alertTime = 1.5f;
		followPersist = 1.8f;
		rangeAttack = 2.3f;
		attackDelay = 1f;
		agent.speed = 1f;

	}



	private void Update()
	{
		if(selfState != monsterState.DIE)
        {
			StateManager();
			WalkControl();
			AnimControl();
			CanSeeEvent();
		}
		
	}


	#region event

	public void GetHit(int amountDmg)
	{
		print(HP);

		if (isDie == true) return;

		HP -= amountDmg;

		if (HP > 0)
		{
			if (selfState != monsterState.FURY) changeState(monsterState.FURY);
			blood.Emit(18);
			anim.SetTrigger("GetHit");
		}
		else // is dead
		{
			changeState(monsterState.DIE);	
		}
	}

	void CanSeeEvent()
	{
		//if (_GameManager.gameState != GameState.GAMEPLAY) return;

		if (fieldOfView.canSeePlayer && !isAlert && selfState != monsterState.FURY)
		{
			changeState(monsterState.ALERT);
		}

	}

	void Attack()
	{
		if (!isAttack && fieldOfView.canSeePlayer)
		{
			LookAt();
			isAttack = true;
			anim.SetTrigger("Attack");

		}
	}

	void AttackIsDone()
	{
		hitInfo = Physics.OverlapCapsule(hitStart.position, hitEnd.position, hitRadius);

		foreach (Collider itemCollided in hitInfo)
		{
			if (itemCollided.name == "DogPolyart")
			{
				itemCollided.gameObject.SendMessage("GetHit", amountDmg, SendMessageOptions.DontRequireReceiver);
				Debug.Log("send dmg to" + itemCollided.name);
				break;
			}
		}

		StartCoroutine("ATTACK");
	}

	#endregion

	#region process 


	void WalkControl()
    {
		if (agent.desiredVelocity.magnitude >= 0.1f) isWalk = true;
		else isWalk = false;
	}

	void AnimControl() // animations
    {
		anim.SetBool("isWalk", isWalk);
		anim.SetBool("isAlert", isAlert);
	}


	//IA
	void StopAgentMovement()
    {
		agent.stoppingDistance = 0;
		destination = transform.position;
		agent.destination = destination;
	}

	void LookAt()
    {
		//get player rotation
		Vector3 LookDiretion = (playerRef.transform.position - transform.position).normalized;
		Quaternion LookRotation = Quaternion.LookRotation(LookDiretion);
		//rotating to direction player
		transform.rotation = Quaternion.Slerp(transform.rotation, LookRotation, lookSpeed * Time.deltaTime);
	}

	// intersperse between idle and patrol
	void StayStill(int yes)
	{
		if (Rand() <= yes) changeState(monsterState.IDLE);
		else changeState(monsterState.PATROL);
	}



#endregion

#region State

    void changeState(monsterState newState)
	{
		StopAllCoroutines();
		selfState = newState;

		print(selfState);

		

		switch (newState)
		{
			case monsterState.IDLE:
				StartCoroutine("IDLE");
				break;


			case monsterState.PATROL:
				StartCoroutine("PATROL");
				break;

			case monsterState.ALERT:
				StopAgentMovement();
				isAlert = true;
				StartCoroutine("ALERT");
				break;

			case monsterState.FOLLOW:
				agent.stoppingDistance = rangeAttack;
				StartCoroutine("FOLLOW");
				break;

			case monsterState.FURY:
				agent.stoppingDistance = rangeAttack;
				break;

			case monsterState.DIE:
				StartCoroutine("DIED");
				break;
		}
	}

	void StateManager()
    {

		//reduce alert sensor - change to normal vision
		if(selfState != monsterState.ALERT && selfState != monsterState.FURY && fieldOfView.currentAngle != fieldOfView.angle)
        {
			fieldOfView.currentAngle = Mathf.Lerp(fieldOfView.currentAngle, fieldOfView.angle, 1f * Time.deltaTime);

			if(selfState != monsterState.FOLLOW && selfState != monsterState.FURY && fieldOfView.currentRadius != fieldOfView.radius)
				fieldOfView.currentRadius = Mathf.Lerp(fieldOfView.currentRadius, fieldOfView.radius, 1f * Time.deltaTime);
		}

		// folloing player and init attack
        if(selfState == monsterState.FOLLOW || selfState == monsterState.FURY)
        {
			destination = playerRef.transform.position;
			agent.destination = destination;

			LookAt();

			if (agent.remainingDistance <= agent.stoppingDistance)
			{
				Attack();
			}
		}

		// normal speed
        if (selfState != monsterState.FURY)
        {
			agent.speed = 1f;
		}

		switch (selfState)
		{
			case monsterState.ALERT:

				//update vision
				fieldOfView.currentAngle = Mathf.Lerp(fieldOfView.currentAngle, (fieldOfView.angle * 2.1f), 1f * Time.deltaTime);
				fieldOfView.currentRadius = Mathf.Lerp(fieldOfView.currentRadius, (fieldOfView.radius * 1.5f), 1f * Time.deltaTime);

				LookAt();
				break;


			case monsterState.FURY:

				float distancePlayer = Vector3.Distance( playerRef.transform.position, transform.position) + 3f;
				fieldOfView.currentRadius = Mathf.Lerp(fieldOfView.currentRadius, distancePlayer, 1f * Time.deltaTime);
				agent.speed = 2f;

                if (!fieldOfView.canSeePlayer)
                {
					StartCoroutine("Fury");
				}

				break;
		}
	}

	#endregion

	#region IEnumerator

	IEnumerator IDLE()
	{
		StopAgentMovement();

		yield return new WaitForSeconds(3);
		StayStill(40);
	}

	IEnumerator PATROL()
	{
		agent.stoppingDistance = 0;

		// adding new point way.
		idWaypoint = Random.Range(0, _GameManager.slimeWayPoint.ToArray().Length);
		destination = _GameManager.slimeWayPoint[idWaypoint].position;
		agent.destination = destination;

		yield return new WaitUntil(() => agent.remainingDistance <= 0);
		StayStill(50);
	}

	IEnumerator ALERT()
	{
		yield return new WaitForSeconds(alertTime);

		if (fieldOfView.canSeePlayer)
		{
			changeState(monsterState.FOLLOW);
		}
		else
		{
			isAlert = false;
			StayStill(10);
		}
	}

	IEnumerator FOLLOW()
	{
		yield return new WaitUntil(() => !fieldOfView.canSeePlayer);
		yield return new WaitForSeconds(followPersist);
		changeState(monsterState.ALERT);
	}

	IEnumerator Fury()
	{

		yield return new WaitForSeconds(followPersist * 3);

		if (fieldOfView.canSeePlayer)
		{
			changeState(monsterState.FURY);
		}
		else
		{
			StayStill(10);
		}
	}

	IEnumerator ATTACK()
	{
		yield return new WaitForSeconds(attackDelay);
		isAttack = false;

	}

	IEnumerator DIED()
	{
		isDie = true;

		StopAgentMovement();

		anim.SetTrigger("Die");

		yield return new WaitForSeconds(2.5f);
		Destroy(this.gameObject);
	}
	#endregion



	private void OnDrawGizmos()
	{

		//(Slime.hitStart.position, Slime.hitEnd.position, Slime.hitRadius);

		Gizmos.color = Color.magenta;
		Gizmos.DrawWireSphere(hitStart.position, hitRadius);
		Gizmos.DrawWireSphere(hitEnd.position, hitRadius);

		Gizmos.DrawLine(hitStart.position, hitEnd.position);

	}
}