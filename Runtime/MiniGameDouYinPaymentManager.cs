// GameFrameX 组织下的以及组织衍生的项目的版权、商标、专利和其他相关权利均受相应法律法规的保护。使用本项目应遵守相关法律法规的许可证的要求。
//
// 本项目主要遵循 MIT 许可证和 Apache 许可证（版本 2.0）进行分发和使用。许可证位于源代码树根目录中的 LICENSE 文件。
//
// 不得利用本项目从事危害国家安全、扰乱社会秩序、侵犯他人合法权益等法律法规禁止的活动！任何基于本项目二次开发而产生的一切法律纠纷和责任，我们不承担任何责任！

using System;
using System.Collections.Generic;
using GameFrameX.Payment.Runtime;
using GameFrameX.Runtime;

namespace GameFrameX.Payment.Minigame.Douyin.Runtime
{
    /// <summary>
    /// 抖音支付管理器
    /// </summary>
    [UnityEngine.Scripting.Preserve]
    public sealed class MiniGameDouYinPaymentManager : BasePaymentManager
    {
        [UnityEngine.Scripting.Preserve]
        public MiniGameDouYinPaymentManager()
        {
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="isDebug">是否是沙盒模式</param>
        /// <param name="isClientVerify">是否执行客户端验证购买成功，如果是强联网验证。不需要开启，设置为false，进行服务器验证</param>
        [UnityEngine.Scripting.Preserve]
        public override void Init(bool isDebug = false, bool isClientVerify = false)
        {
            Log.Warning("抖音支付不需要初始化");
        }

        /// <summary>
        /// 支付系统是否准备好
        /// </summary>
        /// <returns>准备好返回true，否则返回false</returns>
        [UnityEngine.Scripting.Preserve]
        public override bool IsReady()
        {
            return true;
        }

        /// <summary>
        /// 查询购买记录
        /// </summary>
        /// <param name="productType">产品类型</param>
        [UnityEngine.Scripting.Preserve]
        public override void QueryPurchases(PaymentProductType productType)
        {
            Log.Warning("抖音支付不需要查询购买记录");
        }

        /// <summary>
        /// 消耗购买
        /// </summary>
        /// <param name="purchaseToken">购买令牌</param>
        [UnityEngine.Scripting.Preserve]
        public override void ConsumePurchase(string purchaseToken)
        {
            Log.Warning("抖音支付不需要执行消耗购买");
        }

        /// <summary>
        /// 购买 一次性商品
        /// </summary>
        [UnityEngine.Scripting.Preserve]
        [Obsolete("请使用 Buy(PurchaseParams) 替代")]
        public override void BuyInApp(string productId, string orderId, string offerToken = "", string customData = "")
        {
            throw new NotImplementedException("请使用 Buy(PurchaseParams) 传入 DouyinPurchaseParams 来购买");
        }

        /// <summary>
        /// 购买 订阅商品
        /// </summary>
        [UnityEngine.Scripting.Preserve]
        [Obsolete("请使用 Buy(PurchaseParams) 替代")]
        public override void BuySubs(string productId, string orderId, string offerToken = "", string customData = "")
        {
            throw new NotImplementedException("抖音支付不支持购买订阅商品");
        }

        /// <summary>
        /// 购买
        /// </summary>
        [UnityEngine.Scripting.Preserve]
        [Obsolete("请使用 Buy(PurchaseParams) 替代")]
        public override void Buy(string productId, PaymentProductType productType, string orderId, string offerToken = "", string customData = "")
        {
            throw new NotImplementedException("请使用 Buy(PurchaseParams) 传入 DouyinPurchaseParams 来购买");
        }

        /// <summary>
        /// 购买（推荐使用）
        /// </summary>
        /// <param name="purchaseParams">购买参数，必须使用 DouyinPurchaseParams</param>
        [UnityEngine.Scripting.Preserve]
        public override void Buy(PurchaseParams purchaseParams)
        {
            if (!(purchaseParams is DouyinPurchaseParams douyinParams))
            {
                throw new ArgumentException("抖音支付请使用 DouyinPurchaseParams", nameof(purchaseParams));
            }

            DoPayment(douyinParams.ProductId, douyinParams.OrderId, douyinParams.Money, douyinParams.GoodName ?? "", douyinParams.ExtraInfo ?? "");
        }

        private void DoPayment(string productId, string orderId, int money, string goodName, string extraInfo)
        {
            Log.Debug($"Pay itemID: {productId}, money: {money}, orderId: {orderId}, goodName: {goodName}, extData: {extraInfo}");

            if (goodName.Length > 10)
            {
                Log.Warning($"抖音支付 goodName 长度不能超过10个字符，当前长度为 {goodName.Length} ，将会执行截取前10个字符");
                goodName = goodName.Substring(0, 10);
            }

            if (TTSDK.TT.GetSystemInfo().platform.ToLower() == "android")
            {
                var requestGamePaymentParam = new Dictionary<string, object>()
                {
                    { "mode", "game" },
                    { "env", "0" },
                    { "currencyType", "CNY" },
                    { "platform", "android" },
                    { "zoneId", "1" },
                    { "customId", orderId },
                    { "extraInfo", extraInfo },
                    { "goodType", 2 },
                    { "orderAmount", money },
                    { "goodName", goodName },
                };

                Log.Debug($"RequestGamePayment requestGamePaymentParam: {Utility.Json.ToJson(requestGamePaymentParam)}");
                TTSDK.TT.RequestGamePayment(requestGamePaymentParam);
            }
            else
            {
                if (!TTSDK.CanIUse.OpenAwemeCustomerServiceParams.GoodType)
                {
                    Log.Error("抖音支付不支持购买商品类型");
                    return;
                }

                var jsonData = new TTSDK.UNBridgeLib.LitJson.JsonData();
                jsonData["orderAmount"] = money;
                jsonData["goodName"] = goodName;
                jsonData["extraInfo"] = extraInfo;
                jsonData["customId"] = orderId;
                jsonData["zoneId"] = "1";
                jsonData["goodType"] = 2;
                jsonData["currencyType"] = "DIAMOND";
                Log.Debug($"OpenAwemeCustomerService openAwemeCustomerServiceParam: {jsonData.ToJson()}");
                TTSDK.TT.OpenAwemeCustomerService(jsonData, null, null);
            }
        }

        /// <summary>
        /// 设置预加载的预定义商品ID,用于预加载缓存,注意：此方法必须在Initialize()之前调用
        /// </summary>
        /// <param name="inAppProductIds">内购商品ID列表</param>
        /// <param name="subsProductIds">订阅商品ID列表</param>
        [UnityEngine.Scripting.Preserve]
        public override void SetPredefinedProductIds(List<string> inAppProductIds, List<string> subsProductIds)
        {
        }
    }
}
