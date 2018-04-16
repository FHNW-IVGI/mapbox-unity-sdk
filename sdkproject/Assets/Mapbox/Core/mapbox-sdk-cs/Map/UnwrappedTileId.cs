//-----------------------------------------------------------------------
// <copyright file="UnwrappedTileId.cs" company="Mapbox">
//     Copyright (c) 2016 Mapbox. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Mapbox.Map
{
	/// <summary>
	///     Unwrapped tile identifier in a slippy map. Similar to <see cref="CanonicalTileId"/>,
	///     but might go around the globe.
	/// </summary>
	public struct UnwrappedTileId
	{
		/// <summary> The zoom level. </summary>
		public readonly int Z;

		/// <summary> The X coordinate in the tile grid. </summary>
		public readonly int X;

		/// <summary> The Y coordinate in the tile grid. </summary>
		public readonly int Y;
	
		/// <summary> The Y coordinate in the tile grid. </summary>
		public readonly bool FlipY;

		/// <summary>
		///     Initializes a new instance of the <see cref="UnwrappedTileId"/> struct,
		///     representing a tile coordinate in a slippy map that might go around the
		///     globe.
		/// </summary>
		/// <param name="z">The z coordinate.</param>
		/// <param name="x">The x coordinate.</param>
		/// <param name="y">The y coordinate.</param>
		/// <param name="flipY">Flip y axis.</param>
		public UnwrappedTileId(int z, int x, int y, bool flipY=false)
		{
			this.Z = z;
			this.X = x;
			this.Y = y;
			this.FlipY = flipY;
		}

		/// <summary> Gets the canonical tile identifier. </summary>
		/// <value> The canonical tile identifier. </value>
		public CanonicalTileId Canonical
		{
			get
			{
				return new CanonicalTileId(this);
			}
		}

		/// <summary>
		///     Returns a <see cref="T:System.String"/> that represents the current
		///     <see cref="T:Mapbox.Map.UnwrappedTileId"/>.
		/// </summary>
		/// <returns>
		///     A <see cref="T:System.String"/> that represents the current
		///     <see cref="T:Mapbox.Map.UnwrappedTileId"/>.
		/// </returns>
		public override string ToString()
		{
			return this.Z + "/" + this.X + "/" + this.Y;
		}

		public UnwrappedTileId North
		{
			get
			{
				var one = FlipY ? 1 : -1;
				return new UnwrappedTileId(Z, X, Y + one, FlipY);
			}
		}

		public UnwrappedTileId East
		{
			get
			{
				return new UnwrappedTileId(Z, X + 1, Y, FlipY);
			}
		}

		public UnwrappedTileId South
		{
			get
			{
				var one = FlipY ? -1 : 1;
				return new UnwrappedTileId(Z, X, Y + one, FlipY);
			}
		}

		public UnwrappedTileId West
		{
			get
			{
				return new UnwrappedTileId(Z, X - 1, Y, FlipY);
			}
		}

		public UnwrappedTileId NorthEast
		{
			get
			{
				var one = FlipY ? 1 : -1;
				return new UnwrappedTileId(Z, X + 1, Y + one, FlipY);
			}
		}

		public UnwrappedTileId SouthEast
		{
			get
			{
				var one = FlipY ? -1 : 1;
				return new UnwrappedTileId(Z, X + 1, Y + one, FlipY);
			}
		}

		public UnwrappedTileId NorthWest
		{
			get
			{
				var one = FlipY ? 1 : -1;
				return new UnwrappedTileId(Z, X - 1, Y + one, FlipY);
			}
		}

		public UnwrappedTileId SouthWest
		{
			get
			{
				var one = FlipY ? -1 : 1;
				return new UnwrappedTileId(Z, X - 1, Y + one, FlipY);
			}
		}
	}
}
