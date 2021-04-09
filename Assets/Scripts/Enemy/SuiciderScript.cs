using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SuiciderScript : MonoBehaviour
{
	public Animator deadAnimator;
	public HealthSystem hs;
	public int Cost;    //Плата за убийство
	public float gain_HP = 5;//Усиление волны
	public float gain_SH = 60;//
	public float gain_Reg = 10;//
	public float damage = 30;
    public float gain_dmg = 15;
    public float cost_lv = 15;
    public bool isDead = false;
	public float animation_timer = 3;
	private float speed = 3f;
	private Rigidbody2D rb;
	private GameObject target;
	// Start is called before the first frame update
	void Start()
	{
		deadAnimator = gameObject.GetComponent<Animator>();
		hs = gameObject.AddComponent<HealthSystem> ();
		rb = GetComponent<Rigidbody2D> ();
		hs.SetParam (30 + gain_HP*EnemySpawner.wawecounter, 30+ gain_SH * EnemySpawner.wawecounter, 0 + gain_Reg*EnemySpawner.wawecounter, true);
		damage = EnemySpawner.wawecounter * gain_dmg;
        if (EnemySpawner.counter > 1)
        {
            Cost += Convert.ToInt32(EnemySpawner.wawecounter * cost_lv - cost_lv);
        }
    }

	// Update is called once per frame
	void Update()
	{
		FindTarget ();
		hs.SomeVoid ();
		Move ();
		Reverse ();
		Dead ();
	}

	void OnCollisionEnter2D(Collision2D col)
	{
		if(col.gameObject.tag == "Player"||col.gameObject.tag == "Planet"||col.gameObject.tag == "Station")
		{
			col.gameObject.GetComponent<HealthSystem> ().Hit (damage);
			hs.HP = 0;
		}
	}


	void Dead()
	{
		if (hs.HP <= 0)
		{
			if (!isDead)
			{
				GameObject.Find("Player").SendMessage("GiveMoney", Cost);
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
		rb.AddForce((target.transform.position-transform.position) *speed * Time.deltaTime, ForceMode2D.Impulse);

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
		target = GameObject.FindGameObjectWithTag ("Station");
		GameObject planet = GameObject.Find ("Planet");
		if(target==null||Vector2.Distance(target.transform.position, transform.position)>=Vector2.Distance(planet.transform.position, transform.position))
		{
			target = planet;
		}
	}
}
