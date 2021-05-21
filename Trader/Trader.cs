using System.Collections.Generic;

using UnityEngine;
using HarmonyLib;

using Jotunn.Entities;
using Jotunn.Managers;


namespace WeWantEatRice.trader
{
    public static class TraderClass
    {

        private static readonly Harmony harmony = new Harmony("zzzM150_WeWantEatRice_001");

        private static List<CustomItem> TargetCustomItem = new List<CustomItem>();
        private static List<string> targetPrefabName = new List<string>();

        private static List<int> targetItemAmount = new List<int>();
        private static List<int> targetItemPrice = new List<int>();


        public static void addTraderItems(CustomItem ci, int amount, int price)
        {
            TargetCustomItem.Add(ci);
            targetItemAmount.Add(amount);
            targetItemPrice.Add(price);
            targetPrefabName.Add(ci.ItemDrop.name);
            harmony.PatchAll();
        }

        
        [HarmonyPatch(typeof(Trader), nameof(Trader.Interact))]
        class Trader_Patch
        {
            private static Trader.TradeItem targetTradeItem;

            /**
             * 商人に話しかけるとき毎回実行される(Trader.Interactが商人に話しかけた時実行されるスクリプト)
             * ___m_items: 商人の売り物アイテムリスト
             **/
            static void Prefix(ref List<Trader.TradeItem> ___m_items)
            {
                addItem(___m_items);
            }

            
            private static void addItem(List<Trader.TradeItem> ___m_items)
            {
                for (int i = 0; i < TargetCustomItem.Count; i++)
                {
                    targetTradeItem = new Trader.TradeItem();
                    targetTradeItem.m_stack = targetItemAmount[i];
                    targetTradeItem.m_price = targetItemPrice[i];
                    targetTradeItem.m_prefab = TargetCustomItem[i].ItemDrop;

                    // 商人の売り物アイテムリストにaddしたいアイテムがすでに存在するかどうかをチェック
                    var isAlreadyAdd = false;

                    foreach (Trader.TradeItem item in ___m_items)
                    {
                        if (!isAlreadyAdd)
                        {
                            isAlreadyAdd = targetPrefabName[i] == item.m_prefab.name;

                        }
                    }
                    // 存在しない場合アイテムを追加する
                    if (!isAlreadyAdd)
                    {
                        Debug.Log(string.Format("Added Item to the Trader Item List: {0}", targetPrefabName[i]));
                        ___m_items.Add(targetTradeItem);
                    }
                }
            }
        }
        
        


    }
}
