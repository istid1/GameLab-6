using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceParticles : MonoBehaviour
{
   

    private float _rotationSpeed = -1f;
    // Start is called before the first frame update
    // Start is called before the first frame update
    void Start()
    {
        _rotationSpeed = Random.Range(-1f, -2f);
    }
    
    // Update is called once per frame
    void Update()
    {
        this.transform.Rotate(0f, 0f, _rotationSpeed);
    }
}
