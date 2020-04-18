using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NesScripts.Tilemap;

public class EquipmentManager : MonoBehaviour
{
    public List<GameObject> availableEquipment;
    protected List<GameObject> placedEquipment;

    // Start is called before the first frame update
    void Start()
    {
        
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
            gameObject.GetComponent<EquipmentUI>().CreateUIFromAvailableEquipmentAndPosition(Input.mousePosition, availableEquipment);
        }
    }

    /// <summary>
    /// Add equipment to the current brewery. 
    /// TODO: tile will need to be passed in, this can cause the wrong tile to be selected
    /// </summary>
    public void AddEquipment(GameObject equipment)
    {
        Tile tile = FindHoveredTile();
        Instantiate(equipment, tile.transform);
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
}
