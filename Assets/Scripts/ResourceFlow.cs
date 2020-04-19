using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ResourceFlow : MonoBehaviour
{
    public class OnTickEventArgs : EventArgs {
        public int tick;
    }
    
    public static event EventHandler<OnTickEventArgs> OnTick;

    private const float MAX_TICK_DURATION = 0.2f;

    private int tick;

    private float tickTimer;

    protected EquipmentManager equipmentManager;
    

    // Start is called before the first frame update
    void Start()
    {  
        equipmentManager = GetComponent<EquipmentManager>();
    }
    
    void Awake()
    {
        tick = 0;
    }

    // Update is called once per frame
    void Update()
    {
        tickTimer += Time.deltaTime;
        if (tickTimer >= MAX_TICK_DURATION) {
            tickTimer -= MAX_TICK_DURATION;
            tick++;
            
            //trigger event that will notify all eqipment to process their resources.
            if (OnTick != null) {
                OnTick(this, new OnTickEventArgs {tick = tick});
            }

            //handle the flow of resources between equipment
            resourceTick();
        }
    }

    public void resourceTick()
    {
        List<GameObject> placedEquipment = equipmentManager.GetPlacedEquipment();
        
        //get all placed items that arent processing, and dont have an empty processedResouce amount
        IEnumerable<GameObject> equipmentEligableForDispatching = placedEquipment.Where(placedEquipmentItem => (placedEquipmentItem.GetComponent<Equipment>().IsFinishedProcessing == true && placedEquipmentItem.GetComponent<Equipment>().processedResource.amount > 0));
        
        //tell them to distribute the resources they have 
        foreach (GameObject equipment in equipmentEligableForDispatching) {
            sendResources(equipment.GetComponent<Equipment>());
        }
    }

    public void sendResources(Equipment placedEquipment)
    {
        List<Equipment> connections = placedEquipment.GetConnections();

        if (connections.Count > 0) {
            /*
            *   Narrow down connections to the ones we want to send data to;
            *   - must not be processing
            *   - must accept the type this equipment is going to send
            *   - must not be full
            */
            IEnumerable<Equipment> equipmentEligableForIngest = connections.Where(equipment => (equipment.IsFinishedProcessing == true && equipment.unprocessedResource.amount < equipment.maxUnprocessedAmount && equipment.unprocessedResource.type == placedEquipment.processedResource.type));
            if (equipmentEligableForIngest.Count() > 0) {
                //get the max number of resources this item can output in a tick
                //divide it by the number of equipment in equipmentEligableForIngest as to distribute evenly
                float amountPerConnection = placedEquipment.outputAmount / equipmentEligableForIngest.Count();
                foreach (Equipment equipment in equipmentEligableForIngest) {
                    placedEquipment.SendResource(equipment, amountPerConnection);
                }
            }
        } else {
                /*
                *   TODO: Add warning that resources are going to waste.
                *   It would be fairly easy to pop a notification up on this item here as we
                *   have a reference for it.
                */

        }
    }
}
