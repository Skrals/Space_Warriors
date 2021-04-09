using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuilderController : MonoBehaviour
{
    public List<GameObject> Stations;
    public bool BuildEnabled;
    public float Default_cost = 2000;

    private float WaweNum;
    private int NUM;// номер выбранной станции
    private float remove;// возврат средств
    private float count;// общее кол-во станций
    public float Automatic_st_cost;
    public float Laser_st_cost;
    private List<GameObject> buildedStations;
    private GameObject selectedStation;
    private GameObject buildSelector;
    private bool disassembly;

    void Start()
    {
        buildedStations = new List<GameObject>();
        BuildEnabled = false;
        buildSelector = GameObject.Find("Build_selector");

    }

    void Update()
    {
        WaweNum = EnemySpawner.wawecounter;
        //рассчет стоимости//
        Automatic_st_cost = ((Default_cost) + (count * 2000));
        Laser_st_cost = ((Default_cost) + (count * 800));
        remove = (Default_cost);
        /////////////////////
        ///
        InputProcessing();
        Out();

        for (int i = 0; i < buildedStations.Count; i++)
        {
            if (buildedStations[i] == null)
                buildedStations.RemoveAt(i);
        }

        foreach (GameObject station in buildedStations)
        {
            station.GetComponent<SpriteRenderer>().color = Color.white;
        }

        if (disassembly)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            for (int i = 0; i < buildedStations.Count; i++)
            {
                GameObject station = buildedStations[i];
                if (station.GetComponent<BoxCollider2D>().OverlapPoint(mousePos))
                {
                    station.GetComponent<SpriteRenderer>().color = Color.red;
                    if (Input.GetMouseButtonDown(0))
                    {
                        Destroy(station);
                        buildedStations.Remove(station);
                        RemoveMoney();
                    }
                }
                else
                    station.GetComponent<SpriteRenderer>().color = Color.white;
            }
        }

        if (selectedStation != null)
        {

            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            selectedStation.transform.position = new Vector3(mousePos.x, mousePos.y, -1);

            if (CanPlace())
                selectedStation.GetComponent<SpriteRenderer>().color = Color.white;
            else
                selectedStation.GetComponent<SpriteRenderer>().color = Color.red;


            mousePos = Input.mousePosition;
            RectTransform rt = buildSelector.GetComponent<RectTransform>();
            if (Input.GetMouseButtonDown(0))
            {
                if (mousePos.x < rt.anchoredPosition.x ||
                    mousePos.x > rt.anchoredPosition.x + rt.sizeDelta.x ||
                    mousePos.y < rt.anchoredPosition.y ||
                    mousePos.y > rt.anchoredPosition.y + rt.sizeDelta.y)
                    if (CanPlace())
                    {
                        buildedStations.Add(selectedStation);
                        if (NUM == 0)
                        {
                            GameObject.Find("Player").GetComponent<PlayerController>().Money -= Automatic_st_cost;
                            count += 1;
                        }
                        else
                        {
                            if (NUM == 1)
                            {
                                GameObject.Find("Player").GetComponent<PlayerController>().Money -= Laser_st_cost;
                                count += 1;
                            }

                        }
                        selectedStation = null;
                    }

            }
        }




    }

    /// <summary>
    /// Обработка ввода
    /// </summary>
    private void InputProcessing()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {

            if (BuildEnabled)
            {
                DestroySelected();
                buildSelector.SendMessage("Hide");
                BuildEnabled = false;
                disassembly = false;
            }
            else
            {
                buildSelector.SendMessage("Show");
                BuildEnabled = true;
            }
        }
    }

    /// <summary>
    /// Проверка возможности размещения станции
    /// </summary>
    /// <returns>true - если можно разместить</returns>
    bool CanPlace()
    {
        if (selectedStation == null)
            return false;

        ContactFilter2D filter = new ContactFilter2D().NoFilter();
        Collider2D[] coliders = new Collider2D[0];
        if (selectedStation.GetComponent<BoxCollider2D>().OverlapCollider(filter, coliders) == 0)
        {
            return true;
        }
        return false;
    }

    private void DestroySelected()
    {
        if (selectedStation != null)
        {
            Destroy(selectedStation);
        }
    }
    private void RemoveMoney()
    {
        GameObject.Find("Player").GetComponent<PlayerController>().Money += remove;
        count -= 1;
    }

    private void Out()
    {
        GameObject.Find("Gun_station").GetComponent<UnityEngine.UI.Text>().text = "Стоимость " + Automatic_st_cost + "\r\n";
        GameObject.Find("Laser_station").GetComponent<UnityEngine.UI.Text>().text = "Стоимость " + Laser_st_cost + "\r\n";
    }

    private void SelectAutomaticStation()
    {
        if (GameObject.Find("Player").GetComponent<PlayerController>().Money >= Automatic_st_cost)
        {
            SelectStation(0);

        }
    }

    private void SelectLaserStation()
    {
        if (GameObject.Find("Player").GetComponent<PlayerController>().Money >= Laser_st_cost)
        {
            SelectStation(1);

        }
    }

    private void SelectStation(int number)
    {
        disassembly = false;
        DestroySelected();
        selectedStation = Instantiate(Stations[number]);
        NUM = number;

    }

    private void Remove()
    {

        DestroySelected();
        disassembly = true;

    }
}
