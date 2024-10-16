using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public float rotateSpeed = 6;
    Quaternion circle1 = Quaternion.Euler(0, 0, 360);
    Quaternion circle2 = Quaternion.Euler(0, 0, -360);
    void Start()
    {
        
    }
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            gameObject.transform.rotation = Quaternion.RotateTowards(gameObject.transform.rotation, circle2, rotateSpeed * Time.deltaTime);
        }else if (Input.GetMouseButton(1))
        {
            gameObject.transform.rotation = Quaternion.RotateTowards(gameObject.transform.rotation, circle1, rotateSpeed * Time.deltaTime);
        }
    }
}
