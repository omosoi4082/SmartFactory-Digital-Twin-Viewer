# View ↔ Move 연동 가이드 (리스트 ↔ 3D 로봇)

내일 적용·확인할 때 참고용 문서입니다.

---

## 동작 요약

| 동작 | 결과 |
|------|------|
| **리스트(View) 호버** | 3D 로봇(Move) 아웃라인 표시 |
| **리스트(View) 클릭** | 해당 로봇으로 카메라 줌 |
| **3D 로봇(Move) 호버** | 아웃라인 표시 |
| **3D 로봇(Move) 클릭** | 리스트 해당 행 확장(상세보기) + 레이아웃 갱신 |

---

## 수정·추가된 파일

### 수정
- `Scripts/Robot/View/RobotView.cs`  
  - 호버 이벤트: `OnHoverEnter`, `OnHoverExit`, `IPointerEnterHandler`/`IPointerExitHandler`
- `Scripts/Robot/View/RobotMoveView.cs`  
  - 아웃라인: `outlineDisplay`(GameObject), `outlineBehaviour`(Behaviour)  
  - 이벤트: `OnClicked`, `OnHoverEnter`, `OnHoverExit`  
  - `SetOutlineVisible(bool)`, `OnMouseDown`/`OnMouseEnter`/`OnMouseExit`
- `Scripts/Robot/Presenter/RobotPresenter.cs`  
  - 생성자에 `robotId`, `ICameraFocus`, `Action<string> onRequestRefreshLayout`  
  - View/Move 호버·클릭 구독 후 아웃라인·카메라·리스트 확장 연동
- `Scripts/Robot/Factory/RobotPresenterFactory.cs`  
  - 생성자에 `ICameraFocus`, `Action<string> onRequestRefreshLayout`  
  - Presenter 생성 시 `view.robotId` 설정
- `Scripts/Core/Bootstrap/CoreBootstrapper.cs`  
  - `_cameraFocusController`, `_listLayoutController` SerializeField  
  - Factory 생성 시 위 두 개 전달
- `Scripts/UI/ListRootLayoutController.cs`  
  - `content` 자식 순회 방식 수정 (GetChild + GetComponent)  
  - 에디터 전용 `using static UnityEditor.Progress` 제거

### 새로 추가
- `Scripts/Core/Interfaces/ICameraFocus.cs`  
  - `void Focus(Vector3 worldPosition);`
- `Scripts/Core/CameraFocusController.cs`  
  - `MonoBehaviour`, `ICameraFocus` 구현  
  - Target Camera, Zoom Distance, Height Offset, Move Speed 설정

---

## 씬에서 할 설정

1. **RobotMoveView (3D 로봇 프리팹)**  
   - **Collider**: 클릭/호버를 위해 같은 오브젝트 또는 자식에 Collider(예: BoxCollider) 필수.  
   - **아웃라인** (택 1):  
     - **방법 A**: 아웃라인용 자식 GameObject 만들고 인스펙터에서 `Outline Display`에 할당.  
     - **방법 B**: Outline 컴포넌트 사용 시 `Outline Behaviour`에 할당.

2. **CameraFocusController**  
   - 빈 GameObject에 `CameraFocusController` 추가.  
   - `Target Camera`: 메인 카메라(비우면 Camera.main 사용).  
   - `Zoom Distance`, `Height Offset`, `Move Speed`는 원하는 뷰에 맞게 조정.

3. **CoreBootstrapper (씬 오브젝트)**  
   - `Camera Focus Controller`: 위에서 만든 CameraFocusController 할당.  
   - `List Layout Controller`: 리스트 쪽 ListRootLayoutController 할당.

---

## 참고

- 아웃라인은 유니티 기본 컴포넌트가 없을 수 있어, **Outline Display**에 켜/끌 자식 오브젝트를 두는 방식으로 구현되어 있음.  
- 카메라 줌은 `CameraFocusController`의 `Focus(worldPosition)` 호출로 동작함.

이 가이드는 `Assets/SmartFactoryDTViewer/View_Move_연동_가이드.md` 에 저장되어 있습니다.
