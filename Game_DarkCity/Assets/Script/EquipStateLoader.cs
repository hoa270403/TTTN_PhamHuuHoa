using System.Collections.Generic;
using UnityEngine;

namespace Spine.Unity.Examples
{
    public class EquipStateLoader : MonoBehaviour
    {
        public CharacterEquipState equipState;  // Gắn vào Inspector
        public EquipsVisualsComponentExample target;  // Gắn vào Inspector

        void Start()
        {
            // Load trạng thái và áp dụng trang bị
            for (int i = 0; i < equipState.slotIndices.Count; i++)
            {
                int slotIndex = equipState.slotIndices[i];
                string attachmentName = equipState.equippedAttachments[i];
                var attachment = target.skeletonAnimation.Skeleton.GetAttachment(slotIndex, attachmentName);
                if (attachment != null)
                {
                    target.Equip(slotIndex, attachmentName, attachment);
                }
            }
        }
    }
}