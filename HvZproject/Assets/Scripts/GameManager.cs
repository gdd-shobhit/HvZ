﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public List<Human> listOfHumans;
    public List<Zombie> listOfZombies;
    public List<Human> inactiveHumans;
    public GameObject humanPrefab;
    public GameObject zombiePrefab;

    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
        {
            instance = this;

        }
        else
        {
            Destroy(instance);
        }
        SpawnHumans();
        SpawnZombies();
        listOfHumans = new List<Human>(FindObjectsOfType<Human>());
        listOfZombies = new List<Zombie>(FindObjectsOfType<Zombie>());
        inactiveHumans = new List<Human>();
       
    }

    // Update is called once per frame
    void Update()
    {
        CheckCollisions();
        RemoveInactiveObjects();
    }

    void SpawnHumans()
    {
        for(int i = 0; i < 10; i++)
        {
            Instantiate(humanPrefab,
                        new Vector3(Random.Range(-25,25),1, Random.Range(-25, 25)),
                        Quaternion.identity);
        }
    }

    void SpawnZombies()
    {
        for (int i = 0; i < 6; i++)
        {
            Instantiate(zombiePrefab,
                        new Vector3(Random.Range(-25, 25), 1, Random.Range(-25, 25)),
                        Quaternion.identity);
        }
    }

    void CheckCollisions()
    {

        for (int i = 0; i < listOfHumans.Count; i++)
        {
            for (int j = 0; j < listOfZombies.Count; j++)
            {
                if (Vector3.Distance(listOfHumans[i].gameObject.transform.position,
                    listOfZombies[j].gameObject.transform.position) < 1f)
                {
                    listOfHumans[i].gameObject.SetActive(false);
                    Debug.Log("Collision");
                }
            }
        }
    }

    void RemoveInactiveObjects()
    {
        for (int i = 0; i < listOfHumans.Count; i++)
        {
            if (!listOfHumans[i].gameObject.activeSelf)
            {
                inactiveHumans.Add(listOfHumans[i]);
                listOfHumans.RemoveAt(i);
                i--;
            }
        }
    }

    public void ProduceHuman()
    {
        Debug.Log("inside ProduceHuman");
        if (inactiveHumans.Count!=0){
            int randomNumber = Random.Range(0, inactiveHumans.Count - 1);
            inactiveHumans[randomNumber].gameObject.SetActive(true);
            listOfHumans.Add(inactiveHumans[randomNumber]);
            inactiveHumans.RemoveAt(randomNumber);
        }
    }
    private void OnGUI()
    {
        GUI.Box(new Rect(0, 0, 250, 30), "Press Spacebar to enable debuglines");
        GUI.Box(new Rect(0, 50, 250, 50), "Human Count "+listOfHumans.Count+ "\nOnly 10 humans at max are allowed");
    }
}
