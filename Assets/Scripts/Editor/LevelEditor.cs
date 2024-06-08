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
        private Vector2 _enemyInventoryScrollPosition;

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

        #region Defence Inventory System
        
        private void RenderDefenceInventory()
        {
            RenderHeader("Defence Inventory");
            _defenceInventoryScrollPosition = EditorGUILayout.BeginScrollView(_defenceInventoryScrollPosition);
            RenderDefenceItems();
            RenderAddDefenceButton();
            
            EditorGUILayout.EndScrollView();
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
        
        private DefenceItemType RenderDefenceItemType(DefenceItemType itemDefenceType)
        {
            var availableTypes = ExtractAvailableDefenceTypes(LevelData.defenceItemInventories);
            
            availableTypes.Insert(0, itemDefenceType);
            var typeNames = new string[availableTypes.Count];
            for (int i = 0; i < availableTypes.Count; i++)
            {
                typeNames[i] = availableTypes[i].ToString();
            }
            
            return availableTypes[EditorGUILayout.Popup(0, typeNames)];
        }

        #endregion

        #region Enemy Inventory
        
        private void RenderEnemyInventory()
        {
            RenderHeader("Enemy Inventory");
            _enemyInventoryScrollPosition = EditorGUILayout.BeginScrollView(_enemyInventoryScrollPosition);
            RenderEnemyItems();
            RenderAddEnemyButton();
            
            EditorGUILayout.EndScrollView();
        }
        
        private void AddEnemy(EnemyInventory item)
        {
            LevelData.enemyInventories.Add(item);
        }
        
        private void RenderEnemyItems()
        {
            var itemList = LevelData.enemyInventories;
            
            for (var i = 0; i < itemList.Count; i++)
            {
                var item = itemList[i];

                EditorGUILayout.BeginHorizontal();

                var newIndex = RenderChangeIndexButtons(i);
                if (newIndex != i)
                {
                    var temp = LevelData.enemyInventories[newIndex];
                    LevelData.enemyInventories[newIndex] = item;
                    LevelData.enemyInventories[i] = temp;
                    EditorGUILayout.EndHorizontal();
                    return;
                }

                EditorGUILayout.BeginVertical();
                EditorGUILayout.Space(5f);
                EditorGUILayout.BeginHorizontal();
                
                item.amount = RenderItemAmount(item.amount);
                item.enemyType = RenderEnemyItemType(item.enemyType);
                
                if (RenderDeleteButton())
                {
                    LevelData.enemyInventories.RemoveAt(i);
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.EndVertical();
                    EditorGUILayout.EndHorizontal();
                    return;
                }

                LevelData.enemyInventories[i] = item;
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.EndVertical();
                EditorGUILayout.EndHorizontal();
            }
        }

        private int RenderChangeIndexButtons(int index)
        {
            EditorGUILayout.BeginVertical(GUILayout.Width(20f));
            var renderUpButton = index != 0;
            var renderDownButton = index != LevelData.enemyInventories.Count - 1;
            if (renderUpButton)
                if (GUILayout.Button("▲", GUILayout.Height(renderDownButton ? 15f : 30f), GUILayout.Width(20f)))
                {
                    EditorGUILayout.EndVertical();
                    return index - 1;
                }
            
            if (renderDownButton)
                if (GUILayout.Button("▼", GUILayout.Height(renderUpButton ? 15f: 30f), GUILayout.Width(20f)))
                {
                    EditorGUILayout.EndVertical();
                    return index + 1;
                }
            
            EditorGUILayout.EndVertical();
            return index;
        }

        private EnemyType RenderEnemyItemType(EnemyType enemyType)
        {
            var availableTypes = ExtractAvailableEnemyTypes(LevelData.enemyInventories);
            
            availableTypes.Insert(0, enemyType);
            var typeNames = new string[availableTypes.Count];
            for (int i = 0; i < availableTypes.Count; i++)
            {
                typeNames[i] = availableTypes[i].ToString();
            }
            
            return availableTypes[EditorGUILayout.Popup(0, typeNames)];
        }
        #endregion

        
        private void RenderHeader(string headerName)
        {
            EditorGUILayout.LabelField(headerName, EditorStyles.boldLabel);
        }
        
        private bool RenderDeleteButton()
        {
            GUI.backgroundColor = new Color(0.8f, 0f, 0f);
            var result = GUILayout.Button(EditorGUIUtility.IconContent("garbage"), 
                GUILayout.Height(20), GUILayout.Width(40));
            GUI.backgroundColor = Color.white;
            return result;
        }
        
        private int RenderItemAmount(int amount)
        {
            amount = EditorGUILayout.IntField(amount, GUILayout.Width(40));
            amount = Mathf.Max(1, amount);
            return amount;
        }
        
        public static List<DefenceItemType> ExtractAvailableDefenceTypes(List<DefenceItemInventory> alreadyExistingItems)
        {
            return ExtractAvailableTypes(alreadyExistingItems, d => d.defenceItemType, DefenceItemType.None);
        }

        public static List<EnemyType> ExtractAvailableEnemyTypes(List<EnemyInventory> alreadyExistingItems)
        {
            return Enum.GetValues(typeof(EnemyType))
                .Cast<EnemyType>()
                .Where(value => !value.Equals(EnemyType.None))
                .ToList();
        }
        
        private static List<T> ExtractAvailableTypes<T, U>(List<U> alreadyExistingItems, Func<U, T> getType, T noneType) where T : Enum
        {
            return Enum.GetValues(typeof(T))
                .Cast<T>()
                .Where(value => !value.Equals(noneType) && alreadyExistingItems
                    .All(d => !getType(d).Equals(value)))
                .ToList();
        }
        
        private void RenderAddButton<T>(List<T> inventory, Action showWindowAction, int enumLength)
        {
            if (enumLength >= 0 && inventory.Count >= enumLength - 1)
                return;

            GUI.backgroundColor = Color.green;
            if (GUILayout.Button(EditorGUIUtility.IconContent("plus"), GUILayout.Height(40f)))
            {
                showWindowAction();
            }
            GUI.backgroundColor = Color.white;
        }

        private void RenderAddDefenceButton()
        {
            RenderAddButton(LevelData.defenceItemInventories, () => DefenceItemCreationWindow.ShowWindow(LevelData.defenceItemInventories, AddDefenceItem), Enum.GetValues(typeof(DefenceItemType)).Length);
        }

        private void RenderAddEnemyButton()
        {
            RenderAddButton(LevelData.enemyInventories, () => EnemyItemCreationWindow.ShowWindow(LevelData.enemyInventories, AddEnemy), -1);
        }
    }
}