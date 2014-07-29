using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

namespace Pbtk
{
	namespace TileMap2D
	{
		/// <summary>
		/// Manages and displays GUI for tile map controllers
		/// </summary>
		[CustomEditor(typeof(TileMapController))]
		public class TileMapControllerEditor :
			Editor
		{
			/// <summary>
			/// The tabs in the inspector
			/// </summary>
			public static string[] tabs = new string[]{"Chunks", "Aesthetic"};
			/// <summary>
			/// The controller being edited
			/// </summary>
			public TileMapController controller = null;
			/// <summary>
			/// The currently-selected tab in the inspector
			/// </summary>
			public int tab = 0;
			/// <summary>
			/// Whether the cursor is on the map
			/// </summary>
			public bool on_map = false;
			/// <summary>
			/// The X coordinate of the tile under the cursor
			/// </summary>
			public int tile_x = 0;
			/// <summary>
			/// The Y coordinate of the tile under the cursor
			/// </summary>
			public int tile_y = 0;
			/// <summary>
			/// The X coordinate of the chunk under the cursor
			/// </summary>
			public int chunk_x
			{
				get
				{
					if (chunk_manager != null)
						return Pb.Math.FloorDivide(tile_x, chunk_manager.chunk_size_x);
					return 0;
				}
			}
			/// <summary>
			/// The Y coordinate of the chunk under the cursor
			/// </summary>
			public int chunk_y
			{
				get
				{
					if (chunk_manager != null)
						return Pb.Math.FloorDivide(tile_y, chunk_manager.chunk_size_y);
					return 0;
				}
			}
			/// <summary>
			/// The tile map of the controller being edited
			/// </summary>
			public TileMap tile_map
			{
				get
				{
					if (controller == null)
						return null;
					if (controller.tile_map == null)
						return null;
					return controller.tile_map as TileMap;
				}
			}
			/// <summary>
			/// The chunk manager of the controller being edited
			/// </summary>
			public ChunkManager2D chunk_manager
			{
				get
				{
					if (tile_map == null)
						return null;
					if (controller.tile_map.chunk_manager == null)
						return null;
					return controller.tile_map.chunk_manager as ChunkManager2D;
				}
			}
			/// <summary>
			/// Gets the chunk manager as a static chunk manager (if able)
			/// </summary>
			public StaticChunkManager static_chunk_manager
			{
				get
				{
					return (chunk_manager as StaticChunkManager);
				}
			}
			/// <summary>
			/// Gets the chunk manager as a dynamic chunk generator (if able)
			/// </summary>
			public DynamicChunkGenerator dynamic_chunk_generator
			{
				get
				{
					return (chunk_manager as DynamicChunkGenerator);
				}
			}
			/// <summary>
			/// Determines whether the chunk manager is a static chunk manager
			/// </summary>
			public bool is_static_chunk_manager
			{
				get
				{
					return static_chunk_manager != null;
				}
			}
			/// <summary>
			/// Determines whether the chunk manager is a dynamic chunk generator
			/// </summary>
			public bool is_dynamic_generator
			{
				get
				{
					return dynamic_chunk_generator != null;
				}
			}
			/// <summary>
			/// Sets the controller when it changes
			/// </summary>
			public void OnEnable()
			{
				controller = target as TileMapController;
			}
			/// <summary>
			/// Displays the inspector GUI
			/// </summary>
			public override void OnInspectorGUI()
			{
				if (controller == null)
					return;

				EditorGUILayout.BeginHorizontal();
				controller.tile_map = EditorGUILayout.ObjectField("Tile map", controller.tile_map, typeof(TileMap), false) as TileMap;
				EditorGUILayout.EndHorizontal();

				EditorGUILayout.BeginHorizontal();
				tab = GUILayout.SelectionGrid(tab, tabs, tabs.Length);
				EditorGUILayout.EndHorizontal();

				switch (tabs[tab])
				{
					case "Chunks":
						if (chunk_manager == null)
						{
							EditorGUILayout.BeginHorizontal();
							EditorGUILayout.LabelField("No chunk manager found");
							EditorGUILayout.EndHorizontal();
							break;
						}

						EditorGUILayout.BeginHorizontal();
						EditorGUILayout.LabelField("Chunk size:", chunk_manager.chunk_size_x + " x " + chunk_manager.chunk_size_y + " tiles");
						EditorGUILayout.EndHorizontal();

						EditorGUILayout.BeginHorizontal();
						EditorGUILayout.LabelField("Chunk range:", "(" + chunk_manager.chunk_left + ", " + chunk_manager.chunk_bottom + ") - (" + chunk_manager.chunk_right + ", " + chunk_manager.chunk_top + ")");
						EditorGUILayout.EndHorizontal();

						if (is_static_chunk_manager)
						{
							EditorGUILayout.BeginHorizontal();
							EditorGUILayout.LabelField("Resources path:", static_chunk_manager.resources_path);
							EditorGUILayout.EndHorizontal();
						}

						break;
					case "Aesthetic":
						EditorGUILayout.BeginHorizontal();
						controller.when_draw_gizmos = (TileMapController.GizmosDrawTime)EditorGUILayout.EnumPopup("When to draw", controller.when_draw_gizmos);
						EditorGUILayout.EndHorizontal();

						EditorGUILayout.BeginHorizontal();
						controller.draw_tile_boundaries = EditorGUILayout.BeginToggleGroup("Draw tiles", controller.draw_tile_boundaries);
						controller.gizmo_color_tile = EditorGUILayout.ColorField(controller.gizmo_color_tile);
						EditorGUILayout.EndToggleGroup();
						EditorGUILayout.EndHorizontal();

						EditorGUILayout.BeginHorizontal();
						controller.draw_chunk_boundaries = EditorGUILayout.BeginToggleGroup("Draw chunks", controller.draw_chunk_boundaries);
						controller.gizmo_color_chunk = EditorGUILayout.ColorField(controller.gizmo_color_chunk);
						EditorGUILayout.EndToggleGroup();
						EditorGUILayout.EndHorizontal();
						break;
					default:
						Debug.Log("Invalid tab!");
						break;
				}
			}
			/// <summary>
			/// A simple class that represents a context command
			/// </summary>
			public class ContextCommand
			{
				/// <summary>
				/// The name of the context command
				/// </summary>
				public string name;
				/// <summary>
				/// Arguments to the context command
				/// </summary>
				public object[] args;
				/// <summary>
				/// Basic constructor
				/// </summary>
				/// <param name="n">The name of the context command</param>
				/// <param name="a">The arguments to the context command</param>
				public ContextCommand(string n, object[] a)
				{
					name = n;
					args = a;
				}
			}
			/// <summary>
			/// Executes a context command
			/// </summary>
			/// <param name="command_arg">The context command to execute</param>
			public void ExecuteContext(object command_arg)
			{
				ContextCommand command = command_arg as ContextCommand;

				switch (command.name)
				{
					case "ping_chunk":
						EditorGUIUtility.PingObject(chunk_manager.GetChunk((int)command.args[0], (int)command.args[1], 0));
						break;
					case "begin_editing":
						controller.Begin();
						break;
					case "end_editing":
						controller.End();
						break;
					case "load_chunk":
						controller.LoadChunk((int)command.args[0], (int)command.args[1], 0);
						break;
					case "unload_chunk":
						controller.UnloadChunk((int)command.args[0], (int)command.args[1], 0);
						break;
					case "load_all_chunks":
						for (int x = chunk_manager.chunk_left; x <= chunk_manager.chunk_right; ++x)
							for (int y = chunk_manager.chunk_bottom; y <= chunk_manager.chunk_top; ++y)
								controller.LoadChunk(x, y, 0);
						break;
					case "unload_all_chunks":
						for (int x = chunk_manager.chunk_left; x <= chunk_manager.chunk_right; ++x)
							for (int y = chunk_manager.chunk_bottom; y <= chunk_manager.chunk_top; ++y)
								controller.UnloadChunk(x, y, 0);
						break;
					case "get_tile_id":
						Debug.Log(controller.GetTile((int)command.args[0], (int)command.args[1], 0));
						break;
					case "reset_generator":
						dynamic_chunk_generator.Reset();
						break;
					default:
						throw new System.ArgumentException("Invalid context command argument '" + command.name + "'");
				}
			}
			/// <summary>
			/// Draws scene GUI and creates context menus if the space bar is pressed
			/// </summary>
			public void OnSceneGUI()
			{
				if (!controller.initialized)
					controller.Init();
				if (tile_map == null)
					return;

				Vector3 mouse_pos = new Vector3();
				on_map = Pb.Utility.Projection.ProjectMousePosition(Vector3.forward, controller.transform, out mouse_pos);

				bool needs_repaint = false;
				if (on_map)
				{
					int old_x = tile_x;
					int old_y = tile_y;
					int dummy_z = 0;

					mouse_pos = controller.transform.worldToLocalMatrix.MultiplyPoint(mouse_pos);
					mouse_pos = tile_map.geometry.mapToNormalMatrix.MultiplyPoint(mouse_pos);

					tile_map.geometry.NormalToTile(mouse_pos, out tile_x, out tile_y, out dummy_z);
					
					if (old_x != tile_x || old_y != tile_y)
						needs_repaint = true;
				}

				Handles.BeginGUI();
				GUI.Label(new Rect(10, Screen.height - 60, 200, 20), "Tile: " + tile_x + ", " + tile_y);
				GUI.Label(new Rect(10, Screen.height - 80, 200, 20), "Chunk: " + chunk_x + ", " + chunk_y);
				Handles.EndGUI();

				Event current = Event.current;

				if (current.type == EventType.KeyDown && current.keyCode == KeyCode.Space && on_map)
				{
					GenericMenu menu = new GenericMenu();

					if (!controller.ready)
						menu.AddItem(new GUIContent("Begin editing"), false, ExecuteContext, new ContextCommand("begin_editing", null));
					else
					{
						if (chunk_manager == null)
							menu.AddDisabledItem(new GUIContent("No chunk manager"));
						else
						{
							if (chunk_x >= chunk_manager.chunk_left && chunk_x <= chunk_manager.chunk_right && chunk_y >= chunk_manager.chunk_bottom && chunk_y <= chunk_manager.chunk_top)
							{
								if (controller.IsChunkLoaded(chunk_x, chunk_y))
								{
									menu.AddItem(new GUIContent("Unload chunk"), false, ExecuteContext, new ContextCommand("unload_chunk", new object[2] { chunk_x, chunk_y }));

									menu.AddItem(new GUIContent("Print tile ID"), false, ExecuteContext, new ContextCommand("get_tile_id", new object[2] { tile_x, tile_y }));
								}
								else
									menu.AddItem(new GUIContent("Load chunk"), false, ExecuteContext, new ContextCommand("load_chunk", new object[2] { chunk_x, chunk_y }));

								menu.AddItem(new GUIContent("Ping chunk"), false, ExecuteContext, new ContextCommand("ping_chunk", new object[2] { chunk_x, chunk_y }));

								menu.AddSeparator("");
							}

							menu.AddItem(new GUIContent("Chunks/Load all chunks"), false, ExecuteContext, new ContextCommand("load_all_chunks", null));

							menu.AddItem(new GUIContent("Chunks/Unload all chunks"), false, ExecuteContext, new ContextCommand("unload_all_chunks", null));
						}
					}

					if (is_dynamic_generator)
					{
						menu.AddSeparator("");
						menu.AddItem(new GUIContent("Reset generator"), false, ExecuteContext, new ContextCommand("reset_generator", null));
					}

					if (controller.ready)
					{
						menu.AddSeparator("");
						menu.AddItem(new GUIContent("End editing"), false, ExecuteContext, new ContextCommand("end_editing", null));
					}

					menu.ShowAsContext();

					current.Use();
				}

				if (needs_repaint)
					SceneView.lastActiveSceneView.Repaint();
			}
		}
	}
}