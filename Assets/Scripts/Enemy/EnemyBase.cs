using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBase : MonoBehaviour
{

    public float HP;
    public void Hit(float demage)
    {
        HP -= demage;
        if (HP <= 0)
            Destroy(gameObject);
    }
}
