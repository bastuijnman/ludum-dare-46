using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using NesScripts.Tilemap;

public class EquipmentManager : MonoBehaviour
{
    public Tile selectedTile;

    public List<GameObject> availableEquipment;
    protected List<GameObject> placedEquipment;

    protected GameObject activeConnector;

    // Start is called before the first frame update
    void Start()
    {
        placedEquipment = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        Tile hover = FindHoveredTile();
        if (hover) {

            /*
             * DIRTY DIRTY DIRTY method of showing which tile is actively being
             * hovered over
             */
            Vector3 size = hover.gameObject.GetComponent<Renderer>().bounds.size;
            Vector3 position = hover.gameObject.transform.position;

            Vector3 tl = new Vector3( position.x - (size.x / 2), 0.1f, position.z - (size.z / 2) );
            Vector3 tr = new Vector3( position.x - (size.x / 2), 0.1f, position.z + (size.z / 2) );
            Vector3 br = new Vector3( position.x + (size.x / 2), 0.1f, position.z + (size.z / 2) );
            Vector3 bl = new Vector3( position.x + (size.x / 2), 0.1f, position.z - (size.z / 2) );

            Debug.DrawLine(tl, tr, Color.red);
            Debug.DrawLine(tr, br, Color.red);
            Debug.DrawLine(br, bl, Color.red);
            Debug.DrawLine(bl, tl, Color.red);
        }

        /*
         * Only open radial UI when it is not yet active.
         */
        bool isUIEnabled = gameObject.GetComponent<EquipmentUI>().IsUIEnabled();
        if (Input.GetMouseButtonDown(0) && hover && !isUIEnabled) {
            selectedTile = hover;
            Transform equipment = hover.transform.Find("equipment");

            /* 
             * Handle equipment connection when in connection mode and equipment is found
             */
            if (activeConnector && equipment) {
                activeConnector
                    .GetComponent<Equipment>()
                    .AddConnection(equipment.gameObject.GetComponent<Equipment>());
                LeaveConnectionMode();

                // No need to perform any other action
                return;
            }

            if (equipment) {
                gameObject
                    .GetComponent<EquipmentUI>()
                    .ShowUpdateUIFromEquipmentAndPosition(Input.mousePosition, equipment.gameObject);
            } else {
                gameObject
                    .GetComponent<EquipmentUI>()
                    .ShowCreateUIFromAvailableEquipmentAndPosition(Input.mousePosition, availableEquipment);
            }
        }
    }

    /// <summary>
    /// Add equipment to the current brewery. 
    /// TODO: tile will need to be passed in, this can cause the wrong tile to be selected
    /// </summary>
    public void AddEquipment(GameObject equipment)
    {
        Tile tile = selectedTile;
        
		Vector3 blockCentre = new Vector3(tile.gameObject.transform.position.x, tile.gameObject.transform.position.y, tile.gameObject.transform.position.z);

		GameObject placedObject = (GameObject) Instantiate(equipment, blockCentre, equipment.transform.rotation, tile.transform);
        placedObject.name = "equipment";
        placedObject.GetComponent<Equipment>().isPlaced = true;
        placedEquipment.Add(placedObject);
    }

    /// <summary>
    /// Enter connection mode and highlight eligable equipment.
    /// </summary>
    /// <param name="equipment">Equipment that requests an outgoing connection</param>
    public void EnterConnectionMode(GameObject equipment)
    {
        placedEquipment.ForEach(item => {
            if (item != equipment) {
                item.AddComponent<EquipmentSelectionGlow>();
            }
        });
        activeConnector = equipment;
    }

    /// <summary>
    /// Leave the connection mode by setting equipment requesting an outgoing connection
    /// to null. Also make sure to destroy all glow components.
    /// </summary>
    protected void LeaveConnectionMode()
    {
        activeConnector = null;
        placedEquipment.ForEach(item => {
            Destroy(item.GetComponent<EquipmentSelectionGlow>());
        });
    }

    /// <summary>
    /// Tries to find a tile that the user is hovering over
    /// </summary>
    public Tile FindHoveredTile()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 1000f)) {
            Tile tile = hit.collider.gameObject.GetComponent<Tile>();
            if (!tile) {
                return null;
            }

            return tile;
        }
        return null;
    }

    /// <summary>
    /// Get currently placed equipment in the manager
    /// </summary>

    public List<GameObject> GetPlacedEquipment()
    {
        return placedEquipment;
    }

    public float GetRunningCost()
    {
        float total = 0.0f;
        placedEquipment.ForEach(gameObject => {
            total += gameObject.GetComponent<Equipment>().cost;
        });

        return total;
    }
}
