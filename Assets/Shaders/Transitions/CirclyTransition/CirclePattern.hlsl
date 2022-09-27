void CirclePattern_float(in float2 uv, in float2 center, in float radius, in float smooth, out float output)
{
	float cx = uv.x - center.x;
	cx *= cx;

	float cy = uv.y - center.y;
	cy *= cy;

	float circle = cx + cy;
	float radiusQ = radius * radius;

	if(circle < radiusQ)
	{
		output = smoothstep(radiusQ, radiusQ - smooth, circle);
	}
	else
	{
		output = 0;
	}
}

void CirclePattern2_float(in float2 uv, in float2 center, in float radius, in float smooth, out float output)
{
	float2 screenSize;
	screenSize.x = _ScreenParams.x;
	screenSize.y = _ScreenParams.y;

	float2 screenCenter = center * screenSize - 1;
	float2 pixPos = uv * screenSize - 1;

	float dist = distance(pixPos, screenCenter);

	radius = radius * min(screenSize.x, screenSize.y) - 1;
	//float radius2 = 

	if(dist < radius)
	{	
		output = 1;
		return;
	}

	output = 0;
}