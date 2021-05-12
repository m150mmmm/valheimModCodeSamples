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

        // 作成、修理に使用する作業台のid 作業台:piece_workbench , 鍛冶場:forge, 職人テーブル:piece_artisanstation
        public const string CraftingStationPrefabName = "piece_workbench";
        // 修理に使用する作業台のid
        public const string RepairStationPrefabName = "piece_workbench";
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
                    RepairStation = RepairStationPrefabName,
                    MinStationLevel = CraftingStationLevel,
                    Requirements = new[]
                    {
                        // 必要素材
                        // この場合木材10個,火打石5個で作成
                        // レベル2にアップグレードするには木材3個、火打石1個
                        // レベル3にアップグレードするにはレベル2にするのに必要な素材の2倍の木材6個、火打石2個
                        // レベル4にアップグレードするにはレベル2にするのに必要な素材の3倍の木材9個、火打石3個
                        // 素材のidはhttps://wiki3.jp/valheimjp/page/59を参照
                        new RequirementConfig {
                            Item = "Wood", // 作成時必要素材ID
                            Amount = 10, // 作成時必要素材数
                            AmountPerLevel = 3, // アップグレード時必要素材数
                        },
                        new RequirementConfig {
                            Item = "Flint", // 作成時必要素材ID
                            Amount = 5, // 作成時必要素材数
                            AmountPerLevel = 1, // アップグレード時必要素材数
                        },
                    }
                });
            ItemManager.Instance.AddItem(targetItem);
        }

    }
}