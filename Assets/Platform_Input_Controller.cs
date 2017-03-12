using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform_Input_Controller : MonoBehaviour {

    public SimplePlatformPlayer3d player;


    public float minSwipeDistY;
    public float minSwipeDistX;
    public bool vrMode;


    // Use this for initialization
    void Start()
    {
        player = this.gameObject.GetComponent<SimplePlatformPlayer3d>();
        // player.gameStateManager.left.myHero = player;
        // player.gameStateManager.right.myHero = player;

        // player.gameStateManager.shootLeft.myHero = player;
        // player.gameStateManager.shootRight.myHero = player;
    }

    // Update is called once per frame
    void Update()
    {


        if (player.isController == true)
        {

            if (player.colHandler.inCannon == true)//change to canmove
            {

                player.rb.velocity = Vector3.zero;

                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    Debug.Log("fired");
                    player.colHandler.inCannon = false;
                    player.colHandler.curCannon.GetComponent<buffZone>().RunBuffPragma(buffZone.Type.cannon, false, this.gameObject);

                }

            }

            if (player.canMove != false)
            {



                //inputs
                OnDown();
                OnHold();
                OnUp();


                // _touchControls();




            }

        }
    }


    public void OnMouseDown()
    {


    }





    public void UIActions(int action)
    {

        if (action == 2)
        {
            if (player.colHandler.isGrounded == true)
            {
                if (player.sidSpd == player.mvspd * -1)
                {
                    player.sidSpd = 0;
                }
                else
                    player.sidSpd = player.mvspd * -1;
            }
        }
        else if (action == 3)
        {
            if (player.colHandler.isGrounded == true)
            {
                if (player.sidSpd == player.mvspd)
                {

                    player.sidSpd = 0;
                }
                else
                    player.sidSpd = player.mvspd;
            }
        }
    }



    public void OnDown()
    {

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (vrMode)
            {
                if (player.airWalk == true || player.colHandler.isGrounded == true)
                    player.sidSpd = player.mvspd;
            }

        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (vrMode)
            {
                if (player.airWalk == true || player.colHandler.isGrounded == true)
                    player.sidSpd = player.mvspd * -1;
            }

        }
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            if (player.airWalk == true || player.colHandler.isGrounded == true)
                player.sidSpd = player.mvspd * -1;

        }
        else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {

            if (player.airWalk == true || player.colHandler.isGrounded == true)
                player.sidSpd = player.mvspd;


        }


        if (Input.GetKeyDown(KeyCode.Space))
        {

            if (player.jetPack == false)
                player.Jump();

        }
    }







    public void OnHold()
    {

        if (Input.GetKey(KeyCode.Space))
        {
            if (player.jetPack == true)
            {
                player.JetPack();
            }
        }


        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            if (player.airWalk == true || player.colHandler.isGrounded == true)
                player.sidSpd = player.mvspd * -1;

        }
        else if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {

            if (player.airWalk == true || player.colHandler.isGrounded == true)
                player.sidSpd = player.mvspd;


        }

        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
        {

            if (player.allowZmovement == true && vrMode == false)
            {

                if (player.airWalk == true || player.colHandler.isGrounded == true)
                    player.fwdSpd = player.mvspd;

            }
            else
            {
                if (player.jetPack == true)
                {
                    player.JetPack();
                }

                if (vrMode)
                {
                    if (player.airWalk == true || player.colHandler.isGrounded == true)
                        player.sidSpd = player.mvspd;
                }

            }

        }
        else if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
        {

            if (player.allowZmovement == true && vrMode == false)
            {

                if (player.airWalk == true || player.colHandler.isGrounded == true)
                    player.fwdSpd = player.mvspd * -1;
            }
            else if (vrMode)
            {
                if (player.airWalk == true || player.colHandler.isGrounded == true)
                    player.sidSpd = player.mvspd * -1;
            }

        }
    }






    public void OnUp()
    {
        if (Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.A))
        {

            player.sidSpd = 0;
        }
        else if (Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.D))
        {

            player.sidSpd = 0;

        }

        if (Input.GetKeyUp(KeyCode.UpArrow) || Input.GetKeyUp(KeyCode.W))
        {

            if (player.allowZmovement == true && vrMode == false)
            {
                player.fwdSpd = 0;
            }

            if (vrMode == true)
            {
                player.sidSpd = 0;

            }

        }
        else if (Input.GetKeyUp(KeyCode.DownArrow) || Input.GetKeyUp(KeyCode.S))
        {

            if (player.allowZmovement == true && vrMode == false)
            {
                player.fwdSpd = 0;
            }

            if (vrMode == true)
            {
                player.sidSpd = 0;

            }
        }
    }







    public void laneShift(int dir)
    {

        if (dir == 0 && player.curLanePos != 0)
        {
            this.gameObject.transform.position -= new Vector3(1, 0, 0);
            player.curLanePos -= 1;
        }
        else if (dir == 1 && player.curLanePos != player.curLaneLimit)
        {
            this.gameObject.transform.position += new Vector3(1, 0, 0);
            player.curLanePos += 1;

        }
    }




    void _touchControls()
    {

        //#if UNITY_ANDROID
        if (Input.touchCount > 0)
        {

            Touch touch = Input.touches[0];

            switch (touch.phase)

            {

                case TouchPhase.Began:

                    break;

                case TouchPhase.Ended:

                    float swipeDistVertical = (new Vector3(0, touch.position.y, 0) - new Vector3(0, player.startPos.y, 0)).magnitude;
                    float swipeDistHorizontal = (new Vector3(touch.position.x, 0, 0) - new Vector3(player.startPos.x, 0, 0)).magnitude;

                    if (swipeDistVertical > swipeDistHorizontal)

                    {

                        float swipeValue = Mathf.Sign(touch.position.y - player.startPos.y);

                        if (swipeValue < 0)
                        {
                            Debug.Log("DOwn Swype");
                        }
                        else
                        {
                            Debug.Log("Up Swype");


                        }




                    }
                    else if (swipeDistHorizontal > swipeDistVertical)

                    {

                        float swipeValue = Mathf.Sign(touch.position.x - player.startPos.x);

                        if (swipeValue < 0)
                        {
                            Debug.Log("left Swype");


                            if (player.canMove)
                            {

                                laneShift(0);

                            }


                        }
                        else
                        {
                            Debug.Log("right Swype");

                            if (player.canMove)
                            {

                                laneShift(1);

                            }



                        }

                        Debug.Log("Touch flank");
                    }

                    break;
            }
        }
    }
}
