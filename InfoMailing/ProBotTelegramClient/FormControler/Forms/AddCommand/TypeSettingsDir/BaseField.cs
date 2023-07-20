using ProBotTelegramClient.CustomComands;
using ProBotTelegramClient.FormControler.Forms.AddCommand.TypeSettingsDir.Fields;
using ProBotTelegramClient.FormControler.Main.MainScreen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProBotTelegramClient.FormControler.Forms.AddCommand.TypeSettingsDir
{
	public abstract class BaseField : IField
	{
        public BaseField(string name)
        {
			Name = name;
			Visible = true;
			BaseFields = new List<BaseField>();
			_nonStaticFields = new List<BaseField>();

			InitializeComponets();
        }

		public static readonly Func<bool> Empty = () => true;

		public bool IsStatic { get; set; } = true;
        public string Name { get; set; }
		public bool Visible { get; set; }
		public Action OnShow { get; set; }

		public Control LastDrawed { get; set; }
		public IField Controller { get; set; }

		public ScreenSettings settings { protected get; set; }
		public Action<object> result { protected get; set; }

		protected List<BaseField> _nonStaticFields;
		private IEnumerable<BaseField> _baseField;
		public IEnumerable<BaseField> BaseFields { get 
			{
				return _baseField;
			}
			set 
			{
				_baseField = value;
				foreach (var field in _baseField)
				{
					if (!field.IsStatic)
					{
						_nonStaticFields.Add(field);
					}
				}
			}
		}

		public abstract void InitializeComponets();

		public virtual void Hide()
		{
			Visible = false;
			DisableAll();
		}
		public virtual void Activate()
		{
			Visible = true;
			EnableAll();
		}
		public void ActivateNonStatic(BaseField baseField)
		{
			if (baseField.IsStatic) return;
			if (!_nonStaticFields.Contains(baseField)) return;

			foreach (var item in _nonStaticFields)
			{
				if(item.Equals(baseField)) item.Activate();
				else item.Hide();
			}
		}

		protected abstract void Enable();
		protected abstract void Disable();
		protected virtual void EnableAll()
		{
			Enable();
			foreach (var item in BaseFields)
			{
				if (item.Visible) item.EnableAll();
			}
		}
		protected virtual void DisableAll()
		{
			Disable();
			foreach (var item in BaseFields)
			{
				if (item.Visible) item.DisableAll();
			}
		}

		public virtual void Preferance(Action<BaseField> action)
		{
			action.Invoke(this);
			foreach (var item in BaseFields)
			{
				item.Controller = this;
				item.Preferance(action);
			}
		}
		public virtual Func<bool> Show(Panel screen)
		{
			List<Func<bool>> list = new List<Func<bool>>();
			list.Add(ReturnedAction);

			CreateUI(screen);

			foreach (var item in BaseFields)
			{
				list.Add(item.Show(screen));
			}

			Func<bool> result = () =>
			{
				if (!Visible) return true;

				bool res = true;
				foreach (var item in list)
				{
					if (res) res = item.Invoke();
					else
					{
						item.Invoke();
					}
				}
				return res;
			};

			return result;
		}
		public abstract void CreateUI(Panel screen);
		public abstract bool ReturnedAction();
	}
}
