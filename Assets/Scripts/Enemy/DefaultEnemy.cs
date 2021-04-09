using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class DefaultEnemy : MonoBehaviour
{
	public Animator deadAnimator;
	public HealthSystem hs;
	public AutomaticGun SelectedGun;
    public int Cost;    //Плата за убийство
    public float gain_HP = 5;//Усиление волны
    public float gain_SH = 10;//
    public float gain_Reg = 2;//
    public float gain_dmg = 2;
    public float cost_lv = 3;
    public bool isDead = false;
    private float animation_timer = 3;
	private float speed = 2f;
	private Rigidbody2D rb;
	private GameObject target;
	// Start is called before the first frame update
	void Start()
	{
		deadAnimator = gameObject.GetComponent<Animator>();
		hs = gameObject.AddComponent<HealthSystem> ();
		rb = GetComponent<Rigidbody2D> ();
		hs.SetParam (10 + gain_HP*EnemySpawner.wawecounter, 10+ gain_SH * EnemySpawner.wawecounter, 0 + gain_Reg*EnemySpawner.wawecounter, true);
		SelectedGun = Instantiate (SelectedGun);
		SelectedGun.SetParam (0, gain_dmg * EnemySpawner.wawecounter + 10, gameObject);
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
		if(Vector2.Distance(transform.position, target.transform.position)>=2)
		{
			rb.AddForce((target.transform.position-transform.position) *speed * Time.deltaTime, ForceMode2D.Impulse);
		}
		else
		{
			rb.AddForce(-(target.transform.position-transform.position) *speed * Time.deltaTime, ForceMode2D.Impulse);
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
        if (Vector2.Distance(target.transform.position, transform.position)>=Vector2.Distance(planet.transform.position, transform.position))
		{
			target = planet;
		}
    }
}

