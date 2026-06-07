<div align="center">

<img src="https://download.alianblank.com/gameframex/gameframex_logo_320.png" alt="Game Frame X Logo" width="160" />

# Game Frame X Payment MiniGame DouYin

[![License](https://img.shields.io/github/license/GameFrameX/com.gameframex.unity.payment.minigame.douyin)](https://github.com/GameFrameX/com.gameframex.unity.payment.minigame.douyin/blob/main/LICENSE.md)
[![Version](https://img.shields.io/github/v/release/GameFrameX/com.gameframex.unity.payment.minigame.douyin)](https://github.com/GameFrameX/com.gameframex.unity.payment.minigame.douyin/releases)
[![Unity Version](https://img.shields.io/badge/Unity-2019.4-black?logo=unity)](https://unity.com/)
[![Documentation](https://img.shields.io/badge/Documentation-docs-blue)](https://gameframex.doc.alianblank.com)

インディゲーム開発者向けオールインワンソリューション · インディ開発者の夢を支援

<br />

[ドキュメント](https://gameframex.doc.alianblank.com) · [クイックスタート](#クイックスタート) · QQグループ: 467608841 / 233840761

<br />

[English](README.md) | [简体中文](README.zh-CN.md) | [繁體中文](README.zh-TW.md) | **日本語** | [한국어](README.ko.md)

</div>

## 言語

[English](README.md) | [简体中文](README.zh-CN.md) | [繁體中文](README.zh-TW.md) | **日本語** | [한국어](README.ko.md)

---

## プロジェクト概要

このパッケージは、GameFrameX.Payment の「抖音ミニゲーム決済」アダプターです。`BasePaymentManager`（`MiniGameDouYinPaymentManager`）を実装し、抖音ミニゲーム環境で TTSDK を通じてゲーム内商品の決済リクエストを発行します。

## サポート範囲

- 決済の開始：一回限り商品（統一された `Buy(...)` インターフェースを使用）。
- Android：`TTSDK.TT.RequestGamePayment(...)` を呼び出し。
- 非 Android：`TTSDK.TT.OpenAwemeCustomerService(...)` を呼び出し（`CanIUse.OpenAwemeCustomerServiceParams.GoodType` が true である必要があります）。

## 未サポート / スタブ実装

- `BuyInApp(...)`：例外をスロー（`Buy(...)` を使用してください）。
- `BuySubs(...)`：例外をスロー（抖音決済はサブスクリプション商品をサポートしていません）。
- `QueryPurchases(...)` / `ConsumePurchase(...)`：警告ログのみ出力し、実際のロジックは実行しません。
- `SetPredefinedProductIds(...)`：空の実装。
- `Init(...)`：警告ログのみ出力（抖音決済は初期化を必要としません）。

## クイックスタート

### インストール

`Packages/manifest.json` に依存関係を追加（Git 方式）：

```json
{
  "dependencies": {
    "com.gameframex.unity.payment.minigame.douyin": "https://github.com/gameframex/com.gameframex.unity.payment.minigame.douyin.git"
  }
}
```

### 使用方法

1. シーンに GameObject を作成し、`PaymentComponent` を追加します。
2. 同じ GameObject に `GameFrameXPaymentMiniGameDouYinCroppingHelper` を追加します（ビルド時に `MiniGameDouYinPaymentManager` を参照し、ストリッピングを防止するため）。
3. `PaymentComponent.Buy(...)` を使用して決済を開始します。

例：

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

## Buy パラメータマッピング

`MiniGameDouYinPaymentManager.Buy(productId, productType, orderId, offerToken, obfuscatedProfileId)` の内部マッピングは以下の通りです：

- `productId`：ログ出力にのみ使用（`itemID`）。
- `productType`：`Convert.ToInt32(productType)` で `money` に変換され、そのまま `orderAmount` として渡されます（100倍の処理は行いません）。
- `orderId`：`customId` に渡されます。
- `offerToken`：`goodName` に渡されます。
- `offerToken` の長さ制限：`goodName`（`offerToken`）が10文字を超える場合、最初の10文字に切り詰められ、警告ログが出力されます。
- `obfuscatedProfileId`：`extraInfo` に渡されます。

## 決済結果処理について

このパッケージは抖音決済リクエストの発行のみを行います。TTSDK の決済結果コールバックや GameFrameX.Payment の結果イベントブリッジはパッケージ内にカプセル化されていません。クライアント側で結果を取得するには、抖音 TTSDK が提供するコールバック/イベントメカニズムを使用するか、サーバー側の注文コールバックに依存して配送と検証を行ってください。

## プラットフォーム対応

| プラットフォーム | 対応 |
|------------------|------|
| 抖音ミニゲーム   | はい |

## 変更履歴

詳細は [CHANGELOG.md](CHANGELOG.md) をご覧ください。

## ライセンス

このプロジェクトは Apache-2.0 ライセンスの下で公開されています。詳細は [LICENSE.md](LICENSE.md) ファイルをご覧ください。
