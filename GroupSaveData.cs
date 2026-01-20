using UnityEngine;

namespace DS.Data.Save
{
    [System.Serializable]
    public class GroupSaveData
    {
        public string Title { get; set; }
        public Vector2 Position { get; set; }
    }
}
