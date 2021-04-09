using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GaspachoScript : MonoBehaviour
{
	public Animator deadAnimator;
	public HealthSystem hs;
	public AutomaticGun SelectedGun;
	public int Cost;    //Плата за убийство
	public float gain_HP = 20;//Усиление волны
	public float gain_SH = 20;//
	public float gain_Reg = 2;//
    public float gain_dmg = 10;
    public float cost_lv = 20;
    public bool isDead = false;
	private float animation_timer = 3;
	private float speed = 3f;
	private Rigidbody2D rb;
	private GameObject target;
	// Start is called before the first frame update
	void Start()
	{
		deadAnimator = gameObject.GetComponent<Animator>();
		hs = gameObject.AddComponent<HealthSystem> ();
		rb = GetComponent<Rigidbody2D> ();
		hs.SetParam (30 + gain_HP*EnemySpawner.wawecounter, 50+ gain_SH * EnemySpawner.wawecounter, 5 + gain_Reg*EnemySpawner.wawecounter, true);
		SelectedGun = Instantiate (SelectedGun);
		SelectedGun.SetParam (0, gain_dmg * EnemySpawner.wawecounter, gameObject);
		SelectedGun.coolDown = 0.3f;
        if (EnemySpawner.counter > 1)
        {
            Cost += Convert.ToInt32(EnemySpawner.wawecounter * cost_lv - cost_lv);
            SelectedGun.Demage += EnemySpawner.wawecounter * gain_dmg - gain_dmg;
        }
    }

	// Update is called once per frame
	void Update()
	{
		FindTarget ();
		hs.SomeVoid ();
		Move ();
		Reverse ();
		Shoot ();
		Dead ();
	}


	void Shoot()
	{
		if(target!=null&&hs.HP>0)
		{
			SelectedGun.SetRotation(target.transform.position);
			if (Vector2.Distance (transform.position, target.transform.position) <= 2.5)
			{
				SelectedGun.StartShoot();
			}
			else
			{
				SelectedGun.EndShoot();
			}
		}
	}


	void Dead()
	{
		if (hs.HP <= 0)
		{
			if (!isDead)
			{
				GameObject.Find("Player").SendMessage("GiveMoney", Cost);
				SelectedGun.EndShoot();
				Destroy(SelectedGun.gameObject);
				rb.bodyType = RigidbodyType2D.Static;
				deadAnimator.enabled = true;
				this.GetComponent<AudioSource>().enabled = true;
				GetComponent<BoxCollider2D>().enabled = false;
				tag = "Untagged";
				isDead = true;
				GameObject.Find("Minmap").SendMessage("DeleteMarker", this);
			}

			animation_timer -= Time.deltaTime;
			if (animation_timer <= 0)
			{
				Destroy(gameObject);
			}
		}
	}

	void Move()
	{
		float distance = Vector2.Distance (transform.position, target.transform.position);
		if(distance>=2)
		{
			rb.AddForce((target.transform.position-transform.position) *speed*2f * Time.deltaTime, ForceMode2D.Impulse);
		}
		else
		{
			float angle = transform.position.x > target.transform.position.x ? Vector2.Angle (Vector2.up, target.transform.position - transform.position) : -Vector2.Angle (Vector2.up, target.transform.position - transform.position);
			float sin = Mathf.Sin ((angle + 90) / 180f * Mathf.PI);
			float cos = Mathf.Cos ((angle + 90) / 180f * Mathf.PI);
			float x = transform.position.x - distance * sin;
			float y = transform.position.y +distance * cos;
			Vector3 vec = new Vector3 (x - transform.position.x, y - transform.position.y, transform.position.z);
			rb.AddForce (vec * speed * Time.deltaTime, ForceMode2D.Impulse);
		}
		if (rb.velocity.magnitude >= speed)
		{
			rb.velocity = rb.velocity.normalized * speed;
		}
	}

	void Reverse()
	{
		Vector3 position = target.transform.position;
		var angle = Vector2.Angle (Vector2.up, position-transform.position);
		transform.eulerAngles = new Vector3 (0f, 0f, transform.position.x > position.x ? angle : -angle);
	}

	void FindTarget()
	{
		target = GameObject.Find ("Player");
		GameObject planet = GameObject.Find ("Planet");
		GameObject station = GameObject.FindGameObjectWithTag("Station");
        if (station != null && Vector2.Distance(target.transform.position, transform.position) >= Vector2.Distance(station.transform.position, transform.position))
        {
            target = station;
        }
        if (target==null||Vector2.Distance(target.transform.position, transform.position)>=Vector2.Distance(planet.transform.position, transform.position))
		{
			target = planet;
		}
		
	}
}
