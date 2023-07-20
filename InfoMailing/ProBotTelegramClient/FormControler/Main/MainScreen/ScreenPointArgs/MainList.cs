using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProBotTelegramClient.FormControler.Main.MainScreen.ScreenPointArgs
{
	[Serializable]
	public class MainList
	{
        public MainList()
        {
            _items = new MainPoint[0];
        }

		[JsonConstructor]
        public MainList(MainPoint[] items)
        {
			MainPoint[] mainPoints = new MainPoint[items.Count()];
			var list = items.ToArray();

			for (int i = 0; i < mainPoints.Length; i++)
			{
				mainPoints[i] = list[i];
				mainPoints[i].Container = this;
			}
			_items = mainPoints;
        }

        internal MainPoint[] _items;
		
		public MainPoint this[int index] { 
			get 
			{
				return _items[index];
			}
			set 
			{
				_items[index] = value;
			} 
		}
		public MainPoint[] Items => _items;

		public int Count => _items.Length;

		public void Add(MainPoint item)
		{
			MainPoint[] array = new MainPoint[Count + 1];
			for (int i = 0; i < Count; i++)
			{
				array[i] = _items[i];
			}
			array[Count] = item;
			_items = array;

			item.Container = this;
		}

		public void Clear()
		{
			_items = new MainPoint[0];
		}
		
		public bool Contains(MainPoint item)
		{
			return Count != 0 && IndexOf(item) >= 0;
		}

		public int IndexOf(MainPoint item) => Array.IndexOf(_items, item, 0, Count);

		public void Insert(int index, MainPoint item)
		{
			if ((uint)index > (uint)Count) throw new IndexOutOfRangeException();

			_items[index] = item;
		}

		public bool Remove(MainPoint item)
		{
			int index = IndexOf(item);
			if (index >= 0)
			{
				RemoveAt(index);
				return true;
			}

			return false;
		}

		public void RemoveAt(int index)
		{
			if (index >= Count) throw new ArgumentOutOfRangeException();

			MainPoint[] newList = new MainPoint[Count - 1];
			int counter = 0;

			for (int i = 0; i < Count; i++)
			{
				if (i == index) continue;
				newList[counter++] = _items[i];
			}
			
			_items = newList;
		}
	}
}
