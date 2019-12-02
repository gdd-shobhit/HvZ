using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public abstract class Vehicle : MonoBehaviour
{
    // Vectors for the physics
    public Vector3 position;
    public Vector3 direction;
    public Vector3 velocity;
    public Vector3 acceleration;
    public Material material1;
    public Material material2;
    public Material material3;
    public List<Obstacle> obsList;
    public GameObject ground;
    public float radius;
    public float safeDistance;
    private float time;
    Vector3 positionToWander;

    // The mass of the object. Note that this can't be zero
    public float mass = 1;

    public float maxSpeed = 4;

    public const float MIN_VELOCITY = 0.1f;



    private void Start()
    {
        // Initialize all the vectors
        position = transform.position;
        direction = Vector3.right;
        velocity = Vector3.zero;
        acceleration = Vector3.zero;
        obsList = new List<Obstacle>(FindObjectsOfType<Obstacle>());
        radius = gameObject.transform.localScale.x / 2;
        safeDistance = 2f;
        time = Mathf.Infinity;
        positionToWander = Vector3.zero;
    }

    private void Update()
    {
        CalcSteeringForces();
        // Then, calculate the physics
        UpdatePhysics();
        // Make sure the vehicle stays on screen (remove this for the exercise)
        //Bounce();
        // Finally, update the position
        UpdatePosition();
    }

    /// <summary>
    /// Updates the physics properties of the vehicle
    /// </summary>
    private void UpdatePhysics()
    {
        // Add acceleration to velocity, and have that be scaled with time
        velocity += acceleration * Time.deltaTime;
        velocity.y = 0f;

        // Change the position based on velocity over time
        position += velocity * Time.deltaTime;

        // Calculate the direction vector
        direction = velocity.normalized;

        // Reset the acceleration for the next frame
        acceleration = Vector3.zero;
    }

    /// <summary>
    /// Wraps the vehicle around the screen
    /// </summary>
    private void Bounce()
    {
        Camera cam = Camera.main;
        Vector3 max = cam.ScreenToWorldPoint(new Vector3(cam.pixelWidth, cam.pixelHeight, cam.nearClipPlane));
        Vector3 min = cam.ScreenToWorldPoint(new Vector3(0, 0, cam.nearClipPlane));

        if (position.x > 25f && velocity.x > 0)
        {
            velocity.x *= -1;
        }
        if (position.z > 25f && velocity.z > 0)
        {
            velocity.z *= -1;
        }
        if (position.x < -25f && velocity.x < 0)
        {
            velocity.x *= -1;
        }
        if (position.z < -25f && velocity.z < 0)
        {
            velocity.z *= -1;
        }
    }

    /// <summary>
    /// Wraps the vehicle around the screen
    /// </summary>
    private void Wrap()
    {
        Camera cam = Camera.main;
        Vector3 max = cam.ScreenToWorldPoint(new Vector3(cam.pixelWidth, cam.pixelHeight, cam.nearClipPlane));
        Vector3 min = cam.ScreenToWorldPoint(new Vector3(0, 0, cam.nearClipPlane));

        if (position.x > max.x && velocity.x > 0)
        {
            position.x = min.x;
        }
        if (position.y > max.y && velocity.y > 0)
        {
            position.y = min.y;
        }
        if (position.x < min.x && velocity.x < 0)
        {
            position.x = max.x;
        }
        if (position.y < min.y && velocity.y < 0)
        {
            position.y = max.y;
        }
    }

    /// <summary>
    /// Update the vehicle's position
    /// </summary>
    private void UpdatePosition()
    {
        // Atan2 determines angle of velocity against the right vector
        float angle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg;
        //transform.rotation = Quaternion.Euler(0, 0, angle);
        // Sets the rotation of the vehicle to face towards it's velocity
        transform.rotation = Quaternion.LookRotation(velocity.normalized, Vector3.up);
        // Update position
        gameObject.transform.position = position;
    }

    /// <summary>
    /// Applies friction to the vehicle
    /// </summary>
    /// <param name="coeff">The coefficient of friction</param>
    protected void ApplyFriction(float coeff)
    {
        // If the velocity is below a minimum value, just stop the vehicle
        if (velocity.magnitude < MIN_VELOCITY)
        {
            velocity = Vector3.zero;
            return;
        }

        Vector3 friction = velocity * -1;
        friction.Normalize();
        friction = friction * coeff;
        acceleration += friction;
    }

    /// <summary>
    /// Applies a force to the vehicle
    /// </summary>
    /// <param name="force">The force to be applied</param>
    public void ApplyForce(Vector3 force)
    {
        // Make sure the mass isn't zero, otherwise we'll have a divide by zero error
        if (mass == 0)
        {
            Debug.LogError("Mass cannot be zero!");
            return;
        }

        // Add our force to the acceleration for this frame
        acceleration += force / mass;
    }

    private Vector3 Seek(Vector3 targetPosition)
    {
        Vector3 desiredVelocity = targetPosition - position;
        desiredVelocity.Normalize();
        desiredVelocity *= maxSpeed;
        Vector3 steeringForce = desiredVelocity - velocity;
        //Debug.DrawLine(position, position + steeringForce, Color.red);
        return steeringForce;
    }

    public Vector3 Seek(GameObject targetObj)
    {
        return Seek(targetObj.transform.position);
    }

    public Vector3 Pursuit(Human human)
    {
        Vector3 futurePosition = human.gameObject.transform.position + human.velocity*Time.deltaTime*40;
        Vector3 steeringForce = Seek(human.GetFuturePosition(2f));
        Debug.DrawLine(this.position, human.GetFuturePosition(2f), Color.red);
        //Debug.DrawLine(position, position + steeringForce, Color.red);
        return steeringForce;
    }

    private Vector3 Flee(Vector3 targetPosition)
    {
        Vector3 desiredVelocity = position - targetPosition;
        desiredVelocity.Normalize();
        desiredVelocity *= maxSpeed;
        Vector3 steeringForce = desiredVelocity - velocity;
        //Debug.DrawLine(position, position + steeringForce);
        return steeringForce;
    }

    public Vector3 Flee(GameObject targetObject)
    {
        return Flee(targetObject.transform.position);
    }

    public Vector3 Evade(Zombie zombie)
    {
        Vector3 futurePosition = zombie.gameObject.transform.position + zombie.velocity *1f;
        Vector3 steeringForce = Flee(zombie.GetFuturePosition(1));
        Debug.DrawLine(this.position, zombie.GetFuturePosition(1), Color.green);
        //Debug.DrawLine(position, position + steeringForce, Color.red);
        return steeringForce;
    }

    protected abstract void CalcSteeringForces();


    public Vector3 ObstacleAvoidance(Obstacle obs)
    {
        Vector3 desiredVelocity = Vector3.zero;
        Vector3 vectorToObstacle = obs.transform.position - this.position;      
        float weight = 0;
        float rightVectorDot = Vector3.Dot(this.transform.right, vectorToObstacle);
        float distance = vectorToObstacle.magnitude - obs.radius;
        //Debug.Log(rightVectorDot * 0.01);
        // checking if the obstacle is in certain distance
        if (Math.Abs(rightVectorDot) < radius+ obs.radius)
        {
            Debug.Log("inside");
            // calc weight
            if (distance <= 0)
            {
                weight = float.MaxValue;
            }
            else
            {
                weight = (float)Math.Pow(safeDistance/distance, 2f);
            }
            // clamping weight
            weight = Mathf.Min(weight, 100);
            //Debug.Log(safeDistance);
            //Debug.Log(distance);
            if (distance < safeDistance)
            {
                Debug.Log("distance < safeDistance");
                // If the obstacle is in left
                if (rightVectorDot < 0)
                {
                    Debug.Log("left");
                    // if left then is it colliding?
                    if (Math.Abs(rightVectorDot) < radius + obs.radius)
                    {
                        desiredVelocity = transform.right*2;
                        desiredVelocity *= maxSpeed;
                    }
                }
                // Checking if obstacle is right
                else
                {
                    Debug.Log("right");
                    // if right then is it colliding?
                    if (Math.Abs(rightVectorDot) < radius + obs.radius)
                    {
                        desiredVelocity = -transform.right*2;
                        desiredVelocity *= maxSpeed;
                    }
                }
            }
          
           
        }

        Vector3 steeringForce =(desiredVelocity-velocity)* weight;
    
        return steeringForce;

    }

    public Vector3 GetFuturePosition(float seconds = 1f)

    {
        return position + velocity * seconds;
    }


    public Vector3 KeepInPark()
    {
        Vector3 desiredVelocity = Vector3.zero;
        if (GetFuturePosition(1.5f).x > 25)
        {
            desiredVelocity += this.transform.right.normalized * (float)(Math.Pow((25 + GetFuturePosition(1.5f).x)/25,6f));
        }
        if(GetFuturePosition(1.5f).x < -25)
        {
            desiredVelocity += this.transform.right.normalized * (float)(Math.Pow((25 - (GetFuturePosition(1.5f).x)) / 25, 6f));
        }
        if (GetFuturePosition(1.5f).z > 25)
        {
            desiredVelocity += this.transform.right.normalized * (float)(Math.Pow((25+(GetFuturePosition(1.5f).z)) / 25, 6f));
        }
        if (GetFuturePosition(1.5f).z < -25)
        {
            desiredVelocity += this.transform.right.normalized * (float)(Math.Pow((25-GetFuturePosition(1.5f).z) / 25, 6f));
        }
        if (desiredVelocity == Vector3.zero)
        {
            return Vector3.zero;
        }
        return desiredVelocity - velocity;
    }

    protected Vector3 Separate(Vector3 targetPosition, float desiredDistance)
    {
        // Calculate distance to the other object
        float distanceToTarget = Vector3.Distance(position, targetPosition);

        // if the distance is basically 0, then it's probably me'
        if (distanceToTarget <= float.Epsilon)
        {
            return Vector3.zero;
        }

        // Flee away from the other object
        Vector3 fleeForce = Flee(targetPosition);

        // Scale the force based on how close I am
        fleeForce = fleeForce.normalized * Mathf.Pow(desiredDistance / distanceToTarget, 3);

        // Draw that force
        Debug.DrawLine(position, position + fleeForce, Color.cyan);
        return fleeForce*2;
    }


    protected Vector3 Wander()
    {
        Vector3 wanderForce = Vector3.zero;
        time += Time.deltaTime;
        if (time > 2)
        {
            Vector3 circleCenter = GetFuturePosition(1.5f);
           
            positionToWander = circleCenter + GetRandomPositionOnCircle();
            time = 0f;
        }
        
        Debug.DrawLine(position, positionToWander, Color.green);
        if (positionToWander != Vector3.zero)
        {
            wanderForce = Seek(positionToWander);
            return wanderForce;
        }
        return Vector3.zero;
        
    }

    public Vector3 GetRandomPositionOnCircle()
    {
        // circle would be a 5 radius Circle
        float randomAngle = UnityEngine.Random.Range(0, 2 * Mathf.PI);
        Vector3 positionOfPoint = Vector3.zero;
        positionOfPoint.z = Mathf.Sin(randomAngle) * 5f;
        positionOfPoint.x = Mathf.Cos(randomAngle) * 5f;
        return positionOfPoint;
    }


#if UNITY_EDITOR
    private void OnValidate()
    {
        // Make sure that mass isn't set to 0
        mass = Mathf.Max(mass, 0.0001f);
    }
#endif
}
