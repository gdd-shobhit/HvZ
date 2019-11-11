﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Human : Vehicle
{
    public GameObject zombie = null;
    public bool isBeingSeeked = false;

    protected override void CalcSteeringForces()
    {
        Vector3 ultimateForce = Vector3.zero;
        if (zombie != null)
        {

            isBeingSeeked = true;
            ultimateForce += this.Flee(zombie);
            Vector3.ClampMagnitude(ultimateForce, this.maxSpeed);
            this.ApplyForce(ultimateForce);
        }
        else
        {
            isBeingSeeked = false;
        }

        if (isBeingSeeked == false)
        {
            this.velocity = Vector3.zero;
        }
    }

    void CheckIfZombieIsNear()
    {
        for (int i = 0; i < GameManager.instance.listOfZombies.Count; i++)
        {
            if (Vector3.Distance(gameObject.transform.position,
                GameManager.instance.listOfZombies[i].gameObject.transform.position) < 5f){
                this.zombie = GameManager.instance.listOfZombies[i].gameObject;
            }
        }
    }
}