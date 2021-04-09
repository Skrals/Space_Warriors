using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserStation : BaseStation
{
    new void Start()
    {
        base.Start();
        (gun as LaserGun).author = gameObject;
        hs.SetParam(1000, 1500,25, true);
        gun.Demage = 250;
    }

    void Update()
    {
        GameObject target = FindTarget();

        if (target != null)
        {
            RotateTo(target.transform.position);
            gun.SetRotation(target.transform.position);
            gun.StartShoot();
        }
        else
        {
            gun.EndShoot();
        }
		Dead ();
		hs.SomeVoid();

    }

	void Dead()
	{
		if (hs.HP <= 0) {
			Destroy (gun);
			Destroy (gameObject);
		}
	}

    private void RotateTo(Vector3 position)
    {
        float angle = Vector2.Angle(Vector3.up, position * 2f * Time.deltaTime - transform.position  *2f * Time.deltaTime);
        if (transform.position.x < position.x)
            angle = -angle;


        transform.eulerAngles = new Vector3(0, 0, angle);
    }
}
