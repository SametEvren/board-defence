using System;
using System.Collections.Generic;
using Board;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    public class EnemyItemCreationWindow : EditorWindow
    {
        private Action<EnemyInventory> _onConfirm;
        private List<EnemyType> _types;

        private EnemyType _selectedType;
        private int _selectedAmount;
        private int _selectedTypeIndex;
        
        public static void ShowWindow(List<EnemyInventory> alreadyExistingItems, Action<EnemyInventory> onConfirm)
        {
            var window = GetWindow<EnemyItemCreationWindow>();
            window.titleContent = new GUIContent("New Enemy");
            window._types = LevelEditor.ExtractAvailableEnemyTypes(alreadyExistingItems);
            window._onConfirm = onConfirm;
            window.Show();
        }

        private void OnGUI()
        {
            _selectedAmount = EditorGUILayout.IntField("Amount", _selectedAmount);
            _selectedAmount = Math.Max(1, _selectedAmount);

            var typeNames = new string[_types.Count];
            for (int i = 0; i < _types.Count; i++)
            {
                typeNames[i] = _types[i].ToString();
            }

            _selectedTypeIndex = EditorGUILayout.Popup("Enemy Type", _selectedTypeIndex, typeNames);
            _selectedType = _types[_selectedTypeIndex];

            if (GUILayout.Button("Confirm"))
            {
                _onConfirm.Invoke(new EnemyInventory()
                {
                    amount = _selectedAmount,
                    enemyType = _selectedType
                });
                Close();
            }
        }
    }
}