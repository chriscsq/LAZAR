using UnityEngine;
using UnityEngine.Experimental.TerrainAPI;
using System.Collections.Generic;

namespace UnityEditor.Experimental.TerrainAPI
{
    public class PathPaintTool : TerrainPaintTool<PathPaintTool>
    {
        private List<ModuleEditor> modules = null;
        private List<ModuleEditor> onSceneGuiOrderList = null;
        private List<ModuleEditor> paintSegmentOrderList = null;

        private List<IPaintMode> paintModes = null;

        private ModuleEditor bridgeModule = new BridgeModule();
        private ModuleEditor smoothModule = new SmoothModule();
        private ModuleEditor paintModule = new PaintModule();
        private ModuleEditor smudgeModule = new SmudgeModule();
        private ModuleEditor ridgeErodeModule = new RidgeErodeModule();

        private IPaintMode paintBrushPaintMode = new PaintBrushPaintMode();
        private IPaintMode strokePaintMode = new StrokePaintMode();
        private IPaintMode splinePaintMode = new WaypointPaintMode();
        private IPaintMode waypointPaintMode = new SplinePaintMode();

        private IPaintMode paintMode;
        private int paintModeIndex;

        private List<AssetIntegration> assetIntegrations = null;

        private AssetIntegration vegetationStudioProIntegration = new VegetationStudioProIntegration();
        private AssetIntegration vegetationStudioIntegration = new VegetationStudioIntegration();


        public PathPaintTool()
        {
            #region Modules

            // register available modules
            this.modules = new List<ModuleEditor>();

            this.modules.Add(paintModule);
            this.modules.Add(bridgeModule);
            this.modules.Add(smoothModule);
            this.modules.Add(ridgeErodeModule);
            this.modules.Add(smudgeModule);

            // sort order in OnSceneGui
            this.onSceneGuiOrderList = new List<ModuleEditor>();

            this.onSceneGuiOrderList.Add(smoothModule);
            this.onSceneGuiOrderList.Add(smudgeModule);
            this.onSceneGuiOrderList.Add(bridgeModule);
            this.onSceneGuiOrderList.Add(ridgeErodeModule);
            this.onSceneGuiOrderList.Add(paintModule);

            // set default active states
            paintModule.Active = true;
            bridgeModule.Active = true;
            smoothModule.Active = true;
            ridgeErodeModule.Active = false;
            smudgeModule.Active = false;

            // sort order in paintSegment execution
            this.paintSegmentOrderList = new List<ModuleEditor>();

            this.paintSegmentOrderList.Add(bridgeModule);
            this.paintSegmentOrderList.Add(smoothModule);
            this.paintSegmentOrderList.Add(smudgeModule);
            this.paintSegmentOrderList.Add(ridgeErodeModule);
            this.paintSegmentOrderList.Add(paintModule);

            #endregion Modules

            #region Paint Mode

            // register available paint modes
            paintModes = new List<IPaintMode>();

            this.paintModes.Add(new PaintBrushPaintMode());
            this.paintModes.Add(new StrokePaintMode());
            this.paintModes.Add(new WaypointPaintMode());
            this.paintModes.Add(new SplinePaintMode());

            // set initial paint mode
            this.paintModeIndex = 0; // select the first item
            this.paintMode = this.paintModes[this.paintModeIndex];

            #endregion Paint Mode

            // register asset integrations
            assetIntegrations = new List<AssetIntegration>();

            if (vegetationStudioProIntegration.Enabled)
            {
                assetIntegrations.Add(vegetationStudioProIntegration);
            }

            if (vegetationStudioIntegration.Enabled)
            {
                assetIntegrations.Add(vegetationStudioIntegration);
            }

        }

        public override string GetName()
        {
            return "Path Paint Tool";
        }

        public override string GetDesc()
        {
            return "Paint a path along the terrain using combinations of Terrain Tools";
        }

        public override void OnSceneGUI(Terrain currentTerrain, IOnSceneGUI editContext)
        {

            // We're only doing painting operations, early out if it's not a repaint
            if (Event.current.type != EventType.Repaint)
                return;

            if (currentTerrain == null || editContext == null)
                return;

            #region PaintMode

            paintMode.OnSceneGUI(currentTerrain, editContext);

            #endregion PaintMode

            #region Modules

            foreach (ModuleEditor module in onSceneGuiOrderList)
            {
                if (!module.Active)
                    continue;

                module.OnSceneGUI(currentTerrain, editContext);
            }

            #endregion Modules



        }


