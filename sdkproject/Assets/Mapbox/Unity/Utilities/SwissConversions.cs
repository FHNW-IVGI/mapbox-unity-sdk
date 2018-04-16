using System;
using Mapbox.Map;
using Mapbox.Utils;
using swisstopo.geodesy.gpsref;

namespace Mapbox.Unity.Utilities
{
    
    public static class SwissConversions
    {

	    /// <summary>
	    /// Swiss grid projected bounds; correspond to Gridset bounds as defined in GeoServer EPSG:2056 Gridset
	    /// </summary>
	    static class BoundingBox
	    {
		    public const double xMin = 2485014.052451379 - 2600000;
		    public const double xMax = 2837016.9329778464 - 2600000;
		    public const double yMin = 1074188.6943776922 - 1200000;
		    public const double yMax = 1299782.763494124 - 1200000;
	    }

	    static class OriginLv03
	    {
		    public const int X = 600000;
		    public const int Y = 200000;
	    }

	    private const int TileSize = 256; 
	    // Pixel size for zoom level 0 according to GeoServer EPSG:2056 Gridset definition
	    private const double InitialResolution = 881.2268324860615;
        
        /// <summary>
        /// Converts <see cref="T:Mapbox.Utils.Vector2d"/> struct of WGS84 lat/lon to shifted
        /// CH1903 / LV03 (EPSG:21781) xy meters with origin in (0,0) instead of (600'000, 200'000).
        /// </summary>
        /// <param name="v"> The <see cref="T:Mapbox.Utils.Vector2d"/>. </param>
        /// <returns> A <see cref="T:UnityEngine.Vector2d"/> of coordinates in meters. </returns>
        public static Vector2d LatLonToMeters(Vector2d v)
        {
            return LatLonToMeters(v.x, v.y);
        }
        
        /// <summary>
        /// Converts WGS84 lat/lon to shifted CH1903 / LV03 (EPSG:21781) xy meters
        /// with origin in (0,0) instead of (600'000, 200'000).
        /// </summary>
        /// <param name="lat"> The latitude. </param>
        /// <param name="lon"> The longitude. </param>
        /// <returns> A <see cref="T:UnityEngine.Vector2d"/> of xy meters. </returns>
        public static Vector2d LatLonToMeters(double lat, double lon)
        {
	        // TODO geoAR: use LV95 instead of LV03
	        double east = 0.0, north = 0.0, height = 0.0;
	        ApproxSwissProj.WGS84toLV03(lat, lon, 0.0, ref east, ref north, ref height);
            return new Vector2d(east - OriginLv03.X, north - OriginLv03.Y);
        }
            
        /// <summary>
		/// Converts shifted CH1903 / LV03 (EPSG:21781) in xy meters to WGS84 lat/lon.
		/// Inverse of LatLonToMeters.
		/// </summary>
		/// <param name="m"> A <see cref="T:UnityEngine.Vector2d"/> of coordinates in meters. </param>
		/// <returns> The <see cref="T:Mapbox.Utils.Vector2d"/> in lat/lon. </returns>
		public static Vector2d MetersToLatLon(Vector2d m)
        {
	        // TODO geoAR: use LV95 instead of LV03
	        double lat = 0.0, lng = 0.0, height = 0.0;
	        ApproxSwissProj.LV03toWGS84(m.x + OriginLv03.X, m.y + OriginLv03.Y, 0.0, ref lat, ref lng, ref height);
			return new Vector2d(lat, lng);
		}
	    
	    /// <summary>
	    ///  Converts a shifted CH coordinate (LV03 with (0,0) origin) to a tile identifier. 
	    /// </summary>
	    /// <param name="ch"> CH coordinate. </param>
	    /// <param name="zoom"> Zoom level. </param>
	    /// <returns> The tile identifier. </returns>
	    public static UnwrappedTileId MetersToTileId(Vector2d ch, int zoom)
	    {
		    var px = MetersToPixels(ch, zoom);
		    var tile = PixelsToTile(px); 
		    return new UnwrappedTileId(zoom, tile[0], tile[1]);
	    }
	    
	    /// <summary>
	    /// Gets the tile bounds in shifted CH1903 / LV03 (EPSG:21781) meters from an xy tile ID.
	    /// </summary>
	    /// <param name="unwrappedTileId"> ZXY tile ID. </param>
	    /// <returns> A <see cref="T:UnityEngine.Rect"/> in meters. </returns>	    
	    public static RectD TileBounds(UnwrappedTileId unwrappedTileId)
	    {
		    var min = PixelsToMeters(new Vector2d(unwrappedTileId.X * TileSize, unwrappedTileId.Y * TileSize), unwrappedTileId.Z);
		    var max = PixelsToMeters(new Vector2d((unwrappedTileId.X + 1) * TileSize, (unwrappedTileId.Y + 1) * TileSize), unwrappedTileId.Z);
		    return new RectD(min, max - min);
	    }
	    
	    private static double PixelSize(int zoom)
	    {
		    return InitialResolution / Math.Pow(2, zoom);
	    }
        
	    // x-axis is east-oriented, y-axis is north-oriented 
	    private static Vector2d PixelsToMeters(Vector2d p, int zoom)
	    {
		    var pxSize = PixelSize(zoom);
		    return new Vector2d
		    {
			    x = p.x * pxSize + BoundingBox.xMin,
			    y = p.y * pxSize + BoundingBox.yMin
		    };
	    }

	    private static Vector2d MetersToPixels(Vector2d m, int zoom)
	    {
		    var pxSize = PixelSize(zoom);
		    return new Vector2d
		    {
			    x = (m.x - BoundingBox.xMin) / pxSize,
			    y = (m.y - BoundingBox.yMin) / pxSize
		    };
	    }

	    private static int[] PixelsToTile(Vector2d p)
	    {
		    return new[]{
			    (int) Math.Ceiling(p.x / TileSize) - 1,
			    (int) Math.Ceiling(p.y / TileSize) - 1			    
		    };
	    }

    }
}