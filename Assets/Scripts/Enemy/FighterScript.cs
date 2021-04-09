using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class FighterScript : MonoBehaviour
{
	public Animator deadAnimator;
	public HealthSystem hs;
	public LaserGun SelectedGun;
	public int Cost;    //Плата за убийство
    public float gain_HP = 10;//Усиление волны
    public float gain_SH = 10;//
    public float gain_Reg = 5;//
    public float gain_dmg = 2;
    public float cost_lv = 8;
    public bool isDead = false;
	private float animation_timer = 3;
	private float speed = 4f;
	private Rigidbody2D rb;
	private GameObject target;
	// Start is called before the first frame update
	void Start()
	{
		deadAnimator = gameObject.GetComponent<Animator>();
		hs = gameObject.AddComponent<HealthSystem> ();
		rb = GetComponent<Rigidbody2D> ();
        hs.SetParam(10 + gain_HP * EnemySpawner.wawecounter, 30 + gain_SH * EnemySpawner.wawecounter, 10 + gain_Reg * EnemySpawner.wawecounter, true);
        SelectedGun = Instantiate (SelectedGun);
		SelectedGun.author = gameObject;
		SelectedGun.SetParam (0, gain_dmg * EnemySpawner.wawecounter + 15, gameObject);
        if(EnemySpawner.counter >1)
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
		Shoot ();
		Dead ();
	}


	void Shoot()
	{
		if(target!=null&&!isDead)
		{
			SelectedGun.SetRotation(target.transform.position);
			if (Vector2.Distance (transform.position, target.transform.position) <= 3)
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
		if(Vector2.Distance (transform.position, target.transform.position) >= 3)
		{
			rb.AddForce((target.transform.position-transform.position) *speed * Time.deltaTime, ForceMode2D.Impulse);

		}
		else
		{
			rb.AddForce(-(target.transform.position-transform.position) *speed/2 * Time.deltaTime, ForceMode2D.Impulse);
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
        
    }
}
