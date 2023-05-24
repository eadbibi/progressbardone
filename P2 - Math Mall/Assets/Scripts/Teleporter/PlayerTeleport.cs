using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTeleport : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //An if statement that says, if a gameObject collides with a collider with the tag "Teleporter" the gameObject will be be teleported to given coordinates
        if (collision.gameObject.tag == "Teleporter")
        {
            transform.position = new Vector3(10, 25, 0);
        }

        if (collision.gameObject.tag == "Teleporter2")
        {
            transform.position = new Vector3(-30, 57, 0);
        }

        if(collision.gameObject.tag == "Teleporter3")
        {
            transform.position = new Vector3(21, 96, 0);
        }

        if (collision.gameObject.tag == "Teleporter4")
        {
            transform.position = new Vector3(-35, 131, 0);
        }
    }

}
