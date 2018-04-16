using UnityEngine;

namespace Mapbox.Map {
	
	/// <summary>
	///   Vector tile retrieved from an alternative data source.
	/// </summary>
	public sealed class GeoServerVectorTile : VectorTile {
		
		private readonly string _gridset;
		
		public GeoServerVectorTile(string gridset)
		{
			_gridset = gridset;
		}

		internal override TileResource MakeTileResource(string mapId) {		
			TileResource tr = TileResource.MakeGeoServerVector(Id, mapId, _gridset);
			// TODO geoAR: remove logging
			Debug.Log(tr.GetUrl());
			return tr;
		}

	}
}