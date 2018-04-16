//-----------------------------------------------------------------------
// <copyright file="VectorTile.cs" company="Mapbox">
//     Copyright (c) 2016 Mapbox. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using UnityEngine;

namespace Mapbox.Map {

	/// <summary>
	///   Vector tile retrieved from an alternative data source.
	/// </summary>
	public sealed class AlternativeVectorTile : VectorTile {

		internal override TileResource MakeTileResource(string mapId) {
			
			TileResource tr = TileResource.MakeVector(Id, mapId, true);
			// TODO geoAR: remove logging
			Debug.Log(tr.GetUrl());
			return tr;
		}

	}
}