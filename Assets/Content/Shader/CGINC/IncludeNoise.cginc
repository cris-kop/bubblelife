

/*
BANNED VARIABLE LIST : because Unity internally uses these names?
- point


*/

float Hash(float3 p)  // replace this by something better / OR NOT
{
	p  = frac( p*0.3183099+.1 );
	p *= 17.0;
	return frac( p.x*p.y*p.z*(p.x+p.y+p.z) );
}

float Noise(float3 x )
{
	float3 i = floor(x);
	float3 f = frac(x);
	f = f*f*(3.0-2.0*f);
	
	return lerp(lerp(lerp( Hash(i+float3(0,0,0)), 
						Hash(i+float3(1,0,0)),f.x),
					lerp( Hash(i+float3(0,1,0)), 
						Hash(i+float3(1,1,0)),f.x),f.y),
				lerp(lerp( Hash(i+float3(0,0,1)), 
						Hash(i+float3(1,0,1)),f.x),
					lerp( Hash(i+float3(0,1,1)), 
						Hash(i+float3(1,1,1)),f.x),f.y),f.z);
}

float GetLayered3DNoise(float3 pos)
{		
	float3x3 m = float3x3( 0.00,  0.80,  0.60,
        -0.80,  0.36, -0.48,
        -0.60, -0.48,  0.64 );
			
	float3 q = pos;
	float f = 0.0;

	f  = 0.5000 * Noise( q );
	q = mul(m, q * 2.01);
				
	f += 0.2500*Noise( q );
	q = mul(m, q*2.02);
				
	f += 0.1250*Noise( q );
	q = mul(m, q*2.03);
				
	f += 0.0625*Noise( q );
	q = mul(m, q*2.01);

	return f;
}
