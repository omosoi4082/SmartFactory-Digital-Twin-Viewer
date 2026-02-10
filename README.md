# SmartFactory Digital Twin Viewer

스마트팩토리 로봇의 실시간 상태를 3D 디지털 트윈으로 시각화하는 Unity 프로젝트입니다.

---

## 프로젝트 구조

```
SmartFactoryDTViewer/
├── Scripts/
│   ├── Core/              # 핵심 도메인 및 시스템
│   ├── Network/           # 데이터 소스 및 통신
│   ├── Robot/             # 로봇 표시 계층 (MVP)
│   ├── UI/                # UI 레이아웃
│   └── Utils/             # 유틸리티
├── ScriptableObjects/     # 설정 에셋 (이벤트 채널, 데이터 소스 설정)
├── Prefabs/               # UI·로봇 프리팹
├── Models/                # 3D 모델
└── Scenes/                # 씬
```

---

## 아키텍처 개요

```
┌─────────────────────────────────────────────────────────────────────────┐
│                        외부 세계 (MQTT / REST / Fake)                     │
└───────────────────────────────────┬─────────────────────────────────────┘
                                    │ DTO
                                    ▼
┌─────────────────────────────────────────────────────────────────────────┐
│  IRobotDataSource  ──►  RobotDataQueue  ──►  RobotDataMapper             │
│  (Fake / MQTT)           (버퍼)              (DTO → Model 변환)           │
└───────────────────────────────────┬─────────────────────────────────────┘
                                    │
                                    ▼
┌─────────────────────────────────────────────────────────────────────────┐
│  RobotRegistry                                                           │
│  ├── RobotDataModel (보관)                                               │
│  ├── RobotStatusSystem (배터리 → Normal/Warning/Danger)                   │
│  ├── RobotPresenterFactory (View 생성 및 Presenter 관리)                  │
│  └── RobotEventChannelSO.Raise() → UI 갱신 알림                          │
└───────────────────────────────────┬─────────────────────────────────────┘
                                    │ 이벤트
                                    ▼
┌─────────────────────────────────────────────────────────────────────────┐
│  RobotPresenter (로봇 1대당 1개)                                         │
│  ├── RobotView (리스트 UI) ←── RobotViewModel (BindableReactiveProperty)  │
│  ├── RobotMoveView (3D 모델)                                             │
│  └── 카메라 포커스, 리스트 확장, 아웃라인 연동                            │
└─────────────────────────────────────────────────────────────────────────┘
```

---

## 주요 폴더별 역할

### Core

| 경로 | 역할 |
|------|------|
| `Bootstrap/CoreBootstrapper` | 시스템 조립, 의존성 주입, 라이프사이클 관리 |
| `Models/RobotDataModel` | 로봇 도메인 모델 (위치, 배터리, 상태 등) |
| `Systems/RobotRegistry` | 로봇 등록·갱신·이벤트 브로드캐스트 |
| `Systems/RobotStatusSystem` | 배터리 임계값 기반 상태 평가 (Normal/Warning/Danger) |
| `Systems/RobotLivenessSystem` | 연결 끊김 감지 (timeout) |
| `CameraFocusController` | 로봇 선택 시 카메라 이동·줌 |
| `Interfaces/` | `IRobotDataSource`, `ICameraFocus`, `IObjectPool` |

### Network

| 경로 | 역할 |
|------|------|
| `DataSource/RobotDataQueue` | 스레드 안전 DTO 버퍼 |
| `DataSource/RobotDataMapper` | DTO → RobotDataModel 매핑 |
| `DataSource/RobotDataUpdateRunner` | 큐 폴링 후 Registry.UpdateRobot 호출 |
| `DataSource/FakeRobotDataSource` | 테스트용 시뮬레이션 데이터 |
| `MQTT/MqttRobotDataSource` | MQTT 구독 기반 실시간 데이터 |
| `Config/RobotDataSourceConfig` | ScriptableObject 기반 데이터 소스 선택 |

### Robot (MVP 패턴)

| 경로 | 역할 |
|------|------|
| `View/RobotView` | 리스트 행 UI (클릭, 호버, 상세 확장) |
| `View/RobotMoveView` | 3D 로봇 오브젝트 (위치·회전, 아웃라인) |
| `ViewModel/RobotViewModel` | R3 BindableReactiveProperty 기반 바인딩 |
| `Presenter/RobotPresenter` | View ↔ Model 연동, 카메라·레이아웃 연동 |
| `Factory/RobotPresenterFactory` | Presenter 생성, 이벤트 구독 |
| `Factory/RobotViewPool` | 리스트 UI 오브젝트 풀링 |
| `Factory/RobotMoveViewPool` | 3D 로봇 오브젝트 풀링 |

### UI

| 경로 | 역할 |
|------|------|
| `ListRootLayoutController` | 리스트 확장 시 레이아웃 갱신 |

---

## 데이터 흐름

1. **데이터 유입**: `IRobotDataSource` → `RobotDataQueue.Enqueue(dto)`
2. **처리**: `RobotDataUpdateRunner`가 매 프레임 큐에서 DTO를 꺼내 `RobotDataMapper.Apply(dto)` 호출
3. **도메인 갱신**: `RobotRegistry.UpdateRobot()` → 모델 갱신, 상태 평가, `RobotEventChannelSO.Raise(robot)`
4. **UI 반영**: `RobotPresenterFactory`가 이벤트 구독 → `RobotPresenter.OnRobotUpdated()` → ViewModel 바인딩, View 갱신

---

## 설정 (ScriptableObject)

- **RobotEventChannelSO**: 로봇 갱신 이벤트 브로커
- **FakeRobotDataSourceConfig**: 테스트용 데이터 소스 설정
- **MqttRobotDataSourceConfig**: MQTT 데이터 소스 설정

`CoreBootstrapper`에서 `config`에 원하는 데이터 소스 Config를 할당하면 됩니다.

---
