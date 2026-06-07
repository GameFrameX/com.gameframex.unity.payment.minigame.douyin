<div align="center">

<img src="https://download.alianblank.com/gameframex/gameframex_logo_320.png" alt="Game Frame X Logo" width="160" />

# Game Frame X Payment MiniGame DouYin

[![License](https://img.shields.io/github/license/GameFrameX/com.gameframex.unity.payment.minigame.douyin)](https://github.com/GameFrameX/com.gameframex.unity.payment.minigame.douyin/blob/main/LICENSE.md)
[![Version](https://img.shields.io/github/v/release/GameFrameX/com.gameframex.unity.payment.minigame.douyin)](https://github.com/GameFrameX/com.gameframex.unity.payment.minigame.douyin/releases)
[![Unity Version](https://img.shields.io/badge/Unity-2019.4-black?logo=unity)](https://unity.com/)
[![Documentation](https://img.shields.io/badge/Documentation-docs-blue)](https://gameframex.doc.alianblank.com)

All-in-One Solution for Indie Game Development · Empowering Indie Developers' Dreams

<br />

[Documentation](https://gameframex.doc.alianblank.com) · [Quick Start](#quick-start) · QQ Group: 467608841 / 233840761

<br />

**English** | [简体中文](README.zh-CN.md) | [繁體中文](README.zh-TW.md) | [日本語](README.ja.md) | [한국어](README.ko.md)

</div>

## Language

**English** | [简体中文](README.zh-CN.md) | [繁體中文](README.zh-TW.md) | [日本語](README.ja.md) | [한국어](README.ko.md)

---

## Project Overview

This package is the Douyin Mini Game payment adapter for GameFrameX.Payment. It implements a `BasePaymentManager` (`MiniGameDouYinPaymentManager`) for initiating in-game product payment requests through TTSDK in the Douyin Mini Game environment.

## Supported Features

- Payment initiation: One-time products (via the unified `Buy(...)` interface).
- Android: Calls `TTSDK.TT.RequestGamePayment(...)`.
- Non-Android: Calls `TTSDK.TT.OpenAwemeCustomerService(...)` (requires `CanIUse.OpenAwemeCustomerServiceParams.GoodType` to be true).

## Unsupported / Stub Implementations

- `BuyInApp(...)`: Throws an exception (use `Buy(...)` instead).
- `BuySubs(...)`: Throws an exception (Douyin payment does not support subscription products).
- `QueryPurchases(...)` / `ConsumePurchase(...)`: Only outputs warning logs, no actual logic.
- `SetPredefinedProductIds(...)`: Empty implementation.
- `Init(...)`: Only outputs a warning log (Douyin payment does not require initialization).

## Quick Start

### Installation

Add the dependency in `Packages/manifest.json` (Git method):

```json
{
  "dependencies": {
    "com.gameframex.unity.payment.minigame.douyin": "https://github.com/gameframex/com.gameframex.unity.payment.minigame.douyin.git"
  }
}
```

### Usage

1. Create a GameObject in the scene and add `PaymentComponent`.
2. Add `GameFrameXPaymentMiniGameDouYinCroppingHelper` to the same GameObject (used to reference `MiniGameDouYinPaymentManager` at build time to prevent stripping).
3. Use `PaymentComponent.Buy(...)` to initiate payment.

Example:

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

## Buy Parameter Mapping

`MiniGameDouYinPaymentManager.Buy(productId, productType, orderId, offerToken, obfuscatedProfileId)` maps internally as follows:

- `productId`: Used only for log output (`itemID`).
- `productType`: Converted to `money` via `Convert.ToInt32(productType)` and passed directly as `orderAmount` (this package does not multiply by 100).
- `orderId`: Passed to `customId`.
- `offerToken`: Passed to `goodName`.
- `offerToken` length limit: When `goodName` (i.e., `offerToken`) exceeds 10 characters, it is truncated to the first 10 characters with a warning log.
- `obfuscatedProfileId`: Passed to `extraInfo`.

## Payment Result Handling

This package only initiates the Douyin payment request. It does not encapsulate TTSDK payment result callbacks or bridge them with GameFrameX.Payment result events. To receive results on the client side, use the callbacks/events provided by the Douyin TTSDK, or rely on server-side order callbacks for fulfillment and verification.

## Platform Support

| Platform        | Supported |
|-----------------|-----------|
| Douyin MiniGame | Yes       |

## Changelog

See [CHANGELOG.md](CHANGELOG.md) for details.

## License

This project is licensed under the Apache-2.0 License - see the [LICENSE.md](LICENSE.md) file for details.
