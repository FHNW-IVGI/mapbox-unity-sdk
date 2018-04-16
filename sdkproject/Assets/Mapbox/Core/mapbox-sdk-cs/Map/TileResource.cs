//-----------------------------------------------------------------------
// <copyright file="TileResource.cs" company="Mapbox">
//     Copyright (c) 2016 Mapbox. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

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

		public static TileResource MakeVector(CanonicalTileId id, string mapId, bool alternative=false)
		{			
			// GeoServer: {http://localhost:8580/geoserver/gwc/service/tms/1.0.0/test:linesV} @EPSG%3A2056@pbf/ {2/0/0}.pbf
			// Mapbox: {https://api.mapbox.com/v4/mschoenhozer.9bdivm08} / {9/266/180} .vector.pbf
			// TODO geoAR: do not hardcode gridset id
			
			return alternative ? 
				new TileResource(string.Format("{0}@EPSG%3A2056@pbf/{1}.pbf", MapUtils.MapIdToUrl(mapId, true), id)) :
				new TileResource(string.Format("{0}/{1}.vector.pbf", MapUtils.MapIdToUrl(mapId ?? "mapbox.mapbox-streets-v7"), id));
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
