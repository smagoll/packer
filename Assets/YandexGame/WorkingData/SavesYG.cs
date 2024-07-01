
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

        public int money;

        public OfficeSave[] offices = { new OfficeSave(new Vector2(1, 1), new[] { new FurnitureSave(1) }) };
    }
}