using System.Collections.Generic;
using UnityEngine;

namespace Board
{
    [CreateAssetMenu(fileName = "Level Progression", menuName = "ScriptableObjects/Level Progression", order = 0)]
    public class LevelProgression : ScriptableObject
    {
        public List<LevelData> levelDataList;
    }
}