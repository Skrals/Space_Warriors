using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutomaticStation : BaseStation
{
    private UpgradePanel up;
    new void Start()
    {
        base.Start();
        (gun as AutomaticGun).author = gameObject;
        hs.SetParam(2000, 3000, 50, true);
        gun.Demage = 180;
        gun.GetComponent<AutomaticGun>().coolDown = 0.2f;
    }
    void Update()
    {
        GameObject target = FindTarget();

        if (target != null)
        {
            float distance = Vector2.Distance(target.transform.position * 2f* Time.deltaTime, transform.position *2f * Time.deltaTime);
            Vector3 velocity = target.GetComponent<Rigidbody2D>().velocity;
            Vector3 position = target.transform.position + (velocity * distance) / 2;

            RotateTo(position);
            gun.SetRotation(position);
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

        float angle = Vector2.Angle(Vector3.up, position * 2f * Time.deltaTime - transform.position * 2f * Time.deltaTime);
        if (transform.position.x < position.x)
            angle = -angle;


        transform.eulerAngles = new Vector3(0, 0, angle);
    }
}
