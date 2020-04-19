using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipment : MonoBehaviour
{

    public float cost;

    public int maxOutputConnections;

    public GameObject upgrade;

    protected List<Equipment> connections;

    // Start is called before the first frame update
    void Start()
    {
        connections = new List<Equipment>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddConnection(Equipment equipment)
    {
        connections.Add(equipment);
    }

    public void RemoveConnection(Equipment equipment)
    {
        connections.Remove(equipment);
    }

    public bool CanAcceptConnection()
    {
        return connections.Count < maxOutputConnections;
    }
}
