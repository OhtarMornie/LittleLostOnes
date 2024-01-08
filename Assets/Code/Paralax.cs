using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paralax : MonoBehaviour
{


    // Start is called before the first frame update
    void Start()
    {
        lastpos = cam.position;
    }

    public Transform cam;
    public float speedCoefficient;
    Vector3 lastpos;

    void Update()
    {
        transform.position -= ((lastpos - cam.position) * speedCoefficient);
        lastpos = cam.position;
    }
}