using UnityEngine;

using System.Linq;
using System.Reflection;
using System.Collections.Generic;

using ValheimLib;
using ValheimLib.ODB;

using BepInEx;

// 名前
namespace ExampleSword
{
    [BepInDependency(ValheimLib.ValheimLib.ModGuid)]
    [BepInPlugin(ModGuid, ModName, ModVer)]
    // てきとーなクラス名
    public class ExampleSwordClass : BaseUnityPlugin
    {
        public const string ModGuid = "iDeathHD." + ModName;
        // あなたのmodの名前
        private const string ModName = "ExampleSword";
        private const string ModVer = "0.0.3";

        // unityでつけたAssetBundleName
        public const string AssetBundleName = "examplesword";
        public static AssetBundle TargetAssetBundle;

        // unityでSwordIronをコピーしてCustomItemsに移動させたやつの名前
        public const string TargetPrefabPath = "Assets/CustomItems/ExampleSword.prefab";
        public static GameObject TargetPrefab;

        // -------------------------------------------------------------------------------------

        internal static ExampleSwordClass Instance { get; private set; }

        private void Awake() {
            Debug.Log("Run Sample Mod Awake");

            var execAssembly = Assembly.GetExecutingAssembly();
            var resourceName = execAssembly.GetManifestResourceNames().Single(str => str.EndsWith(AssetBundleName));
            var stream = execAssembly.GetManifestResourceStream(resourceName);
            TargetAssetBundle = AssetBundle.LoadFromStream(stream);
            TargetPrefab = TargetAssetBundle.LoadAsset<GameObject>(TargetPrefabPath);

            // ---------------------------------------------------------------------------

            AddCustomRecipe();
            AddCustomItem();
            SetItemName();

            Instance = this;

            Debug.Log("Done Sample Mod Awake");
        }

        public static CustomItem CustomItem;
        public static CustomRecipe CustomRecipe;

        /*
         * レシピを追加する
         */
        private static void AddCustomRecipe() {
            var recipe = ScriptableObject.CreateInstance<Recipe>();

            // このレシピで作成できるアイテムを指定(unityをエクスポートしたファイルに指定する)
            recipe.m_item = TargetPrefab.GetComponent<ItemDrop>();

            // 必要素材
            // この場合木材4個,火打石2個で作成出来るレシピ
            // 素材のidはhttps://wiki3.jp/valheimjp/page/59を参照
            var neededResources = new List<Piece.Requirement>
            {
                MockRequirement.Create("Wood", 4),
                MockRequirement.Create("Flint", 2),
            };
            recipe.m_resources = neededResources.ToArray();

            // 作成できる作業台の指定(指定しない場合ハンマーのように作業台無しで作成出来る)
            // recipe.m_craftingStation = (XXX 書き方まだ調べてません)
            // 修復できる作業台の指定
            // recipe.m_repairStation = (XXX 書き方まだ調べてません)
            // 必要な作業台のレベル(int) (1なら書く必要なし)
            // recipe.m_minStationLevel = 1;

            CustomRecipe = new CustomRecipe(recipe, false, true);
            ObjectDBHelper.Add(CustomRecipe);
        }

        /*
         * アイテムの一覧に追加する 多分これをやるとspawnコマンドで出せるようになる
         */
        private static void AddCustomItem() {
            CustomItem = new CustomItem(TargetPrefab, true);
            ObjectDBHelper.Add(CustomItem);
        }

        // -------------------------------------------------------------------------------

        // アイテム名のID(unityで指定したID)
        public const string TargetItemNameId = "$custom_item_name";
        // アイテム名
        public const string TargetItemName_Ja = "ドーンブレイカー";
        public const string TargetItemName_En = "Dawnbreaker";

        // アイテム説明のID(unityで指定したID)
        public const string TargetDescId = "$custom_item_description";
        // アイテム説明
        public const string TargetDesc_Ja = "説明文";
        public const string TargetDesc_En = "Description";

        public const string CraftingStationPrefabName = "piece_workbench";

        // 言語リスト
        public const string TokenLanguage_Ja = "Japanese";
        public const string TokenLanguage_En = "English";

        /*
         * アイテムの名前、説明をセットする
         */
        private static void SetItemName()
        {
            // 日本語をセット
            Language.AddToken(TargetItemNameId, TargetItemName_Ja, TokenLanguage_Ja);
            Language.AddToken(TargetDescId, TargetDesc_Ja, TokenLanguage_Ja);

            // 英語をセット
            Language.AddToken(TargetItemNameId, TargetItemName_En, TokenLanguage_En);
            Language.AddToken(TargetDescId, TargetDesc_En, TokenLanguage_En);

            // XXX セットしていない言語での表示は未確認
        }
    }
}