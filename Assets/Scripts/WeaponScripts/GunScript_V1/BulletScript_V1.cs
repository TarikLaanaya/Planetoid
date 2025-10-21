using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public bool debugMode = false;
    public GameObject hitEffect;
    private void OnCollisionEnter(Collision collision) 
    {
        if(debugMode)
        {
            if(collision.gameObject.CompareTag("Target"))
            {
                print("hit " + collision.gameObject.name + "!");
                Destroy(gameObject);
            }
            if(collision.gameObject.CompareTag("Ground"))
            {
                Destroy(gameObject);
            }
        }
        else
        {
            Instantiate(hitEffect, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
        
    }
}
