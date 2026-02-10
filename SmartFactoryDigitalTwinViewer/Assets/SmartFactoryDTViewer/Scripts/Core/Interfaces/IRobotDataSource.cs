using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;

/// <summary>
/// 로봇 센서 데이터 소스 인터페이스 (MQTT, REST, Fake 등).
/// </summary>
public interface IRobotDataSource
{
    UniTask StartAsync(CancellationToken ct);
    UniTask StopAsync();
}
