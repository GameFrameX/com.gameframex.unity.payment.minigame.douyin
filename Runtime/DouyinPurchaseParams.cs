// GameFrameX 组织下的以及组织衍生的项目的版权、商标、专利和其他相关权利均受相应法律法规的保护。使用本项目应遵守相关法律法规的许可证的要求。
//
// 本项目主要遵循 MIT 许可证和 Apache 许可证（版本 2.0）进行分发和使用。许可证位于源代码树根目录中的 LICENSE 文件。
//
// 不得利用本项目从事危害国家安全、扰乱社会秩序、侵犯他人合法权益等法律法规禁止的活动！任何基于本项目二次开发而产生的一切法律纠纷和责任，我们不承担任何责任！

using GameFrameX.Payment.Runtime;

namespace GameFrameX.Payment.Minigame.Douyin.Runtime
{
    /// <summary>
    /// 抖音小游戏购买参数
    /// </summary>
    public sealed class DouyinPurchaseParams : PurchaseParams
    {
        /// <summary>
        /// 支付金额（单位：分）
        /// </summary>
        public int Money { get; set; }

        /// <summary>
        /// 商品名称（最长10个字符）
        /// </summary>
        public string GoodName { get; set; }

        /// <summary>
        /// 自定义扩展数据
        /// </summary>
        public string ExtraInfo { get; set; }

        public DouyinPurchaseParams(string productId, string orderId, int money)
            : base(productId, orderId, PaymentProductType.InApp)
        {
            Money = money;
        }
    }
}
