using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PortalScript : MonoBehaviour {


	public GameObject enemy;
    public GameObject enemy_1;
    public GameObject enemy_2;
    public GameObject enemy_3;
    public int alertlevel;
	public float spawntimer = 2;
	public float lifetime = 10;
    private int count;

	private GameObject enem;
    private GameObject enem_1;
    private GameObject enem_2;
    private GameObject enem_3;
    private float sptimer;
	// Use this for initialization
	void Start ()
    {
	}
	
	// Update is called once per frame
	void FixedUpdate ()
    {
		Spawn ();
	}

	void Spawn()
	{
        
        if (sptimer <= 0)
		{

            count = EnemySpawner.counter;
            enem = Instantiate (enemy);
			/*
			int enemynum = Random.Range (alertlevel, alertlevel+3);
			GameObject enemy;
			if (enemynum >= enemies.Length)
			{
				enemy = enemies [enemies.Length - 1];
			}
			else
			{
				enemy = enemies [enemynum];
			}
			*/
			enem.transform.position = transform.position;
			enem.transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z+90);
            if(count % 4 == 0)
            {
                enem_1 = Instantiate(enemy_1);
                enem_1.transform.position = transform.position;
                enem_1.transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z + 90);
            }
            if (count % 8 == 0)
            {
                enem_2 = Instantiate(enemy_2);
                enem_2.transform.position = transform.position;
                enem_2.transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z + 90);
            }
            if (count % 12 == 0)
            {
                enem_3 = Instantiate(enemy_3);
                enem_3.transform.position = transform.position;
                enem_3.transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z + 90);
            }
            sptimer = spawntimer;
            EnemySpawner.counter += 1;
        }
        
        sptimer -= Time.deltaTime;
		lifetime -= Time.deltaTime;
		if (lifetime <= 0)
		{
			Destroy (gameObject);
		}
	}
}
