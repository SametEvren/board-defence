using System;
using System.Collections.Generic;
using Board;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(LevelData))]
    public class LevelEditor : UnityEditor.Editor
    {
        private LevelData LevelData => (LevelData)target;
        private Vector2 _defenceInventoryScrollPosition;
        
        public override void OnInspectorGUI()
        {
            //base.OnInspectorGUI();

            RenderGridOptions();
            
            EditorGUILayout.Space(10f);

            RenderDefenceInventory();

            RenderEnemyInventory();
            
            if (GUI.changed)
            {
                EditorUtility.SetDirty(LevelData);
            }
        }

        private void RenderDefenceInventory()
        {
            RenderDefenceHeader();
            _defenceInventoryScrollPosition = EditorGUILayout.BeginScrollView(_defenceInventoryScrollPosition);
            RenderDefenceItems();
            // RenderAddDefenceButton();
            
            EditorGUILayout.EndScrollView();
        }

        private void RenderDefenceItems()
        {
            var oldItems = LevelData.defenceItemInventories;
            
            var indexToRemove = -1;
            
            for (var i = 0; i < oldItems.Count; i++)
            {
                var item = oldItems[i];
                var resultItem = RenderDefenceItem(item);
                
                if (resultItem == null)
                {
                    indexToRemove = i;
                    break;
                }
                
                if (resultItem.Value == item)
                    continue;

                LevelData.defenceItemInventories[i] = resultItem.Value;
            }
            
            if(indexToRemove >= 0)
                LevelData.defenceItemInventories.RemoveAt(indexToRemove);
        }

        private DefenceItemInventory? RenderDefenceItem(DefenceItemInventory item)
        {
            var newItem = item;
            EditorGUILayout.BeginHorizontal(GUILayout.Height(20));
            
            newItem.amount = EditorGUILayout.IntField(item.amount);
            newItem.amount = Mathf.Max(1, newItem.amount);
            
            newItem.defenceItemType = (DefenceItemType)EditorGUILayout.EnumPopup(item.defenceItemType);
            
            GUI.color = Color.red;
            if (GUILayout.Button(EditorGUIUtility.IconContent("garbage"), GUILayout.Height(20)))
            {
                EditorGUILayout.EndHorizontal();
                GUI.color = Color.white;
                return null;
            }
            GUI.color = Color.white;
            
            EditorGUILayout.EndHorizontal();
            
            return newItem;
        }

        private void RenderDefenceHeader()
        {
            EditorGUILayout.LabelField("Defence Inventory",EditorStyles.boldLabel);
        }

        private void RenderEnemyInventory()
        {
            //throw new NotImplementedException();
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