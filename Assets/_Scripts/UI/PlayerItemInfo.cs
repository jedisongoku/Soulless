using UnityEngine;
using System;

namespace UnityEngine.UI
{
    [Serializable]
    public class PlayerItemInfo
    {
        public string itemId;
        public string itemInstanceId;    
        public string isEquipped;

        public PlayerItemInfo(string _itemId, string _itemInstanceId, string _isEquipped)
        {
            itemId = _itemId;
            itemInstanceId = _itemInstanceId;
            isEquipped = _isEquipped;
        }

    }
}