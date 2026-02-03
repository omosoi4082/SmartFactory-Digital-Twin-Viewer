using System.Collections.Generic;
using UnityEngine;

public class RobotViewPool : IObjectPool<RobotView>
{
    private readonly RobotView _prefab;
    private readonly Transform _root;
    private readonly Stack<RobotView> _pool = new Stack<RobotView>();

    public RobotViewPool(RobotView prefab, Transform root)
    {
        _prefab = prefab;
        _root = root;
    }
    
    public RobotView Get()
    {
      if (_pool.Count > 0)
        {
            var view= _pool.Pop();
            view.gameObject.SetActive(true);
            return view;
        }
        return GameObject.Instantiate(_prefab, _root);//없으면
    }

    public void Release(RobotView item)
    {
        item.gameObject.SetActive(false);   
        _pool.Push(item);
    }

}
