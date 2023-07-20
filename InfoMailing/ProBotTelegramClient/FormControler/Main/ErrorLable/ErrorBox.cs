using ProBotTelegramClient.FormControler.Main.TimerDir;
using ProBotTelegramClient.Inputs.InputsCompilerDir;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Color = System.Drawing.Color;

namespace ProBotTelegramClient.FormControler.Main.ErrorLable
{
    public class ErrorBox
    {
        private static Label label;

        public static void Initial(Label control)
        {
            label = control;
            label.Visible = false;
        }

        public static void Message(string message)
        {
            label.Text = message;
            label.ForeColor = Color.Red; 
            label.Visible = true;

            TimerController.SetEvent("Error", 2000, () => 
            {
				label.Visible = false;
            });
        }
    }
}
