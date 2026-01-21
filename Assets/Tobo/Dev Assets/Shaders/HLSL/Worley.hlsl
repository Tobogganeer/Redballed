//UNITY_SHADER_NO_UPGRADE
#ifndef INCLUDE_WORLEY
#define INCLUDE_WORLEY

float3 mod(float3 x, float y)
{
    return float3(x.x - y * floor(x.x/y), x.y - y * floor(x.y/y), x.z - y * floor(x.z/y));
}

float3 mod(float3 x, float3 y)
{
    return float3(x.x - y.x * floor(x.x/y.x), x.y - y.y * floor(x.y/y.y), x.z - y.z * floor(x.z/y.z));
}

/*

// https://www.shadertoy.com/view/3dVXDc

// Hash by David_Hoskins
#define UI0 1597334673U
#define UI1 3812015801U
#define UI2 float3(UI0, UI1)
#define UI3 uint3(UI0, UI1, 2798796415U)
#define UIF (1.0 / float(0xffffffffU))

float3 hash33(float3 p)
{
	uint3 q = uint3(int3(p)) * UI3;
	q = (q.x ^ q.y ^ q.z)*UI3;
	return -1. + 2. * float3(q) * UIF;
}

float remap(float x, float a, float b, float c, float d)
{
    return (((x - a) / (b - a)) * (d - c)) + c;
}

// Gradient noise by iq (modified to be tileable)
float gradientNoise(float3 x, float freq)
{
    // grid
    float3 p = floor(x);
    float3 w = frac(x);
    
    // quintic interpolant
    float3 u = w * w * w * (w * (w * 6. - 15.) + 10.);

    
    // gradients
    float3 ga = hash33(mod(p + float3(0., 0., 0.), freq));
    float3 gb = hash33(mod(p + float3(1., 0., 0.), freq));
    float3 gc = hash33(mod(p + float3(0., 1., 0.), freq));
    float3 gd = hash33(mod(p + float3(1., 1., 0.), freq));
    float3 ge = hash33(mod(p + float3(0., 0., 1.), freq));
    float3 gf = hash33(mod(p + float3(1., 0., 1.), freq));
    float3 gg = hash33(mod(p + float3(0., 1., 1.), freq));
    float3 gh = hash33(mod(p + float3(1., 1., 1.), freq));
    
    // projections
    float va = dot(ga, w - float3(0., 0., 0.));
    float vb = dot(gb, w - float3(1., 0., 0.));
    float vc = dot(gc, w - float3(0., 1., 0.));
    float vd = dot(gd, w - float3(1., 1., 0.));
    float ve = dot(ge, w - float3(0., 0., 1.));
    float vf = dot(gf, w - float3(1., 0., 1.));
    float vg = dot(gg, w - float3(0., 1., 1.));
    float vh = dot(gh, w - float3(1., 1., 1.));
	
    // interpolation
    return va + 
           u.x * (vb - va) + 
           u.y * (vc - va) + 
           u.z * (ve - va) + 
           u.x * u.y * (va - vb - vc + vd) + 
           u.y * u.z * (va - vc - ve + vg) + 
           u.z * u.x * (va - vb - ve + vf) + 
           u.x * u.y * u.z * (-va + vb + vc - vd + ve - vf - vg + vh);
}

// Tileable 3D worley noise
float worleyNoise(float3 uv, float freq)
{    
    float3 id = floor(uv);
    float3 p = frac(uv);
    
    float minDist = 10000.;
    for (float x = -1.; x <= 1.; ++x)
    {
        for(float y = -1.; y <= 1.; ++y)
        {
            for(float z = -1.; z <= 1.; ++z)
            {
                float3 _offset = float3(x, y, z);
            	float3 h = hash33(mod(id + _offset, float3(freq, freq, freq))) * .5 + float3(.5, .5, .5);
    			h += _offset;
            	float3 d = p - h;
           		minDist = min(minDist, dot(d, d));
            }
        }
    }
    
    // inverted worley noise
    return 1. - minDist;
}

// Fbm for Perlin noise based on iq's blog
float perlinfbm(float3 p, float freq, int octaves)
{
    float G = exp2(-.85);
    float amp = 1.;
    float noise = 0.;
    for (int i = 0; i < octaves; ++i)
    {
        noise += amp * gradientNoise(p * freq, freq);
        freq *= 2.;
        amp *= G;
    }
    
    return noise;
}

// Tileable Worley fbm inspired by Andrew Schneider's Real-Time Volumetric Cloudscapes
// chapter in GPU Pro 7.
float worleyFbm(float3 p, float freq)
{
    return worleyNoise(p*freq, freq) * .625 +
        	 worleyNoise(p*freq*2., freq*2.) * .25 +
        	 worleyNoise(p*freq*4., freq*4.) * .125;
}

*/

