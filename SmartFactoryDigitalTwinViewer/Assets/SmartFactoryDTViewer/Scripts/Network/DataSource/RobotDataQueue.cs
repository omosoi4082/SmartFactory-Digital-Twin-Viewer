using System.Collections.Concurrent;
using UnityEngine;

public class RobotDataQueue 
{
   private readonly ConcurrentQueue<RobotMpttDto>_queue = new ();

    public void Enquene(RobotMpttDto dto)
    {
        _queue.Enqueue(dto);
    }
    public bool TryDequeue(out RobotMpttDto dto)//빈값안전
    {
        return _queue.TryDequeue(out dto);  
    }
}
