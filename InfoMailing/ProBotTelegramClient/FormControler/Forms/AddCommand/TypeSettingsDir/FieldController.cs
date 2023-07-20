using ProBotTelegramClient.FormControler.Forms.AddCommand.TypeSettingsDir.Fields;
using ProBotTelegramClient.FormControler.Main.MainScreen;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProBotTelegramClient.FormControler.Forms.AddCommand.TypeSettingsDir
{
	public class FieldController : IField
	{
        public FieldController(ScreenSettings settings, IEnumerable<BaseField> fields)
        {
            valuePairs = new Dictionary<string, object>();
            List<Action> actions = new List<Action>();

            Action<BaseField> action = (bc) =>
            {
                bc.settings = settings;
                bc.result = (o) =>
                {
                    if (!bc.Visible) return;

                    if (valuePairs.ContainsKey(bc.Name))
                    {
                        valuePairs[bc.Name] = o;
                    }
                    else
                    {
                        valuePairs.Add(bc.Name, o);
                    }
                };
                actions.Add(bc.OnShow);
            };

            foreach (BaseField field in fields)
            {
                field.Controller = this;
                field.Preferance(action);
            }
            BaseFields = fields;

            actions.Reverse();
            foreach (var item in actions)
            {
                OnShown += item;
            }
        }

        public event Action OnShown;
		public Control LastDrawed { get; set; }
		public IDictionary<string, object> valuePairs { get; set; }
        public IEnumerable<BaseField> BaseFields { get; set; }
        public Func<bool> useAction { get; set; }

        public Func<bool> Show(Panel screen)
        {
            if (useAction is null)
            {
                LastDrawed = null;
                List<Func<bool>> list = new List<Func<bool>>();

                foreach (var field in BaseFields)
                {
					list.Add(field.Show(screen));
                }
                OnShown?.Invoke();

                useAction = () =>
                {
                    bool result = true;
                    valuePairs.Clear();

                    foreach (var item in list)
                    {
                        if (result) result = item.Invoke();
                        else
                        {
                            item.Invoke();
                        }
                    }

                    return result;
                };
            }
            return useAction;
        }

        public void Hide()
        {
            foreach (var item in BaseFields)
            {
                item.Hide();
            }
        }
        public void Activate() 
        {
			foreach (var item in BaseFields)
			{
				item.Activate();
			}
		}
    }
}
