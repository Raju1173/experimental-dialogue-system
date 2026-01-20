using System.Collections.Generic;
using UnityEngine;

namespace DS.Data.Save
{
    [System.Serializable]
    public class NodeSaveData
    {
        public string ID { get; set; }
        public string Dialogue { get; set; }
        public List<ChoicesSaveData> Choices { get; set; }
        public string GroupID { get; set; }
        public Vector2 Position { get; set; }
    }
}
