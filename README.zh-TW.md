<div align="center">

<img src="https://download.alianblank.com/gameframex/gameframex_logo_320.png" alt="Game Frame X Logo" width="160" />

# Game Frame X Payment MiniGame DouYin

[![License](https://img.shields.io/github/license/GameFrameX/com.gameframex.unity.payment.minigame.douyin)](https://github.com/GameFrameX/com.gameframex.unity.payment.minigame.douyin/blob/main/LICENSE.md)
[![Version](https://img.shields.io/github/v/release/GameFrameX/com.gameframex.unity.payment.minigame.douyin)](https://github.com/GameFrameX/com.gameframex.unity.payment.minigame.douyin/releases)
[![Unity Version](https://img.shields.io/badge/Unity-2019.4-black?logo=unity)](https://unity.com/)
[![Documentation](https://img.shields.io/badge/Documentation-docs-blue)](https://gameframex.doc.alianblank.com)

獨立遊戲前後端一體化解決方案 · 獨立遊戲開發者的圓夢大使

<br />

[文檔](https://gameframex.doc.alianblank.com) · [快速開始](#快速開始) · QQ群: 467608841 / 233840761

<br />

[English](README.md) | [简体中文](README.zh-CN.md) | **繁體中文** | [日本語](README.ja.md) | [한국어](README.ko.md)

</div>
## 語言

[English](README.md) | [简体中文](README.zh-CN.md) | **繁體中文** | [日本語](README.ja.md) | [한국어](README.ko.md)

---

## 項目簡介

本套件為 GameFrameX.Payment 的「抖音小遊戲支付」適配器，實作了一個 `BasePaymentManager`（`MiniGameDouYinPaymentManager`），用於在抖音小遊戲環境下透過 TTSDK 發起遊戲內商品支付請求。

## 支援範圍

- 支付發起：一次性商品（透過統一的 `Buy(...)` 介面）。
- Android：呼叫 `TTSDK.TT.RequestGamePayment(...)`。
- 非 Android：呼叫 `TTSDK.TT.OpenAwemeCustomerService(...)`（需 `CanIUse.OpenAwemeCustomerServiceParams.GoodType` 為 true）。

## 不支援/空實作

- `BuyInApp(...)`：拋出例外（請使用 `Buy(...)`）。
- `BuySubs(...)`：拋出例外（抖音支付不支援訂閱商品）。
- `QueryPurchases(...)` / `ConsumePurchase(...)`：僅輸出警告日誌，不執行實際邏輯。
- `SetPredefinedProductIds(...)`：空實作。
- `Init(...)`：僅輸出警告日誌（抖音支付不需要初始化）。

## 快速開始

### 安裝

在 `Packages/manifest.json` 中新增依賴（Git 方式）：

```json
{
  "dependencies": {
    "com.gameframex.unity.payment.minigame.douyin": "https://github.com/gameframex/com.gameframex.unity.payment.minigame.douyin.git"
  }
}
```

### 使用

1. 在場景中建立一個 GameObject，新增 `PaymentComponent`。
2. 同一個 GameObject 上新增 `GameFrameXPaymentMiniGameDouYinCroppingHelper`（用於在建置時參照 `MiniGameDouYinPaymentManager`，避免裁剪/剝離導致類型不可用）。
3. 使用 `PaymentComponent.Buy(...)` 發起支付。

範例：

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

## Buy 參數映射

`MiniGameDouYinPaymentManager.Buy(productId, productType, orderId, offerToken, obfuscatedProfileId)` 內部映射如下：

- `productId`：僅用於日誌輸出（`itemID`）。
- `productType`：會被 `Convert.ToInt32(productType)` 轉為 `money`，並直接作為 `orderAmount` 傳入（本套件不會對金額做 `* 100`）。
- `orderId`：傳入 `customId`。
- `offerToken`：傳入 `goodName`。
- `offerToken` 長度限制：當 `goodName`（即 `offerToken`）長度超過 10 個字元時，會截斷為前 10 個字元並輸出警告日誌。
- `obfuscatedProfileId`：傳入 `extraInfo`。

## 支付結果處理說明

本套件當前只負責發起抖音支付請求，未在套件內封裝 TTSDK 的支付結果回調與 GameFrameX.Payment 的結果事件橋接。如需在客戶端得知結果，請使用抖音 TTSDK 提供的回調/事件機制，或以伺服器端訂單回調為準完成發貨與校驗。

## 平台支援

| 平台         | 支援 |
|--------------|------|
| 抖音小遊戲   | 是   |

## 更新日誌

詳見 [CHANGELOG.md](CHANGELOG.md)。

## 開源協議

該專案根據 Apache-2.0 協議授權 - 有關詳細資訊，請參閱 [LICENSE.md](LICENSE.md) 檔案。
