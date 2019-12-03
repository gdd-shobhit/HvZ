using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Human : Vehicle
{
    public GameObject zombie = null;
    public bool isBeingSeeked = false;

    protected override void CalcSteeringForces()
    {
        
        Vector3 ultimateForce = Vector3.zero;
        this.velocity.y = 0;
        this.position.y = 0.5f;
        if (CheckIfZombieIsNear())
        {
            isBeingSeeked = true;
            //ultimateForce += this.Flee(zombie);
            ultimateForce += this.Evade(zombie.GetComponent<Zombie>());
            Vector3.ClampMagnitude(ultimateForce, this.maxSpeed);      
            CheckIfZombieIsNear();
        }
        else
        {           
            isBeingSeeked = false;
        }

        if (isBeingSeeked == false)
        { 
            ultimateForce+=this.Wander();
        }

        for (int i = 0; i < obsList.Count; i++)
        {

            // if Obstacle is in front
            if (Vector3.Dot(this.transform.forward, obsList[i].transform.position - this.position) > 0)
            {
                ultimateForce += ObstacleAvoidance(obsList[i]);
            }
        }
        for(int i=0;i< GameManager.instance.listOfHumans.Count; i++)
        {
            ultimateForce += Separate(GameManager.instance.listOfHumans[i].position,2f);

        }
        ultimateForce += KeepInPark();
        this.ApplyForce(ultimateForce);
    }

    bool CheckIfZombieIsNear()
    {
        for (int i = 0; i < GameManager.instance.listOfZombies.Count; i++)
        {
            if (Vector3.Distance(gameObject.transform.position,
                GameManager.instance.listOfZombies[i].gameObject.transform.position) < 5f){
                this.zombie = GameManager.instance.listOfZombies[i].gameObject;
                return true;
            }          
        }
        return false;
    }


}
