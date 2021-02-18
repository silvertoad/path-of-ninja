using System.Collections.Generic;
using PathToNinja.UI.ChargeGroup;
using UnityEngine;

namespace PathToNinja.UI
{
    public class DashCounter : MonoBehaviour
    {
        [SerializeField] private ChargeDataGroup _dataGroup;

        public void SetCount(int value, int valueMaxDashCount)
        {
            var data = new List<bool>();
            var lasts = valueMaxDashCount - value;
            for (var i = 0; i < valueMaxDashCount; i++)
                data.Add(i < lasts);

            _dataGroup.SetData(data);
        }
    }
}