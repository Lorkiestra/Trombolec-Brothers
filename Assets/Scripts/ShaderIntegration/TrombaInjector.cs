using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrombaInjector : ShaderPasser
{
	public float succPower1;
	public float succPower2;
	private float succReduction = 1.0f;

	private Transform tromba1;
	private Transform tromba2;
	protected override void FakeStart()
	{
		//get tromba from each player
		tromba1 = GameObject.FindObjectOfType<PukaszTrombolec>().tromba.transform;
		tromba2 = GameObject.FindObjectOfType<LawelTrombolec>().tromba.transform;
	}

	protected override void FakeUpdate()
	{
		//remove sucking power
		if (succPower1 < 0)
		{
			succPower1 = Mathf.Max(0, succPower1 + succReduction);
		}
		else
		{
			succPower1 = Mathf.Min(0, succPower1 - succReduction);
		}

		if (succPower2 < 0)
		{
			succPower2 = Mathf.Max(0, succPower2 + succReduction);
		}
		else
		{
			succPower2 = Mathf.Min(0, succPower2 - succReduction);
		}
	}

	protected override void BakePropertyNames()
	{
		PropertyNames.Add("TrombaPos1");
		PropertyNames.Add("TrombaPos2");
		PropertyNames.Add("SuccPower1");
		PropertyNames.Add("SuccPower2");
	}

	protected override void PassToRender()
	{
		MainMaterial.SetVector(PropertyIDs[0], tromba1.position);
		MainMaterial.SetVector(PropertyIDs[1], tromba2.position);
		MainMaterial.SetFloat(PropertyIDs[2], succPower1);
		MainMaterial.SetFloat(PropertyIDs[3], succPower2);
	}
}
