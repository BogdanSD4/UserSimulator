using Newtonsoft.Json;
using ProBotTelegramClient.Inputs.BaseInputs;
using ProBotTelegramClient.Inputs.InputsData;
using ProBotTelegramWinForm;
using ProBotTelegramWinForm.Inputs;
using System.IO;
using System.Text;

namespace ProBotTelegramClient.Inputs.InputsCompilerDir
{
	public class InputIOControler
    {
        private static readonly string SAVE_PATH = "../../../inputs.txt";
        public static IEnumerable<Input> DownloadInputs(StreamReader? reader = null)
        {
            List<Input> inputs = new List<Input>();

            using (var stream = reader?? new StreamReader(SAVE_PATH))
            {
                while (!stream.EndOfStream)
                {
					var input = BaseDownloadInputs(stream.ReadLine());
					inputs.Add(input);
				}
            }

            return inputs;
        }
		public static IEnumerable<Input> DownloadInputs(IEnumerable<string> list)
		{
			List<Input> inputs = new List<Input>();

			foreach (var item in list)
			{
				var input = BaseDownloadInputs(item);
				inputs.Add(input);
			}

			return inputs;
		}
		public static Input BaseDownloadInputs(string node)
		{
			var line = node.Split(';');
			var type = (BaseInputType)Enum.Parse(typeof(BaseInputType), line[0]);

			Input result = null;
			InputType inputType = (InputType)Enum.Parse(typeof(InputType), line[1]);

			switch (type)
			{
				case BaseInputType.Mouse:
					{
						MouseData mouseData = JsonConvert.DeserializeObject<MouseData>(line[2]);
						result = new MouseInput(inputType, mouseData);
					}
					break;
				case BaseInputType.Key:
					{
						KeyData keyData = JsonConvert.DeserializeObject<KeyData>(line[2]);
						result = new KeyInput(inputType, keyData);
					}
					break;
			}

			result.SetAction();
			return result;
		}

		public static void UploadInputs()
        {
            IEnumerable<string> inputs = InputsCompiler.Compilation().Values;
            StringBuilder result = new StringBuilder();

            foreach (var item in inputs)
            {
                result.Append($"{item}\n");
            }
            File.WriteAllText(SAVE_PATH, result.ToString());
        }
    }
}
