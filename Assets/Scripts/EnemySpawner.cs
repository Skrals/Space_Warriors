using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemySpawner : MonoBehaviour
{
    public static bool isActive = true;
    public float End_Game = 50;
    public static int counter = 1;
    public static int wawecounter = 1;

    public float End_wawe = 100;
    public GameObject next_wawe;
    public GameObject End_anim;
	public GameObject portal;
	public float spawntimer = 7;
	public float wavetimer = 40;
	public float nowavetimer = 40;
    public float Portal_distance = 14;
	public int alert = 0;
    private bool active = false;
	private float sptimer;
	private float wvtimer;
	private float nwtimer;
    private float end_timer = 7;
    

    void Start()
	{
        wawecounter = 1;
		sptimer = spawntimer;
		wvtimer = wavetimer;
		nwtimer = 0;
    }

    void Update()
    {
        GameObject.Find("Wawe").GetComponent<UnityEngine.UI.Text>().text = "Волна: " + wawecounter;

        if (isActive)
        {
            Spawn();
            Rest();
            
        }
        else
        {
            end_timer -= Time.deltaTime;
            if( end_timer <=0)
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene(0);
            }
        }
    }

	/// <summary>
	/// Спавнит порталы каждые spawntimer секунд в течение wavetimer секунд
	/// </summary>
	void Spawn()
	{
		if (nwtimer <= 0)
		{
			if (sptimer <= 0 && wvtimer > 0)
			{
				float angle = UnityEngine.Random.Range (0, 360);
				Portal_distance = UnityEngine.Random.Range (Portal_distance - 2, Portal_distance + 2);
				float x = -Portal_distance * Mathf.Sin (angle*Mathf.PI/180);
				float y = Portal_distance * Mathf.Cos (angle*Mathf.PI/180);
				portal.transform.position = new Vector3 (x, y, 0);
				portal.transform.eulerAngles = new Vector3 (0, 0, 90 + angle);
//				portal.alertlevel = alert;
				sptimer = spawntimer;
				Instantiate (portal);
                if (wavetimer >= End_wawe)
                {
                    End_Game -= 1;
                }
            }
			wvtimer -= Time.deltaTime;
			sptimer -= Time.deltaTime;
			if (wvtimer <= 0)
			{
				nwtimer = nowavetimer;
			}
		}
	}

	/// <summary>
	/// Отсчитывает время до следующей волны
	/// </summary>
	void Rest()
	{
		if (wvtimer <= 0)
		{
			nwtimer -= Time.deltaTime;
            if (!active)
            {
                next_wawe.SetActive(!next_wawe.activeSelf);
                active = true;
            }
            next_wawe.GetComponent<UnityEngine.UI.Text>().text = "Следующая волна начнется через: " + Convert.ToInt32(nwtimer);
        }
		if (nwtimer <= 0&&wvtimer<=0)
		{
            if (active)
            {
                next_wawe.SetActive(!next_wawe.activeSelf);
                active = false;
            }
            wvtimer = wavetimer;
            wawecounter += 1;
            if(wavetimer <=End_wawe)
            {
                wavetimer += 2;
                nowavetimer +=1f;
            }
            if (spawntimer >= 2)
            {
                spawntimer -= 0.2f;
            }
        }
        if(End_Game <=0)
        {
            isActive = false;
            End_anim.SetActive(true);
        }
	}
}
