# 실제 데이터 소스(MQTT/REST) 전환 시

## 변경할 것 (한 곳만)

| 파일 | 작업 |
|------|------|
| `Scripts/Network/DataSource/RobotDataSourceFactory.cs` | `Create()` 반환값을 `MqttRobotDataSource` 등으로 교체 |

## 삭제할 것 (선택)

| 파일 | 비고 |
|------|------|
| `FakeRobotDataSource.cs` | 삭제 가능. 단위 테스트용으로 남겨둘 수도 있음 |

## 추가할 것

| 파일 | 설명 |
|------|------|
| `MqttRobotDataSource.cs` 등 | `IRobotDataSource` 구현, `mapper.Apply(dto)` 호출 |

## 전환 후 CoreBootstrapper

- `RobotDataSourceFactory.Create()` 호출만 유지 → 내부 구현만 바뀜
- 나머지 코드 수정 없음
