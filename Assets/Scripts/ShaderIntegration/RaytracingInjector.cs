using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class RaytracingInjector : ShaderPasser
{
	public Transform RayScene;
	public ComputeShader shader;
	public RenderTexture result;
	public Vector4 SkyboxScale;
	public int SkyboxMode;

	[SerializeField] private Texture blokTexture;
	private Texture cumDepthTexture;

	private Camera rayCamera;
	private List<Vector4> ShapePosition = new List<Vector4>();
	private List<Vector4> ShapeRotation = new List<Vector4>();
	private List<Vector4> ShapeScale = new List<Vector4>();

	protected override void FakeStart()
	{
		result = new RenderTexture(Screen.width, Screen.height, 24);
		result.enableRandomWrite = true;
		result.Create();

		rayCamera = gameObject.GetComponent<Camera>();
		rayCamera.depthTextureMode = DepthTextureMode.Depth;

		int kernel = shader.FindKernel("RayTracing");
		shader.SetTexture(kernel, "BlokTexture", blokTexture);
		shader.SetVector("BlokResolution", new Vector4(blokTexture.width, blokTexture.height, 20, 20));
	}

	public void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		//use raytracing and update
		int kernel = shader.FindKernel("RayTracing");

		shader.SetTexture(kernel, "CumTexture", source);
		shader.SetTexture(kernel, "Result", result);
		shader.SetTexture(kernel, "CumDepthTexture", Shader.GetGlobalTexture("_CameraDepthTexture"));
		shader.Dispatch(kernel, Screen.width / 8, Screen.height / 8, 1);

		Graphics.Blit(result, destination);
	}

	private Vector4 Vect3To4(Vector3 vect3)
	{
		return new Vector4(vect3.x, vect3.y, vect3.z, 0);
	}

	protected override void FakeUpdate()
	{
		ShapePosition.Clear();
		ShapeRotation.Clear();
		ShapeScale.Clear();

		//collecting data about objects
		RayObject[] rayObjects = RayScene.GetComponentsInChildren<RayObject>(includeInactive: false);

		for (int i = 0; i < rayObjects.Length; ++i)
		{
			ShapePosition.Add(Vect3To4(rayObjects[i].transform.position));
			ShapeRotation.Add(Vect3To4(rayObjects[i].transform.rotation.eulerAngles));
			ShapeScale.Add(Vect3To4(rayObjects[i].transform.lossyScale));
		}

		PassToRender(0);
	}

	protected override void BakePropertyNames()
	{
		PropertyNames.Add("Resolution");
		PropertyNames.Add("CameraPosition");
		PropertyNames.Add("CameraRotation");
		PropertyNames.Add("ObjectsCap");
		PropertyNames.Add("Frustum");
		PropertyNames.Add("ShapePosition");
		PropertyNames.Add("ShapeRotation");
		PropertyNames.Add("ShapeScale");
		PropertyNames.Add("SkyboxScale");
		PropertyNames.Add("NightMode");
	}
	protected override void PassToRender(int j)
	{
		shader.SetVector(PropertyIDs[0], new Vector4(Screen.width, Screen.height, 0, 0));
		shader.SetVector(PropertyIDs[1], new Vector4(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z, 1));
		shader.SetVector(PropertyIDs[2], Vect3To4(gameObject.transform.rotation.eulerAngles));
		shader.SetInt(PropertyIDs[3], ShapePosition.Count);

		Vector3[] frustum = new Vector3[4];
		rayCamera.CalculateFrustumCorners(new Rect(0, 0, 1, 1), rayCamera.farClipPlane, Camera.MonoOrStereoscopicEye.Mono, frustum);
		Vector4[] realFrustum = new Vector4[4];
		for (int i = 0; i < frustum.Length; ++i)
		{
			realFrustum[i] = Vect3To4(frustum[i]);
		}

		shader.SetVectorArray(PropertyIDs[4], realFrustum);

		shader.SetVectorArray(PropertyIDs[5], ShapePosition.ToArray());
		shader.SetVectorArray(PropertyIDs[6], ShapeRotation.ToArray());
		shader.SetVectorArray(PropertyIDs[7], ShapeScale.ToArray());

		shader.SetVector(PropertyIDs[8], new Vector4(SkyboxScale.x, SkyboxScale.y, SkyboxScale.z, Time.time * SkyboxScale.w));
		shader.SetInt(PropertyIDs[9], SkyboxMode);
	}
}
