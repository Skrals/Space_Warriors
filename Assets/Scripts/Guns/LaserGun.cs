using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserGun : BaseGun
{
    public float ChargeTime = 3f;
    public float DemageCD = 0.01f;
    public float Range = 5;
    public GameObject RayPrefab;

    private GameObject ray;
    private Vector3 gunTarget;
	private bool isShooted;
	private float chargeTimer;
	public float demageCDTimer;
    private GameObject target;

	public override void SetParam(float accur, float damage, GameObject you)
	{
		accuracy = accur;
		Demage = damage;
		author = you;
	}

    private void Update()
    {
        if (isShooted)
        {
            if (chargeTimer <= 0)
            {
                if (ray == null)
                {
                    ray = Instantiate(RayPrefab);
                    demageCDTimer = 0;
                }

                FindTarget();
                RotateRay();
                Hit();
            }
            else
            {
                chargeTimer -= Time.deltaTime;
                demageCDTimer -= Time.deltaTime;
            }
        }
    }

    private void Hit()
    {
        if (target != null)
        {
			target.GetComponent<HealthSystem>().PoisonHit(Demage, Time.deltaTime);
            demageCDTimer = DemageCD;   
        }
    }

    void FindTarget()
    {
		RaycastHit2D[] raycast = Physics2D.RaycastAll(author.transform.position, gunTarget - author.transform.position, Range);
        for (int i = 0; i < raycast.Length; i++)
        {
            string targetTag = raycast[i].collider.gameObject.tag;
			if ((author.tag == "Player" && targetTag == "Enemy") ||
				(author.tag == "Station" && targetTag == "Enemy")||
				(author.tag == "Enemy" && (targetTag == "Player" || targetTag == "Planet" || targetTag == "Enemy"|| targetTag == "Station") && raycast[i].collider.gameObject != author))
            {
                target = raycast[i].collider.gameObject;
                return;
            }
        }
        target = null;
    }

    void RotateRay()
    {
        float distance;
        if (target != null)
        {
			distance = Vector2.Distance(author.transform.position, target.transform.position);
        }
        else
        {
            distance = Range;
        }

        ray.transform.localScale = new Vector3(distance, 0.5f, 1);
		float angle = Vector2.Angle(Vector3.right, gunTarget - author.transform.position);
		if (author.transform.position.y > gunTarget.y)
            angle = -angle;
        
		ray.transform.position = new Vector3(author.transform.position.x, author.transform.position.y, author.transform.position.z);
        ray.transform.eulerAngles = new Vector3(0, 0, angle);

    }
    public override void SetRotation(Vector3 target)
    {
        gunTarget = target;
        if (ray != null)
        {
            RotateRay();
        }
    }
    public override void StartShoot()
    {
        if (!isShooted && demageCDTimer <0.1f)
        {
            isShooted = true;
            chargeTimer = ChargeTime;
        }
    }
    public override void EndShoot()
    {
        if (ray != null)
            Destroy(ray);

        isShooted = false;
    }

}
