using UnityEngine;

using BepInEx;
using Jotunn.Configs;
using Jotunn.Entities;
using Jotunn.Managers;
using Jotunn.Utils;

namespace SamplePiece
{
    [BepInPlugin(PluginGUID, PluginName, PluginVersion)]
    [BepInDependency(Jotunn.Main.ModGuid)]
    [NetworkCompatibility(CompatibilityLevel.EveryoneMustHaveMod, VersionStrictness.Major)]
    internal class SamplePieceClass : BaseUnityPlugin
    {
        // GUID guidなら多分被るとまずい筈
        public const string PluginGUID = "zzzM150_SamplePiece_001";
        // あなたのmodの名前
        public const string PluginName = "SamplePiece";
        public const string PluginVersion = "0.0.1";

        // unityでつけたAssetBundleの名前
        public const string AssetBundleName = "samplecubeassetbundle";
        public static AssetBundle TargetAssetBundle;
        // unityでつけたプレハブの名前
        public const string TargetPrefabName = "SamplePiece001";


        // 作成時近くに必要な作業台のid 作業台:piece_workbench , 鍛冶場:forge, 職人テーブル:piece_artisanstation
        public const string CraftingStationPrefabName = "piece_workbench";

        // 使用する道具名 ハンマー=Hammer, 鍬=Hoe, 耕運具=Cultivator
        public const string CraftingToolName = "Hammer";

        private void Awake()
        {
            TargetAssetBundle = AssetUtils.LoadAssetBundleFromResources(AssetBundleName, typeof(SamplePieceClass).Assembly);
            var targetAsset = TargetAssetBundle.LoadAsset<GameObject>(TargetPrefabName);

            var targetPiece = new CustomPiece(targetAsset,
                new PieceConfig
                {
                    CraftingStation = CraftingStationPrefabName,
                    Enabled = true,
                    PieceTable = CraftingToolName,
                    Requirements = new[]
                    {
                        // 作成に必要なアイテム
                        // この場合、木材２、火打石１が作成に必要で
                        // 破壊時には木材は返ってくるが火打石は返ってこない
                        new RequirementConfig {
                            Item = "Wood",
                            Amount = 2,
                            Recover = true, // 破壊時にこの材料が返ってくるか?
                        },
                        new RequirementConfig {
                            Item = "Flint",
                            Amount = 1,
                            Recover = false,
                        },
                    }
                }
                );
            PieceManager.Instance.AddPiece(targetPiece);
        }

    }
}