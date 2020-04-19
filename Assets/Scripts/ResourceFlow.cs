using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceFlow : MonoBehaviour
{

    protected EquipmentManager equipmentManager;

    // Start is called before the first frame update
    void Start()
    {  
        equipmentManager = GetComponent<EquipmentManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
        List<GameObject> equipment = equipmentManager.GetPlacedEquipment();

        // TODO: Handle process ticks.
        equipment.ForEach(obj => {

            Equipment item = obj.GetComponent<Equipment>();

            /*
             * When an item is finished processing we want to get it's results
             * and pass it along to any of the connected equipment.
             */
            if (item.IsFinishedProcessing()) {

                List<Equipment> connections = item.GetConnections();

                if (connections.Count > 0) {

                    Resource resource = item.GetProcessedResource();
                    connections.ForEach(connection => connection.IngestResource(resource));

                } else {
                    // TODO: Add warning that resources are going to waste.
                }

            }

        });

    }
}
