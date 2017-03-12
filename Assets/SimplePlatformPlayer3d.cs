using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class SimplePlatformPlayer3d : MonoBehaviour
{



    public Platform_CollisionHandler   colHandler;
    public Platform_Input_Controller inputController;

 
    public Transform myTrans;

    public Vector2 startPos;

    public PlayerClasses playerClass;

    public CameraTracking cam;

    public Rigidbody rb;

    //player variables for movement
    public bool isController;

    public float jumpHeight;

    public bool isJumping, canDblJump, dblJumped;
    public bool doubleJump = false;
    public bool isDoubleJumping = false;
    public float doubleJumpedAt;

    public float jumpForce = 700f;

    //raycast variables
    public float upCheck, horCheck, downCheck;

    public bool canMove, matchStarted,allowZmovement;
    public enum directionMoving { not, left, right, up, down };
    public directionMoving dir;

    public int curLanePos,curLaneLimit;

    public enum PlayerClasses
    {
        Fighter,
        Mage,
        Healer,
        Archer,
    }



    //Player Character stats
    public float  hp, mhp, mp, mmp, lvl, baseHP, baseMp, mvspd;

    //player Game Stats
    public float myID;

    public Vector3 initPos;

    public bool  drawn;


    public float xDis, yDis;

    //movement
    public float fwdSpd = 0, sidSpd = 0;


    public float jetPack_fuel,jetPack_rate, jetPack_force;
    public bool jetPack;



    public float screen_half = Screen.width / 2;





    public bool isAndroid, airWalk;


    public void Awake()
    {

        colHandler = this.gameObject.GetComponent<Platform_CollisionHandler>();
        inputController = this.gameObject.GetComponent<Platform_Input_Controller>();


        this.rb = this.GetComponent<Rigidbody>();
        this.myTrans = gameObject.transform;
        this.hp =  this.lvl * this.baseHP;
        this.mp =  this.lvl * this.baseMp;
        this.mhp = this.hp;
        this.mmp = this.mp;

        this.initPos = gameObject.transform.position;



        if (this.cam == null)
            this.cam = Camera.main.GetComponent<CameraTracking>();


    }

    public void Update()
    {



        if (hp <= 0)
            {
                Debug.Log("you died;");
                this.gameObject.SetActive(false);
            }

        Wall_GravityCheck();
        HorColCheck();
        if (isController == true)
            {
                if (canMove != false)
                {

                SpeedUpates();
                }

              
            }
    }

    void FixedUpdate()
    {
        if (colHandler.onWall == false)
        {
            if (jetPack)
            {
                rb.AddForce(Physics.gravity * rb.mass);
                rb.useGravity = false;
            }


            this.gameObject.transform.Translate(sidSpd * Time.fixedDeltaTime, 0, fwdSpd * Time.fixedDeltaTime);
        }


    }



    public void SpeedUpates()
    {
        if (dir == directionMoving.left)
        {
            sidSpd = -mvspd;
        }
        else if (dir == directionMoving.right)
        {
            sidSpd = mvspd;

        }
        else if (dir == directionMoving.up)
        {
            fwdSpd = mvspd;

        }
        else if (dir == directionMoving.down)
        {
            fwdSpd = -mvspd;

        }
    }



    public void Wall_GravityCheck()
    {
        //Wall jump functions
        //if player has been holding wall for x time make em fall
        if (colHandler.onWall == true)
        {
            if (Time.time >= colHandler.wallTimeStart + colHandler.wallTimeLimit)
            {
                Debug.Log("free fall");
                colHandler.onWall = false;
                rb.isKinematic = false;
                colHandler.lastWallExitTIme = Time.time;
                rb.useGravity = true;
            }
        }
        else//delete wall after x amount of seconds so he can regrab
        {
            if (Time.time >= colHandler.lastWallExitTIme + colHandler.wallAllowedStoreTime  && colHandler.lastWall != null)
            {
                Debug.Log("deleted wall");
                colHandler.lastWall = null;
                rb.useGravity = true;
                rb.isKinematic = false;
            }
        }
        if (rb.useGravity == false && rb.isKinematic == false && colHandler.onWall == false)
        {
            if (Time.time >= colHandler.LastCannonAt + colHandler.noGravityTimeLimit)
            {

                rb.useGravity = true;
            }
        }
    }



    private void HorColCheck()
    {
        RaycastHit hitL, hitR;
        bool isLeft = Physics.Raycast(gameObject.transform.position, -gameObject.transform.right, out hitL, horCheck);
        bool isRight = Physics.Raycast(gameObject.transform.position, gameObject.transform.right, out hitR, horCheck);
        Debug.DrawRay(gameObject.transform.position, -gameObject.transform.right, Color.green);
        Debug.DrawRay(gameObject.transform.position, gameObject.transform.right, Color.green);

        if (isRight == true)
        {
            Debug.Log("There is right next to me!");
            if (hitR.transform.gameObject.tag != "Player" || hitR.transform.gameObject.tag != "enemy" || hitR.transform.gameObject.tag != "border")
                isRight = false;
            else
            {
                if (sidSpd > 0)
                    sidSpd = 0;
            }

        }
        else if (isLeft == true)
        {
            // Debug.Log("There is left next to me!");
            if (hitL.transform.gameObject.tag != "Player" || hitL.transform.gameObject.tag != "enemy" || hitL.transform.gameObject.tag != "border" )
                isLeft = false;
            else
            {
                if (sidSpd < 0)
                    sidSpd = 0;
            }
        }
    }


    private void VerColCheck()
    {

        RaycastHit hitL, hitR;
        bool isLeft = Physics.Raycast(gameObject.transform.position, -gameObject.transform.right, out hitL, horCheck);
        bool isRight = Physics.Raycast(gameObject.transform.position, gameObject.transform.right, out hitR, horCheck);


        if (isRight == true)
        {
            Debug.Log("There is something above to me!");
            if (hitR.transform.gameObject.tag != "Player" || hitR.transform.gameObject.tag != "enemy" || hitR.transform.gameObject.tag != "border" )
                isRight = false;
            else
            {
                if (sidSpd > 0)
                    sidSpd = 0;
            }



        }
        else if (isLeft == true)
        {
            Debug.Log("There is something under to me!");
            if (hitL.transform.gameObject.tag != "Player" || hitL.transform.gameObject.tag != "enemy" || hitL.transform.gameObject.tag != "border")
                isLeft = false;
            else
            {
                if (sidSpd < 0)
                    sidSpd = 0;
            }


        }


    }






    public void Jump()
    {



        if (!doubleJump && !colHandler.isGrounded && jetPack == false && colHandler.onWall == false)//double jump
        {
            if (canDblJump == true)
            {
                doubleJump = true;
                rb.velocity = new Vector2(rb.velocity.x, 0);
                rb.AddForce(new Vector2(0f, jumpForce), ForceMode.Impulse);

                isDoubleJumping = true;
                doubleJumpedAt = Time.time;
            }
        }
        else if (colHandler.isGrounded && !doubleJump && jetPack == false && colHandler.onWall == false)//normal jump
        {
            rb.velocity = new Vector2(rb.velocity.x, 0);
            rb.AddForce(new Vector2(0f, jumpForce),ForceMode.Impulse);

            isJumping = true;
        }
        else if (colHandler.onWall == true)
        {//if my player is grounded do whats in the barckets
            rb.isKinematic = false;

            if (sidSpd < 0)
                sidSpd = -mvspd;
            else if (sidSpd > 0)
                sidSpd = mvspd;

               
                rb.isKinematic = false;
            

            colHandler.onWall = false;
            colHandler.lastWallExitTIme = Time.time;
            rb.AddForce(new Vector3(sidSpd, jumpForce, 0), ForceMode.Impulse);//Add a force to my players RigidBody.

        }



    }


    public void JetPack()
    {
        rb.velocity = new Vector2(rb.velocity.x, 0);
        rb.AddForce(new Vector2(0f, jetPack_force));
        jetPack_fuel -= jetPack_rate;
        if (jetPack_fuel <= 0)
        {
            rb.useGravity = true;
            jetPack = false;
            jetPack_fuel = 100;
        }
    }
}