/*

//https://www.shadertoy.com/view/WddyW8

float3 hash33(float3 p3)
{
	float3 p = frac(p3 * float3(.1031,.11369,.13787));
    p += dot(p, p.yxz+19.19);
    return -1.0 + 2.0 * frac(float3((p.x + p.y)*p.z, (p.x+p.z)*p.y, (p.y+p.z)*p.x));
}

float worley(float3 p, float scale){

    float3 id = floor(p);
    float3 fd = frac(p);

    float n = 0.;

    float minimalDist = 1.;


    for(float x = -2.; x <=2.; x++){
        for(float y = -2.; y <=2.; y++){
            for(float z = -2.; z <=2.; z++){

                float3 coord = float3(x,y,z);
                float3 rId = hash33(mod(abs(id+coord),scale))*0.33;

                float3 r = coord + rId - fd; 

                float d = dot(r,r);

                if(d < minimalDist){
                    minimalDist = d;
                }

            }//z
        }//y
    }//x

  return minimalDist;
}


float fbm(float3 p,float scale){
  float G = exp(-0.3);
  float amp = 1.;
  float freq = 1.;
  float n = 0.;
    
#define iter 3

  for(int i = 0; i < iter; i++){
    n += worley(p*freq,scale*freq)*amp;
    freq*=2.;
    amp*=G;
  }
    
  return n*n;  
}

*/


// https://www.shadertoy.com/view/XtBGDG

float noise3D(float3 p)
{
	return frac(sin(dot(p ,float3(12.9898,78.233,128.852))) * 43758.5453)*2.0-1.0;
}

float simplex3D(float3 p)
{
	
	float f3 = 1.0/3.0;
	float s = (p.x+p.y+p.z)*f3;
	int i = int(floor(p.x+s));
	int j = int(floor(p.y+s));
	int k = int(floor(p.z+s));
	
	float g3 = 1.0/6.0;
	float t = float((i+j+k))*g3;
	float x0 = float(i)-t;
	float y0 = float(j)-t;
	float z0 = float(k)-t;
	x0 = p.x-x0;
	y0 = p.y-y0;
	z0 = p.z-z0;
	
	int i1,j1,k1;
	int i2,j2,k2;
	
	if(x0>=y0)
	{
		if(y0>=z0){ i1=1; j1=0; k1=0; i2=1; j2=1; k2=0; } // X Y Z order
		else if(x0>=z0){ i1=1; j1=0; k1=0; i2=1; j2=0; k2=1; } // X Z Y order
		else { i1=0; j1=0; k1=1; i2=1; j2=0; k2=1; }  // Z X Z order
	}
	else 
	{ 
		if(y0<z0) { i1=0; j1=0; k1=1; i2=0; j2=1; k2=1; } // Z Y X order
		else if(x0<z0) { i1=0; j1=1; k1=0; i2=0; j2=1; k2=1; } // Y Z X order
		else { i1=0; j1=1; k1=0; i2=1; j2=1; k2=0; } // Y X Z order
	}
	
	float x1 = x0 - float(i1) + g3; 
	float y1 = y0 - float(j1) + g3;
	float z1 = z0 - float(k1) + g3;
	float x2 = x0 - float(i2) + 2.0*g3; 
	float y2 = y0 - float(j2) + 2.0*g3;
	float z2 = z0 - float(k2) + 2.0*g3;
	float x3 = x0 - 1.0 + 3.0*g3; 
	float y3 = y0 - 1.0 + 3.0*g3;
	float z3 = z0 - 1.0 + 3.0*g3;	
				 
	float3 ijk0 = float3(i,j,k);
	float3 ijk1 = float3(i+i1,j+j1,k+k1);	
	float3 ijk2 = float3(i+i2,j+j2,k+k2);
	float3 ijk3 = float3(i+1,j+1,k+1);	
            
	float3 gr0 = normalize(float3(noise3D(ijk0),noise3D(ijk0*2.01),noise3D(ijk0*2.02)));
	float3 gr1 = normalize(float3(noise3D(ijk1),noise3D(ijk1*2.01),noise3D(ijk1*2.02)));
	float3 gr2 = normalize(float3(noise3D(ijk2),noise3D(ijk2*2.01),noise3D(ijk2*2.02)));
	float3 gr3 = normalize(float3(noise3D(ijk3),noise3D(ijk3*2.01),noise3D(ijk3*2.02)));
	
	float n0 = 0.0;
	float n1 = 0.0;
	float n2 = 0.0;
	float n3 = 0.0;

	float t0 = 0.5 - x0*x0 - y0*y0 - z0*z0;
	if(t0>=0.0)
	{
		t0*=t0;
		n0 = t0 * t0 * dot(gr0, float3(x0, y0, z0));
	}
	float t1 = 0.5 - x1*x1 - y1*y1 - z1*z1;
	if(t1>=0.0)
	{
		t1*=t1;
		n1 = t1 * t1 * dot(gr1, float3(x1, y1, z1));
	}
	float t2 = 0.5 - x2*x2 - y2*y2 - z2*z2;
	if(t2>=0.0)
	{
		t2 *= t2;
		n2 = t2 * t2 * dot(gr2, float3(x2, y2, z2));
	}
	float t3 = 0.5 - x3*x3 - y3*y3 - z3*z3;
	if(t3>=0.0)
	{
		t3 *= t3;
		n3 = t3 * t3 * dot(gr3, float3(x3, y3, z3));
	}
	return 96.0*(n0+n1+n2+n3);
	
}

float fbm(float3 p)
{
	float f;
    f  = 0.50000*simplex3D( p ); p = p*2.01;
    f += 0.25000*simplex3D( p ); p = p*2.02; //from iq
    f += 0.12500*simplex3D( p ); p = p*2.03;
    f += 0.06250*simplex3D( p ); p = p*2.04;
    f += 0.03125*simplex3D( p );
	return f;
}

void Worley_float(float3 position, float freq, out float result)
{
    result = fbm(position * freq);
    //result = fbm(position, freq);
    //result = worleyFbm(position, freq);
}

#endif