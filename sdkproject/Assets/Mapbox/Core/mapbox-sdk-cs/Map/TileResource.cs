//-----------------------------------------------------------------------
// <copyright file="TileResource.cs" company="Mapbox">
//     Copyright (c) 2016 Mapbox. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using Mapbox.Utils;
using UnityEngine;

namespace Mapbox.Map
{
	using Platform;
	using System;
	using Mapbox.Unity.Telemetry;

	internal sealed class TileResource : IResource
	{
		readonly string _query;

		internal TileResource(string query)
		{
			_query = query;
		}

		public static TileResource MakeRaster(CanonicalTileId id, string styleUrl)
		{
			return new TileResource(string.Format("{0}/{1}", MapUtils.NormalizeStaticStyleURL(styleUrl ?? "mapbox://styles/mapbox/satellite-v9"), id));
		}

		internal static TileResource MakeRetinaRaster(CanonicalTileId id, string styleUrl)
		{
			return new TileResource(string.Format("{0}/{1}@2x", MapUtils.NormalizeStaticStyleURL(styleUrl ?? "mapbox://styles/mapbox/satellite-v9"), id));
		}

		public static TileResource MakeClassicRaster(CanonicalTileId id, string mapId)
		{
			return new TileResource(string.Format("{0}/{1}.png", MapUtils.MapIdToUrl(mapId ?? "mapbox.satellite"), id));
		}

		internal static TileResource MakeClassicRetinaRaster(CanonicalTileId id, string mapId)
		{
			return new TileResource(string.Format("{0}/{1}@2x.png", MapUtils.MapIdToUrl(mapId ?? "mapbox.satellite"), id));
		}

		public static TileResource MakeRawPngRaster(CanonicalTileId id, string mapId)
		{
			return new TileResource(string.Format("{0}/{1}.pngraw", MapUtils.MapIdToUrl(mapId ?? "mapbox.terrain-rgb"), id));
		}

		public static TileResource MakeVector(CanonicalTileId id, string mapId)
		{			
			// Mapbox URL format: {https://api.mapbox.com/v4/map_id} / {9/266/180} .vector.pbf
			return new TileResource(string.Format("{0}/{1}.vector.pbf", MapUtils.MapIdToUrl(mapId ?? "mapbox.mapbox-streets-v7"), id));
		}
		
		public static TileResource MakeGeoServerVector(CanonicalTileId id, string mapId, string gridset)
		{			
			// GeoServer URL format: {http://host:port/geoserver/gwc/service/tms/1.0.0/layer_name} @ {gridset_name} @pbf/ {2/0/0}.pbf
			if (Constants.WebMercatorGridset.Equals(gridset))
			{
				// flip y-axis in case of WebMercator projection
				var y = (int) Math.Pow(2, id.Z) - id.Y - 1;
				id = new CanonicalTileId(id.Z, id.X, y);	
			}
			
			var url = string.Format("{0}@{1}@pbf/{2}.pbf", MapUtils.MapIdToUrl(mapId, true), WWW.EscapeURL(gridset), id);
			return new TileResource(url);
		}

		internal static TileResource MakeStyleOptimizedVector(CanonicalTileId id, string mapId, string optimizedStyleId, string modifiedDate)
		{
			return new TileResource(string.Format("{0}/{1}.vector.pbf?style={2}@{3}", MapUtils.MapIdToUrl(mapId ?? "mapbox.mapbox-streets-v7"), id, optimizedStyleId, modifiedDate));
		}

		public string GetUrl()
		{
			var uriBuilder = new UriBuilder(_query);
			if (uriBuilder.Query != null && uriBuilder.Query.Length > 1)
			{
				uriBuilder.Query = uriBuilder.Query.Substring(1) + "&" + TelemetryFactory.EventQuery;
			}
			else
			{
				uriBuilder.Query = TelemetryFactory.EventQuery;
			}

			return uriBuilder.ToString();
		}
	}
}
