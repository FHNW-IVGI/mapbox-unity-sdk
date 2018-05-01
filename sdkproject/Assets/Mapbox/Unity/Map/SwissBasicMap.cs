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
            // Swiss grid meters (east/north), not lat/lon
            _centerLatitudeLongitude = SwissConversions.LatLonToMeters(latLon);
            _zoom = zoom;
            _initialZoom = zoom;

            var referenceTileRect =
                SwissConversions.TileBounds(SwissConversions.MetersToTileId(_centerLatitudeLongitude, AbsoluteZoom));
            _centerMercator = referenceTileRect.Center;

            _worldRelativeScale = (float) (_unityTileSize / referenceTileRect.Size.x);

            // TODO geoAR: remove logging
            var rootVector2d = (_centerMercator - _centerLatitudeLongitude) * _worldRelativeScale;
            var rootPosition = rootVector2d.ToVector3xz();
            Debug.Log("Root shifted position: " + rootPosition + "; zoom level: " + _zoom);

            _mapVisualizer.Initialize(this, _fileSource);
            _tileProvider.Initialize(this);

            SendInitialized();
        }
    }
}