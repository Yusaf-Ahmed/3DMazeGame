using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpController : MonoBehaviour
{
    //public shovelscript SC; (will be added)
    public Rigidbody rb;
    public BoxCollider coll;
    public Transform player, ShovelContainer, Cam;

    public float pickUpRange;
    public float dropForwardForce;
    public float dropUpwardForce;

    public bool equipped;
    public static bool slotFull;

    private void Start()
    {
        //SetUpStart
        if(!equipped)
        {
            //ShovelScript.enabled = false;
            rb.isKinematic = false;
            coll.isTrigger = false;
        }

        else
        {
            rb.isKinematic = true;
            coll.isTrigger = true;
            slotFull = true;
        }
    }

    private void FixedUpdate()
    {
        //Checks if player is in range and when E is pressed
        Vector3 distanceToPlayer = player.position - transform.position;
        if (!equipped && distanceToPlayer.magnitude <= pickUpRange && Input.GetKeyDown(KeyCode.E) && !slotFull)
        {
            PickUpItem();
        }

        //Checks if player has item and when Q is pressed to drop it. 
        if (equipped && Input.GetKeyDown(KeyCode.Q))
        {
            DropItem();
        }
    }

    private void PickUpItem()
    {
        equipped = true;
        slotFull = true;

        //Make item a child of the container and move it to a default position
        transform.SetParent(ShovelContainer);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.Euler(Vector3.zero);
        transform.localScale = Vector3.one;
        //Make Rigidbody kinematic and the box collider false
        rb.isKinematic = true;
        coll.isTrigger = true;

        //Enable Shovel script here
        //shovelscript.enabled = true;
    }

    private void DropItem()
    {
        equipped = false;
        slotFull = false;
        //UnParent Item so it doesnt stay with player
        transform.SetParent(null);

        //Make Rigidbody kinematic and the box collider false
        rb.isKinematic = false;
        coll.isTrigger = false;

        //Make Gun carry same velocity as player
        rb.velocity = player.GetComponent<Rigidbody>().velocity;

        //AddForce when thrown (Forward)
        rb.AddForce(Cam.forward * dropForwardForce, ForceMode.Impulse);
        
        //AddForce when thrown (Upward)
        rb.AddForce(Cam.up* dropUpwardForce, ForceMode.Impulse); 

        float random = Random.Range(-1f, 1f);

        //Disable Shovel script 
        //shovelscript.enabled = false;
    }
}
