using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;

public interface IRobotDataSource 
{
    UniTask StartAaync(CancellationToken ct);
    UniTask StopAaync();
}
