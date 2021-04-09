using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Planet : MonoBehaviour
{

    public HealthSystem hs;
    public GameObject Over;
    public Slider HP_S;
    public Slider Shield_S;

    private float end_timer = 7;

    void Start()
    {
        hs = gameObject.AddComponent<HealthSystem>();
        hs.SetParam(5000, 10000, 110, true);
        HP_S.maxValue = hs.MaxHP;
        Shield_S.maxValue = hs.MaxArmor;
    }

    void Update()
    {
        hs.SomeVoid();

        HP_S.value = hs.HP;
        Shield_S.value = hs.Armor;

        if (hs.HP <= 0)
        {
            end_timer -= Time.deltaTime;
            GameObject.Find("Player").GetComponent<PlayerController>().dead = true;
            Over.SetActive(true);
            if (end_timer <= 0)
            {
                Application.LoadLevel(0);
            }
        }
    }
}
