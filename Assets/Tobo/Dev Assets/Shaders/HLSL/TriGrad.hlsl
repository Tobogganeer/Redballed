//UNITY_SHADER_NO_UPGRADE
#ifndef INCLUDE_TRIGRAD
#define INCLUDE_TRIGRAD

float invLerp(float from, float to, float value)
{
    return (value - from) / (to - from);
}

void TriGrad_float(float4 col1, float4 col2, float4 col3, float fac1, float fac2, float fac3, float t, out float4 colour)
{
    if (t <= fac1)
    {
        colour = col1;
    }
    else if (t <= fac2)
    {
        float val = invLerp(fac1, fac2, t);
        colour = lerp(col1, col2, val);

    }
    else if (t <= fac3)
    {
        float val = invLerp(fac2, fac3, t);
        colour = lerp(col2, col3, val);
    }
    else
    {
        colour = col3;
    }
}

#endif