using UnityEngine;

namespace Watermelon
{
    [System.Serializable]
    public class SavedDataContainer
    {
        [SerializeField] int hash;
        public int Hash => hash;

        [SerializeField] string json;
        public bool Restored { get; set; }

        [System.NonSerialized] ISaveObject saveObject;
        public ISaveObject SaveObject => saveObject;

        public SavedDataContainer(int hash, ISaveObject saveObject)
        {
            this.hash = hash;
            this.saveObject = saveObject;
            Restored = true;
        }

        public void Flush()
        {
            if (saveObject != null) saveObject.Flush();
            if (Restored) json = JsonUtility.ToJson(saveObject);
        }

        public void Restore<T>() where T : ISaveObject
        {
            saveObject = JsonUtility.FromJson<T>(json);
            Restored = true;
        }
    }
}