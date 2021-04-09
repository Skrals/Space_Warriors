using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyDisplay : MonoBehaviour
{
    public PlayerController player;
    public UnityEngine.UI.Text text;

    void Update()
    {
        text.text = "Очки: " + player.Money.ToString();
    }
}
