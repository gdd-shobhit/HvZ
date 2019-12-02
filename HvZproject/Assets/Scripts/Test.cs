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
        gameObject.transform.position += new Vector3(2, 0, 0)*Time.deltaTime;
    }
}
