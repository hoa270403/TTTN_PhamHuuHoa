

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Spine.Unity.Examples {
    [CreateAssetMenu(fileName = "NewEquipAsset", menuName = "Spine.Unity.Examples/EquipAssetExample")]
    public class EquipAssetExample : ScriptableObject {
		public EquipSystemExample.EquipType equipType;
		public Sprite sprite;
		public string description;
		public int yourStats;
	}
}
