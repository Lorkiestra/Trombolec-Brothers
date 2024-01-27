#include "UsefulCalculations.cginc"

float SphereShape(float3 p)
{
	float result = 0;
	result = length(p) - 1;

	return result;
}

float CubeShape(float3 p)
{
	p = abs(p);

	float result = length(float3(max(0, p.x - 1), max(0, p.y - 1), max(0, p.z - 1)));
	if (p.x < 1 && p.y < 1 && p.z < 1)
	{
		result = max(p.x - 1, max(p.y - 1, p.z - 1));
	}

	return result;
}

float InfiniCubeShape(float3 p)
{
	//p = abs(p);
	p = float3(abs(p.x), p.y, abs(p.z));

	float result = length(float3(max(0, p.x - 1), max(0, p.y - 1), max(0, p.z - 1)));
	if (p.x < 1 && p.y < 1 && p.z < 1)
	{
		result = max(p.x - 1, max(p.y - 1, p.z - 1));
	}

	return result;
}

//shapes calculations:
float3 SphereShapeVector(float3 p)
{

	float3 norm = normalize(p);

	return -(p - norm);
}


float3 CubeShapeVector(float3 p)
{
	float3 bound = min(abs(p), 1) * sign(p);

	return -(p - bound);
}

bool SimpleCube(float3 pos, float4 hit, float3 dir, float3 scale, inout float3 normals)
{
	//float t1.x, t2.x, t1.y, t2.y, t1.z, t2.z, tNear, tFar;
	float3 t1;
	float3 t2;
	t1.x = (pos.x - scale.x) / dir.x;
	t2.x = (pos.x + scale.x) / dir.x;
	t1.y = (pos.y - scale.y) / dir.y;
	t2.y = (pos.y + scale.y) / dir.y;
	t1.z = (pos.z - scale.z) / dir.z;
	t2.z = (pos.z + scale.z) / dir.z;

	//float tNear = DirectedMax(float3(min(t1.x, t2.x), min(t1.y, t2.y), min(t1.z, t2.z)), normals);
	float tNear = max(min(t1.x, t2.x), max(min(t1.y, t2.y), min(t1.z, t2.z)));
	float3 suspectedNormals = float3(0, 0, 0);
	DirectedMax(float3(min(t1.x, t2.x), min(t1.y, t2.y), min(t1.z, t2.z)), suspectedNormals);
	if (t1.x < t2.x)
	{
		suspectedNormals.x = -suspectedNormals.x;
	}
	if (t1.y < t2.y)
	{
		suspectedNormals.y = -suspectedNormals.y;
	}
	if (t1.z < t2.z)
	{
		suspectedNormals.z = -suspectedNormals.z;
	}
	float tFar = min(max(t1.x, t2.x), min(max(t1.y, t2.y), max(t1.z, t2.z)));

	//far planes are closer or direction is outwards
	if (tNear > tFar || tFar < 0)
	{
		return false;
	}
	//the actual result is tNear where plane is based on max() function
	normals = suspectedNormals;
	return true;
}