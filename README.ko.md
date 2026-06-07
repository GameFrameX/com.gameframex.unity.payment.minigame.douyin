<div align="center">

<img src="https://download.alianblank.com/gameframex/gameframex_logo_320.png" alt="Game Frame X Logo" width="160" />

# Game Frame X Payment MiniGame DouYin

[![License](https://img.shields.io/github/license/GameFrameX/com.gameframex.unity.payment.minigame.douyin)](https://github.com/GameFrameX/com.gameframex.unity.payment.minigame.douyin/blob/main/LICENSE.md)
[![Version](https://img.shields.io/github/v/release/GameFrameX/com.gameframex.unity.payment.minigame.douyin)](https://github.com/GameFrameX/com.gameframex.unity.payment.minigame.douyin/releases)
[![Unity Version](https://img.shields.io/badge/Unity-2019.4-black?logo=unity)](https://unity.com/)
[![Documentation](https://img.shields.io/badge/Documentation-docs-blue)](https://gameframex.doc.alianblank.com)

인디 게임 개발자를 위한 올인원 솔루션 · 인디 개발자의 꿈을 실현

<br />

[문서](https://gameframex.doc.alianblank.com) · [빠른 시작](#빠른-시작) · QQ 그룹: 467608841 / 233840761

<br />

[English](README.md) | [简体中文](README.zh-CN.md) | [繁體中文](README.zh-TW.md) | [日本語](README.ja.md) | **한국어**

</div>
## 언어

[English](README.md) | [简体中文](README.zh-CN.md) | [繁體中文](README.zh-TW.md) | [日本語](README.ja.md) | **한국어**

---

## 프로젝트 개요

이 패키지는 GameFrameX.Payment의 '抖音 미니게임 결제' 어댑터입니다. `BasePaymentManager`(`MiniGameDouYinPaymentManager`)를 구현하여, 抖音 미니게임 환경에서 TTSDK를 통해 게임 내 상품 결제 요청을 시작합니다.

## 지원 범위

- 결제 시작: 일회성 상품 (통합된 `Buy(...)` 인터페이스 사용).
- Android: `TTSDK.TT.RequestGamePayment(...)` 호출.
- 비 Android: `TTSDK.TT.OpenAwemeCustomerService(...)` 호출 (`CanIUse.OpenAwemeCustomerServiceParams.GoodType`이 true여야 함).

## 미지원 / 스텁 구현

- `BuyInApp(...)`: 예외 발생 (`Buy(...)`를 사용하세요).
- `BuySubs(...)`: 예외 발생 (抖音 결제는 구독 상품을 지원하지 않음).
- `QueryPurchases(...)` / `ConsumePurchase(...)`: 경고 로그만 출력, 실제 로직 없음.
- `SetPredefinedProductIds(...)`: 빈 구현.
- `Init(...)`: 경고 로그만 출력 (抖音 결제는 초기화가 필요하지 않음).

## 빠른 시작

### 설치

`Packages/manifest.json`에 종속성 추가 (Git 방식):

```json
{
  "dependencies": {
    "com.gameframex.unity.payment.minigame.douyin": "https://github.com/gameframex/com.gameframex.unity.payment.minigame.douyin.git"
  }
}
```

### 사용

1. 씬에 GameObject를 만들고 `PaymentComponent`를 추가합니다.
2. 같은 GameObject에 `GameFrameXPaymentMiniGameDouYinCroppingHelper`를 추가합니다 (빌드 시 `MiniGameDouYinPaymentManager`를 참조하여 스트리핑을 방지).
3. `PaymentComponent.Buy(...)`를 사용하여 결제를 시작합니다.

예시:

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

## Buy 매개변수 매핑

`MiniGameDouYinPaymentManager.Buy(productId, productType, orderId, offerToken, obfuscatedProfileId)` 내부 매핑:

- `productId`: 로그 출력에만 사용 (`itemID`).
- `productType`: `Convert.ToInt32(productType)`로 `money`로 변환되어 `orderAmount`로 직접 전달됩니다 (이 패키지는 금액에 * 100을 하지 않습니다).
- `orderId`: `customId`로 전달됩니다.
- `offerToken`: `goodName`으로 전달됩니다.
- `offerToken` 길이 제한: `goodName`(즉, `offerToken`)이 10자를 초과하면 처음 10자로 잘리고 경고 로그가 출력됩니다.
- `obfuscatedProfileId`: `extraInfo`로 전달됩니다.

## 결제 결과 처리

이 패키지는 抖音 결제 요청 시작만 담당합니다. TTSDK 결제 결과 콜백과 GameFrameX.Payment 결과 이벤트 브리징은 패키지 내에 캡슐화되어 있지 않습니다. 클라이언트 측에서 결과를 받으려면 抖音 TTSDK에서 제공하는 콜백/이벤트 메커니즘을 사용하거나, 서버 측 주문 콜백에 의존하여 배송 및 검증을 완료하세요.

## 플랫폼 지원

| 플랫폼          | 지원 |
|-----------------|------|
| 抖音 미니게임   | 예   |

## 변경 로그

자세한 내용은 [CHANGELOG.md](CHANGELOG.md)를 참조하세요.

## 라이선스

이 프로젝트는 Apache-2.0 라이선스에 따라 배포됩니다. 자세한 내용은 [LICENSE.md](LICENSE.md) 파일을 참조하세요.
