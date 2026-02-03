using System.Collections.Generic;
using UnityEngine;

public class RobotMoveViewPool : IObjectPool<RobotMoveView>
{
    private readonly RobotMoveView _prefab;
    private readonly Transform _root;
    private readonly Stack<RobotMoveView> _pool=new();

    public RobotMoveViewPool(RobotMoveView view, Transform root)
    {
        _prefab = view;
        _root = root;
    }
    
    public RobotMoveView Get()
    {
       if (_pool.Count> 0)
        {
            var view= _pool.Pop();  
            view.gameObject.SetActive(true);
            return view;
        }
        return GameObject.Instantiate(_prefab, _root);
    }

    public void Release(RobotMoveView item)
    {
        item.gameObject.SetActive(false);
        _pool.Push(item);   
    }
}
