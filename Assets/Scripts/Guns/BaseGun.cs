using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseGun : MonoBehaviour
{
	public float Demage;
	public float accuracy;
	public string Name;
	public GameObject author;


	public abstract void SetRotation(Vector3 target);
	public abstract void StartShoot();
	public abstract void EndShoot();
	public abstract void SetParam (float accuracyAngle, float Damage, GameObject auth);

	public void Destroy()
	{
		GameObject.Destroy(gameObject);
	}

}
