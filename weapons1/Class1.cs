using UnityEngine;

using BepInEx;
using Jotunn.Configs;
using Jotunn.Entities;
using Jotunn.Managers;
using Jotunn.Utils;

namespace SampleSword
{
    [BepInPlugin(PluginGUID, PluginName, PluginVersion)]
    [BepInDependency(Jotunn.Main.ModGuid)]
    [NetworkCompatibility(CompatibilityLevel.EveryoneMustHaveMod, VersionStrictness.Major)]
    internal class ModItemClass : BaseUnityPlugin
    {
        // GUID guidなら多分被るとまずい筈
        public const string PluginGUID = "zzzM150_SampleSword_001";
        // あなたのmodの名前
        public const string PluginName = "SampleSword";
        public const string PluginVersion = "0.0.1";

        // unityでつけたAssetBundleの名前
        public const string AssetBundleName = "samplesword";
        public static AssetBundle TargetAssetBundle;

        // unityでつけたプレハブの名前
        public const string TargetPrefabName = "SampleSword";

        // アイテム名のID(unityで指定したID)
        public const string TargetItemNameId = "$sample_sword_name_id";

        // 作成、修理に使用する作業台のid
        public const string CraftingStationPrefabName = "piece_workbench";
        // 使用する作業台のレベル
        public const int CraftingStationLevel = 1;


        private void Awake()
        {
            TargetAssetBundle = AssetUtils.LoadAssetBundleFromResources(AssetBundleName, typeof(ModItemClass).Assembly);
            var targetAsset = TargetAssetBundle.LoadAsset<GameObject>(TargetPrefabName);
            var targetItem = new CustomItem(targetAsset, fixReference: true,
                new ItemConfig {
                    Amount = 1,
                    Name = TargetItemNameId,
                    Enabled = true,
                    CraftingStation = CraftingStationPrefabName,
                    RepairStation = CraftingStationPrefabName,
                    MinStationLevel = CraftingStationLevel,
                    Requirements = new[]
                    {
                        // 必要素材
                        // この場合木材2個,火打石1個で作成
                        // 素材のidはhttps://wiki3.jp/valheimjp/page/59を参照
                        new RequirementConfig {
                            Item = "Wood",
                            Amount = 2
                        },
                        new RequirementConfig {
                            Item = "Flint",
                            Amount = 1
                        },
                    }
                });
            ItemManager.Instance.AddItem(targetItem);
        }

    }
}