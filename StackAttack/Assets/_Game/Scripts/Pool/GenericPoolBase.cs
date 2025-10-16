using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public abstract class GenericPoolBase<TPool, TObject> : MonoSingleton<TPool>
	where TPool : GenericPoolBase<TPool, TObject>
	where TObject : Component
{
	[SerializeField] private TObject _prefab;
	[SerializeField] private int _size = 10;
	[SerializeField] private bool _expandable = true;

	private IObjectPool<TObject> _pool;

	public int Size => Mathf.Max(_size, 0);
	public bool Expandable => _expandable;

	protected override void Initialize()
	{
		base.Initialize();

		if (_prefab == null)
		{
			Debug.LogError($"{nameof(GenericPoolBase<TPool, TObject>)} requires a prefab reference on {name}.");
			return;
		}

		var initialSize = Mathf.Max(_size, 1);
		var maxSize = _expandable ? int.MaxValue : initialSize;

		_pool = new ObjectPool<TObject>(CreateItem, OnTakeFromPool, OnReturnedToPool, OnDestroyPoolObject, defaultCapacity: initialSize, maxSize: maxSize);

		Prewarm(initialSize);
	}

	public TObject GetFromPool()
	{
		if (_pool == null)
		{
			Debug.LogWarning($"Pool for {typeof(TObject).Name} on {name} is not initialized. Returning null.");
			return null;
		}

		return _pool.Get();
	}

	public void SendToPool(TObject instance)
	{
		if (_pool == null || instance == null)
		{
			return;
		}

		// Avoid releasing the same instance multiple times
		if (instance.gameObject.activeSelf == false)
		{
			return;
		}

		_pool.Release(instance);
	}

	private TObject CreateItem()
	{
		var created = Instantiate(_prefab, transform);
		created.gameObject.SetActive(false);
		return created;
	}

	private void OnTakeFromPool(TObject instance)
	{
		if (instance != null)
		{
			instance.gameObject.SetActive(true);
		}
	}

	private void OnReturnedToPool(TObject instance)
	{
		if (instance != null)
		{
			instance.gameObject.SetActive(false);
		}
	}

	private void OnDestroyPoolObject(TObject instance)
	{
		if (instance != null)
		{
			Destroy(instance.gameObject);
		}
	}

	private void Prewarm(int count)
	{
		if (_pool == null || count <= 0)
		{
			return;
		}

		var buffer = new List<TObject>(count);

		for (var i = 0; i < count; i++)
		{
			buffer.Add(_pool.Get());
		}

		for (var i = 0; i < buffer.Count; i++)
		{
			_pool.Release(buffer[i]);
		}
	}
}
