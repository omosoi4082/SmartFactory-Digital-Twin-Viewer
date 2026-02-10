using System.Collections.Concurrent;
using UnityEngine;

/// <summary>
/// 스레드 안전 로봇 DTO 큐. 외부 데이터 소스와 업데이트 러너 사이 버퍼.
/// </summary>
public class RobotDataQueue
{
    private readonly ConcurrentQueue<RobotMpttDto> _queue = new();

    public void Enqueue(RobotMpttDto dto)
    {
        if (dto != null)
            _queue.Enqueue(dto);
    }

    public bool TryDequeue(out RobotMpttDto dto)
    {
        return _queue.TryDequeue(out dto);
    }
}
