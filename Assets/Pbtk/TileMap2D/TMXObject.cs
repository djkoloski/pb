using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pb.Collections;

namespace Pbtk
{
	namespace TileMap2D
	{
		/// <summary>
		/// The possible types of a TMX object
		/// </summary>
		public enum TMXObjectType
		{
			/// <summary>
			/// Indicates a TMX object of no type
			/// <summary>
			None,
			/// <summary>
			/// Indicates a rectangle
			/// </summary>
			Rectangle,
			/// <summary>
			/// Indicates an ellipse
			/// </summary>
			Ellipse,
			/// <summary>
			/// Indicates a polyline
			/// </summary>
			Polyline,
			/// <summary>
			/// Indicates a polygon
			/// </summary>
			Polygon
		}
		/// <summary>
		/// Holds information about a single TMX object
		/// </summary>
		public class TMXObject
		{
			/// <summary>
			/// The name of the object
			/// </summary>
			public string name;
			/// <summary>
			/// The type of the object
			/// </summary>
			public TMXObjectType type;
			/// <summary>
			/// The rotation of the object
			/// </summary>
			public float rotation;
			/// <summary>
			/// The points in the object
			/// </summary>
			public List<IVector2> points;
			/// <summary>
			/// The properties associated with the object
			/// </summary>
			public PropertyMap properties;
			/// <summary>
			/// The position of the object (if any)
			/// </summary>
			public IVector2 position
			{
				get
				{
					if (points.Count > 0)
						return points[0];
					return IVector2.zero;
				}
			}
			/// <summary>
			/// The size of the object (if any)
			/// </summary>
			public IVector2 size
			{
				get
				{
					if (points.Count > 1)
						return points[1];
					return IVector2.zero;
				}
			}

			/// <summary>
			/// Constructor for the TMX object
			/// </summary>
			public TMXObject()
			{
				type = TMXObjectType.None;
				points = new List<IVector2>();
			}
		}
		/// <summary>
		/// Holds information about a TMX object layer and the objects in the layer
		/// </summary>
		public class TMXObjectLayer
		{
			/// <summary>
			/// The name of the object layer
			/// </summary>
			public string name;
			/// <summary>
			/// The default alpha value of objects in the layer
			/// </summary.
			public float default_alpha;
			/// <summary>
			/// The properties associated with the layer
			/// </summary>
			public PropertyMap properties;
			/// <summary>
			/// The objects in the layer
			/// </summary>
			public List<TMXObject> objects;

			/// <summary>
			/// Constructor for the TMX object layer
			/// </summary>
			public TMXObjectLayer()
			{
				name = null;
				default_alpha = 0.0f;
				properties = null;
				objects = new List<TMXObject>();
			}
		}
	}
}
