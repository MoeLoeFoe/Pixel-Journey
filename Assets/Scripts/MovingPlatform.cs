using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{

    [SerializeField]private GameObject[] waypoints;
    private int currWaypointIndex=0;
    [SerializeField] private float moveSpeed;
    private bool playerUnder = false;
    void Update()
    {
        if (Vector2.Distance(waypoints[currWaypointIndex].transform.position, transform.position) < .1f||playerUnder)
        {
            currWaypointIndex++;
            if (currWaypointIndex >= waypoints.Length)
            {
                currWaypointIndex = 0;
            }
            playerUnder = false;
        }
        transform.position = Vector2.MoveTowards(transform.position,
            waypoints[currWaypointIndex].transform.position,
            Time.deltaTime * moveSpeed);
        
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerUnder = true;
        }
    }
}
