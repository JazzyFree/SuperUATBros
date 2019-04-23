using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedEnemy : MonoBehaviour {

    public Transform[] patrolPoints;
    public Transform currentPatrolPoint;
    public int currentPatrolPointIndex = 0;
    public float speed;
    public int health;

	// Use this for initialization
	void Start () {
        currentPatrolPoint = patrolPoints[currentPatrolPointIndex];
        if(currentPatrolPoint.position.x < transform.position.x)
        {
            speed = -1f;
        }
        else
        {
            speed = 1f;
        }
	}
	
	// Update is called once per frame
	void Update () {
        transform.Translate(Vector2.right * Time.deltaTime * speed);

        if(Vector2.Distance(currentPatrolPoint.position,transform.position) < .2f)
        {
            if(currentPatrolPointIndex < patrolPoints.Length - 1)
            {
                currentPatrolPointIndex++;
            }
            else
            {
                currentPatrolPointIndex = 0;

            }
            currentPatrolPoint = patrolPoints[currentPatrolPointIndex];

            if (currentPatrolPoint.position.x < transform.position.x)
            {
                speed = -1f;
            }
            else
            {
                speed = 1f;
            }
        }
	}
      void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Laser"))
        {//destroy the laser 
            Destroy(other.gameObject);
            //take away health
            health -= 5;
            //check for dead
            if(health <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
