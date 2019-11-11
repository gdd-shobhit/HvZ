using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : Vehicle
{
    public Human nearestHuman;

    protected override void CalcSteeringForces()
    {
        nearestHuman=FindNearestHuman(GameManager.instance.listOfHumans);
        this.velocity.y = 0;
        this.position.y = 1.5f;
        this.mass = 3f;
        Vector3 seekingForce = Vector3.zero;

        if (nearestHuman!=null){
            seekingForce += (this.Seek(nearestHuman.gameObject));
            this.ApplyForce(seekingForce);
        }
        
        
       
    }

    private Human FindNearestHuman(List<Human> list)
    {
        float distance = Mathf.Infinity;
        Human pseudoHuman=null;
        for(int i = 0; i < list.Count; i++)
        {  
                if (Vector3.Distance(gameObject.transform.position, list[i].position) < distance)
                {
                    distance = Vector3.Distance(gameObject.transform.position, list[i].position);
                    pseudoHuman = list[i];
                }
            pseudoHuman.zombie = gameObject;
              
        }
        return pseudoHuman;


    }

    private void CheckCollision()
    {
        for(int i = 0; i < GameManager.instance.listOfHumans.Count; i++)
        {
            if(Vector3.Distance(gameObject.transform.position, 
                GameManager.instance.listOfHumans[i].gameObject.transform.position) < 0.5f)
            {
                Debug.Log("Collison");
                GameManager.instance.listOfHumans[i].gameObject.SetActive(false);
            }
        }
    }
}
