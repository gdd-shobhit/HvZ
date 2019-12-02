//// Returns a steering force that avoids an obstacle if necessary
//public Vector3 AvoidObstacle(Vector3 targetPosition, float otherRadius)
//{
//	// Get a vector from my position to the obstacle's position
//    Vector3 meToOther = targetPosition - position;

//    // Project that vector onto my forward vector
//    float fwdMeToOtherDot = Vector3.Dot(transform.forward, meToOther);
    
//    // if that projection is less than 0, we know the obstacle is behind us, and can ignore it
//    if (fwdMeToOtherDot < 0)
//    {
//        return Vector3.zero;
//    }

//	// Project the vector onto my right vector 
//    float rightMeToOtherDot = Vector3.Dot(transform.right, meToOther);

//    // If that projection is greater than the sum of my radius, and the obstacle's radius,
//    // there's no potential collision
//    if (Mathf.Abs(rightMeToOtherDot) > otherRadius + myRadius)
//    {
//        return Vector3.zero;
//    }
 
// 	// Get the distance from me to the closest edge of the obstacle
//    float distance = meToOther.magnitude - otherRadius;

//    // If the obstacle is out of range, it can be ignored
//    if (distance > safeDistance)
//    {
//        return Vector3.zero;
//    }
	
//	// Calculate a weight for avoiding the obstacle, based on how far away it is 
//    float weight = 0;
//    if (distance <= 0)
//    {
//    	// If we're on top of, or inside the obstacle, then we want the maximum possible weight
//        weight = float.MaxValue;
//    }
//    else
//    {
//    	// The weight should be higher the closer we are to the object, and that should scale exponentially
//        weight = Mathf.Pow(safeDistance / distance, 2f);
//    }
 
// 	// If our weight is too high, we want to clamp it so that we don't run into issues with vectors being too large
//    weight = Mathf.Min(weight, 100000);
 
// 	// Calculate the desired velocity
//    Vector3 desiredVelocity = Vector3.zero;
   
//    // Check which side of the vehicle the obstacle is on
//    if (rightMeToOtherDot < 0)
//    {
//    	// If obstacle is on the left, steer right
//        desiredVelocity = transform.right * maxSpeed;
//    }
//    else
//    {
//    	// If obstacle is on the right, steer left
//        desiredVelocity = transform.right * -maxSpeed;
//    }
 
// 	// Calculate our steering force to get away from the obstacle
//    Vector3 steeringForce = (desiredVelocity - velocity) * weight;
    
//    // do any debug drawing here

//    // return the steering force
//    return steeringForce;
//}