﻿using UnityEngine;

public class ParticleSystemAutoDestroy : PoolableObject
{
	private ParticleSystem ps;

	public void Start()
	{
		ps = GetComponent<ParticleSystem>();
	}

	public void Update()
	{
		if (ps && !ps.IsAlive())
		{
			PoolObject();
		}
	}
}