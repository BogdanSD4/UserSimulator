namespace ProBotTelegramClient.Inputs.InputsData
{
    public struct KeyData
    {
        public KeyData()
        {
            keyCode = Keys.None;
            modifiedKeys = new List<Keys>();
        }
        public KeyData(KeyEventArgs key)
        {
            keyCode = key.KeyCode;
            modifiedKeys = new List<Keys>();

            Keys[] modified = new Keys[] {
                Keys.Shift, Keys.LShiftKey, Keys.RShiftKey,
                Keys.LControlKey, Keys.RControlKey, Keys.LWin,
                Keys.RWin, Keys.Control, Keys.Alt,
                Keys.LMenu, Keys.RMenu};
            if (modified.Contains(keyCode)) isModified = true;
        }
        public bool isModified;
        public Keys keyCode;
        public List<Keys> modifiedKeys;

        public IEnumerable<string> GetKeys()
        {
            List<string> keys = new List<string>();

            if (modifiedKeys is not null)
            {
                foreach (var item in modifiedKeys)
                {
                    keys.Add(item.ToString());
                }
            }

            keys.Add(keyCode.ToString());

            return keys;
        }
    }
}
