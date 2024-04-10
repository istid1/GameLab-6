using System.Collections.Generic;
using UnityEngine;

namespace _Scripts
{
    public class SquadLeader : MonoBehaviour
    {
        [HideInInspector] public float speed;
        [HideInInspector] public float speedAfterWall;
        public List<GameObject> mySquad;

    
        //EnemyFlySpawnerScript is set through instantiate in EnemyFlySpawner;
        public EnemyFlySpawner enemyFlySpawnerScript;
        private Vector3 myWaypoint;

        private GameObject lightningParent;

        private GameManager _gm;
    
        private void Awake()
        {
            _gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        }
    
        // Start is called before the first frame update
        void Start()
        {
            //Grabs the speed variables from the GM
            speed = _gm.speedBeforeWall;
            speedAfterWall = _gm.speedAfterWall;
        
            lightningParent = GameObject.FindGameObjectWithTag("LightningParent");
        
            //Sets waypoint to travel to
            myWaypoint = new Vector3(transform.position.x, 4, -28);
            GatherSquad();
        }

        private void FixedUpdate()
        {
            //Movement
            transform.position = Vector3.MoveTowards(transform.position, myWaypoint, speed * Time.deltaTime);
        }



        private void GatherSquad()      //Finds all FlyingEnemies that are not currently in a Squad
        {
            foreach (Transform child in lightningParent.transform)
            {
                //If "HasSquad" list doesn't contain gameobject, add it to "HasSquad" & "MySquad" lists
                if (enemyFlySpawnerScript.hasSquad.Contains(child.gameObject) == false)
                {
                    mySquad.Add(child.gameObject);
                    enemyFlySpawnerScript.hasSquad.Add(child.gameObject);
                }
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("FlySlowDownWall"))
            {
                foreach (GameObject child in mySquad)
                {
                    //Sets the speed of All flying enemies in the Squad
                    child.GetComponent<EnemyFlyMovement>().speed = speedAfterWall;
                    //Sets it so that the enemies now can be damaged
                    child.GetComponent<EnemyHealth>().canTakeDamage = true;
                }
            
                Destroy((gameObject));
            }
        }
    }
}
