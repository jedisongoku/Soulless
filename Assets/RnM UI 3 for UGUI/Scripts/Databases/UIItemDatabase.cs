using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace UnityEngine.UI
{
	public class UIItemDatabase : ScriptableObject {

        public static UIItemDatabase uiItemDatabase;
        public List<UIItemInfo> items;

        void Awake()
        {
            //uiItemDatabase = this;
        }
		
		/// <summary>
		/// Get the specified ItemInfo by index.
		/// </summary>
		/// <param name="index">Index.</param>
		public UIItemInfo Get(int index)
		{
			return (this.items[index]);
		}

		
		/// <summary>
		/// Gets the specified ItemInfo by ID.
		/// </summary>
		/// <returns>The ItemInfo or NULL if not found.</returns>
		/// <param name="ID">The item ID.</param>
		public UIItemInfo GetByID(int id)
		{
			/*for (int i = 0; i < this.items.Count; i++)
			{
				if (this.items[i].id == id)
					return this.items[i];
			}*/
			if(items.Count > id)
            {
                return items[id];
            }
            return null;
		}
	}
}