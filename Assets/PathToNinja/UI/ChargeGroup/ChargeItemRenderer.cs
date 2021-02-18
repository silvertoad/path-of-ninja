using Common.UI.DataGroup;
using UnityEngine;

namespace PathToNinja.UI.ChargeGroup
{
    public class ChargeItemRenderer : MonoBehaviour, IItemRenderer<bool>
    {
        [SerializeField] private GameObject _hasChargeIcon;
        public void SetData(bool hasCharge)
        {
            _hasChargeIcon.SetActive(hasCharge);
        }
    }
}