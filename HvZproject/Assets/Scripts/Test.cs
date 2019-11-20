using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    Vehicle vehi;
    // Start is called before the first frame update
    void Start()
    {
        vehi=gameObject.GetComponent<Vehicle>();
    }

    // Update is called once per frame
    void Update()
    {
        vehi.ApplyForce(new Vector3(16, 0, 16));
    }
}
