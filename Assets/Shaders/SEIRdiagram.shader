// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Custom/SEIRdiagram"
{
  Properties
  {
     [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
      [Header(Recovered)]_RecoveredColor ("Recovered Color", Color) = (0, 0, 1, 1)
      _RecoveredPercent ("Recovered Percent", Float) = 0.2

      [Header(Susceptible)]_SusceptibleColor ("Susceptible color", Color) = (0, 1, 0, 1)
      _SusceptiblePercent ("Susceptible Percent", Float) = 0.4

      [Header(Exposed)]_ExposedColor ("Exposed color", Color) = (1, 0.75, 0, 0)
      _ExposedPercent ("Exposed Percent", Float) = 0.4

      [Header(Infected)]_InfectedColor ("Infected color", Color) = (1, 0, 0, 1)
      _InfectedPercent ("Infected Percent", Float) = 0.8

      [Header(Dead)]_DeadColor ("Dead color", Color) = (0, 0, 0, 0)
      _DeadPercent ("Dead Percent", Float) = 0

      [Header(Height)]
      _Height ("Height", Float) = 1.6
  }

  SubShader
  { 
      Pass
      {
      CGPROGRAM
          #pragma vertex vert
          #pragma fragment frag
          #include "UnityCG.cginc"

          struct v2f
          {
              float4 vertex   : SV_POSITION;
              float2 uv        : TEXCOORD0;
              float3 wpos : POSITION1;
          };

          fixed4 _RecoveredColor;
          half _RecoveredPercent;

          fixed4 _SusceptibleColor;
          half _SusceptiblePercent;

          fixed4 _ExposedColor;
          half _ExposedPercent;

          fixed4 _InfectedColor;
          half _InfectedPercent;

          fixed4 _DeadColor;
          half _DeadPercent;

          half _Height;

          v2f vert(float4 pos : POSITION, float2 uv : TEXCOORD0)
          {
            v2f o;
            o.vertex = UnityObjectToClipPos(pos);
            o.uv = uv * 1;
            o.wpos = mul(unity_ObjectToWorld, pos).xyz;
            return o;
          }

          fixed4 frag(v2f IN) : SV_Target
          {
              float2 c = IN.uv;
              fixed4 cout;

              float y = IN.wpos.y / _Height;

              if (y < _RecoveredPercent){
                cout = _RecoveredColor;
              }
              else if (y < _SusceptiblePercent + _RecoveredPercent){
                cout = _SusceptibleColor;
              }
              else if(y < _ExposedPercent + _SusceptiblePercent + _RecoveredPercent){
                cout = _ExposedColor;
              }
              else if (y < _InfectedPercent + _ExposedPercent + _SusceptiblePercent + _RecoveredPercent){
                cout = _InfectedColor;
              }
              else
              {
                cout = _DeadColor;
              }
              return cout;
          }
      ENDCG
      }
  }

  FallBack "Diffuse"
}
