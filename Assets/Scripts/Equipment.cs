using System;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey;

public class Equipment : MonoBehaviour
{
    /// <summary>
    /// Defines the operational cost of the equipment, set in the editor.
    /// </summary>
    public float cost;

    public float maxUnprocessedAmount = 10f;

    public float maxProcessedAmount = 100f;

    //Ticks are every 200ms, so to set this value in seconds multiply your number by 5
    public float processTime = 15f;

    //This is the amount of resources the equipment will process per tick.
    //To set this value in amount/second divide your number by 5
    public float processAmount = 0.2f;
    
    public float outputAmount = 1;

    /// <summary>
    /// The max number of output connections this piece of equipment can have.
    /// Set in the editor.
    /// </summary>
    public int maxOutputConnections;

    /// <summary>
    /// If set this equipment can be upgraded to another equipment object.
    /// </summary>
    public GameObject upgrade;

    public bool IsFinishedProcessing = false;

    /// <summary>
    /// Holds a list of the output connections for this piece of equipment. Output of the equipment
    /// from the process should be sent to the input of these connections.
    /// </summary>
    protected List<Equipment> connections;

    public bool isPlaced = false;

    public Resource unprocessedResource;
    public Resource processedResource;

    public float startingAmount = 10f;

    public string resourceType;

    public string outputType;

    public Equipment()
    {
        ResourceFlow.OnTick += OnTick;
    }

    public Resource GetProcessedResource() 
    {
        if (processedResource == null) {
            processedResource = new Resource(outputType, 0f);
        }
        return processedResource;
    }

    public Resource GetUnprocessedResource() 
    {
        if (unprocessedResource == null) {
            unprocessedResource = new Resource(resourceType, startingAmount);
        }
        return unprocessedResource;
    }

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
        float newAmount = processedResource.amount - processAmount;
        if (newAmount < 0f) {
            newAmount = 0f;
        }

        processedResource.amount = newAmount;

        recipient.IngestResource(amount);
    }

    public void IngestResource(float amount)
    {
        GetUnprocessedResource().amount = GetUnprocessedResource().amount + amount;
        if (GetUnprocessedResource().amount > maxUnprocessedAmount) {
            GetUnprocessedResource().amount = maxUnprocessedAmount;
        }
    }

    private void OnTick(object sender, ResourceFlow.OnTickEventArgs e) {
        
        Debug.Log("ticky tick");
        Debug.Log(IsFinishedProcessing);

        HandleProcessing();
        ShowDebugInfo();
    }

    private void ShowDebugInfo()
    {
        string uText = GetUnprocessedResource().name + " " + GetUnprocessedResource().amount.ToString() + " " + GetProcessedResource().name + " " + GetProcessedResource().amount.ToString();
        if (this != null) {
            if (isPlaced) {
                CMDebug.TextPopup(uText, this.transform.position);
            }
        }
    }
    

    private void HandleProcessing()
    {
        Debug.Log("processing");
        if (isPlaced != true) {//never process unplaced equipment
            Debug.Log("is placed is false");
            return;
        }

        //the only thing that would stop us processing this tick are;
        //if the processedResource amount is == to max or if the unProcessed is empty
        if (GetProcessedResource().amount == maxProcessedAmount || GetUnprocessedResource().amount == 0) {
            IsFinishedProcessing = true;
        }

        //conditions for starting processing
        //the amount of unprocessed resource is == to max
        //the amount of processed resource is == to 0
        if (IsFinishedProcessing && GetUnprocessedResource().amount == maxUnprocessedAmount && GetProcessedResource().amount == 0f) {
            IsFinishedProcessing = false;
        }

        //conditions for continuing processing
        //isnt finished processing
        if (IsFinishedProcessing != true) { //continue processing
            //decrement unprocessed amount by proceessAmount
            //increment processed amount by processAmount
            processResources();
        }
        
    }

    private void processResources()
    {
        Debug.Log("processing");
        GetUnprocessedResource().amount = GetUnprocessedResource().amount - processAmount;
        GetProcessedResource().amount = GetProcessedResource().amount + processAmount;

        if (GetUnprocessedResource().amount < 0f) {
            GetUnprocessedResource().amount = 0f;
        }

        if (GetProcessedResource().amount > maxProcessedAmount) {
            GetProcessedResource().amount = maxProcessedAmount;
        }
    }
}
