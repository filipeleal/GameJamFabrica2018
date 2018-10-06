using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchAndGuard : MonoBehaviour {

    // Use this for initialization
    private GameObject player;
    private State state;

    private enum State
    {
        GUARD,
        PURSUIT,
        SLEEP
    }

    public float pursuitDistance;
    public float speedPerSecond;
    public float timeAfterDamage;


    void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
        state = 0;
    }
	
	// Update is called once per frame
	void Update () {
        switch (state)
        {
            case State.GUARD:
                Guard();
                break;
            case State.PURSUIT:
                Pursuit();
                break;
        }
	}

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            GetComponent<Renderer>().material.color = Color.white;
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            rb.Sleep();
            rb.simulated = false;
            this.state = State.SLEEP;
            Invoke("Reawaken", timeAfterDamage);
            /*
            this.gameObject.
            collision.gameObject.SendMessage("ApplyDamage", 10);
            */
        }
    }

    private void Reawaken()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.WakeUp();
        rb.simulated = true;
        this.state = State.GUARD;
        GetComponent<Renderer>().material.color = Color.green;

    }
    

    private void Pursuit()
    {
        
        if (Vector3.Distance(this.transform.position, player.transform.position) < pursuitDistance)
        {

            this.transform.position = Vector3.MoveTowards(this.transform.position, player.transform.position, Time.deltaTime*this.speedPerSecond);
           

        }else
        {
            GetComponent<Renderer>().material.color = Color.green;
            state = State.GUARD;
        }
    }

    private void Guard()
    {
        if(Vector3.Distance(this.transform.position, player.transform.position) < pursuitDistance)
        {
            GetComponent<Renderer>().material.color = Color.yellow;
            state = State.PURSUIT;
        }

        

    }

}
