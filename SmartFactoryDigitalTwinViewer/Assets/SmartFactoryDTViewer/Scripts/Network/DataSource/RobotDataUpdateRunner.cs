using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;

public class RobotDataUpdateRunner
{
    private readonly RobotDataQueue _queue;
    private readonly RobotDataMapper _mapper;

    public RobotDataUpdateRunner(RobotDataQueue queue, RobotDataMapper mapper)
    {
        _queue = queue;
        _mapper = mapper;
    }

    public void Start(CancellationToken token)
    {
        RunAsync(token).Forget();
    }

    private async UniTask RunAsync(CancellationToken token)
    {
        try
        {
            while(!token.IsCancellationRequested)
            {
                while (_queue.TryDequeue(out var dot))
                {
                    _mapper.Apply(dot);
                }
                await UniTask.Yield(PlayerLoopTiming.Update, token);
            }
        }
        catch (OperationCanceledException)
        {
            //정상중지
        }
    }
}
