Shader "SolAR/DepthMask"
{
	SubShader
	{
		Tags{ "Queue" = "Background" }
		Pass
		{
			Name "BASE"
			Lighting Off
			ColorMask 0
		}
	}
}
