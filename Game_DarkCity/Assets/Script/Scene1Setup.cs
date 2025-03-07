using UnityEngine;

namespace Spine.Unity.Examples
{
    public class SceneLoader : MonoBehaviour
    {
        public CharacterEquipState equipState;
        public EquipsVisualsComponentExample target;

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
