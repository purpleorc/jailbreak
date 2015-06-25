using UnityEngine;
using System.Collections.Generic;
using System;

public class PathFinder : MonoBehaviour {

	private Tile _targetTile;

	public static PathFinder Instance { get; private set; }
	
	private void Awake() {
		Instance = this;
	}

	public List<TileCoordinate> FindPath( TileCoordinate sourceCoordinate, TileCoordinate targetCoordinate ) {

		Tile sourceTile = TileMapManager.Instance.GetTileAtCoordinate( sourceCoordinate );
		_targetTile = TileMapManager.Instance.GetTileAtCoordinate( targetCoordinate );

		TileMapManager.Instance.ResetTiles( _targetTile );

		List<TileCoordinate> path = new List<TileCoordinate>();
		if ( AStar( sourceTile ) ) {
			Tile currentTile = _targetTile;
			while ( currentTile != null ) {
				path.Add( currentTile.Coordinate );
				currentTile = currentTile.ParentTile;
			}
			path.Reverse();
		}

		return path;
	}

	private bool AStar( Tile currentTile ) {
		currentTile.State = Tile.TileState.CLOSED;

		List<Tile> neighbourTiles = GetValidNeighbours( currentTile );
		neighbourTiles.Sort( ( tileA, tileB ) => tileA.F.CompareTo( tileB.F ) );
		foreach ( Tile neighbourTile in neighbourTiles ) {
			if ( neighbourTile == _targetTile ) {
				return true;
			} 
			else if ( AStar( neighbourTile ) ) {
				return true;
			}
		}		
		return false;
	}

	private List<Tile> GetValidNeighbours( Tile currentTile ) {
		List<Tile> validTiles = new List<Tile>();

		for ( int i = 0, count = currentTile.Neighbours.Count; i < count; i++ ) {
			Tile neighbourTile = currentTile.Neighbours[ i ];

			if ( neighbourTile.IsBlocked || neighbourTile.State == Tile.TileState.CLOSED ) {
				continue;
			}

			if ( neighbourTile.State == Tile.TileState.OPEN ) {
				float traversalCost = Tile.GetTraversalCost( neighbourTile, neighbourTile.ParentTile );
				float gTemp = currentTile.G + traversalCost;
				if ( gTemp < neighbourTile.G ) {
					neighbourTile.ParentTile = currentTile;
					validTiles.Add( neighbourTile );
				}
			}
			else {
				neighbourTile.ParentTile = currentTile;
				neighbourTile.State = Tile.TileState.OPEN;
				validTiles.Add( neighbourTile );
			}
		}
		
		return validTiles;
	}
}
