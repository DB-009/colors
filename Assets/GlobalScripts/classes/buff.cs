using UnityEngine;
using System.Collections;

public class buff : MonoBehaviour {


    public bool casual;

    public int buffID;
    public bool isAlive;
    public float createdAt, lifeSpan;
    public bool die;
    // Use this for initialization
    void Awake()
    {
        //myFunctionz = GameObject.FindGameObjectWithTag("GameStateManager").GetComponent<myFunctions>();
        createdAt = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (die == true && Time.time >= createdAt + lifeSpan)
        {
            Destroy(this.gameObject);
            Debug.Log("buff faded remove from list bug");
        }

    }


    public void BuffEffect(GameObject targetEffect)
    {
        Debug.Log("Remove the extra code in this logic on export to seperate project if casual etc");


    }
           
    
}
