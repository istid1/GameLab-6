using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBulletBehavoir : MonoBehaviour
{
    // How long the bullet will go into a random upwards direction
    private const float RANDOM_DIRECTION_TIME = 1f;

    // The time the bullet started moving
    private float _startTime;

    // Whether the bullet is still in the random direction phase
    private bool _randomDirection;

    // The direction of the bullet during the random phase
    private Vector3 _randomUpwardsDirection;

    public float speed;
    private Transform _target;
   
    [SerializeField] private float dragFactor;
    
    public void SetTarget(Transform target)
    {
        _target = target;
        // Reset the start time every time a new target is set
        _startTime = Time.time;
        // Bullet will start moving in random upwards direction
        _randomDirection = true;
        // Choose a random upwards direction within some range
        _randomUpwardsDirection = Quaternion.Euler(Random.Range(-45, 45), Random.Range(0, 360), 
            Random.Range(-45, 45)) * Vector3.up;  
    }

    void Update()
    {
        if (_randomDirection)
        {
            speed = 15;
        }
        else
        {
            speed = 50;
        }

        if (_target == null)
        {
            Destroy(gameObject);
            return;
        }
        var elapsedTime = Time.time - _startTime;
        // Determine if the bullet should change direction towards the target
        if (_randomDirection && elapsedTime > RANDOM_DIRECTION_TIME)
        {
            _randomDirection = false;
        }
        Vector3 direction = _randomDirection ?
            _randomUpwardsDirection : (_target.position - transform.position).normalized;

        // Apply drag when bullet is moving upwards
        if (_randomDirection)
        {
            
            direction -= direction * (speed * dragFactor);
        }

        transform.position += direction * speed * Time.deltaTime;
    }
    
}
