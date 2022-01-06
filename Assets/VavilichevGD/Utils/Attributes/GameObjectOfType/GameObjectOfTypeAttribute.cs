using System;
using UnityEngine;

namespace VavilichevGD.Utils.Attributes.GameObjectOfType {
    public class GameObjectOfTypeAttribute : PropertyAttribute {
        public Type type { get; }
 
        public GameObjectOfTypeAttribute(Type type)
        {
            this.type = type;
        }
    }
}