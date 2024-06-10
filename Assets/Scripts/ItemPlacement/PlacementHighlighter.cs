using System;
using System.Collections.Generic;
using Board;
using Defence;
using UnityEngine;

namespace ItemPlacement
{
    public static class PlacementHighlighter
    {
        private static readonly List<BoardSlot> CurrentlyAffectedSlots = new();

        public static void ResetHighLights()
        {
            foreach (var slot in CurrentlyAffectedSlots) 
                slot.SetHighlight(false);
            
            CurrentlyAffectedSlots.Clear();
        }
        
        public static void HighlightPlacementRange(BoardSlot[,] slotMatrix, Vector2Int origin, DefenceItemData itemData)
        {
            if(CurrentlyAffectedSlots is { Count: > 0 }) ResetHighLights();
            
           var affectedCoordinates = GetAffectedCoordinates(slotMatrix, origin, itemData.range, itemData.attackPattern);
           
           foreach (var coordinate in affectedCoordinates)
           {
               var slot = slotMatrix[coordinate.x, coordinate.y];
               CurrentlyAffectedSlots.Add(slot);
               slot.SetHighlight(true);
           } 
        }

        private static List<Vector2Int> GetAffectedCoordinates(BoardSlot[,] slotMatrix, Vector2Int origin, int range, AttackPattern attackPattern)
        {
            var coordinateList = new List<Vector2Int>();
            var xMax = slotMatrix.GetLength(0);
            var yMax = slotMatrix.GetLength(1);
            
            switch (attackPattern)
            {
                case AttackPattern.Forward:
                    GetForwardSlots();
                    break;
                case AttackPattern.Backward:
                    GetBackwardSlots();
                    break;
                case AttackPattern.Left:
                    GetLeftSlots();
                    break;
                case AttackPattern.Right:
                    GetRightSlots();
                    break;
                case AttackPattern.Plus:
                    GetPlusShapeSlots();
                    break;
                case AttackPattern.Diagonal:
                    GetDiagonalShapeSlots();
                    break;
                case AttackPattern.All:
                    GetSlotsInTotalArea();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(attackPattern), attackPattern, null);
            }

            return coordinateList;

            void GetForwardSlots()
            {
                for (var i = origin.y; i < yMax; i++)
                    coordinateList.Add(new Vector2Int(origin.x, i));
            }

            void GetBackwardSlots()
            {
                for (var i = origin.y; i >= 0; i--)
                    coordinateList.Add(new Vector2Int(origin.x, i));
            }

            void GetLeftSlots()
            {
                for (var i = origin.x; i >= 0; i--)
                    coordinateList.Add(new Vector2Int(i, origin.y));
            }

            void GetRightSlots()
            {
                for (var i = origin.x; i < xMax; i++)
                    coordinateList.Add(new Vector2Int(i, origin.y));
            }

            void GetPlusShapeSlots()
            {
                for (var i = 0; i < yMax; i++)
                    coordinateList.Add(new Vector2Int(origin.x, i));
                for (var i = 0; i < xMax; i++)
                {
                    if (i == origin.x) continue;
                    coordinateList.Add(new Vector2Int(i, origin.y));
                }
            }

            void GetDiagonalShapeSlots()
            {
                for (var i = 0; i < range; i++)
                {
                    var topY = (origin.y + range) - i;
                    var bottomY = (origin.y - range) + i;
                    var leftX = (origin.x - range) + i;
                    var rightX = (origin.x + range) - i;

                    if (topY < yMax)
                    {
                        if (leftX >= 0)
                            coordinateList.Add(new Vector2Int(leftX, topY)); //Top Left
                        if (rightX < xMax)
                            coordinateList.Add(new Vector2Int(rightX, topY)); //Top Right
                    }

                    if (bottomY >= 0)
                    {
                        if (leftX >= 0)
                            coordinateList.Add(new Vector2Int(leftX, bottomY)); //Bottom Left
                        if (rightX < xMax)
                            coordinateList.Add(new Vector2Int(rightX, bottomY)); //Bottom Right
                    }
                }

                coordinateList.Add(origin);
            }

            void GetSlotsInTotalArea()
            {
                var bottom = Math.Max(origin.y - range, 0);
                var top = Math.Min(origin.y + range, yMax - 1);
                var left = Math.Max(origin.x - range, 0);
                var right = Math.Min(origin.x + range, xMax - 1);

                for (var x = left; x <= right; x++)
                for (var y = bottom; y <= top; y++)
                {
                    coordinateList.Add(new Vector2Int(x, y));
                }
            }
        }
    }
}