using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

/// <summary>
/// 테스트용 데이터 소스.
/// - robotIds 있음: SO 설정 기반 다중 로봇 시뮬레이션 (확장)
/// </summary>
public class FakeRobotDataSource : IRobotDataSource
{
    private readonly RobotDataMapper _dataMapper;
    private CancellationTokenSource _canceTokenSource;
    private readonly List<string> _robotIds = new()
    {
        "Robot-01",
        "Robot-02",
        "Robot-03"
    };
  
    public FakeRobotDataSource(RobotDataMapper dataMapper)
    {
        _dataMapper = dataMapper ?? throw new ArgumentNullException(nameof(dataMapper));

    }

    public void RobotStart()
    {
        _canceTokenSource=new CancellationTokenSource();   //여기인가? 
        RunsimulationAsync(_canceTokenSource.Token).Forget();
    }

    public void RobotStop()
    {
        _canceTokenSource?.Cancel();
        _canceTokenSource?.Dispose();
        _canceTokenSource=null;
    }
    private async UniTaskVoid RunsimulationAsync(CancellationToken token)//취소확인
    {
        try
        {
            while(!token.IsCancellationRequested)//취소 신호
            {
                foreach (var item in _robotIds)
                {
                    var dto = CresteFakeDto(item);
                    _dataMapper.Apply(dto);
                }
                await UniTask.Delay(1000, cancellationToken: token);//1초대기,Delay 중에도 취소 가능
            }
        }
        catch (OperationCanceledException) //정상적으로 ‘취소되었다’는 신호
        {
            // 취소 토큰으로 정상 취소
        }
    }

    private RobotMpttDto CresteFakeDto(string id)
    {
        return new RobotMpttDto
        {
            robotId = id,
            battery = UnityEngine.Random.Range(10f, 100f),
            px = UnityEngine.Random.Range(-5f, 5f),
            py = 0,
            pz = UnityEngine.Random.Range(-5f, 5f),
            yaw = UnityEngine.Random.Range(0, 360),
            hasPayload = UnityEngine.Random.value > 0.5f
        };
    }

}
