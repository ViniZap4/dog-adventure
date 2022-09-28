using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class Slime : Monster
{
	public ParticleSystem blood;

    void Start()
    {
		agent = GetComponent<NavMeshAgent>();
		StayStill(50);
	}



	private void Update()
	{
		WalkControl();
		AnimControl();
	}



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

  #region process

    void WalkControl()
    {
		if (agent.desiredVelocity.magnitude >= 0.1f) isWalk = true;
		else isWalk = false;
	}

	void AnimControl() // animations
    {
		anim.SetBool("isWalk", isWalk);
	}

	void StopAgentMovement()
    {
		agent.stoppingDistance = 0;
		destination = transform.position;
		agent.destination = destination;
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

			case monsterState.DIE:
				StartCoroutine("DIED");
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



	IEnumerator DIED()
	{
		isDie = true;

		StopAgentMovement();

		anim.SetTrigger("Die");

		yield return new WaitForSeconds(2.5f);
		Destroy(this.gameObject);
	}

	#endregion
}