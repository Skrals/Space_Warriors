using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseStation : MonoBehaviour
{
	public HealthSystem hs;
    public BaseGun GunPrefab;
    public float Range;

    protected BaseGun gun;

    protected void Start()
    {
        gun = Instantiate(GunPrefab);
		gun.author = gameObject;
		hs = gameObject.AddComponent<HealthSystem> ();
		hs.SetParam (1, 30, 5, true);
    }

    private void OnDestroy()
    {
        gun.EndShoot();
        Destroy(gun);
    }



    /// <summary>
    /// Находить ближайшего противника
    /// </summary>
    /// <returns>ближайший противник</returns>
    protected GameObject FindTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        if (enemies.Length > 0)
        {
            GameObject target = enemies[0];
            float minDistance = Vector3.Magnitude(target.transform.position - transform.position);
            for (int i = 0; i < enemies.Length; i++)
            {
                float distance = Vector3.Magnitude(enemies[i].transform.position - transform.position);
                if (distance < minDistance)
                {
                    target = enemies[i];
                    minDistance = distance;
                }
            }

            if (minDistance < Range)
            {
                return target;
            }
            else
            {
                return null;
            }
        }

        return null;
    }
}
