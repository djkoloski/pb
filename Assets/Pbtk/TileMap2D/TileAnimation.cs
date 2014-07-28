using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pb.Collections;

namespace Pbtk
{
	namespace TileMap2D
	{
		/// <summary>
		/// A simple, frame-by-frame animation for tiles
		/// </summary>
		[System.Serializable]
		public class TileAnimation
		{
			/// <summary>
			/// A map class for the frames of the animation
			/// </summary>
			[System.Serializable]
			public class FrameMap :
				Map<int, int>
			{ }
			/// <summary>
			/// A map of the frames
			/// </summary>
			public FrameMap frames;
			/// <summary>
			/// The total length of the animation
			/// </summary>
			public int length;
			/// <summary>
			/// Constructs the animation
			/// </summary>
			public TileAnimation()
			{
				frames = new FrameMap();
				length = 0;
			}
			/// <summary>
			/// Adds a new frame to the animation with the given ID and duration
			/// </summary>
			/// <param name="id">The ID of the tile to display</param>
			/// <param name="duration">The duration to display the tile</param>
			public void AddFrame(int id, int duration)
			{
				frames[length] = id;
				length += duration;
			}
			/// <summary>
			/// Gets the current ID of the tile at the given time in milliseconds
			/// </summary>
			/// <param name="milliseconds">The milliseconds into the animation to find the ID for</param>
			/// <returns>The ID of the tile currently displayed</returns>
			public int GetFrameID(int milliseconds)
			{
				milliseconds %= length;
				int nearest = frames.Keys.BinarySearch(milliseconds);
				if (nearest >= 0)
					return frames.Values[nearest];
				nearest = ~nearest;
				if (nearest <= 0)
					throw new System.InvalidOperationException();
				return frames.Values[nearest - 1];
			}
		}
	}
}