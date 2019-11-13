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
    }

    private void Update()
    {
        CalcSteeringForces();
        // Then, calculate the physics
        UpdatePhysics();
        // Make sure the vehicle stays on screen (remove this for the exercise)
        Bounce();
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
        transform.rotation = Quaternion.Euler(0, 0, angle);

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

    protected abstract void CalcSteeringForces();

  



#if UNITY_EDITOR
    private void OnValidate()
    {
        // Make sure that mass isn't set to 0
        mass = Mathf.Max(mass, 0.0001f);
    }
    #endif 
}
