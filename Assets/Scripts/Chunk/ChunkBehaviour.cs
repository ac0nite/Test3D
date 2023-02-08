using UnityEngine;
using Utils;

public interface IChunkBehaviour
{
    void SetDefault();
    void Check();
}

public class ChunkBehaviour : IChunkBehaviour
{
    private Camera _camera;
    private const float Offset = 44.8f;
    
    private SimplePool<Chunk> _pool;
    private Chunk _top;
    private Chunk _center;
    private Chunk _bottom;
    private Coroutine _checkCoroutine;
    private Vector3 _topViewportPoint;
    private Vector3 _bottomViewportPoint;

    public ChunkBehaviour(Transform parent, Camera camera)
    {
        _pool = new SimplePool<Chunk>(parent.GetComponentsInChildren<Chunk>(true));
        _camera = camera;
    }
    
    public void SetDefault()
    {
        var active = _pool.Active();
        active.ForEach(c => _pool.SetPool(c));

        _center = _pool.GetPool().Active(0);
        _top = Spawn(Offset);
        _bottom = Spawn(-Offset);
    }

    public void Check()
    {
        _topViewportPoint = _camera.WorldToViewportPoint(_top.transform.position);
        _bottomViewportPoint = _camera.WorldToViewportPoint(_bottom.transform.position);

        if (_topViewportPoint.y < 0.8f)
            SpawnTop();
        else if (_bottomViewportPoint.y > 0.2f)
            SpawnBottom();
    }

    private void SpawnTop()
    {
        var position = _top.transform.position.y;
        _pool.SetPool(_bottom);
        _bottom = _center;
        _center = _top;
        _top = Spawn(position + Offset);
    }
    
    private void SpawnBottom()
    {
        var position = _bottom.transform.position.y;
        _pool.SetPool(_top);
        _top = _center;
        _center = _bottom;
        _bottom = Spawn(position - Offset);
    }

    private Chunk Spawn(float position)
    {
        return _pool.GetRandomPool().Active(position);
    }
}
