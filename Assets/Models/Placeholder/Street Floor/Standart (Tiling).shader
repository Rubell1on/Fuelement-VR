// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Street Floor"
{
	Properties
	{
		_Tiling("Tiling", Vector) = (1,1,0,0)
		_Offset("Offset", Vector) = (0,0,0,0)
		_Smoothness("Smoothness", Range( 0 , 1)) = 0
		_Metallic("Metallic", Range( 0 , 1)) = 0
		_Albedo("Albedo", 2D) = "white" {}
		_Normal("Normal", 2D) = "white" {}
		_Emission("Emission", 2D) = "white" {}
		_AO("AO", 2D) = "white" {}
		_Normalintensity("Normal intensity", Float) = 1
		_Emissionintensity("Emission intensity", Float) = 0
		_Albedocolor("Albedo color", Color) = (1,1,1,1)
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" "IsEmissive" = "true"  }
		Cull Back
		CGPROGRAM
		#include "UnityStandardUtils.cginc"
		#pragma target 3.0
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows 
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform sampler2D _Normal;
		uniform float2 _Tiling;
		uniform float2 _Offset;
		uniform float _Normalintensity;
		uniform float4 _Albedocolor;
		uniform sampler2D _Albedo;
		uniform sampler2D _Emission;
		uniform float _Emissionintensity;
		uniform float _Metallic;
		uniform float _Smoothness;
		uniform sampler2D _AO;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_TexCoord7 = i.uv_texcoord * _Tiling + _Offset;
			o.Normal = UnpackScaleNormal( tex2D( _Normal, uv_TexCoord7 ), _Normalintensity );
			o.Albedo = ( _Albedocolor * tex2D( _Albedo, uv_TexCoord7 ) ).rgb;
			o.Emission = ( tex2D( _Emission, uv_TexCoord7 ) * _Emissionintensity ).rgb;
			o.Metallic = _Metallic;
			o.Smoothness = _Smoothness;
			float grayscale30 = Luminance(tex2D( _AO, uv_TexCoord7 ).rgb);
			o.Occlusion = grayscale30;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18900
0;24;1906;995;1308.712;645.5272;1.3;True;False
Node;AmplifyShaderEditor.Vector2Node;5;-1574.488,52.88761;Inherit;False;Property;_Tiling;Tiling;0;0;Create;True;0;0;0;False;0;False;1,1;2,2;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.Vector2Node;8;-1576.488,201.8876;Inherit;False;Property;_Offset;Offset;1;0;Create;True;0;0;0;False;0;False;0,0;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.TexturePropertyNode;28;-847.8954,408.9381;Inherit;True;Property;_AO;AO;7;0;Create;True;0;0;0;False;0;False;None;None;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.TexturePropertyNode;16;-737.0557,-252.6028;Inherit;True;Property;_Albedo;Albedo;4;0;Create;True;0;0;0;False;0;False;None;None;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.TextureCoordinatesNode;7;-1360.488,155.8876;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;1,1;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TexturePropertyNode;18;-802.9807,192.0461;Inherit;True;Property;_Emission;Emission;6;0;Create;True;0;0;0;False;0;False;None;None;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.SamplerNode;29;-568.0146,389.192;Inherit;True;Property;_t4;t3;2;0;Create;True;0;0;0;False;0;False;-1;c8b38541505fbb6459289b645fa94975;c8b38541505fbb6459289b645fa94975;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;26;-753.0051,33.965;Inherit;False;Property;_Normalintensity;Normal intensity;8;0;Create;True;0;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;32;-251.005,266.765;Inherit;False;Property;_Emissionintensity;Emission intensity;9;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TexturePropertyNode;17;-1016.878,-47.91114;Inherit;True;Property;_Normal;Normal;5;0;Create;True;0;0;0;False;0;False;None;None;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.ColorNode;33;-336.312,-488.2271;Inherit;False;Property;_Albedocolor;Albedo color;10;0;Create;True;0;0;0;False;0;False;1,1,1,1;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;10;-553,164.5;Inherit;True;Property;_t3;t3;2;0;Create;True;0;0;0;False;0;False;-1;c8b38541505fbb6459289b645fa94975;c8b38541505fbb6459289b645fa94975;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;1;-462,-253.5;Inherit;True;Property;_t1;t1;0;0;Create;True;0;0;0;False;0;False;-1;c8b38541505fbb6459289b645fa94975;c8b38541505fbb6459289b645fa94975;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;31;-23.70501,189.465;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;13;-26,-30.5;Inherit;False;Property;_Smoothness;Smoothness;2;0;Create;True;0;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCGrayscale;30;-236.005,379.965;Inherit;True;0;1;0;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;9;-546,-34.5;Inherit;True;Property;_t2;t2;1;0;Create;True;0;0;0;False;0;False;-1;c8b38541505fbb6459289b645fa94975;c8b38541505fbb6459289b645fa94975;True;0;False;white;Auto;True;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;14;-30,-104.5;Inherit;False;Property;_Metallic;Metallic;3;0;Create;True;0;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;34;19.88806,-307.5272;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;449.6,-188.2;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;Street Floor;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;7;0;5;0
WireConnection;7;1;8;0
WireConnection;29;0;28;0
WireConnection;29;1;7;0
WireConnection;10;0;18;0
WireConnection;10;1;7;0
WireConnection;1;0;16;0
WireConnection;1;1;7;0
WireConnection;31;0;10;0
WireConnection;31;1;32;0
WireConnection;30;0;29;0
WireConnection;9;0;17;0
WireConnection;9;1;7;0
WireConnection;9;5;26;0
WireConnection;34;0;33;0
WireConnection;34;1;1;0
WireConnection;0;0;34;0
WireConnection;0;1;9;0
WireConnection;0;2;31;0
WireConnection;0;3;14;0
WireConnection;0;4;13;0
WireConnection;0;5;30;0
ASEEND*/
//CHKSM=33154AAF8753A3F08780C609513E2948C60D54D3