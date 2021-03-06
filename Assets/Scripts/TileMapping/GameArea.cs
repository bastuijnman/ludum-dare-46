﻿/**
 * 3D tilemap, made of game objects with tile components, arranged in a grid.
 *   
 * Author: Ronen Ness.
 * Since: 2017. 
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NesScripts.Tilemap
{
	/// <summary>
	/// Represent a 3d tilemap, made of tile components.
	/// Note: see Tile component for more info.
	/// </summary>
	public class GameArea : MonoBehaviour {

		// array of tiles
		Tile[,] _tiles;

		// tilemap width and height
		public int Width = 60;
		public int Height = 100;

		// tile size (if 0, will be calculated on start based on tile renderers)
		public Vector2 TileSize = new Vector2(0, 0);

		/// <summary>
		/// Creates new standard game area
		/// </summary>
		/// <param name="width">Map width.</param>
		/// <param name="height">Map height.</param>
		public void CreateGameArea()
		{
			// set params
			Width = 30;
			Height = 50;
			TileSize.x = 5;
			TileSize.y = 5;
		
			// if there's a previous tilemap, destroy it first
			DestroyTilemap();

			// create tiles array
			_tiles = new Tile[Width, Height];

			CreateLoadingZone(new Vector2 (0, 0), new Vector2 (10, 3));
			CreateBrewingZone(new Vector2 (0, 3), new Vector2 (10, 14));
			CreatePackagingZone(new Vector2 (0, 14), new Vector2 (10, 18));

			//DecorateGameArea();

		}

		public void DecorateGameArea()
		{
			//Add wall strips to seperate zones
			AddSeperators();
		}

		public void AddSeperators()
		{
			GameObject prefab = getWallResource("Walls/BrewingWall");
			PlaceObjectOverRange(prefab, new Vector2 (0, 14), new Vector2 (30, 15));
		}

		public void CreateLoadingZone(Vector2 startPosition, Vector2 endPosition)
		{
			GameObject prefab = getTileResource("Tiles/LoadingZoneTile");
			SetTileRange(prefab, startPosition, endPosition, "LoadingTileType");
		}

		public void CreateBrewingZone(Vector2 startPosition, Vector2 endPosition)
		{
			GameObject prefab = getTileResource("Tiles/BrewingZoneTile");
			SetTileRange(prefab, startPosition, endPosition, "BrewingTileType");
		}

		public void CreatePackagingZone(Vector2 startPosition, Vector2 endPosition)
		{
			GameObject prefab = getTileResource("Tiles/PackagingZoneTile");
			SetTileRange(prefab, startPosition, endPosition, "PackagingTileType");
		}

		private void SetTileRange(GameObject TileObject, Vector2 startPosition, Vector2 endPosition, string tileTypeName)
		{
			// create the tilemap
			for (float i = startPosition.x; i < endPosition.x; ++i) {
				for (float j = startPosition.y; j < endPosition.y; ++j) {
					// set the tile
					SetTile(TileObject, new Vector2 (i, j), tileTypeName, false);
				}
			}

			// build all tiles
			for (float i = startPosition.x; i < endPosition.x; ++i) {
				for (float j = startPosition.y; j < endPosition.y; ++j) {
					_tiles [(int) i, (int) j].Build (false);
				}
			}
		}

		private void PlaceObjectOverRange(GameObject gameObject, Vector2 startPosition, Vector2 endPosition)
		{
			// create the tilemap
			for (float i = startPosition.x; i < endPosition.x; ++i) {
				for (float j = startPosition.y; j < endPosition.y; ++j) {
					// set the tile
					PlaceObject(gameObject, new Vector2 (i, j));
				}
			}
		}

		public void PlaceObject(GameObject gameObject, Vector2 index)
		{
			//get tile game object
			Tile gameTile = GetTile(index);
			if (gameTile == null) {
				return;
			}

			float gameTileTop = gameTile.gameObject.transform.position.y + gameTile.gameObject.transform.lossyScale.y/2;

			Vector3 blockCentre = new Vector3(gameTile.gameObject.transform.position.x, gameTileTop + gameObject.transform.lossyScale.y/2, gameTile.gameObject.transform.position.z);
			Instantiate(gameObject, blockCentre, Quaternion.identity);
		}


		private GameObject getWallResource(string path)
		{
			GameObject prefab = (Resources.Load(path) as GameObject);
		
			if (prefab == null) {
				throw new UnityException ("You must set a wall prefab when creating empty tilemap!");
			}


			return prefab;
		}

		private GameObject getTileResource(string path)
		{
			GameObject prefab = (Resources.Load(path) as GameObject);

			if (prefab == null) {
				throw new UnityException ("You must set a tile prefab when creating empty tilemap!");
			}

			// make sure tile prototype got Tile component
			if (prefab.GetComponent<Tile>() == null) {
				throw new UnityException ("Tile prototype must contain a 'Tile' component.");
			}

			return prefab;
		}

		/// <summary>
		/// Destroy the tilemap.
		/// </summary>
		public void DestroyTilemap()
		{
			// if there's a previous tilemap, destroy it first
			if (_tiles != null) {
				foreach (var tile in _tiles) {
					Destroy (tile.gameObject);
				}
				_tiles = null;
			}
		}

		/// <summary>
		/// Set a tile.
		/// </summary>
		/// <param name="tilePrefab">Tile to set. May be null to remove the tile without substitute.</param>
		/// <param name="index">Index to put this tile.</param>
		/// <param name="build">If true, will also build the tile and its immediate neighbors.</param>
		/// <param name="type">Type so we can create tthe correct tile</param>
		public void SetTile(GameObject tilePrefab, Vector2 index, string tileTypeName, bool build = true)
		{
			// destroy previous tile, if exists
			if (_tiles [(int)index.x, (int)index.y] != null) {
				Destroy (_tiles[(int)index.x, (int)index.y].gameObject);
			}

			// if only destroying tile, stop here
			if (tilePrefab == null) {

				// remove the tile component
				_tiles [(int)index.x, (int)index.y] = null;

				// rebuild neighbors
				for (int i = -1; i <= 1; ++i) {
					for (int j = -1; j <= 1; ++j) {
						if (i != 0 || j != 0) {
							var ntile = GetTile (index + new Vector2 (i, j));
							if (ntile != null) {
								ntile.Build (false);
							}
						}
					}
				}
				return;
			}

			// create tile and set its position and parent
			GameObject tile = GameObject.Instantiate(tilePrefab);
			tile.transform.position = new Vector3 (index.x * TileSize.x, 0, index.y * TileSize.y);
			tile.transform.parent = transform;

			// init tile component
			Tile tileComponent = tile.GetComponent<Tile>();
			tileComponent.Init (this, index, tileTypeName);

			// add to tiles array
			_tiles[(int)index.x, (int)index.y] = tileComponent;

			// if build is true, build tile
			if (build) {
				tileComponent.Build (true);
			}
		}
		
		// get tile by index (or null if out of boundaries / not set)
		public Tile GetTile(Vector2 index)
		{
			try {
				return _tiles [(int)index.x, (int)index.y];
			}
			catch (System.IndexOutOfRangeException ) {
				return null;
			}
		}
			
		// Use this for initialization
		void Start () {
			CreateGameArea();
		}
	}
}