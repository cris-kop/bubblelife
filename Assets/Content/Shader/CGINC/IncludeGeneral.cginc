

/*
BANNED VARIABLE LIST : because Unity internally uses these names?
- point


*/

 static const float Custom_PI = 3.141593;


//Conversions
	float3 LocalToWorldPos(float3 localPos)
	{
		return mul (unity_ObjectToWorld, float4(localPos.x, localPos.y, localPos.z, 1)).xyz;
	}

	float3 LocalToWorldPos(float4 localPos)
	{
		return mul (unity_ObjectToWorld, localPos).xyz;
	}

	float4 ClipToScreenPos(float4 clipPos)
	{
		return ComputeScreenPos(clipPos);
	}

	float3 WorldToLocalPos(float4 worldPos)
	{
		return mul (unity_WorldToObject, worldPos).xyz;
	}

	float3 LocalToWorldNormal(float3 localNormal)
	{
		return UnityObjectToWorldNormal(localNormal).xyz;
	}
	
	float2 LocalToScreenNormal(float3 localNormal)
	{
		return 	normalize(mul((float3x3)UNITY_MATRIX_MV, localNormal)).xy;
		return normalize(mul((float3x3)UNITY_MATRIX_MV, localNormal)).xy;
	}

    float3 PivotWorldPos()
    {
        return unity_ObjectToWorld._m03_m13_m23;
    }

    float3 ModelWorldForward()
    {
        return normalize(unity_ObjectToWorld._m02_m12_m22);
    }

    float3 ModelWorldRight()
    {
        return normalize(unity_ObjectToWorld._m00_m10_m20);
    }

    float3 ModelWorldUp()
    {
        return normalize(unity_ObjectToWorld._m01_m11_m21);
    }

	float3 GetWorldViewDir(float3 worldPos)
	{
		return normalize(UnityWorldSpaceViewDir(worldPos)).xyz;
	}

    float4 GammaToLinear(float4 col)
    {
        return pow(col, 1.0 / 2.2);
    }

    float4 LinearToGamma(float4 col)
    {
        return pow(col, 2.2);
    }

	float InverseLerp (float from, float to, float value)
	{
		return saturate((value - from) / (to - from));
	}

	float Remap (float origFrom, float origTo, float targetFrom, float targetTo, float value)
	{
		float rel = InverseLerp(origFrom, origTo, value);
		return lerp(targetFrom, targetTo, saturate(rel));
	}

	float DegreesToRadians(float degrees)
	{
		return degrees * Custom_PI / 180.0;
	}

	float RadiansToDegrees(float radians)
	{
		return radians * 180.0 / Custom_PI;
	}
//


//Rotations
	float2 Rotate2D (float angle, float2 pos)
	{
		float sinTheta = sin(angle);
		float cosTheta = cos(angle);
		float2x2 rotationMatrix = float2x2(cosTheta, -sinTheta, sinTheta, cosTheta);
		return mul(pos, rotationMatrix);
	}

	float Angle(float2 pos)
	{
		return atan2(pos.y, pos.x );
	}

	float2 PolarCoordinate (float2 pos)
	{
		// get the angle of our point, and divide it in half
		float a = Angle(pos) * 0.5;

		// the square root of the dot product will convert our angle into our distance from center
		float r = sqrt(dot(pos, pos)); 
		float2 uv;
		uv.x = r;
		uv.y = a / Custom_PI;

		return uv;
	}

	float2 Voronoi (float2 UV, float offset) // copied from "unity_voronoi_noise_randomVector"
	{
		float2x2 m = float2x2(15.27, 47.63, 99.41, 89.98);
		UV = frac(sin(mul(UV, m)) * 46839.32);
		return float2(sin(UV.y * offset)*0.5+0.5, cos(UV.x * offset)*0.5+0.5);
	}
//


 //Easing
 	float EaseInOutSine(float x) 
	{
		return -(cos(Custom_PI * x) - 1) / 2;
	}

	float EaseInOutCubic(float x) 
	{
		return x < 0.5 ? 4 * x * x * x : 1 - pow(-2 * x + 2, 3) / 2;
	}
//


//specular
float SpecularCalc(float3 normal, float3 lightDirection, float3 viewDirection, float doubleSided_Bool)
{
	float _Shininess = 0.3;

	float3 specularReflectionDir = normalize(reflect(lightDirection, normal));

	float specularReflection = 0;
	specularReflection = dot(specularReflectionDir, viewDirection);

	specularReflection = lerp(specularReflection, abs(specularReflection), doubleSided_Bool);

	specularReflection = max(0, specularReflection);
	specularReflection = pow(specularReflection, _Shininess);

	return specularReflection;
}


float SmoothSpecular(float specular)
{
	float SPEC = 0.985;
	float pixelDiff = fwidth(specular);

	specular = smoothstep(SPEC - pixelDiff, SPEC, specular);
	specular *= (1 - saturate(pow(pixelDiff * 5, 2)));
				
	return saturate(specular);
}
