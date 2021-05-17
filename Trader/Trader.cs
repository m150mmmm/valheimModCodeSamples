using System.Collections.Generic;

using UnityEngine;
using HarmonyLib;

using Jotunn.Entities;
using Jotunn.Managers;

/**
* 商人の売り物リストにアイテムを追加する
**/

namespace WeWantEatRice.trader
{
    public static class TraderClass
    {

        private static readonly Harmony harmony = new Harmony("sampleTrader_001");

        private static CustomItem TargetCustomItem;
        private static string targetPrefabName;

        private static int targetItemAmount;
        private static int targetItemPrice;

        /**
        * 使う側はこれを呼べばOK
        * ci: JotunnのCustomItem
        * amount: 売ってくれる時のアイテム数
        * price: 値段
        **/
        public static void addTraderItems(CustomItem ci, int amount, int price)
        {
            TargetCustomItem = ci;
            targetItemAmount = amount;
            targetItemPrice = price;
            targetPrefabName = ci.ItemDrop.name;
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
                targetTradeItem = new Trader.TradeItem();
                targetTradeItem.m_stack = targetItemAmount;
                targetTradeItem.m_price = targetItemPrice;
                targetTradeItem.m_prefab = TargetCustomItem.ItemDrop;

                // 商人の売り物アイテムリストにaddしたいアイテムがすでに存在するかどうかをチェック
                var isAlreadyAdd = false;
                foreach (Trader.TradeItem item in ___m_items)
                {
                    if (!isAlreadyAdd)
                    {
                        isAlreadyAdd = targetPrefabName == item.m_prefab.name;
                  
                    }
                }
                // 存在しない場合アイテムを追加する
                if (!isAlreadyAdd)
                {
                    Debug.Log(string.Format("Added Item to the Trader Item List: {0}", targetPrefabName));
                    ___m_items.Add(targetTradeItem);
                }
            }
        }
        
        


    }
}
