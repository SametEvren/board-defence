using System;
using Board;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(LevelData))]
    public class LevelEditor : UnityEditor.Editor
    {
        private LevelData LevelData => (LevelData)target;
        public override void OnInspectorGUI()
        {
            // base.OnInspectorGUI();

            RenderGridOptions();

            RenderDefenceInventory();

            RenderEnemyInventory();
            
            if (GUI.changed)
            {
                EditorUtility.SetDirty(LevelData);
            }
        }

        private void RenderDefenceInventory()
        {
            throw new NotImplementedException();
        }

        private void RenderEnemyInventory()
        {
            throw new NotImplementedException();
        }

        private void RenderGridOptions()
        {
            RenderGridSize();
            RenderBuildableArea();
        }

        private void RenderGridSize()
        {
            var oldSize = LevelData.gridSize;
            var newSize = EditorGUILayout.Vector2IntField("Grid Size", oldSize);

            if (oldSize != newSize)
            {
                LevelData.gridSize = new Vector2Int(Math.Max(1, newSize.x), Math.Max(1, newSize.y));
                DynamicResizeBuildableArea();
            }
        }
        
        private void RenderBuildableArea()
        {
            var oldSize = LevelData.buildableArea;
            var newSize = EditorGUILayout.Vector2IntField("Buildable Area Size", oldSize);

            if (oldSize != newSize)
            {
                LevelData.buildableArea = new Vector2Int(Math.Max(1, newSize.x), Math.Max(1, newSize.y));
                DynamicResizeBuildableArea();
            }
        }
        
        private void DynamicResizeBuildableArea()
        {
            LevelData.buildableArea = new Vector2Int(Math.Min(LevelData.gridSize.x, LevelData.buildableArea.x),
                Math.Min(LevelData.gridSize.y, LevelData.buildableArea.y));
        }
    }
}