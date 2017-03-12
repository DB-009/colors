using UnityEngine;
using System.Collections;

public class buffZone : MonoBehaviour
{

    public GameObject lane;
    public GameObject invisbleExploder;
    public GameObject fwdTarget, bkwrdTarget;
    public GameObject realTarget = null;
    public enum Type { cannon, speedChange, obstacle, portal, depthChange };
    public Type type;
    public GameObject objGen;

    public float valChange1, valChange2;
    public bool inverted;

    //type = speed up, slow down, posion, stun

    // Use this for initialization
    void Awake()
    {
        if (lane)
            transform.position = new Vector3(this.transform.position.x, lane.transform.position.y, this.transform.position.z);


    }

    public void RunBuffPragma(Type disType, bool is2D, GameObject disObj)
    {


        Rigidbody obj = null;
        Rigidbody2D obj2 = null;

        if (is2D == false)
        {
            obj = disObj.GetComponent<Rigidbody>();


            if (disType == Type.speedChange)
            {
                obj.AddForce(new Vector3(valChange1 * 1.5f, 0, 0), ForceMode.Impulse);

            }
            else if (disType == Type.cannon)
            {
                //Debug.Log("god script here");

                Platform_CollisionHandler player = obj.GetComponent<Platform_CollisionHandler>();


                if (Time.time >= player.LastCannonAt + player.timeBetweenCanon && player.LastCannonAt != 0 || bkwrdTarget == null)
                {
                    player.rb.velocity = Vector3.zero;
                    player.isLastCannon = false;
                    player.player.canMove = false;
                    player.player.sidSpd = 0;
                    obj.transform.position = this.transform.position;
                }


                if (player != null)
                {
                    if (obj.isKinematic == false && Time.time >= player.LastCannonAt + player.timeBetweenCanon)
                    {


                        // obj2.gravityScale = 0;
                        obj.isKinematic = true;
                        obj.velocity = Vector3.zero;
                        // Debug.Log("here1");
                        obj.transform.position = this.transform.position;
                    }
                    else if (obj.isKinematic == true)
                    {

                        bool isLAstCannon = false;
                        obj.velocity = Vector3.zero;


                        if (Time.time >= player.LastCannonAt + player.timeBetweenCanon)
                        {
                            // obj2.transform.position = targetobj2.transform.position;
                            player.LastCannonAt = Time.time;
                            // Debug.Log("here2");
                            //if last cannon == target objet shoot playerCollisionHandler to go back object



                            if (bkwrdTarget == null)//if in first cannon bkwrd target is false so go forward
                            {
                                Debug.Log("here3");

                                if (player.cannonBackward == true)//if going backwards check if youve exited and entered first cannon
                                {
                                    isLAstCannon = true;
                                    if (player.lastCannon[1] == this.gameObject.transform.parent.gameObject)
                                    {
                                        isLAstCannon = false;

                                        player.cannonBackward = false;//jumped back into first cannon fo forward
                                        realTarget = fwdTarget;
                                    }
                                }//if you arent going backwards its because you just entered it.
                                else//else just move forward
                                {
                                    realTarget = fwdTarget;
                                    player.cannonBackward = false;
                                }





                            }

                            else if (player.lastCannon[1] == this.gameObject.transform.parent.gameObject)
                            {
                                Debug.Log("here5");

                                realTarget = bkwrdTarget;
                                player.cannonBackward = true;

                            }
                            else if (fwdTarget == null)//if in last connon go backward to beginning
                            {
                                // realTarget = bkwrdTarget;
                                Debug.Log("here4");
                                isLAstCannon = true;
                                realTarget = bkwrdTarget;
                                player.cannonBackward = true;
                            }//change of direction
                            else if (player.lastCannon[1] == fwdTarget)//if going backwards go backwards til end
                            {
                                Debug.Log("here6");

                                realTarget = bkwrdTarget;
                                player.cannonBackward = true;

                            }
                            else if (fwdTarget != null)//if going forward keep going forward til end
                            {
                                Debug.Log("here7");

                                realTarget = fwdTarget;
                                player.cannonBackward = false;



                            }


                            if (isLAstCannon == false || (bkwrdTarget != null && player.cannonBackward == false))
                            {
                                if (Input.GetKeyDown(KeyCode.Mouse0))
                                {
                                    Debug.Log("here8" + realTarget);

                                    Ray targetPoint = Camera.main.ScreenPointToRay(realTarget.transform.position);

                                    Vector3 direction = Vector3.zero;

                                    Vector3 pos = targetPoint.GetPoint(0);


                                    direction = realTarget.transform.position - this.transform.position;

                                    float sqrLen = direction.sqrMagnitude;


                                    Quaternion targetRotation = Quaternion.LookRotation(pos - realTarget.transform.position);

                                    // Smoothly rotate towards the target point.
                                    // this.transform.rotation = targetRotation;

                                    obj.isKinematic = false;

                                    player.lastCannon[0] = player.lastCannon[1];
                                    player.lastCannon[1] = this.gameObject.transform.parent.gameObject;
                                    obj.AddForce(new Vector3(direction.x, direction.y + 2, 0) * valChange1, ForceMode.Impulse);
                                    player.player.dblJumped = true;
                                }
                            }
                            else
                            {
                                if (Input.GetKeyDown(KeyCode.Mouse0))
                                {
                                    Debug.Log("Yoo");
                                    // Debug.Log("bug");
                                    obj.isKinematic = false;

                                    //obj2.gravityScale = 0;
                                    float xDis = 0;
                                    if (inverted)
                                        xDis = -valChange2;
                                    else
                                        xDis = valChange2;

                                    //Debug.Log("bug " + xDis);
                                    player.player.canMove = true;
                                    obj.AddForce(new Vector3(0, xDis, 0), ForceMode.Impulse);
                                    player.isLastCannon = true;
                                    player.player.dblJumped = false;
                                    player.LastCannonAt = Time.time;
                                    player.lastCannon[0] = player.lastCannon[1];
                                    player.lastCannon[1] = this.gameObject.transform.parent.gameObject;
                                }
                            }


                        }


                    }
                }
                else
                {
                    Debug.Log("seems like a computer landed in a cannon what sould I do?!");
                }
            }
        }
    
        
    }
}
