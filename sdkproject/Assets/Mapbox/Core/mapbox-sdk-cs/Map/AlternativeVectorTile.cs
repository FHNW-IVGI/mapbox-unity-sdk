namespace Mapbox.Map {
	
	/// <summary>
	///   Vector tile retrieved from an alternative data source.
	/// </summary>
	public sealed class AlternativeVectorTile : VectorTile {

		internal override TileResource MakeTileResource(string mapId) {
			
			TileResource tr = TileResource.MakeVector(Id, mapId, true);
			return tr;
		}

	}
}