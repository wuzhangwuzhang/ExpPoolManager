using UnityEngine;
using System.Collections;
using PathologicalGames;
using System;

public class ExpPoolManager : MonoBehaviour
{
	public SpawnPool ExpPool;
	public UIButton AddExp;
	public TweenPosition tweenPosition;
	PrefabPool prefabPool;

	// Use this for initialization
	void Start()
	{
		ExpPool = PoolManager.Pools["ExpPool"];
		if (!ExpPool._perPrefabPoolOptions.Contains(prefabPool))
		{
			prefabPool = new PrefabPool(Resources.Load<Transform>("LabExp"));
			//默认初始化一个Prefab
			prefabPool.preloadAmount = 1;
			//开启限制
			prefabPool.limitInstances = true;
			//关闭无限取Prefab
			prefabPool.limitFIFO = false;
			//限制池子里最大的Prefab数量
			prefabPool.limitAmount = 10;
			//开启自动清理池子
			prefabPool.cullDespawned = true;
			//最终保留
			prefabPool.cullAbove = 10;
			//多久清理一次
			prefabPool.cullDelay = 5;
			//每次清理几个
			prefabPool.cullMaxPerPass = 5;
			//初始化内存池
			ExpPool._perPrefabPoolOptions.Add(prefabPool);
			ExpPool.CreatePrefabPool(ExpPool._perPrefabPoolOptions[ExpPool.Count]);
		}
		else
		{
			Debug.Log("Already in prefabPool！");
		}
		UIEventListener.Get(AddExp.gameObject).onClick = OnAddExpClick;
		
	
	}

	void OnAddExpClick(GameObject go)
	{
		Transform labExp = ExpPool.Spawn("LabExp");
		labExp.transform.localScale = Vector3.zero;
		labExp.transform.localPosition = new Vector3(-200, 0, 0);
		labExp.GetComponent<TweenPosition>().PlayForward();
		labExp.GetComponent<TweenScale>().PlayForward();
		labExp.GetComponent<TweenAlpha>().PlayForward();
		StartCoroutine(ResetPrefab(labExp));
	}


	IEnumerator ResetPrefab(Transform obj)
	{
		Debug.Log(obj.name);
		yield return new WaitForSeconds(2f);
		obj.GetComponent<TweenPosition>().ResetToBeginning();
		obj.GetComponent<TweenScale>().ResetToBeginning();
		obj.GetComponent<TweenAlpha>().ResetToBeginning();
		ExpPool.Despawn(obj);
	}

}
