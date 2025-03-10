using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Misc", menuName = "Item/Misc")]
public class MiscClass : ItemClass
{
    [Header("Misc")]
    public string itemDescription;
    public override ItemClass GetItem() { return this; }
    public override ToolClass GetTool() { return null; }
    public override MiscClass GetMisc() { return this; }
    public override ConsumableClass GetConsum() { return null; }
}
