# Cat Hero Playable Ad — Architecture Document
> `Assets/GameDuo/Script/InGame/` 아래의 모든 시스템을 기술한다.

---

## 1. 폴더 구조 및 책임

```
Assets/GameDuo/
├── Scene/
│   └── Play.unity              ← 이미 존재; InGame 씬으로 활용
└── Script/
    └── InGame/
        ├── Core/
        │   ├── GameManager.cs      — 상태 머신, 페이즈 진입, 엔딩 제어
        │   ├── AdSequencer.cs      — 20초 타임라인 코루틴
        │   ├── ObjectPool.cs       — 제네릭 풀 + IPoolable 인터페이스
        │   └── PoolRegistry.cs     — 모든 풀의 단일 진입점 (Singleton)
        ├── Input/
        │   └── VirtualJoystick.cs  — IPointerDownHandler/IDragHandler 기반
        ├── Player/
        │   ├── PlayerController.cs — Rigidbody2D + 조이스틱 이동, 반경 제한
        │   └── PlayerHealth.cs     — HP, 비네트·슬라이더 갱신, 사망 처리
        ├── Cat/
        │   ├── CatOrbitManager.cs  — 원형 대형 + 대형 회전, AddCats/Set* API
        │   └── CatUnit.cs          — 독립 타겟 탐색 + 멀티샷 발사 루프
        ├── Projectile/
        │   ├── HomingRocket.cs     — 조향 호밍, OnHit AoE, IPoolable
        │   └── ExplosionFX.cs      — 애니메이터 재생 후 자동 반납, IPoolable
        ├── Enemy/
        │   ├── EnemyUnit.cs        — 플레이어 추적, 접촉 데미지, IPoolable
        │   ├── WaveSpawner.cs      — 원형 링 스폰, 시간별 밀도 증가
        │   └── DamagePopup.cs      — 상승·페이드 팝업, IPoolable
        ├── Upgrade/
        │   └── UpgradeSystem.cs    — 3페이즈 고정 테이블, CatOrbitManager 위임
        ├── FX/
        │   ├── CameraShake.cs      — 로컬 위치 진동
        │   ├── HitStop.cs          — timeScale 순간 감소
        │   └── VignetteController.cs — Sin 맥박 비네트, 페이드 아웃
        └── UI/
            ├── UpgradePanel.cs     — 3버튼 선택 UI, phase별 레이블
            └── HUDController.cs    — HP바, 업그레이드 패널 표시, 엔딩 화면
```

---

## 2. 시스템 상호작용 개요

```
[AdSequencer]
    │  타임라인 경과
    ▼
[GameManager]  ←───────────────────────────────────────┐
    │ EnterPhase(1/2/3)                                 │
    ▼                                                   │
[HUDController] → ShowUpgradePanel(phase)               │
    │                                                   │
    ▼                                                   │
[UpgradePanel] — 버튼 선택 → GameManager.OnUpgradeSelected()
                                    │
                                    ▼
                            [UpgradeSystem]
                                    │
                 ┌──────────────────┼──────────────────┐
                 ▼                  ▼                   ▼
          AddCats(n)     SetAttackSpeedMult(x)   SetMultishot(n)
                 └──────────────────┴──────────────────┘
                                    │
                            [CatOrbitManager]
                                    │
                    ┌───────────────┴──────────────┐
                    ▼                              ▼
              [CatUnit ×n]                  (위치 업데이트)
                    │ FindClosestEnemy()
                    │ FireRockets()
                    ▼
            [PoolRegistry.Rockets.Get()]
                    │
                    ▼
            [HomingRocket] → OnTriggerEnter2D(Enemy)
                    │
         ┌──────────┴──────────┐
         ▼                     ▼
   [EnemyUnit.TakeDamage]  [PoolRegistry.Explosions.Get()]
         │                     │
         ▼                     ▼
   [DamagePopup]          [ExplosionFX]
   [PoolRegistry.Popups]

[WaveSpawner]
    │ 원형 링 스폰
    ▼
[PoolRegistry.Enemies.Get()] → [EnemyUnit] → PlayerHealth.TakeDamage()
                                                    │
                                                    ▼
                                          [VignetteController]
                                          [CameraShake]
```

---

## 3. 단계별 구현 계획 (안전한 커밋 단위)

