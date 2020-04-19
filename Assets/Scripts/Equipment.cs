using System;
using System.Collections.Generic;
using UnityEngine;

public class Equipment : MonoBehaviour
{

    /// <summary>
    /// Defines the operational cost of the equipment, set in the editor.
    /// </summary>
    public float cost;

    public float maxUnprocessedAmount = 100f;

    public float maxProcessedAmount = 100f;

    //Ticks are every 200ms, so to set this value in seconds multiply your number by 5
    public float processTime;

    //This is the amount of resources the equipment will process per tick.
    //To set this value in amount/second divide your number by 5
    public float processAmount;
    
    public float outputAmount;

    /// <summary>
    /// The max number of output connections this piece of equipment can have.
    /// Set in the editor.
    /// </summary>
    public int maxOutputConnections;

    /// <summary>
    /// If set this equipment can be upgraded to another equipment object.
    /// </summary>
    public GameObject upgrade;

    public bool IsFinishedProcessing = true;

    /// <summary>
    /// Holds a list of the output connections for this piece of equipment. Output of the equipment
    /// from the process should be sent to the input of these connections.
    /// </summary>
    protected List<Equipment> connections;

    public bool isPlaced = false;

    public Resource unprocessedResource = new Resource {
            name = "test",
            amount = 10f
        };

    public Resource processedResource = new Resource {
            name = "test",
            amount = 10f
        };

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

    /// <summary>
    /// Get the output connections
    /// </summary>
    /// <returns>List of the connections</returns>
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

    public void SendResource(Equipment recipient, float amount)
    {
        recipient.IngestResource(amount);
    }

    public void IngestResource(float amount)
    {
        unprocessedResource.amount = unprocessedResource.amount + amount;
        if (unprocessedResource.amount > maxUnprocessedAmount) {
            unprocessedResource.amount = maxUnprocessedAmount;
        }
    }

    private void OnTick(object sender, ResourceFlow.OnTickEventArgs e) {
        if (!IsFinishedProcessing) {
            HandleProcessing();
        }
    }

    private void HandleProcessing()
    {
        if (!isPlaced) {//never process unplaced equipment
            return;
        }

        //conditions for starting processing
        //the amount of unprocessed resource is == to max
        //the amount of processed resource is == to 0
        if (IsFinishedProcessing && unprocessedResource.amount == maxUnprocessedAmount && processedResource.amount == 0f) {
            IsFinishedProcessing = false;

            processResources();
        }

        //conditions for continuing processing
        //isnt finished processing
        if (!IsFinishedProcessing) { //continue processing
            //decrement unprocessed amount by proceessAmount
            //increment processed amount by processAmount
            processResources();
        }
        
        //the only thing that would stop us processing this tick are;
        //if the processedResource amount is == to max
        if (processedResource.amount == maxProcessedAmount) {
            IsFinishedProcessing = true;
        }
    }

    private void processResources()
    {
        unprocessedResource.amount = unprocessedResource.amount - processAmount;
        processedResource.amount = processedResource.amount + processAmount;

        if (unprocessedResource.amount < 0f) {
            unprocessedResource.amount = 0f;
        }

        if (processedResource.amount > maxProcessedAmount) {
            processedResource.amount = maxProcessedAmount;
        }
    }
}
