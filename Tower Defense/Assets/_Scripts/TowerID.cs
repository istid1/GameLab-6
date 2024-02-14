using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerID : MonoBehaviour
{
    public int ID { get; private set; }  // Changed public property name to 'ID'
    public TowerID(int id)
    {
        ID = id;
    }
}
