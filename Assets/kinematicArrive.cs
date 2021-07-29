using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class kinematicArrive : MonoBehaviour
{
    // Player utilizes movement as a Unity AI Agent
    NavMeshAgent player;
    [SerializeField] NavMeshAgent player2;
    [SerializeField] NavMeshAgent player3;
    [SerializeField] NavMeshAgent player4;
    [SerializeField] GameObject obstacle;

    // Boolean to determine whether player can move
    private bool moving;

    // Start is called before the first frame update
    void Start()
    {
        // Sets the NavMeshAgent to the player object in the scene
        player = GetComponent<NavMeshAgent>();
        // Let the game know the player isn't moving by default
        moving = false;
    }

    // Update is called once per frame
    void Update()
    {
        // If the player isn't moving and the player clicks left mouse
        if (Input.GetMouseButtonDown(0) && !moving)
        {
            // Set the player to moving in the boolean
            moving = true;
            // create a raycast hit to hold raycast information
            RaycastHit hit;
            //Shoot a raycast from the main camera to the mouse position relative to the plane
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            // If the ray hit something
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                // Tell the agent to path to the destination using the Agent and Navmesh
                Vector3 startPos = transform.position;

                player.SetDestination(hit.point);
                moveToPoint(hit.point, startPos, player2, 2);
                moveToPoint(hit.point, startPos, player3, -2);
                moveToPoint(hit.point, startPos, player4, -4);
            }
        }
        // If the player is moving
        else if (moving)
        {
            // check to see if the player is still moving by comparing distance to
            // path location vs the stopping threshold
            if (player.remainingDistance <= player.stoppingDistance)
            {
                // if the player is close enough, the player is no longer moving
                // and can receive new instructions.
                moving = false;
            }
        }

        if (Input.GetMouseButton(1))
        {
            RaycastHit hit;
            //Shoot a raycast from the main camera to the mouse position relative to the plane
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            // If the ray hit something
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                Instantiate(obstacle, new Vector3(hit.point.x, 1, hit.point.z), Quaternion.identity);
            }
        }
    }

    void moveToPoint(Vector3 pos, Vector3 startPos, NavMeshAgent agent, int posMod)
    {
        float modifier = 0f;
        Vector3 targetDir = pos - startPos;
        float angle = Vector3.Angle(targetDir, transform.forward);
        float angleMultiplier = angle / 180;

        float dist2 = Vector3.Distance(agent.gameObject.transform.position, pos);
        float dist1 = Vector3.Distance(startPos, pos);

        if (dist1 <= dist2)
        {
            modifier = 1f;
        }

        Vector3 targetPos = new Vector3(pos.x + (posMod - angleMultiplier) - modifier, 1f, pos.z - (posMod - angleMultiplier) - modifier);

        

        Debug.Log(angle);
        agent.SetDestination(targetPos);
    }
}
