﻿using System;
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

        bool isUIEnabled = gameObject.GetComponent<EquipmentUI>().IsUIEnabled();
        if (Input.GetMouseButtonDown(0) && hover && !isUIEnabled) {
            gameObject.GetComponent<EquipmentUI>().CreateUIFromAvailableEquipmentAndPosition(Input.mousePosition, availableEquipment);
        }
    }

    public void AddEquipment(GameObject equipment)
    {
        Tile tile = FindHoveredTile();
        equipment.transform.parent = tile.transform.parent;
        
        //The idea here is that i get the height of the object from the mesh, then half it and add
        //it as an offset to the placement height so it's alayws flush with the floor.
        //The idea however, doesnt work. No clue where to go from here but things are all floaty.
        float equipmentHeight = equipment.GetComponent<MeshFilter>().sharedMesh.bounds.extents.y * 2;
		Vector3 blockCentre = new Vector3(tile.gameObject.transform.position.x, tile.gameObject.transform.position.y + (equipmentHeight / 2), tile.gameObject.transform.position.z);
        
		GameObject placedObject = (GameObject) Instantiate(equipment, blockCentre, equipment.transform.rotation);
        
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
