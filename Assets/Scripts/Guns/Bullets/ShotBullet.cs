using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotBullet : BaseBullet
{
	public float Speed = 20;
	public float MaxDistance = 25;

	float distance = 0;

	void Update()
	{
		Vector3 move = transform.up * Time.deltaTime * Speed;
		transform.position += move;
		distance += Vector3.Magnitude(move);
		if (distance > MaxDistance)
			Destroy(gameObject);

	}
	void OnTriggerEnter2D(Collider2D col)
	{
        
        if ((author.tag == "Player" && col.tag == "Enemy") ||
			(author.tag == "Station" && col.tag == "Enemy")||
				(author.tag == "Enemy" && (col.tag == "Player" || col.tag == "Planet" || col.tag == "Enemy"|| col.tag == "Station") && col.gameObject != author))
		{
			HealthSystem enemy = col.gameObject.GetComponent<HealthSystem>();
			if (enemy != null)
			{
				enemy.Hit(base.Demage);
			}
			Destroy(gameObject);
		}
	}


}
