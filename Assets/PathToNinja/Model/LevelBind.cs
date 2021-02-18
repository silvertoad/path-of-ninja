using UnityEngine;
using UnityEngine.Events;

namespace PathToNinja.Model
{
    [CreateAssetMenu(fileName = "LevelBind", menuName = "Binds/Level", order = 1)]
    public class LevelBind : ScriptableObject
    {
        public readonly ReactiveProperty<LevelSettings> Settings = new ReactiveProperty<LevelSettings>();
        public readonly IntProperty CurrentDashCount = new IntProperty();

        public void Init(LevelSettings settings)
        {
            Settings.Value = settings;
            CurrentDashCount.Value = 0;
        }

        public int DashLasts => Settings.Value.MaxDashCount - CurrentDashCount.Value;
    }
}