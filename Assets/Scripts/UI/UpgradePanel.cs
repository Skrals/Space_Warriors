using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradePanel : MonoBehaviour
{
    public bool Showed = false;
    public GameObject Player;
    public GameObject Planet;
    public GameObject[] stations;
    //Переменные для апгрейда//
    public float HP_UP = 40;
    public float Shield_UP = 20;
    public float Laser_dmg = 25;
    public float Automatic_gun_dmg = 10;
    public float Automatic_gun_speed = 0.01f;
    public float Planet_HP = 50;
    public float Planet_SH = 25;
    public float Planet_reg = 2;
    public float Stations_hp = 150;
    public float Stations_Sh = 250;
    public float Stations_reg = 5;
    public float Stations_dmg = 15;
    /// <summary>
    /// /////////
    /// </summary>
    /// Переменные для подсчета стоимости апгрейда///
    public float Default_cost = 50;
    public float General_default_cost = 10;
    private float General_scale = EnemySpawner.wawecounter;
    private float Upgrade_count = 1;
    private float HP_count = 1;
    private float Shield_count = 1;
    private float Laser_count = 1;
    private float Auto_gun_count = 1;
    private float Planet_count = 1;

    /// <summary>
    /// ///////////
    /// </summary>
    ///
    private string sp;//замена вывода
    private float HP_COST;
    private float SHIELD_COST;
    private float LASER_COST;
    private float GUN_COST;
    private float PLANET_COST;
    ///
    private Vector2 size;
    private RectTransform rt;
    // Use this for initialization
    void Start()
    {
        rt = GetComponent<RectTransform>();
        size = rt.sizeDelta;

    }

    // Update is called once per frame
    void Update()
    {
        Out();
        InputProcessing();
        Vector2 pos = rt.anchoredPosition;
        if (Showed)
        {
            if (pos.x < 10)
            {
                float move = Time.deltaTime * 700;
                if (pos.x + move > 10)
                    move = 10 - pos.x;
                rt.anchoredPosition = new Vector3(pos.x + move, pos.y);
            }

        }
        else
        {
            if (pos.x > -size.x)
            {
                float move = -Time.deltaTime * 700;
                if (pos.x + move < -size.x)
                    move = -size.x - pos.x;
                rt.anchoredPosition = new Vector3(pos.x + move, pos.y);
            }

        }
        //подсчет улучшений//
        HP_COST = ((Default_cost * HP_count) + (Upgrade_count * General_default_cost));
        SHIELD_COST = ((Default_cost * Shield_count) + (Upgrade_count * General_default_cost));
        LASER_COST = ((Default_cost * Laser_count) + (Upgrade_count * General_default_cost));
        GUN_COST = ((Default_cost * Auto_gun_count) + (Upgrade_count * General_default_cost));
        PLANET_COST = ((Default_cost * Planet_count) + (Upgrade_count * General_default_cost*3));
        //подсчет улучшений//
    }

    /// <summary>
    /// Обработка ввода
    /// </summary>
    private void InputProcessing()
    {

        if (Input.GetKeyDown(KeyCode.I))
        {
            Showed = !Showed;
            if (Showed)
            {
                GameObject.Find("Player").GetComponent<BuilderController>().BuildEnabled = false;
                GameObject.Find("Build_selector").GetComponent<BuildSelectorController>().Showed = false;
            }
        }
    }
    public void Out()
    {
        if (Player.GetComponent<PlayerController>().Guns[0].GetComponent<AutomaticGun>().coolDown > 0.11)
        {
            sp = "+ " + Automatic_gun_speed + " скорости";
        }
        else
        {
            sp = "";
        }
        GameObject.Find("HP_cost").GetComponent<UnityEngine.UI.Text>().text = "Стоимость: " + HP_COST + "\r\n" + "+ " + HP_UP + " прочности";
        GameObject.Find("Shield_cost").GetComponent<UnityEngine.UI.Text>().text = "Стоимость: " + SHIELD_COST + "\r\n" + "+ " + Shield_UP + " щита\r\n" + "+ " + Shield_UP / 10 + " регенерации";
        GameObject.Find("Laser_cost").GetComponent<UnityEngine.UI.Text>().text = "Стоимость: " + LASER_COST + "\r\n" + "+ " + Laser_dmg + " урона";
        GameObject.Find("Automatic_gun_cost").GetComponent<UnityEngine.UI.Text>().text = "Стоимость: " + GUN_COST + "\r\n" + "+ " + Automatic_gun_dmg + " урона\r\n" + sp;
        GameObject.Find("Defence_cost").GetComponent<UnityEngine.UI.Text>().text = "Стоимость: " + PLANET_COST + "\r\n"  + "+ " + Planet_HP + " прочности" + "\r\n" + "+ " + Planet_SH + " щита" + "\r\n" + "+ " + Planet_reg + " регенерации" + "\r\n \r\n Станции\r\n" + "+ " + Stations_hp +" прочности\r\n"+ "+ " + Stations_Sh +" Щита\r\n"+"+ " + Stations_reg + " регенерации";
    }
    public void Laser_upgrade()
    {
        if (Player.GetComponent<PlayerController>().Money >= LASER_COST)
        {
            Player.GetComponent<PlayerController>().Guns[1].Demage += Laser_dmg;
            Player.GetComponent<PlayerController>().Money -= LASER_COST;
            Upgrade_count += 1;
            Laser_count += 1;
        }
    }
    public void Automatic_gun_upgrade()
    {
        if (Player.GetComponent<PlayerController>().Money >= GUN_COST)
        {
            Player.GetComponent<PlayerController>().Guns[0].Demage += Automatic_gun_dmg;
            if (Player.GetComponent<PlayerController>().Guns[0].GetComponent<AutomaticGun>().coolDown > 0.11)
            {
                Player.GetComponent<PlayerController>().Guns[0].GetComponent<AutomaticGun>().coolDown -= Automatic_gun_speed;
            }
            Player.GetComponent<PlayerController>().Money -= GUN_COST;
            Upgrade_count += 1;
            Auto_gun_count += 1;
        }
    }
    public void HP_upgrade()
    {
        if (Player.GetComponent<PlayerController>().Money >= HP_COST)
        {
            Player.GetComponent<PlayerController>().Money -= HP_COST;
            Player.GetComponent<HealthSystem>().MaxHP += HP_UP;
            Player.GetComponent<HealthSystem>().HP += HP_UP;
            GameObject.Find("HP").GetComponent<Slider>().maxValue += HP_UP;
            Upgrade_count += 1;
            HP_count += 1;
        }
    }
    public void Shield_upgrade()
    {

        if (Player.GetComponent<PlayerController>().Money >= SHIELD_COST)
        {
            Player.GetComponent<PlayerController>().Money -= SHIELD_COST;
            Player.GetComponent<HealthSystem>().MaxArmor += Shield_UP;
            Player.GetComponent<HealthSystem>().Armor += Shield_UP;
            Player.GetComponent<HealthSystem>().RegenHight += Shield_UP / 10;
            GameObject.Find("Shield").GetComponent<Slider>().maxValue += Shield_UP;
            Upgrade_count += 1;
            Shield_count += 1;
        }
    }
    public void Defensive_upgrade()
    {

        if (Player.GetComponent<PlayerController>().Money >= PLANET_COST)
        {
            Upgrade_count += 1;
            Planet_count += 1;
            Player.GetComponent<PlayerController>().Money -= PLANET_COST;
            Planet.GetComponent<HealthSystem>().MaxArmor += Planet_SH;
            Planet.GetComponent<HealthSystem>().Armor += Planet_SH;
            Planet.GetComponent<HealthSystem>().MaxHP+= Planet_HP;
            Planet.GetComponent<HealthSystem>().HP += Planet_HP;
            Planet.GetComponent<HealthSystem>().RegenHight += Planet_reg;
            GameObject.Find("Planet_shield").GetComponent<Slider>().maxValue += Planet_SH;
            GameObject.Find("Planet_HP").GetComponent<Slider>().maxValue += Planet_HP;
            if (GameObject.FindGameObjectWithTag("Station")!= null)
            {
                stations = GameObject.FindGameObjectsWithTag("Station");
                for (int i = 0; i<stations.Length; i++)
                {
                    stations[i].GetComponent<HealthSystem>().MaxHP += Stations_hp;
                    stations[i].GetComponent<HealthSystem>().HP += Stations_hp;
                    stations[i].GetComponent<HealthSystem>().MaxArmor += Stations_Sh;
                    stations[i].GetComponent<HealthSystem>().Armor += Stations_Sh;
                    stations[i].GetComponent<HealthSystem>().RegenHight += Stations_reg;
                }
            }
            
        }
    }
}
