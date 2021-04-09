using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinmapController : MonoBehaviour
{

    public float Scale = 1;
    public float Border = 20;
    public GameObject EnemyPrefab;
    public GameObject PlanetPrefab;
    public GameObject PlayerPrefab;


    GameObject playerMarker;
    GameObject planetMarker;

    Dictionary<DefaultEnemy,GameObject> enemyMarkers = new Dictionary<DefaultEnemy, GameObject>();
    Dictionary<FighterScript, GameObject> fighterMarkers = new Dictionary<FighterScript, GameObject>();
    Vector2 size;

    void Update()
    {
        size = gameObject.GetComponent<RectTransform>().sizeDelta;
        DrawEnemy();
        DrawFighter();
        DrawPlanet();
        DrawPlayer();

    }

    private void DrawPlanet()
    {
        GameObject player = GameObject.Find("Planet");
        Vector3 pos = player.transform.position * Scale;
        if (InMap(pos))
        {
            if (planetMarker == null)
                planetMarker = Instantiate(PlanetPrefab, gameObject.transform);
            planetMarker.transform.localPosition = pos;
        }
    }

    private void DrawPlayer()
    {
        GameObject player = GameObject.Find("Player");
        Vector3 pos = player.transform.position * Scale;
        if (InMap(pos))
        {
            if (playerMarker == null)
                playerMarker = Instantiate(PlayerPrefab, gameObject.transform);
            playerMarker.transform.localPosition = pos;
        }
    }

    private void DrawEnemy()
    {
        DefaultEnemy[] enemyes = GameObject.FindObjectsOfType<DefaultEnemy>();

        foreach (DefaultEnemy enemy in enemyes)
        {
            if (!enemy.isDead)
            {
                Vector3 pos = enemy.transform.position * Scale;
                if (InMap(pos))
                {
                    GameObject enemyMarker;
                    if (!enemyMarkers.ContainsKey(enemy))
                    {
                        enemyMarker = Instantiate(EnemyPrefab, gameObject.transform);
                        enemyMarkers.Add(enemy, enemyMarker);
                    }
                    else
                    {
                        enemyMarker = enemyMarkers[enemy];
                    }
                    enemyMarker.transform.localPosition = pos;
                }
                else
                {
                    if (enemyMarkers.ContainsKey(enemy))
                    {
                        Destroy(enemyMarkers[enemy]);
                        enemyMarkers.Remove(enemy);
                    }
                }
            }
        }
    }

    private void DrawFighter()
    {
        FighterScript[] fighters = GameObject.FindObjectsOfType<FighterScript>();

        foreach (FighterScript fighter in fighters)
        {
            if (!fighter.isDead)
            {
                Vector3 pos = fighter.transform.position * Scale;
                if (InMap(pos))
                {
                    GameObject enemyMarker;
                    if (!fighterMarkers.ContainsKey(fighter))
                    {
                        enemyMarker = Instantiate(EnemyPrefab, gameObject.transform);
                        fighterMarkers.Add(fighter, enemyMarker);
                    }
                    else
                    {
                        enemyMarker = fighterMarkers[fighter];
                    }
                    enemyMarker.transform.localPosition = pos;
                }
                else
                {
                    if (fighterMarkers.ContainsKey(fighter))
                    {
                        Destroy(fighterMarkers[fighter]);
                        fighterMarkers.Remove(fighter);
                    }
                }
            }
        }
    }


    private bool InMap(Vector3 pos)
    {
        return (Mathf.Abs(pos.x) < (size.x / 2) - Border) &&
               (Mathf.Abs(pos.y) < (size.y / 2) - Border);
    }

    private void DeleteMarker(DefaultEnemy enemy)
    {
        if (enemyMarkers.ContainsKey(enemy))
        {
            Destroy(enemyMarkers[enemy]);
            enemyMarkers.Remove(enemy);
        }
    }

    private void DeleteMarker(FighterScript fighter)
    {
        if (fighterMarkers.ContainsKey(fighter))
        {
            Destroy(fighterMarkers[fighter]);
            fighterMarkers.Remove(fighter);
        }
    }
}
