using System;
using System.Collections.Generic;
using System.Linq;
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

        private int _selectedDefenceTypeIndex;
        
        public override void OnInspectorGUI()
        {
            RenderGridOptions();
            
            EditorGUILayout.Space(10f);
            
            RenderDefenceInventory();
            
            EditorGUILayout.Space(10f);
            
            RenderEnemyInventory();
            
            if (GUI.changed)
            {
                EditorUtility.SetDirty(LevelData);
            }
        }

        #region Grid System
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
        #endregion
        
        private void RenderDefenceInventory()
        {
            RenderDefenceHeader();
            _defenceInventoryScrollPosition = EditorGUILayout.BeginScrollView(_defenceInventoryScrollPosition);
            RenderDefenceItems();
            RenderAddDefenceButton();
            
            EditorGUILayout.EndScrollView();
        }

        private void RenderAddDefenceButton()
        {
            if (LevelData.defenceItemInventories.Count >= Enum.GetValues(typeof(DefenceItemType)).Length - 1)
                return;
            
            GUI.backgroundColor = Color.green;
            if (GUILayout.Button(EditorGUIUtility.IconContent("plus"), GUILayout.Height(40f)))
            {
                DefenceItemCreationWindow.ShowWindow(LevelData.defenceItemInventories, AddDefenceItem);
            }
            GUI.backgroundColor = Color.white;
        }

        private void AddDefenceItem(DefenceItemInventory item)
        {
            LevelData.defenceItemInventories.Add(item);
        }

        private void RenderDefenceItems()
        {
            var itemList = LevelData.defenceItemInventories;
            
            for (var i = 0; i < itemList.Count; i++)
            {
                var item = itemList[i];

                EditorGUILayout.BeginHorizontal();
                item.amount = RenderItemAmount(item.amount);
                item.defenceItemType = RenderDefenceItemType(item.defenceItemType);
                
                if (RenderDeleteButton())
                {
                    LevelData.defenceItemInventories.RemoveAt(i);
                    EditorGUILayout.EndHorizontal();
                    return;
                }

                LevelData.defenceItemInventories[i] = item;
                EditorGUILayout.EndHorizontal();
            }
        }

        private List<DefenceItemType> ExtractAvailableTypes(List<DefenceItemInventory> alreadyExistingItems)
        {
            return Enum.GetValues(typeof(DefenceItemType))
                .Cast<DefenceItemType>()
                .Where(value => value != DefenceItemType.None && alreadyExistingItems
                    .All(d => d.defenceItemType != value))
                .ToList();
        }
        
        private DefenceItemType RenderDefenceItemType(DefenceItemType itemDefenceType)
        {
            var availableTypes = ExtractAvailableTypes(LevelData.defenceItemInventories);
            availableTypes.Insert(0, itemDefenceType);
            var typeNames = new string[availableTypes.Count];
            for (int i = 0; i < availableTypes.Count; i++)
            {
                typeNames[i] = availableTypes[i].ToString();
            }

            _selectedDefenceTypeIndex = 0;

            GUI.color = itemDefenceType == DefenceItemType.None ? Color.red : Color.white;
            _selectedDefenceTypeIndex = EditorGUILayout.Popup(_selectedDefenceTypeIndex, typeNames);
            GUI.color = Color.white;
            return availableTypes[_selectedDefenceTypeIndex];
        }

        private bool RenderDeleteButton()
        {
            GUI.backgroundColor = new Color(0.8f, 0f, 0f);
            var result = GUILayout.Button(EditorGUIUtility.IconContent("garbage"), GUILayout.Height(20), GUILayout.Width(40));
            GUI.backgroundColor = Color.white;
            return result;
        }

        private int RenderItemAmount(int amount)
        {
            amount = EditorGUILayout.IntField(amount, GUILayout.Width(40));
            amount = Mathf.Max(1, amount);
            return amount;
        }

        private void RenderDefenceHeader()
        {
            EditorGUILayout.LabelField("Defence Inventory",EditorStyles.boldLabel);
        }

        private void RenderEnemyInventory()
        {
            //throw new NotImplementedException();
        }

    }
}