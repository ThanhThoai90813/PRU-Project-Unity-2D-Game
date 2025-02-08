using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEye : MonoBehaviour
{
	// Start is called once before the first execution of Update after the MonoBehaviour is created

	public float flightSpeed = 2f;
	private float waypointReachedDistance = 0.1f;
	public DetectionZone biteDetectionZone;
	public List<Transform> waypoints;

	Animator animator;
	Rigidbody2D rb;
	Damageable damageable;

	Transform nextWaypoint;
	int waypointNum = 0;

	public bool _hasTarget = false;
	

	public bool HasTarget
	{
		get { return _hasTarget; }
		private set
		{
			_hasTarget = value;
			animator.SetBool(AnimationStrings.hasTarget, value);
		}
	}

	public bool CanMove
	{
		get
		{
			return animator.GetBool(AnimationStrings.canMove);
		}
	}

	private void Awake()
	{
		animator = GetComponent<Animator>();
		rb = GetComponent<Rigidbody2D>();
		damageable = GetComponent<Damageable>();
	}

	private void Start()
	{
		nextWaypoint = waypoints[waypointNum];
	}

	// Update is called once per frame
	void Update()
    {
        HasTarget = biteDetectionZone.detectedColliders.Count > 0;
    }

	private void FixedUpdate()
	{
		if (damageable.IsAlive)
		{
			if (CanMove)
			{
				Flight();
			}
			else
			{
				rb.linearVelocity = Vector3.zero;
			}
		}
	}

	private void Flight()
	{
		Vector2 directionToWaypoint = (nextWaypoint.position - transform.position).normalized;
		
		float distance = Vector2.Distance(nextWaypoint.position, transform.position);

		rb.linearVelocity = directionToWaypoint * flightSpeed;
		UpdateDirection();
		if(distance <= waypointReachedDistance)
		{
			waypointNum++;

			if(waypointNum >= waypoints.Count)
			{
				waypointNum = 0;
			}

			nextWaypoint = waypoints[waypointNum]; 
		}
	}

	private void UpdateDirection()
	{
		Vector3 localScale = transform.localScale;

		if(transform.localScale.x > 0)
		{
			if (rb.linearVelocity.x < 0)
			{
				transform.localScale = new Vector3(-1 * localScale.x, localScale.y, localScale.z);
			}
		}
		else
		{
			if(rb.linearVelocity.x > 0)
			{
				transform.localScale = new Vector3(-1 * localScale.x, localScale.y, localScale.z);
			}
		}
	}
}
