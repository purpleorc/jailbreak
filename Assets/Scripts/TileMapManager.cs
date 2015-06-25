using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Text;

public class TileCoordinate {
	public int x { get; set; }
	public int z { get; set; }

	public TileCoordinate( int x, int z ) {
		this.x = x;
		this.z = z;
	}
}

public class Tile {
	
	public enum TileState { 
		UNTESTED,
		OPEN,
		CLOSED
	}
	
	public bool IsBlocked { 
		get { 
			return TileType == TileMapManager.TileType.BUILDING;
		}
	}
	
	public float G { get; private set; }
	public float H { get; private set; }
	public float F { get { return this.G + this.H; } }
	public TileState State { get; set; }

	private Tile _parentTile = null;
	public Tile ParentTile {
		get {
			return _parentTile;
		}
		set {
			_parentTile = value;
			G = _parentTile.G + GetTraversalCost( this, this._parentTile );
		}
	}

	public TileCoordinate Coordinate { get; private set; }

	public TileMapManager.TileType TileType { get; private set; }

	private List<Tile> _neighbours = null;
	public List<Tile> Neighbours {
		get {
			if ( _neighbours == null ) {
				_neighbours = new List<Tile>();

				if ( Coordinate.x + 1 < TileMapManager.TILE_MAP_WIDTH ) {
					_neighbours.Add( TileMapManager.Instance.GetTileAtCoordinate( Coordinate.x + 1, Coordinate.z ) );
				}
				if ( Coordinate.x - 1 >= 0 ) {
					_neighbours.Add( TileMapManager.Instance.GetTileAtCoordinate( Coordinate.x - 1, Coordinate.z ) );
				}
				if ( Coordinate.z + 1 < TileMapManager.TILE_MAP_LENGTH ) {
					_neighbours.Add( TileMapManager.Instance.GetTileAtCoordinate( Coordinate.x, Coordinate.z + 1 ) );
				}
				if ( Coordinate.z - 1 >= 0 ) {
					_neighbours.Add( TileMapManager.Instance.GetTileAtCoordinate( Coordinate.x, Coordinate.z - 1 ) );
				}
			}
			return _neighbours;
		}
	}

	private Tile( TileMapManager.TileType type, TileCoordinate coordinate ) {
		TileType = type;
		Coordinate = coordinate;
	}

	public void Reset( Tile targetTile ) {
		State = TileState.UNTESTED;
		H = GetTraversalCost( this, targetTile );
		G = 0;
	}

	public static Tile CreateRoadTile( int x, int z ) {
		TileCoordinate coordinate = new TileCoordinate( x, z );
		return new Tile( TileMapManager.TileType.ROAD, coordinate );
	}

	public static Tile CreateBuildingTile( int x, int z ) {
		TileCoordinate coordinate = new TileCoordinate( x, z );
		return new Tile( TileMapManager.TileType.BUILDING, coordinate );
	}

	public static float GetTraversalCost( Tile tileA, Tile tileB ) {
		float deltaX = tileA.Coordinate.x - tileB.Coordinate.x;
		float deltaZ = tileA.Coordinate.z - tileB.Coordinate.z;

		float distance = (float)Math.Sqrt( deltaX * deltaX + deltaZ * deltaZ );
		return distance;
	}
}

public class TileMapManager : MonoBehaviour {

	public enum TileType {
		ROAD,
		BUILDING
	}

	public const int TILE_MAP_WIDTH = 10;
	public const int TILE_MAP_LENGTH = 15;

	private const int X_DIMENSION = 0;
	private const int Z_DIMENSION = 1;

	private Tile[,] _tileMap = new Tile[ TILE_MAP_WIDTH, TILE_MAP_LENGTH ];
	
	public static TileMapManager Instance { get; private set; }

	private void Awake() {
		Instance = this;
	}

	private void Start() {
		Initialize();
	}

	private void Initialize() {
		// for now make everything a road
		for ( int x = 0, length = _tileMap.GetLength( X_DIMENSION ); x < length; x++ ) {
			for ( int z = 0, longLength = _tileMap.GetLength( Z_DIMENSION ); z < longLength; z++ ) {
				_tileMap[ x, z ] = Tile.CreateRoadTile( x, z );
			}
		}

		DebugDraw();

		TileCoordinate start = new TileCoordinate( 0, 2 );
		TileCoordinate goal = new TileCoordinate( 6, 12 );

		List<TileCoordinate> path = PathFinder.Instance.FindPath( start, goal );

		DebugDraw( path );
	}

	public Tile GetTileAtCoordinate( int x, int z ) {
		return _tileMap[ x, z ];
	}

	public Tile GetTileAtCoordinate( TileCoordinate coordinate ) {
		return _tileMap[ coordinate.x, coordinate.z ];
	}

	public void ResetTiles( Tile target ) {
		for ( int x = 0, length = _tileMap.GetLength( X_DIMENSION ); x < length; x++ ) {
			for ( int z = 0, longLength = _tileMap.GetLength( Z_DIMENSION ); z < longLength; z++ ) {
				_tileMap[ x, z ].Reset( target );
			}
		}
	}

	private void DebugDraw( List<TileCoordinate> path = null ) {
		StringBuilder sb = new StringBuilder();
		for ( int x = 0, length = _tileMap.GetLength( X_DIMENSION ); x < length; x++ ) {
			for ( int z = 0, longLength = _tileMap.GetLength( Z_DIMENSION ); z < longLength; z++ ) {
				Tile tile = _tileMap[ x, z ];

				if ( path != null ) {
					bool found = false;
					for ( int i = 0, count = path.Count; i < count; i++ ) {
						TileCoordinate coord = path[ i ];
						if ( coord.x == x && coord.z == z ) {
							found = true;
							sb.Append( "*" );
							break;
						}
					}

					if ( found ) {
						continue;
					}
				}

				if ( tile.TileType == TileType.ROAD ) { 
					sb.Append( ". " );
				} else if ( tile.TileType == TileType.BUILDING ) {
					sb.Append( "#" );
				}
			}

			sb.AppendLine();
		}

		Debug.Log ( sb.ToString() );
	}
}
