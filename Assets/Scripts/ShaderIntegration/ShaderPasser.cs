﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ShaderPasser : MonoBehaviour
{
    //I know I know, it's in shader folder, but this is the base class
    //for EVERY script that passes data to shaders,
    //so that's why here

    public Material MainMaterial;

    protected List<string> PropertyNames = new List<string>();
    protected List<int> PropertyIDs = new List<int>();
    void Start()
    {
        MeshRenderer meshRenderer = gameObject.GetComponent<MeshRenderer>();
        if (meshRenderer)
		{
            MainMaterial = meshRenderer.material;
		}
        BakePropertyNames();
        BakePropertyIDs();
        FakeStart();
    }

    protected abstract void FakeStart();
    protected abstract void FakeUpdate();

    //trust me it is that 1% scenario when hardcoding values is better then setting in inspector, as they are shader based, not material based
    protected abstract void BakePropertyNames();

    //we need to do that, it's just how shaders works
    void BakePropertyIDs()
	{
        //Shader shader = MainMaterial.shader;
        for (int i = 0; i < PropertyNames.Count; ++i)
		{
            PropertyIDs.Add(Shader.PropertyToID(PropertyNames[i]));
        }
	}

	void Update()
	{
        FakeUpdate();
        PassToRender();
	}

    //this thing has to be custom-made every time because of different data formats
    protected abstract void PassToRender();
}