        public override void OnInspectorGUI(Terrain terrain, IOnInspectorGUI editContext)
        {
            EditorGUI.BeginChangeCheck();
            {
                #region General Notification

                EditorGUILayout.HelpBox("Please note that Undo isn't implemented yet. Better backup your terrain before you perform any modifications.", MessageType.Info);

                #endregion General Notification


                #region Module Editor

                GUILayout.BeginVertical("box");
                {

                    GUILayout.Label(PathPaintStyles.activeTerrainToolsContent, EditorStyles.boldLabel);

                    EditorGUILayout.BeginHorizontal();

                    foreach (ModuleEditor module in modules)
                    {
                        // toggle active state
                        if (GUILayout.Button(module.GetName(), PathPaintStyles.GetButtonToggleStyle(module.Active)))
                        {
                            module.Active = !module.Active;
                        }
                    }


                    EditorGUILayout.EndHorizontal();
                }
                GUILayout.EndVertical();

                #endregion Module Editor


                #region Paint Mode

                GUILayout.BeginVertical("box");
                {
                    List<GUIContent> paintModeContents = new List<GUIContent>();
                    foreach (IPaintMode editor in paintModes)
                    {
                        paintModeContents.Add(new GUIContent(editor.GetName(), editor.GetDescription()));
                    }

                    EditorGUILayout.LabelField(PathPaintStyles.paintModesContent, EditorStyles.boldLabel);

                    EditorGUILayout.BeginHorizontal();

                    paintModeIndex = GUILayout.Toolbar(paintModeIndex, paintModeContents.ToArray());
                    paintMode = paintModes[paintModeIndex];

                    EditorGUILayout.EndHorizontal();

                }
                GUILayout.EndVertical();

                GUILayout.BeginVertical("box");
                {
                    paintMode.OnInspectorGUI(terrain, editContext);
                }
                GUILayout.EndVertical();

                #endregion Paint Mode


                #region Module Editor Settings

                foreach (ModuleEditor module in modules)
                {

                    if (!module.Active)
                        continue;

                    GUILayout.BeginVertical("box");
                    {
                        module.OnInspectorGUI(terrain, editContext);
                    }
                    GUILayout.EndVertical();
                }

                #endregion Module Editor Settings


                #region Brush

                GUILayout.BeginVertical("box");
                {
                    editContext.ShowBrushesGUI(0);
                }
                GUILayout.EndVertical();

                #endregion Brush

                #region Integrations
                foreach (AssetIntegration asset in assetIntegrations)
                {
                    GUILayout.BeginVertical("box");
                    {
                        asset.OnInspectorGUI();
                    }
                    GUILayout.EndVertical();
                }
                #endregion Integrations
            }

            if (EditorGUI.EndChangeCheck())
            {
                Save(true);
            }

        }



        public override bool OnPaint(Terrain terrain, IOnPaint editContext)
        {

            StrokeSegment[]  segments = paintMode.OnPaint(terrain, editContext);

            // abort if we have nothing to paint
            if (segments == null)
                return true;

            // perform paint operation
            PaintSegments(segments, editContext);

            // perform operatons on integrations
            OnPaintIntegrations(segments, terrain, editContext);

            return false;
        }


        #region Integrations
        private void OnPaintIntegrations(StrokeSegment[] segments, Terrain terrain, IOnPaint editContext)
        {

            // integreation calls are redundant if the system isn't active.
            // but this way we can move the #if-endif clauses to a dedicated class and don't
            // have to clutter the main class. another option might be to have a dedicated
            // integraction checker class and return a list of what's active and what not.
            // we'll see how it develops

            foreach( AssetIntegration asset in assetIntegrations)
            {
                if (!asset.Active)
                    continue;

                vegetationStudioProIntegration.Update(segments);
            }

        }
        #endregion Integrations

        /// <summary>
        /// Perform paint operations on registered and active modules
        /// </summary>
        /// <param name="segments"></param>
        /// <param name="editContext"></param>
        private void PaintSegments(StrokeSegment[] segments, IOnPaint editContext)
        {
            foreach (ModuleEditor module in paintSegmentOrderList)
            {
                if (!module.Active)
                    continue;

                module.PaintSegments(segments, editContext);
            }
        }

    }

}
