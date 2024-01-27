using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrombaInjector : ShaderPasser
{
	public float succPower1;
	public float succPower2;

	private Transform tromba1;
	private Transform tromba2;
	protected override void FakeStart()
	{
		//get tromba from each player
	}

	protected override void FakeUpdate()
	{

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
