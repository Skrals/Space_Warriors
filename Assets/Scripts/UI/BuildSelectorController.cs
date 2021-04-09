using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildSelectorController : MonoBehaviour
{
    public bool Showed;

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

    }

    void Show()
    {
        Showed = true;
        GameObject.Find("Upgrade_panel").GetComponent<UpgradePanel>().Showed = false;
    }

    void Hide()
    {
        Showed = false;
    }
}
