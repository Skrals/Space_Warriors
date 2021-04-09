using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutomaticGun : BaseGun
{
	public ShotBullet Bullet;
	public float coolDown = 0.3f;
	float coolDownTimer = 0;
	Vector3 gunTarget;

	void Start()
	{

	}

	public override void SetParam(float accur, float damage, GameObject you)
	{
		accuracy = accur;
		Demage = damage;
		author = you;
	}

	public override void SetRotation(Vector3 target)
	{
		gunTarget = target;
		transform.position = author.transform.position;
	}

	public override void StartShoot()
	{
		if (coolDownTimer <= 0)
		{
			float range = UnityEngine.Random.Range (-accuracy, accuracy);
			float angle = transform.position.x > gunTarget.x ? Vector2.Angle (Vector2.up, gunTarget - transform.position) : -Vector2.Angle (Vector2.up, gunTarget - transform.position);
			ShotBullet bullet = Instantiate(Bullet);
			bullet.Demage = Demage;
			Vector3 size = Bullet.GetComponent<Renderer>().bounds.size;
			bullet.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
			bullet.author = author;
			bullet.transform.eulerAngles = new Vector3(0, 0, angle+range);
			coolDownTimer = coolDown;
		}
		coolDownTimer -= Time.deltaTime;
	}

	public override void EndShoot()
	{
		return;
	}
}
