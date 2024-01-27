using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrombaInjector : ShaderPasser
{
	public float succPower1;
	public float succPower2;
	public float succSpeed1 = 3;
	public float succSpeed2 = 3;
	private float succReduction = 1.0f;

	private Transform tromba1;
	private Transform tromba2;
	protected override void FakeStart()
	{
		//get tromba from each player
		tromba1 = GameObject.FindObjectOfType<PukaszTrombolec>().trombaModel.transform;
		tromba2 = GameObject.FindObjectOfType<LawelTrombolec>().trombaModel.transform;
	}

	protected override void FakeUpdate()
	{
		Debug.Log(succPower1);
		//remove sucking power
		if (succPower1 < 0)
		{
			succPower1 = Mathf.Min(0, succPower1 + succReduction);
		}
		else
		{
			succPower1 = Mathf.Max(0, succPower1 - succReduction);
		}

		if (succPower2 < 0)
		{
			succPower2 = Mathf.Min(0, succPower2 + succReduction);
		}
		else
		{
			succPower2 = Mathf.Max(0, succPower2 - succReduction);
		}
	}

	protected override void BakePropertyNames()
	{
		PropertyNames.Add("TrombaPos1");
		PropertyNames.Add("TrombaPos2");
		PropertyNames.Add("SuccPower1");
		PropertyNames.Add("SuccPower2");
		PropertyNames.Add("SuccSpeed1");
		PropertyNames.Add("SuccSpeed2");
	}

	protected override void PassToRender(int j)
	{
		shadedMaterials[j].SetVector(PropertyIDs[0], tromba1.position);
		shadedMaterials[j].SetVector(PropertyIDs[1], tromba2.position);
		shadedMaterials[j].SetFloat(PropertyIDs[2], succPower1);
		shadedMaterials[j].SetFloat(PropertyIDs[3], succPower2);
		shadedMaterials[j].SetFloat(PropertyIDs[4], succSpeed1);
		shadedMaterials[j].SetFloat(PropertyIDs[5], succSpeed2);
	}
}
