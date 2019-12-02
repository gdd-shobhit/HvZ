//protected Vector3 Separate(Vector3 targetPosition, float desiredDistance)
//{
//    // Calculate distance to the other object
//    float distanceToTarget = Vector3.Distance(position, targetPosition);
    
//    // if the distance is basically 0, then it's probably me'
//    if (distanceToTarget <= float.Epsilon)
//    {
//        return Vector3.zero;
//    }
    
//    // Flee away from the other object
//    Vector3 fleeForce = Flee(targetPosition);
    
//    // Scale the force based on how close I am
//    fleeForce = fleeForce.normalized * Mathf.Pow(desiredDistance / distanceToTarget, 2);
    
//    // Draw that force
//    Debug.DrawLine(position, position + fleeForce, Color.cyan);
//    return fleeForce;
//}