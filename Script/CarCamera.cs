using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
	public Transform target;

	[SerializeField] public float smoothSpeed = 10.125f;
	public Vector3 offset;


	void FixedUpdate()
	{
		// Make camera smoothly follow player
		Vector3 desiredPosition = target.position + offset;
		Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.fixedDeltaTime);
		transform.position = smoothedPosition;

		transform.LookAt(target);
	}
}