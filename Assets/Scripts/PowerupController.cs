using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupController : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D col)
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        MegaManController megaManController = player.GetComponent<MegaManController>();
        if(col.gameObject.CompareTag("Player"))
        {
            megaManController.health += 3;
            Object.Destroy(this.gameObject);
        }
    }
}
