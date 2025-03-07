using System.Collections.Generic;
using UnityEngine;
using Spine;

namespace Spine.Unity.Examples
{
    [CreateAssetMenu(fileName = "CharacterEquipState", menuName = "EquipState/CharacterEquipState")]
    public class CharacterEquipState : ScriptableObject
    {
        public List<string> equippedAttachments = new List<string>();
        public List<int> slotIndices = new List<int>();

        public void SaveEquipState(List<int> slots, List<string> attachments)
        {
            slotIndices.Clear();
            equippedAttachments.Clear();
            slotIndices.AddRange(slots);
            equippedAttachments.AddRange(attachments);
        }
    }
}
