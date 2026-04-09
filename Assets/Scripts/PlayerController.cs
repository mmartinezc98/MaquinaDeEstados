using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
   [SerializeField] private float _speed = 10f;

   
    void Update()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector3 movement= new Vector3 (moveX,0, moveZ);
       transform.Translate (movement*_speed*Time.deltaTime);
    }
}
