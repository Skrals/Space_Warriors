using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
	public GameObject message;
    public GameObject Over;
	public Image image;
	public Slider HP_S;
	public Slider Shield_S;
	public HealthSystem hs;
	public float Speed = 10;
	public float Zone = 2f;
	public float Currzonetime;
    public List<BaseGun> Guns;
    public float Money = 0;
    public bool dead = false;

    private BaseGun SelectedGun;
	private BuilderController builderController;
	static float death_timer = 5;
	static float dt_curr = death_timer;
	private Sprite sprite;
	private float Zonetimer = 5;
    private float vol;
    public float end_timer = 7;
    int selectedGunId;

	void Start()
	{
        vol = Global.sound;
        AudioListener.volume = vol;
		builderController = gameObject.GetComponent<BuilderController>();
		hs = gameObject.AddComponent<HealthSystem> ();
		hs.SetParam (600, 600, 50, true);
        HP_S.maxValue = hs.MaxHP;
        Shield_S.maxValue = hs.MaxArmor;
		ChangeGun(0);

        for (int i = 0; i < Guns.Count; i++)
        {
            Guns[i] = Instantiate(Guns[i]);
            Guns[i].author = gameObject;
        }
	}

    void Update()
    {
		if (dt_curr <=0)
        {
			dt_curr = death_timer;
			dead = false;
		}
        hs.SomeVoid();
		
        Dead();
        if (!dead)
        {
            Rotation();
            Move();
            MoveCamera();
            ZoneDamage();
            if (!builderController.BuildEnabled)
            {
                Shoot();
                SelectGun();
            }
            HUD();
            //Отображение здоровья и щитов//
            HP_S.value = hs.HP;
		
			if (Shield_S.value < 1)
            {
                Shield_S.gameObject.SetActive(false);
            }
            else
            {
                Shield_S.gameObject.SetActive(true);
            }
            Shield_S.value = hs.Armor;
            //Отображение здоровья и щитов//
        }
        else
        {
			if(Money <= -1000)
			{
				end_timer -= Time.deltaTime;
				if (end_timer <= 0)
				{
					Application.LoadLevel(0);
				}
			}
        }
    }
    void GiveMoney(int count)
    {
        Money += count;
    }

	private void HUD()
	{
		
		//Переключение иконок оружия//
		if (SelectedGun.Name == "Laser gun")
		{
            GameObject.Find("Demage out").GetComponent<UnityEngine.UI.Text>().text = "Урон " + SelectedGun.Demage;
            image = GameObject.Find("Gun_Icon").GetComponent<Image>();
			sprite = Resources.Load<Sprite>("Laser_gun");
			image.sprite = sprite;
		}
		if (SelectedGun.Name == "Automatic gun")
		{
            GameObject.Find("Demage out").GetComponent<UnityEngine.UI.Text>().text = "Урон " + SelectedGun.Demage + "\r\n" + "Скорость " + SelectedGun.GetComponent<AutomaticGun>().coolDown;
            image = GameObject.Find("Gun_Icon").GetComponent<Image>();
			sprite = Resources.Load<Sprite>("Shot_gun");
			image.sprite = sprite;
		}
		//Переключение иконок оружия//
	}

	//Опасная зона//
	private void ZoneDamage()
	{
		if ((Mathf.Pow (transform.position.x, 2f) + Mathf.Pow (transform.position.y, 2f)) > Mathf.Pow (Zone, 2f))
		{
			if (Currzonetime >= 0)
			{
				Currzonetime -= Time.deltaTime;
				message.gameObject.SetActive(true);
				GameObject.Find("Warning").GetComponent<UnityEngine.UI.Text>().text = " Вы вошли в опасную зону !!!\r\n Вы начнете получать урон от радиации\r\n Через: " + Convert.ToInt32(Currzonetime);
			}
			else
			{
				hs.PoisonHit (hs.MaxArmor+hs.MaxHP/20, Time.deltaTime);
				GameObject.Find("Warning").GetComponent<UnityEngine.UI.Text>().text = " Вы вошли в опасную зону !!!";
			}
		}
		else
		{
			Currzonetime = Zonetimer;
			message.gameObject.SetActive(false);
		}
	}
	//Опасная зона//

	/// <summary>
	/// Выбор оружия
	/// </summary>
	private void SelectGun()
	{
		if (selectedGunId > 0 && Input.mouseScrollDelta.y < 0)
			ChangeGun(selectedGunId - 1);

		if (selectedGunId < Guns.Count-1 && Input.mouseScrollDelta.y > 0)
			ChangeGun(selectedGunId + 1);

		if (Input.GetKeyDown(KeyCode.Alpha1))
			ChangeGun(0);
		else if (Input.GetKeyDown(KeyCode.Alpha2))
			ChangeGun(1);

	}

	private void ChangeGun(int gunNumber)
	{
		if (SelectedGun != null)
		{
			SelectedGun.EndShoot();
		}
		selectedGunId = gunNumber;
		SelectedGun = Guns[gunNumber];
		SelectedGun.author = gameObject;
	}

    /// <summary>
    /// Поворот корабля за мышью
    /// </summary>
    private void Rotation()
    {

        Vector3 mousePosition = Input.mousePosition;
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
        var angle = Vector2.Angle(Vector2.up, mousePosition - transform.position);

        transform.eulerAngles = new Vector3(0f, 0f, transform.position.x > mousePosition.x ? angle : -angle);

    }

	/// <summary>
	/// Стрельба
	/// </summary>
	private void Shoot()
	{
		SelectedGun.SetRotation(Camera.main.ScreenToWorldPoint(Input.mousePosition));
		if (Input.GetMouseButton(0))
			SelectedGun.StartShoot();
		else
			SelectedGun.EndShoot();
	}

    /// <summary>
    /// Перемещение
    /// </summary>
    private void Move()
    {

        float x = Input.GetAxis("Horizontal") * Time.deltaTime * Speed;
        float y = Input.GetAxis("Vertical") * Time.deltaTime * Speed;
        transform.position += new Vector3(x, y);

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.A))
        {
            this.GetComponent<AudioSource>().enabled = true;
        }
        else
        {
            this.GetComponent<AudioSource>().enabled = false;
        }
    }

	/// <summary>
	/// Перемещение камеры за кораблем
	/// </summary>
	private void MoveCamera()
	{
		Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		var camPos = mousePosition - transform.position;
		camPos /= 8;
		camPos += transform.position;
		Camera.main.transform.position = new Vector3(camPos.x, camPos.y, Camera.main.transform.position.z);
	}

	/// <summary>
	/// Получение урона
	/// </summary>
	/// <param name="Demage"></param>




	/// <summary>
	/// Смерть
	/// </summary>
	private void Dead()
	{
		if (hs.HP <= 0)
		{
			if (dt_curr == 5)
			{
				Money -= 100 * EnemySpawner.wawecounter;
			}
            if (Money <= -1000)
            {
               Over.SetActive(true);
				dead = true;
				transform.position = new Vector3(0, 0, transform.position.z);
                hs.SetDefault();
            }
			
            else
            {
				dead = true;
				dt_curr -= Time.deltaTime;
				if (dt_curr <= 0)
				{
					transform.position = new Vector3(0, 0, transform.position.z);
					hs.SetDefault();
				}
            }
		}
	}
}
