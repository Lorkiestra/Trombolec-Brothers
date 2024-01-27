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

float raySphere(float3 r0, float3 rd, float3 s0, float sr)
{
	// - r0: ray origin
	// - rd: normalized ray direction
	// - s0: sphere center
	// - sr: sphere radius
	// - Returns distance from r0 to first intersecion with sphere,
	//   or -1.0 if no intersection.
	float a = dot(rd, rd);
	float3 s0_r0 = r0 - s0;
	float b = 2.0 * dot(rd, s0_r0);
	float c = dot(s0_r0, s0_r0) - (sr * sr);
	if (b * b - 4.0 * a * c < 0.0)
	{
		return -1.0;
	}
	return (-b - sqrt((b * b) - 4.0 * a * c)) / (2.0 * a);
}

bool SimplePlane(float3 rayDir, float3 pos, float3 scale, inout float4 Hit, int facing)
{
	if (facing = 0) //x
	{
		//now for testing only check y downwards:
		if (rayDir.x >= 0) //needed condition
		{
			return false;
		}

		float magnitude = pos.x / rayDir.x;
		float4 hit = float4(float3(0, 0, 0) + rayDir * magnitude, 0);
		Hit = hit;
		if (abs(hit.y - pos.y) < scale.y && abs(hit.z - pos.z) < scale.z) //check if within plane bounds
		{
			return true;
		}
		else
		{
			return false;
		}
	}
	else if (facing = 1) //y
	{
		//now for testing only check y downwards:
		if (rayDir.y >= 0) //needed condition
		{
			return false;
		}

		float magnitude = pos.y / rayDir.y;
		float4 hit = float4(float3(0, 0, 0) + rayDir * magnitude, 0);
		if (abs(hit.x - pos.x) < scale.x && abs(hit.z - pos.z) < scale.z) //check if within plane bounds
		{
			Hit = float4(hit + pos, 1);
			return true;
		}
		else
		{
			return false;
		}
	}
	else //z
	{
		//now for testing only check y downwards:
		if (rayDir.z >= 0) //needed condition
		{
			return false;
		}

		float magnitude = pos.z / rayDir.z;
		float4 hit = float4(float3(0, 0, 0) + rayDir * magnitude, 0);
		Hit = hit;
		if (abs(hit.x - pos.x) < scale.x && abs(hit.y - pos.y) < scale.y) //check if within plane bounds
		{
			return true;
		}
		else
		{
			return false;
		}
	}

	return false;
}

bool CompositeCube(float3 rayDir, float3 pos, float3 scale, inout float4 Hit)
{
	if (SimplePlane(rayDir, pos, scale, Hit, 0))
	{
		return true;
	}
	//actual infinite cube
	if (SimplePlane(rayDir, pos - scale.z / 2, float3(scale.x, scale.y, 131072), Hit, 1))
	{
		if (Hit.y > pos.y) //show only "lower" planes
		{

		}
	}

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

bool SimpleInfiniCube(float3 pos, inout float4 hit, float3 dir, float3 scale, inout float3 normals)
{
	//float t1.x, t2.x, t1.y, t2.y, t1.z, t2.z, tNear, tFar;
	pos.y -= 8192;
	float3 t1;
	float3 t2;
	t1.x = (pos.x - scale.x) / dir.x;
	t2.x = (pos.x + scale.x) / dir.x;
	t1.y = (pos.y - 8192) / dir.y;
	t2.y = (pos.y + 8192) / dir.y;
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
	hit = float4(dir * tNear, 1);
	//hit = float4(pos, 1);
	normals = suspectedNormals;
	return true;
}