using System.Collections;
using System.Collections.Generic;
using Naninovel;
using UnityEngine;


public class cameraTarget1 : MonoBehaviour
{

[SerializeField] Camera cam;
[SerializeField] Transform player;
[SerializeField] float threshold;


    void Start()
    {
        Debug.Log("SETDRYTFU");
    }
    
    void Update()
    {
        Camera cam = GameObject.Find("Main Camera(Clone)").GetComponent<Camera>();
        Vector3 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        Vector3 targetPos = (player.position + mousePos) / 2f;

        targetPos.x = Mathf.Clamp(targetPos.x, -threshold + player.position.x, threshold + player.position.x);
        targetPos.y = Mathf.Clamp(targetPos.y, -threshold + player.position.y, threshold + player.position.y);
        targetPos.z = 0;

        this.transform.position = targetPos;
    }
}
