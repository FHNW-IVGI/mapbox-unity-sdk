using Mapbox.Unity.Utilities;

namespace Mapbox.Unity.Map
{
	using UnityEngine;
	using Mapbox.Map;

	/// <summary>
	///  RangeTileProvider counterpart for SwissBasicMap.
	/// </summary>
	public class SwissRangeTileProvider : AbstractTileProvider
	{
		[SerializeField]
		private int _west;
		[SerializeField]
		private int _north;
		[SerializeField]
		private int _east;
		[SerializeField]
		private int _south;

		public override void OnInitialized()
		{
			var centerTile = SwissConversions.MetersToTileId(_map.CenterLatitudeLongitude, _map.AbsoluteZoom);
			AddTile(new UnwrappedTileId(_map.AbsoluteZoom, centerTile.X, centerTile.Y));
			for (int x = centerTile.X - _west; x <= centerTile.X + _east; x++)
			{
				// iterate from south to north because y-axis is pointing to north
				for (int y = centerTile.Y - _south; y <= centerTile.Y + _north; y++)
				{
					AddTile(new UnwrappedTileId(_map.AbsoluteZoom, x, y));
				}
			}
		}
	}
}
