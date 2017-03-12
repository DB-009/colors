using UnityEngine;
using System.Collections;

[System.Serializable]
public class taps  {

    public float id;
    public float time;
	public Vector2 position;
	public Vector2 start_pos;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public taps (float time, Vector2 position, Vector2 start_pos , float id)
	{
		this.time = time;
		this.position = position;
		this.start_pos = start_pos;
		this.id = id;
	}

	public taps ()
	{
		}

}
