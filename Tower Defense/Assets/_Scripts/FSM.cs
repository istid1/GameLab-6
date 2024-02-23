using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM : MonoBehaviour
{

    //Next destination for NPC
    protected Vector3 destinationPos;



    //Virtual functions
    protected virtual void Initialize()
    {

    }

    protected virtual void FSMUpdate()
    {

    }

    protected virtual void FSMFixedUpdate()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        FSMUpdate();
    }

    private void FixedUpdate()
    {
        FSMFixedUpdate();
    }
}