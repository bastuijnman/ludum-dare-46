using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipment : MonoBehaviour
{

    /// <summary>
    /// Defines the operational cost of the equipment, set in the editor.
    /// </summary>
    public float cost;

    public float processTime;

    /// <summary>
    /// The max number of output connections this piece of equipment can have.
    /// Set in the editor.
    /// </summary>
    public int maxOutputConnections;

    /// <summary>
    /// If set this equipment can be upgraded to another equipment object.
    /// </summary>
    public GameObject upgrade;

    /// <summary>
    /// Holds a list of the output connections for this piece of equipment. Output of the equipment
    /// from the process should be sent to the input of these connections.
    /// </summary>
    protected List<Equipment> connections;

    // Start is called before the first frame update
    void Start()
    {
        connections = new List<Equipment>();
    }

    // Update is called once per frame
    void Update()
    {

        /*
         * HERE YE HERE YE, THIS MIGHTY PIECE OF CODE BE JUST FOR DEBUGGING
         * PURPOSES AND SHOULD FIND A WORTHY REPLACEMENT IN THE FUTURE AS
         * IT HAS BEEN FORETOLD BY THE GODS.
         */
        Vector3 position = transform.position;
        connections.ForEach(connection => {
            Debug.DrawLine(position, connection.gameObject.transform.position, Color.green);
        });

    }

    /// <summary>
    /// Add an output connection.
    /// </summary>
    /// <param name="equipment">The equipment to connect to</param>
    public void AddConnection(Equipment equipment)
    {
        connections.Add(equipment);
    }

    /// <summary>
    /// Remove an output connection
    /// </summary>
    /// <param name="equipment">The equipment which' connection should be severed</param>
    public void RemoveConnection(Equipment equipment)
    {
        connections.Remove(equipment);
    }

    public List<Equipment> GetConnections()
    {
        return connections;
    }

    /// <summary>
    /// Determines whether this equipment can have more output connections
    /// </summary>
    /// <returns></returns>
    public bool CanAddConnection()
    {
        return connections.Count < maxOutputConnections;
    }

    public bool IsFinishedProcessing()
    {
        return false;
    }
}
