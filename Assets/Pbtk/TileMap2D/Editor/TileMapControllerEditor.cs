using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using Pb.Collections;

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
			/// The coordinates of the tile under the cursor
			/// </summary>
			public IVector2 tile = IVector2.zero;
			/// <summary>
			/// The coordinates of the chunk under the cursor
			/// </summary>
			public IVector2 chunk
			{
				get
				{
					if (chunk_manager != null)
						return Pb.Math.FloorDivide(tile, chunk_manager.chunk_size);
					return IVector2.zero;
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
				Pb.Utility.Undo.RegisterChange<Pb.TileMap.TileMap>(
					EditorGUILayout.ObjectField("Tile map", controller.tile_map, typeof(TileMap), false) as Pb.TileMap.TileMap,
					ref controller.tile_map, controller,
					"Changed tile map");
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
						EditorGUILayout.LabelField("Chunk size:", chunk_manager.chunk_size.x + " x " + chunk_manager.chunk_size.y + " tiles");
						EditorGUILayout.EndHorizontal();

						EditorGUILayout.BeginHorizontal();
						EditorGUILayout.LabelField("Chunk range:", "(" + chunk_manager.chunk_least.x + ", " + chunk_manager.chunk_least.y + ") - (" + chunk_manager.chunk_greatest.x + ", " + chunk_manager.chunk_greatest.y + ")");
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
						Pb.Utility.Undo.RegisterChange<TileMapController.GizmosDrawTime>(
							(TileMapController.GizmosDrawTime)EditorGUILayout.EnumPopup("When to draw", controller.when_draw_gizmos),
							ref controller.when_draw_gizmos, controller,
							"Changed gizmos draw condition");
						EditorGUILayout.EndHorizontal();

						EditorGUILayout.BeginHorizontal();
						Pb.Utility.Undo.RegisterChange<bool>(
							EditorGUILayout.BeginToggleGroup("Draw tiles", controller.draw_tile_boundaries),
							ref controller.draw_tile_boundaries, controller,
							"Changed whether to draw tile boundaries");
						Pb.Utility.Undo.RegisterChange<Color32>(
							EditorGUILayout.ColorField(controller.gizmo_color_tile),
							ref controller.gizmo_color_tile, controller,
							"Changed tile boundary draw color");
						EditorGUILayout.EndToggleGroup();
						EditorGUILayout.EndHorizontal();

						EditorGUILayout.BeginHorizontal();
						Pb.Utility.Undo.RegisterChange<bool>(
							EditorGUILayout.BeginToggleGroup("Draw chunks", controller.draw_chunk_boundaries),
							ref controller.draw_chunk_boundaries, controller,
							"Changed whether to draw chunk boundaries");
						Pb.Utility.Undo.RegisterChange<Color32>(
							EditorGUILayout.ColorField(controller.gizmo_color_chunk),
							ref controller.gizmo_color_chunk, controller,
							"Changed chunk boundary draw color");
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
						EditorGUIUtility.PingObject(controller.GetChunk((IVector3)command.args[0]));
						break;
					case "begin_editing":
						controller.Begin();
						break;
					case "end_editing":
						controller.End();
						break;
					case "load_chunk":
						controller.LoadAndRenderChunk((IVector3)command.args[0]);
						break;
					case "unload_chunk":
						controller.UnloadAndUnrenderChunk((IVector3)command.args[0]);
						break;
					case "load_all_chunks":
						for (int x = chunk_manager.chunk_least.x; x <= chunk_manager.chunk_greatest.x; ++x)
							for (int y = chunk_manager.chunk_least.y; y <= chunk_manager.chunk_greatest.y; ++y)
								controller.LoadAndRenderChunk((IVector3)(new IVector2(x, y)));
						break;
					case "unload_all_chunks":
						for (int x = chunk_manager.chunk_least.x; x <= chunk_manager.chunk_greatest.x; ++x)
							for (int y = chunk_manager.chunk_least.y; y <= chunk_manager.chunk_greatest.y; ++y)
								controller.UnloadAndUnrenderChunk((IVector3)(new IVector2(x, y)));
						break;
					case "get_tile_id":
						Debug.Log(controller.GetTile((IVector2)command.args[0], 0));
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
					int old_x = tile.x;
					int old_y = tile.y;

					mouse_pos = controller.transform.worldToLocalMatrix.MultiplyPoint(mouse_pos);
					mouse_pos = tile_map.geometry.mapToNormalMatrix.MultiplyPoint(mouse_pos);

					tile = tile_map.geometry.NormalToTile(mouse_pos).discardZ();
					
					if (old_x != tile.x || old_y != tile.y)
						needs_repaint = true;
				}

				Handles.BeginGUI();
				GUI.Label(new Rect(10, Screen.height - 60, 200, 20), "Tile: " + tile.x + ", " + tile.y);
				GUI.Label(new Rect(10, Screen.height - 80, 200, 20), "Chunk: " + chunk.x + ", " + chunk.y);
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
							if (chunk.inInterval(chunk_manager.chunk_least, chunk_manager.chunk_greatest))
							{
								if (controller.IsChunkLoaded((IVector3)chunk))
								{
									menu.AddItem(new GUIContent("Unload chunk"), false, ExecuteContext, new ContextCommand("unload_chunk", new object[1] { new IVector3(chunk.x, chunk.y, 0) }));

									menu.AddItem(new GUIContent("Print tile ID"), false, ExecuteContext, new ContextCommand("get_tile_id", new object[1] { new IVector2(tile.x, tile.y) }));
								}
								else
									menu.AddItem(new GUIContent("Load chunk"), false, ExecuteContext, new ContextCommand("load_chunk", new object[1] { new IVector3(chunk.x, chunk.y, 0) }));

								menu.AddItem(new GUIContent("Ping chunk"), false, ExecuteContext, new ContextCommand("ping_chunk", new object[1] { new IVector3(chunk.x, chunk.y, 0) }));

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