using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform_CollisionHandler : MonoBehaviour
{

    public SimplePlatformPlayer3d player;
    public bool isGrounded, cannonBackward;

    public GameObject trigger;
    //wall jump variables
    public float wallTimeStart, wallTimeLimit, lastWallExitTIme, wallAllowedStoreTime;

    //cannon variables
    public float timeBetweenCanon, LastCannonAt, noGravityTimeLimit;
    public List<GameObject> lastCannon = new List<GameObject>(2);

    public bool onWall, jumpThrough, isLastCannon;
    public GameObject lastWall;

    public bool inCannon;
    public GameObject curCannon;

    public Rigidbody rb;

    // Use this for initialization
    void Awake()
    {
        player = this.gameObject.GetComponent<SimplePlatformPlayer3d>();//get default character controller
        rb = this.GetComponent<Rigidbody>();
    }



    public void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "ground")
        {

            isGrounded = true;

            player.isJumping = false;
            player.isDoubleJumping = false;
            player.doubleJump = false;
        }

        else if (col.gameObject.tag == "obstacle")
        {

            Destroy(this.gameObject);
        }
        else if (col.collider.tag == "wall" && isGrounded == false && onWall == false)//if the object you collided withs tag is ground your player is on the floor
        {

            if (lastWall != col.gameObject)
            {
                player.rb.isKinematic = true;
                player.rb.useGravity = false;
                wallTimeStart = Time.time;
                onWall = true;
                lastWall = col.gameObject;
                player.isDoubleJumping = false;
                player.doubleJump = false;

            }
        }
    }

    public void OnCollisionStay(Collision col)
    {
        if (col.collider.tag == "wall" && isGrounded == false && onWall == false)//if the object you collided withs tag is ground your player is on the floor
        {
            Debug.Log("Eh colsaty;");
            if (lastWall != col.gameObject)
            {
                player.rb.isKinematic = true;
                wallTimeStart = Time.time;
                onWall = true;
                lastWall = col.gameObject;
                player.rb.useGravity = false;
                player.isDoubleJumping = false;
                player.doubleJump = false;
            }
        }
    }

    public void OnCollisionExit(Collision col)
    {
        if (col.gameObject.tag == "ground")
        {
            isGrounded = false;

            player.isDoubleJumping = false;
        }
        else if (col.collider.tag == "wall" && isGrounded == false)//if the object you collided withs tag is ground your player is on the floor
        {

            player.rb.useGravity = true;
        }
    }





    public void OnTriggerEnter(Collider col)
    {

        if (col.gameObject.tag == "SpeedUp")
        {
            //access the buffZone
            Debug.Log("Speed up");
            player.rb.AddForce(new Vector3(player.sidSpd * 3f, 0, 0), ForceMode.Impulse);

        }
        if (col.gameObject.tag == "buffZone")
        {
            //access the buffZone
            if (col.GetComponent<buffZone>().type == buffZone.Type.cannon)
            {
                if (!isLastCannon)
                    col.GetComponent<buffZone>().RunBuffPragma(col.GetComponent<buffZone>().type, false, this.gameObject);
                else if (isLastCannon && Time.time >= LastCannonAt + timeBetweenCanon)
                    col.GetComponent<buffZone>().RunBuffPragma(col.GetComponent<buffZone>().type, false, this.gameObject);

            }
        }


    }


    public void OnTriggerStay(Collider col)
    {

        if (col.gameObject.tag == "buffZone")
        {
            //access the buffZone

            if (col.GetComponent<buffZone>().type == buffZone.Type.cannon)
            {
                //Debug.Log("another yo0");

                //Debug.Log("another yo0");
                if (rb.isKinematic == false)
                {
                    if (!isLastCannon)
                        col.GetComponent<buffZone>().RunBuffPragma(col.GetComponent<buffZone>().type, false, this.gameObject);
                    else if (isLastCannon && Time.time >= LastCannonAt + timeBetweenCanon)
                        col.GetComponent<buffZone>().RunBuffPragma(col.GetComponent<buffZone>().type, false, this.gameObject);
                }
                else
                {
                    inCannon = true;
                    curCannon = col.gameObject;

                }

            }

        }
    }

    public void OnTriggerExit(Collider col)
    {

    }

}
