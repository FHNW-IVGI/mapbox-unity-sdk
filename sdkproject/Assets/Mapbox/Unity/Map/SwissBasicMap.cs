using Mapbox.Unity.Utilities;
using Mapbox.Utils;
using UnityEngine;

namespace Mapbox.Unity.Map
{
    /// <summary>
    /// Basic Map counterpart that uses Swiss grid instead of Web Mercator.
    /// _centerLatitudeLongitude is Swiss grid meters, not lat/lon.
    /// </summary>
    /// 
    public class SwissBasicMap : AbstractMap
    {
        public override void Initialize(Vector2d latLon, int zoom)
        {
            _worldHeightFixed = false;
            // Swiss grid meters, not lat/lag
            _centerLatitudeLongitude = SwissConversions.LatLonToMeters(latLon);
            _zoom = zoom;
            _initialZoom = zoom;

            var referenceTileRect =
                SwissConversions.TileBounds(SwissConversions.MetersToTileId(_centerLatitudeLongitude, AbsoluteZoom));
            _centerMercator = referenceTileRect.Center;

            _worldRelativeScale = (float) (_unityTileSize / referenceTileRect.Size.x);
            _mapVisualizer.Initialize(this, _fileSource);
            _tileProvider.Initialize(this);
            
            // TODO geoAR: remove logging
            Debug.Log("Center lat/lon: " + _centerLatitudeLongitude);
            Debug.Log("Center Mercator: " + _centerMercator);
            Debug.Log("Reference tile: min=" + referenceTileRect.Min + ", max=" + referenceTileRect.Max + ", size="+ referenceTileRect.Size);
            Debug.Log("Relative scale: " + _worldRelativeScale);

            SendInitialized();
        }
    }
}