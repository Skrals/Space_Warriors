using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour{
	
	public float MaxHP = 250;
	public float HP = 250;
	public float MaxArmor = 250;
	public float Armor = 250;
	public bool Regeneratable = true; //Имеет ли способность к регенерации
	public float RegenHight = 1; //как сильно регенерирует
	private float PoisonTime = 0; //время действия яда
	private float PoisonDamage = 0; //урон яда
	private float RegenCoolDown = 2; //время после которого будет возможна регенерация
	private bool Regenerating = true; //буль для регенерации
	private float immortalTime = 0.3f; // время действия бессмертия
	private bool isImmortal = false; // буль для бессмертия после урона


	public void SomeVoid()
	{
		Immortal ();//Это таймер бессмертия
		RegenCooling();//это таймер на регенерацию после удара
		Regenerate ();//это регенерация
		PoisonHit ();//это урон ядом
	}

	public void SetParam(float Health, float Armor, float RegenHight, bool regenerating)
	{
		MaxHP = HP = Health;
		this.Armor = this.MaxArmor = Armor;
		this.RegenHight = RegenHight;
		Regeneratable = regenerating;
	}
		

	public void SetDefault()
	{
		HP = MaxHP;
		Armor = MaxArmor;
		Regenerating = true;
		PoisonTime = 0;
		PoisonDamage = 0;
		RegenCoolDown = 0;
	}

	public void PoisonHit(float Demage, float time)
	{
		PoisonDamage = Demage;
		PoisonTime = time;
	}

	private void PoisonHit()
	{
		if (PoisonTime >= 0)
		{
			Regenerating = false;
			RegenCoolDown = 5;
			Regeneratable = false;
			float Demage = Time.deltaTime * PoisonDamage;
			if (Armor >= Demage)
			{
				Armor -= Demage;
			}
			else
			{
				HP += Armor - Demage;
				Armor = 0;
			}
			PoisonTime -= Time.deltaTime;
		}
		else
		{
			Regeneratable = true;
		}
	}

	public void Hit(float Demage)
	{
		Regenerating = false;
		RegenCoolDown = 5;
		if (!isImmortal)
		{
			isImmortal = true;
			immortalTime = 0.3f;
			if (Armor >= Demage) {
				Armor -= Demage;
			} else {
				HP += Armor - Demage;
				Armor = 0;
			}
		}
	}

	/// <summary>
	/// Отсчёт времени бессмертия
	/// </summary>
	private void Immortal()
	{
		if (isImmortal)
		{
			immortalTime -= Time.deltaTime;
			if (immortalTime <= 0) {
				isImmortal = false;
			}
		}
	}

	private void RegenCooling()
	{
		if (!Regenerating&&Regeneratable) {
			RegenCoolDown -= Time.deltaTime;
			if (RegenCoolDown <= 0) {
				Regenerating = true;
			}
		}
	}

	private void Regenerate()
	{
		if (Regenerating && Armor != MaxArmor && Regeneratable)
		{
			if (Armor + RegenHight * Time.deltaTime < MaxArmor)
			{
				Armor += RegenHight * Time.deltaTime;
			}
			else
			{
				Armor = MaxArmor;
			}
		}
	}
}
