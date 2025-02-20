﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YooAsset;

internal class FsmUpdateStaticVersion : IFsmNode
{
	public string Name { private set; get; } = nameof(FsmUpdateStaticVersion);

	void IFsmNode.OnEnter()
	{
		PatchEventDispatcher.SendPatchStepsChangeMsg(EPatchStates.UpdateStaticVersion);
		BootScene.Instance.StartCoroutine(GetStaticVersion());
	}
	void IFsmNode.OnUpdate()
	{
	}
	void IFsmNode.OnExit()
	{
	}

	private IEnumerator GetStaticVersion()
	{
		yield return new WaitForSecondsRealtime(0.5f);

		// 更新资源版本号
		var package = YooAssets.GetAssetsPackage("DefaultPackage");
		var operation = package.UpdateStaticVersionAsync(30);
		yield return operation;

		if (operation.Status == EOperationStatus.Succeed)
		{
			PatchUpdater.PackageVersion = operation.PackageVersion;
			FsmManager.Transition(nameof(FsmUpdateManifest));
		}
		else
		{
			Debug.LogWarning(operation.Error);
			PatchEventDispatcher.SendStaticVersionUpdateFailedMsg();
		}
	}
}