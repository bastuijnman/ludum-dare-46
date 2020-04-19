/**
 * Represent a tile type you can attach to tiles.
 * Mostly contain metadata about a tile and how to build it.
 *   
 * Author: Ronen Ness.
 * Since: 2017. 
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;


namespace NesScripts.Tilemap
{
	/// <summary>
	/// An basic tile with a constant type of walls to merge.
	/// </summary>
	public class LoadingTileType : TileType
	{
		/// <summary>
		/// Only if true will try to build other tile parts, using the GetXxxPart() functions.
		/// </summary>
		/// <value>true</value>
		override public bool UseDynamicPartsBuild { get { return false; } }

		/// <summary>
		/// The path of the prefab we use for walls for this tile type (under Resources folder).
		/// </summary>
		virtual protected string WallResourcePath { get { return "Tiles/LoadingZoneTile"; } }

		/// <summary>
		/// When a tile is built and its front (positive Z) neighbor is of a different type, this function will be called.
		/// If returns a GameObject and not null, will use this prefab as the "front part" of this tile.
		/// For example, if your tile is a wall, this will be its front.
		/// 
		/// Note: handle positioning and rotation. Your prefab is assumed to be facing forward.
		/// </summary>
		/// <value>The front part for this tile. Note: will be cloned, not used directly.</value>
		override public GameObject GetFrontPart(Tile self, Tile neighbor) {
			return null;
			//return (GameObject)Resources.Load(WallResourcePath, typeof(GameObject));
		}

		/// <summary>
		/// When a tile is built and its back (negative Z) neighbor is of a different type, this function will be called.
		/// If returns a GameObject and not null, will use this prefab as the "back part" of this tile.
		/// For example, if your tile is a wall, this will be its back.
		/// 
		/// Note: handle positioning and rotation. Your prefab is assumed to be facing forward.
		/// </summary>
		/// <value>The back part for this tile. Note: will be cloned, not used directly.</value>
		override public GameObject GetBackPart(Tile self, Tile neighbor) {
			return null;
			//return (GameObject)Resources.Load(WallResourcePath, typeof(GameObject));
		}

		/// <summary>
		/// When a tile is built and its left (negative X) neighbor is of a different type, this function will be called.
		/// If returns a GameObject and not null, will use this prefab as the "left part" of this tile.
		/// For example, if your tile is a wall, this will be its left size.
		/// 
		/// Note: handle positioning and rotation. Your prefab is assumed to be facing forward.
		/// </summary>
		/// <value>The left part for this tile. Note: will be cloned, not used directly.</value>
		override public GameObject GetLeftPart(Tile self, Tile neighbor) {
			return null;
			//return (GameObject)Resources.Load(WallResourcePath, typeof(GameObject));
		}

		/// <summary>
		/// When a tile is built and its right (positive X) neighbor is of a different type, this function will be called.
		/// If returns a GameObject and not null, will use this prefab as the "right part" of this tile.
		/// For example, if your tile is a wall, this will be its right size.
		/// 
		/// Note: handle positioning and rotation. Your prefab is assumed to be facing forward.
		/// </summary>
		/// <value>The right part for this tile. Note: will be cloned, not used directly.</value>
		override public GameObject GetRightPart(Tile self, Tile neighbor) {
			return null;
			//return (GameObject)Resources.Load(WallResourcePath, typeof(GameObject));
		}
	}
}
