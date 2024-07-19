using System.Collections.Generic;

namespace YG
{
    [System.Serializable]
    public class SavesYG
    {
        public int idSave;
        public bool isFirstSession = true;
        public string language = "ru";
        public bool promptDone;

        public float money = 100;
        public long lastDateEnter;
        public List<OfficeSave> offices;

        public SavesYG()
        {
            offices = new();
        }
    }
}