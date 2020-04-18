/**
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
	public class GameArea : TileMap {

		// array of tiles
		Tile[,] _tiles;

		/// <summary>
		/// Creates new standard game area
		/// </summary>
		/// <param name="width">Map width.</param>
		/// <param name="height">Map height.</param>
		public void CreateGameArea()
		{
			// set params
			Width = 120;
			Height = 200;
			TileSize.x = 10;
			TileSize.y = 10;
		
			// if there's a previous tilemap, destroy it first
			DestroyTilemap();

			// create tiles array
			_tiles = new Tile[Width, Height];

			CreateLoadingZone(new Vector2 (0, 0), new Vector2 (119, 50));
			CreateBrewingZone(new Vector2 (0, 50), new Vector2 (119, 100));
			CreatePackagingZone(new Vector2 (0, 100), new Vector2 (119, 150));
		}

		public void CreateLoadingZone(Vector2 startPosition, Vector2 endPosition)
		{
			GameObject prefab = getTileResource("Tiles/LoadingZoneTile");
			SetTileRange(prefab, startPosition, endPosition);
		}

		public void CreateBrewingZone(Vector2 startPosition, Vector2 endPosition)
		{
			GameObject prefab = getTileResource("Tiles/BrewingZoneTile");
			SetTileRange(prefab, startPosition, endPosition);
		}

		public void CreatePackagingZone(Vector2 startPosition, Vector2 endPosition)
		{
			GameObject prefab = getTileResource("Tiles/PackagingZoneTile");
			SetTileRange(prefab, startPosition, endPosition);
		}

		private void SetTileRange(GameObject TileObject, Vector2 startPosition, Vector2 endPosition)
		{
			// create the tilemap
			for (float i = startPosition.x; i < endPosition.x; ++i) {
				for (float j = startPosition.y; j < endPosition.y; ++j) {
					// set the tile
					SetTile(TileObject, new Vector2 (i, j), false);
				}
			}

			// build all tiles
			for (float i = startPosition.x; i < endPosition.x; ++i) {
				for (float j = startPosition.y; j < endPosition.y; ++j) {
					_tiles [(int) i, (int) j].Build (false);
				}
			}
		}

		private GameObject getTileResource(string path)
		{
			GameObject prefab = (Resources.Load(path) as GameObject);

			if (prefab == null) {
				throw new UnityException ("You must set a default tile prefab when creating empty tilemap!");
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
		public void SetTile(GameObject tilePrefab, Vector2 index, bool build = true)
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
			tileComponent.Init (this, index);

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