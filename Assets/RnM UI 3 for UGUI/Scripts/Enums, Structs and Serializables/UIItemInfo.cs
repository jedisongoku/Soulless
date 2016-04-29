using UnityEngine;
using System;

namespace UnityEngine.UI
{
	[Serializable]
	public class UIItemInfo
	{
        //public static int counter = 1;
		public string itemId;
		public string displayName;
		public Sprite icon;
		//public string description;
		public UIEquipmentType equipType;
		public string itemType;
        public string itemClass;
		//public string subtype;
		public int damage;
		//public float attackSpeed;
		//public int block;
		public int armor;
		public int vitality;
        public int strength;
        public int dexterity;
        public int intellect;
        public int spirit;
        public int crit;

        public UIItemInfo(string _itemId, string _name, string _iconName, int _equipType, string _itemType, int _damage, int _armor, int _vitality, int _strength, int _dexterity, int _intellect, int _spirit, int _crit)
        {
            //id = counter;
            itemId = _itemId;
            displayName = _name;
            icon = Resources.Load<Sprite>("ItemIcons/" + _iconName);
            equipType = (UIEquipmentType)_equipType;
            itemType = _itemType;
            itemClass = "Item";
            damage = _damage;
            armor = _armor;
            vitality = _vitality;
            strength = _strength;
            dexterity = _dexterity;
            intellect = _intellect;
            spirit = _spirit;
            crit = _crit;

            //counter++;
        }

    }
}