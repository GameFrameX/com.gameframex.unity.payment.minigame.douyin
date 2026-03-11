# GameFrameX.Payment.Minigame.Douyin

本包为 GameFrameX.Payment 的「抖音小游戏支付」适配器，实现了一个 `BasePaymentManager`（`MiniGameDouYinPaymentManager`），用于在抖音小游戏环境下通过 TTSDK 发起游戏内商品支付请求。

## 支持范围

- 支付发起：一次性商品（通过统一的 `Buy(...)` 接口）。
- Android：调用 `TTSDK.TT.RequestGamePayment(...)`。
- 非 Android：调用 `TTSDK.TT.OpenAwemeCustomerService(...)`（需 `CanIUse.OpenAwemeCustomerServiceParams.GoodType` 为 true）。

## 不支持/空实现

- `BuyInApp(...)`：抛出异常（请使用 `Buy(...)`）。
- `BuySubs(...)`：抛出异常（抖音支付不支持订阅商品）。
- `QueryPurchases(...)` / `ConsumePurchase(...)`：仅输出警告日志，不执行实际逻辑。
- `SetPredefinedProductIds(...)`：空实现。
- `Init(...)`：仅输出警告日志（抖音支付不需要初始化）。

## 依赖

- `GameFrameX.Payment.Runtime`（提供 `PaymentComponent` / `BasePaymentManager`）。
- 抖音小游戏 Unity SDK 相关程序集（本包运行时程序集引用：`TTWebGL`、`TTLitJson`）。

## 安装

在 `Packages/manifest.json` 中添加依赖（Git 方式）：

```json
{
  "dependencies": {
    "com.gameframex.unity.payment.minigame.douyin": "https://github.com/gameframex/com.gameframex.unity.payment.minigame.douyin.git"
  }
}
```

## 快速开始

1. 在场景中创建一个 GameObject，添加 `PaymentComponent`。
2. 同一个 GameObject 上添加 `GameFrameXPaymentMiniGameDouYinCroppingHelper`（用于在构建时引用 `MiniGameDouYinPaymentManager`，避免裁剪/剥离导致类型不可用）。
3. 使用 `PaymentComponent.Buy(...)` 发起支付。

示例：

```csharp
using GameFrameX.Payment.Runtime;
using UnityEngine;

public class DouyinPayExample : MonoBehaviour
{
    [SerializeField] private PaymentComponent payment;

    private void Awake()
    {
        if (payment == null)
        {
            payment = FindObjectOfType<PaymentComponent>();
        }

        payment.Init();
    }

    public void Pay(string itemId, int amount, string orderId, string goodName, string extraInfo)
    {
        payment.Buy(itemId, amount.ToString(), orderId, goodName, extraInfo);
    }
}
```

## Buy 参数映射（本包当前实现）

`MiniGameDouYinPaymentManager.Buy(productId, productType, orderId, offerToken, obfuscatedProfileId)` 内部做了如下映射：

- `productId`：仅用于日志输出（`itemID`）。
- `productType`：会被 `Convert.ToInt32(productType)` 转为 `money`，并直接作为 `orderAmount` 传入（本包不会对金额做 `* 100`）。
- `orderId`：传入 `customId`。
- `offerToken`：传入 `goodName`。
- `obfuscatedProfileId`：传入 `extraInfo`。

## 支付结果处理说明

本包当前只负责发起抖音支付请求，未在包内封装 TTSDK 的支付结果回调与 GameFrameX.Payment 的结果事件桥接。如需在客户端获知结果，请使用抖音 TTSDK 提供的回调/事件机制，或以服务端订单回调为准完成发货与校验。

## 许可证

该项目根据 Apache-2.0 许可证授权 - 有关详细信息，请参阅 [LICENSE.md](LICENSE.md) 文件。
