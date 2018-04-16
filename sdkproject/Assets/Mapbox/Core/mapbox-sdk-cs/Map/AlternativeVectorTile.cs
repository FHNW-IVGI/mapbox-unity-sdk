using Mapbox.Utils;
using UnityEngine;

namespace Mapbox.Map {
	
	/// <summary>
	///   Vector tile retrieved from an alternative data source.
	/// </summary>
	public sealed class AlternativeVectorTile : VectorTile {
		// TODO geoAR: rename to GeoServerVT

		internal override TileResource MakeTileResource(string mapId) {		
			TileResource tr = TileResource.MakeGeoServerVector(Id, mapId, Constants.WebMercatorGridset);
			// TODO geoAR: remove logging
			Debug.Log(tr.GetUrl());
			return tr;
		}

	}
}