| # | 커밋 | 내용 | 검증 |
|---|------|------|------|
| 1 | `[InGame] Core 기반 구축` | GameManager, AdSequencer, ObjectPool, PoolRegistry 스켈레톤 | 씬 실행 시 오류 없음 |
| 2 | `[InGame] VirtualJoystick + PlayerController` | 조이스틱 이동, 반경 제한, 스프라이트 반전 | 에디터에서 마우스 드래그로 이동 확인 |
| 3 | `[InGame] PlayerHealth + VignetteController` | HP 감소, 비네트 맥박, HP 슬라이더 | HP 감소 시 빨간 테두리 확인 |
| 4 | `[InGame] WaveSpawner + EnemyUnit` | 원형 링 스폰, 플레이어 추적, 풀링 | 40마리 cap 확인 |
| 5 | `[InGame] HomingRocket + ExplosionFX` | 호밍, AoE 데미지, 풀 반납 | 로켓이 적에게 날아가 폭발 확인 |
| 6 | `[InGame] CatUnit + CatOrbitManager` | 오빗, 회전, 독립 공격 루프 | 고양이 3마리 오빗 + 자동 발사 확인 |
| 7 | `[InGame] UpgradeSystem + UpgradePanel` | 3페이즈 선택 UI | 업그레이드 선택 후 고양이 즉시 추가 확인 |
| 8 | `[InGame] HUDController + 엔딩` | HP바, Download Now 화면 | 20초 후 다운로드 화면 표시 확인 |
| 9 | `[InGame] CameraShake + HitStop` | 폭발 시 쉐이크·히트스톱 | 타격감 느낌 확인 |
| 10 | `[InGame] DamagePopup + 밸런스 튜닝` | 팝업, 스폰 속도 커브 조정 | 전체 20초 플레이 스루 확인 |

---

## 4. 비주얼 피드백 노트

### Camera Shake
- **소형 (로켓 폭발)**: duration 0.08s, magnitude 0.07
- **중형 (적 다수 동시 사망)**: duration 0.15s, magnitude 0.12
- **최종 슬로우모션 진입**: duration 0.3s, magnitude 0.2

### HitStop
- 폭발 1회당 0.06초, timeScale → 0.05
- 동시 다중 폭발 시 중첩 방지 (`if _active return`)

### Vignette
- 시작 시 즉시 critical 상태 → 강렬한 빨간 테두리
- 업그레이드 후 일시적으로 페이드 아웃 → 파워업 느낌

### Flash (추가 권장)
- 적 사망 시 흰색 플래시 머티리얼 1프레임
- `MaterialPropertyBlock` 사용 → 드로콜 유지

### Slow Motion (Phase 4)
- TimeScale = 0.15, duration 1.2s (real time)
- 로켓 궤적이 화면에 그려지는 찰나 → 컷 → Download Now

---

## 5. 밸런스 기준값

### 적 스폰
| 구간 | 간격 | 동시 스폰 | 누적 적 수 |
|------|------|-----------|------------|
| 0–5s | 0.50s | 1 | ~10 |
| 8–12s | 0.30s | 2 | ~26 |
| 15–20s | 0.18s | 3 | ~40 (cap) |

### 고양이 공격
| 업그레이드 | 공격 간격 | 비고 |
|-----------|----------|------|
| 기본 | 1.2s | — |
| Speed x1.5 | 0.8s | Phase 2 Low |
| Speed x2.0 | 0.6s | Phase 2 Mid |
| Speed x3.0 | 0.4s | Phase 2 High |

### 데미지
| 항목 | 값 |
|------|-----|
| 로켓 직격 | 15 |
| 로켓 AoE | 8 |
| 적 접촉 데미지 | 10 |
| 플레이어 시작 HP | 15 (max 100) |
| 적 HP | 30 |

### 고양이 수 커브
| 페이즈 | 옵션 A | 옵션 B | 옵션 C |
|--------|--------|--------|--------|
| Phase 1 (Cats) | +3 | +5 | +10 |
| Phase 3 (Multishot) | 3발 | 5발 | 7발 |

---

## 6. 씬 Hierarchy 권장 구조

```
[InGame Scene]
├── _GameManager         ← GameManager, AdSequencer, WaveSpawner
├── _Pools               ← PoolRegistry (런타임에 [Pools] 자식 생성)
├── _FX                  ← CameraShake, HitStop
│
├── Player               ← PlayerController, PlayerHealth, SpriteRenderer
│   └── CatOrbitRoot     ← CatOrbitManager
│
└── Canvas (ScreenSpace Overlay)
    ├── Joystick         ← VirtualJoystick, 투명 풀스크린 패널
    ├── HUD              ← HUDController, HP슬라이더
    ├── UpgradePanel     ← UpgradePanel (기본 비활성)
    ├── Vignette         ← VignetteController (전체화면 Image)
    └── DownloadScreen   ← (기본 비활성)
```
