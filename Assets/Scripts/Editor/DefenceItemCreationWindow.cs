using System;
using System.Collections.Generic;
using Board;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    public class DefenceItemCreationWindow : EditorWindow
    {
        private Action<DefenceItemInventory> _onConfirm;
        private List<DefenceItemType> _types;

        private DefenceItemType _selectedType;
        private int _selectedAmount;
        private int _selectedTypeIndex;
        
        public static void ShowWindow(List<DefenceItemInventory> alreadyExistingItems, Action<DefenceItemInventory> onConfirm)
        {
            var window = GetWindow<DefenceItemCreationWindow>();
            window.titleContent = new GUIContent("New Defence Item");
            window._types = LevelEditor.ExtractAvailableDefenceTypes(alreadyExistingItems);
            window._onConfirm = onConfirm;
            window.minSize = new Vector2(300,75);
            window.maxSize = new Vector2(300,75);
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

            _selectedTypeIndex = EditorGUILayout.Popup("Defence Item Type", _selectedTypeIndex, typeNames);
            _selectedType = _types[_selectedTypeIndex];

            if (GUILayout.Button("Confirm"))
            {
                _onConfirm.Invoke(new DefenceItemInventory()
                {
                    amount = _selectedAmount,
                    defenceItemType = _selectedType
                });
                Close();
            }
        }
    }
}