
using UnityEngine;

namespace YG
{
    [System.Serializable]
    public class SavesYG
    {
        // "Технические сохранения" для работы плагина (Не удалять)
        public int idSave;
        public bool isFirstSession = true;
        public string language = "ru";
        public bool promptDone;

        public int money = 100;

        public OfficeSave[] offices = { new OfficeSave(1, new[] { new FurnitureSave(1, new Vector2(0,0)) }) };
    }
}