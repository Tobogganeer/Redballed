using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tobo.Attributes
{
    public class RenameAttribute : PropertyAttribute
    {
        public string NewName { get; private set; }
        public RenameAttribute(string name)
        {
            NewName = name;
        }
    }
}
