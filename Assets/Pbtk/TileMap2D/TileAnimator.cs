using UnityEngine;

namespace Pbtk
{
	namespace TileMap2D
	{
		/// <summary>
		/// Animates a tile map tile
		/// </summary>
		public class TileAnimator :
			MonoBehaviour
		{
			/// <summary>
			/// The tile set the animation is from
			/// </summary>
			public TileSet tile_set;
			/// <summary>
			/// The tile animation
			/// </summary>
			public new TileAnimation animation;
			/// <summary>
			/// The sprite renderer
			/// </summary>
			public SpriteRenderer sprite_renderer;
			/// <summary>
			/// Gets the sprite renderer
			/// </summary>
			public void Awake()
			{
				sprite_renderer = GetComponent<SpriteRenderer>();
			}
			/// <summary>
			/// Changes the sprite to match the current ID's sprite
			/// </summary>
			public void Update()
			{
				int milliseconds = Mathf.FloorToInt(Time.time * 1000);
				int id = animation.GetFrameID(milliseconds);
				sprite_renderer.sprite = tile_set.GetTile<TileInfo>(id).sprite;
			}
		}
	}
}