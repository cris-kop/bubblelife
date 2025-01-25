

/*
BANNED VARIABLE LIST : because Unity internally uses these names?
- point


*/

// 3D https://www.iquilezles.org/www/articles/distfunctions/distfunctions.htm
// 2D https://www.iquilezles.org/www/articles/distfunctions2d/distfunctions2d.htm

 static const float PI = 3.141593;


// 2D
	float sdBox2D(float2 pos, float2 size )
	{
		float2 q = abs(pos) - size;
		return length( max(q, 0.0) ) + min(max(q.x, q.y),0.0);
	}

	float sdCircle2D(float2 pos, float radius )
	{
		return length(pos) - radius;
	}
//




// 3D
	float sdSphere3D(vec3 pos, float radius )
	{
	  return length(pos) - radius;
	}

	float sdBox3D(vec3 pos, vec3 size )
	{
	  vec3 q = abs(pos) - size;
	  return length(max(q,0.0)) + min(max(q.x,max(q.y,q.z)),0.0);
	}
//
