// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Shader created with Shader Forge v1.05 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.05;sub:START;pass:START;ps:flbk:Diffuse,lico:1,lgpr:1,nrmq:1,limd:1,uamb:True,mssp:True,lmpd:False,lprd:False,rprd:False,enco:False,frtr:True,vitr:True,dbil:False,rmgx:True,rpth:1,hqsc:True,hqlp:False,tesm:0,blpr:0,bsrc:0,bdst:0,culm:2,dpts:2,wrdp:True,dith:0,ufog:True,aust:True,igpj:False,qofs:0,qpre:1,rntp:1,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.4251494,fgcg:0.4785952,fgcb:0.5322372,fgca:1,fgde:0.0005,fgrn:0,fgrf:8000,ofsf:0,ofsu:0,f2p0:False;n:type:ShaderForge.SFN_Final,id:1,x:34339,y:32248,varname:node_1,prsc:2|diff-31-OUT,spec-48-OUT,gloss-45-OUT;n:type:ShaderForge.SFN_VertexColor,id:2,x:32703,y:32458,varname:node_2,prsc:2;n:type:ShaderForge.SFN_ValueProperty,id:3,x:32703,y:32601,ptovrint:False,ptlb:Vertex Occlusion,ptin:_VertexOcclusion,varname:node_519,prsc:2,glob:False,v1:1;n:type:ShaderForge.SFN_Power,id:6,x:32895,y:32567,varname:node_6,prsc:2|VAL-2-R,EXP-3-OUT;n:type:ShaderForge.SFN_Color,id:7,x:32739,y:32271,ptovrint:False,ptlb:Diffuse Colour,ptin:_DiffuseColour,varname:node_6732,prsc:2,glob:False,c1:0.5,c2:0.5,c3:0.5,c4:1;n:type:ShaderForge.SFN_Multiply,id:8,x:33113,y:32408,varname:node_8,prsc:2|A-7-RGB,B-6-OUT;n:type:ShaderForge.SFN_Dot,id:24,x:33517,y:31829,varname:node_24,prsc:2,dt:0|A-25-OUT,B-29-OUT;n:type:ShaderForge.SFN_NormalVector,id:25,x:33337,y:31727,prsc:2,pt:False;n:type:ShaderForge.SFN_Vector3,id:29,x:33337,y:31875,varname:node_29,prsc:2,v1:0,v2:1,v3:0;n:type:ShaderForge.SFN_If,id:31,x:33736,y:32133,varname:node_31,prsc:2|A-24-OUT,B-33-OUT,GT-44-OUT,EQ-44-OUT,LT-8-OUT;n:type:ShaderForge.SFN_Lerp,id:33,x:33447,y:32008,varname:node_33,prsc:2|A-34-OUT,B-36-OUT,T-38-OUT;n:type:ShaderForge.SFN_Vector1,id:34,x:33261,y:31972,varname:node_34,prsc:2,v1:1;n:type:ShaderForge.SFN_Vector1,id:36,x:33261,y:32037,varname:node_36,prsc:2,v1:-1;n:type:ShaderForge.SFN_Slider,id:38,x:33104,y:32128,ptovrint:False,ptlb:Snow Level,ptin:_SnowLevel,varname:node_927,prsc:2,min:0,cur:0,max:1;n:type:ShaderForge.SFN_Color,id:39,x:32868,y:32146,ptovrint:False,ptlb:Snow Colour,ptin:_SnowColour,varname:node_9921,prsc:2,glob:False,c1:0.5,c2:0.5,c3:0.5,c4:1;n:type:ShaderForge.SFN_Multiply,id:44,x:33113,y:32256,varname:node_44,prsc:2|A-39-RGB,B-6-OUT;n:type:ShaderForge.SFN_ValueProperty,id:45,x:33951,y:32453,ptovrint:False,ptlb:Specular Power,ptin:_SpecularPower,varname:node_8012,prsc:2,glob:False,v1:0;n:type:ShaderForge.SFN_Color,id:46,x:33501,y:32365,ptovrint:False,ptlb:Snow Specular Colour,ptin:_SnowSpecularColour,varname:node_3776,prsc:2,glob:False,c1:0.5,c2:0.5,c3:0.5,c4:1;n:type:ShaderForge.SFN_If,id:48,x:33951,y:32251,varname:node_48,prsc:2|A-24-OUT,B-33-OUT,GT-50-OUT,EQ-50-OUT,LT-52-OUT;n:type:ShaderForge.SFN_Multiply,id:50,x:33682,y:32475,varname:node_50,prsc:2|A-46-RGB,B-6-OUT;n:type:ShaderForge.SFN_Multiply,id:52,x:33665,y:32649,varname:node_52,prsc:2|A-54-RGB,B-6-OUT;n:type:ShaderForge.SFN_Color,id:54,x:33501,y:32559,ptovrint:False,ptlb:Specular Colour,ptin:_SpecularColour,varname:node_402,prsc:2,glob:False,c1:0.5,c2:0.5,c3:0.5,c4:1;proporder:3-7-38-39-45-46-54;pass:END;sub:END;*/

Shader "Custom/Diffuse Flat Snow Shader Vertex Colours" {
    Properties {
        _VertexOcclusion ("Vertex Occlusion", Float ) = 1
        _DiffuseColour ("Diffuse Colour", Color) = (0.5,0.5,0.5,1)
        _SnowLevel ("Snow Level", Range(0, 1)) = 0
        _SnowColour ("Snow Colour", Color) = (0.5,0.5,0.5,1)
        _SpecularPower ("Specular Power", Float ) = 0
        _SnowSpecularColour ("Snow Specular Colour", Color) = (0.5,0.5,0.5,1)
        _SpecularColour ("Specular Colour", Color) = (0.5,0.5,0.5,1)
    }
    SubShader {
        Tags {
            "RenderType"="Opaque"
        }
        Pass {
            Name "PrePassBase"
            Tags {
                "LightMode"="PrePassBase"
            }
            Cull Off
            
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_PREPASSBASE
            #include "UnityCG.cginc"
            #pragma multi_compile_fog
            #pragma exclude_renderers xbox360 ps3 flash d3d11_9x 
            #pragma target 3.0
            uniform fixed4 unity_Ambient;
            uniform float _SpecularPower;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float4 posWorld : TEXCOORD0;
                float3 normalDir : TEXCOORD1;
                #ifndef LIGHTMAP_OFF
                    float4 uvLM : TEXCOORD2;
                #else
                    float3 shLight : TEXCOORD2;
                #endif
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.normalDir = mul(unity_ObjectToWorld, float4(v.normal,0)).xyz;
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                o.pos = UnityObjectToClipPos(v.vertex);
                return o;
            }
            fixed4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
/////// Vectors:
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 normalDirection = i.normalDir;
                
                float nSign = sign( dot( viewDirection, i.normalDir ) ); // Reverse normal if this is a backface
                i.normalDir *= nSign;
                normalDirection *= nSign;
                
                return fixed4( normalDirection * 0.5 + 0.5, max(_SpecularPower,0.0078125) );
            }
            ENDCG
        }
        Pass {
            Name "PrePassFinal"
            Tags {
                "LightMode"="PrePassFinal"
            }
            Cull Off
            ZWrite Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_PREPASSFINAL
            #include "UnityCG.cginc"
            #pragma multi_compile_prepassfinal
            #pragma multi_compile_fog
            #pragma exclude_renderers xbox360 ps3 flash d3d11_9x 
            #pragma target 3.0
            uniform sampler2D _LightBuffer;
            #if defined (SHADER_API_XBOX360) && defined (HDR_LIGHT_PREPASS_ON)
                sampler2D _LightSpecBuffer;
            #endif
            uniform fixed4 unity_Ambient;
            uniform float _VertexOcclusion;
            uniform float4 _DiffuseColour;
            uniform float _SnowLevel;
            uniform float4 _SnowColour;
            uniform float _SpecularPower;
            uniform float4 _SnowSpecularColour;
            uniform float4 _SpecularColour;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 vertexColor : COLOR;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float4 posWorld : TEXCOORD0;
                float3 normalDir : TEXCOORD1;
                float4 vertexColor : COLOR;
                float4 projPos : TEXCOORD2;
                UNITY_FOG_COORDS(3)
                #ifndef LIGHTMAP_OFF
                    float4 uvLM : TEXCOORD4;
                #else
                    float3 shLight : TEXCOORD4;
                #endif
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.vertexColor = v.vertexColor;
                o.normalDir = mul(unity_ObjectToWorld, float4(v.normal,0)).xyz;
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                o.pos = UnityObjectToClipPos(v.vertex);
                UNITY_TRANSFER_FOG(o,o.pos);
                o.projPos = ComputeScreenPos (o.pos);
                COMPUTE_EYEDEPTH(o.projPos.z);
                return o;
            }
            fixed4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
/////// Vectors:
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 normalDirection = i.normalDir;
                
                float nSign = sign( dot( viewDirection, i.normalDir ) ); // Reverse normal if this is a backface
                i.normalDir *= nSign;
                normalDirection *= nSign;
                
////// Lighting:
                half4 lightAccumulation = tex2Dproj(_LightBuffer, UNITY_PROJ_COORD(i.projPos));
                #if defined (SHADER_API_GLES) || defined (SHADER_API_GLES3)
                    lightAccumulation = max(lightAccumulation, half4(0.001));
                #endif
                #ifndef HDR_LIGHT_PREPASS_ON
                    lightAccumulation = -log2(lightAccumulation);
                #endif
                #if defined (SHADER_API_XBOX360) && defined (HDR_LIGHT_PREPASS_ON)
                    lightAccumulation.w = tex2Dproj (_LightSpecBuffer, UNITY_PROJ_COORD(i.projPos)).r;
                #endif
////// Specular:
                float node_24 = dot(i.normalDir,float3(0,1,0));
                float node_33 = lerp(1.0,(-1.0),_SnowLevel);
                float node_48_if_leA = step(node_24,node_33);
                float node_48_if_leB = step(node_33,node_24);
                float node_6 = pow(i.vertexColor.r,_VertexOcclusion);
                float3 node_50 = (_SnowSpecularColour.rgb*node_6);
                float3 specularColor = lerp((node_48_if_leA*(_SpecularColour.rgb*node_6))+(node_48_if_leB*node_50),node_50,node_48_if_leA*node_48_if_leB);
                float3 directSpecular = (lightAccumulation.rgb * 2)*lightAccumulation.a;
                float3 specular = directSpecular * specularColor;
/////// Diffuse:
                float3 indirectDiffuse = float3(0,0,0);
                float3 directDiffuse = lightAccumulation.rgb * 0.5;
                indirectDiffuse += unity_Ambient.rgb*0.5; // Ambient Light
                float node_31_if_leA = step(node_24,node_33);
                float node_31_if_leB = step(node_33,node_24);
                float3 node_44 = (_SnowColour.rgb*node_6);
                float3 diffuse = (directDiffuse + indirectDiffuse) * lerp((node_31_if_leA*(_DiffuseColour.rgb*node_6))+(node_31_if_leB*node_44),node_44,node_31_if_leA*node_31_if_leB);
/// Final Color:
                float3 finalColor = diffuse + specular;
                fixed4 finalRGBA = fixed4(finalColor,1);
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
            }
            ENDCG
        }
        Pass {
            Name "ForwardBase"
            Tags {
                "LightMode"="ForwardBase"
            }
            Cull Off
            
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #pragma multi_compile_fwdbase_fullshadows
            #pragma multi_compile_fog
            #pragma exclude_renderers xbox360 ps3 flash d3d11_9x 
            #pragma target 3.0
            uniform float4 _LightColor0;
            uniform float _VertexOcclusion;
            uniform float4 _DiffuseColour;
            uniform float _SnowLevel;
            uniform float4 _SnowColour;
            uniform float _SpecularPower;
            uniform float4 _SnowSpecularColour;
            uniform float4 _SpecularColour;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 vertexColor : COLOR;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float4 posWorld : TEXCOORD0;
                float3 normalDir : TEXCOORD1;
                float4 vertexColor : COLOR;
                LIGHTING_COORDS(2,3)
                UNITY_FOG_COORDS(4)
                #ifndef LIGHTMAP_OFF
                    float4 uvLM : TEXCOORD5;
                #else
                    float3 shLight : TEXCOORD5;
                #endif
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.vertexColor = v.vertexColor;
                o.normalDir = mul(unity_ObjectToWorld, float4(v.normal,0)).xyz;
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = UnityObjectToClipPos(v.vertex);
                UNITY_TRANSFER_FOG(o,o.pos);
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            fixed4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
/////// Vectors:
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 normalDirection = i.normalDir;
                
                float nSign = sign( dot( viewDirection, i.normalDir ) ); // Reverse normal if this is a backface
                i.normalDir *= nSign;
                normalDirection *= nSign;
                
                float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);
                float3 lightColor = _LightColor0.rgb;
                float3 halfDirection = normalize(viewDirection+lightDirection);
////// Lighting:
                float attenuation = LIGHT_ATTENUATION(i);
                float3 attenColor = attenuation * _LightColor0.xyz;
///////// Gloss:
                float gloss = _SpecularPower;
                float specPow = exp2( gloss * 10.0+1.0);
////// Specular:
                float NdotL = max(0, dot( normalDirection, lightDirection ));
                float node_24 = dot(i.normalDir,float3(0,1,0));
                float node_33 = lerp(1.0,(-1.0),_SnowLevel);
                float node_48_if_leA = step(node_24,node_33);
                float node_48_if_leB = step(node_33,node_24);
                float node_6 = pow(i.vertexColor.r,_VertexOcclusion);
                float3 node_50 = (_SnowSpecularColour.rgb*node_6);
                float3 specularColor = lerp((node_48_if_leA*(_SpecularColour.rgb*node_6))+(node_48_if_leB*node_50),node_50,node_48_if_leA*node_48_if_leB);
                float3 directSpecular = (floor(attenuation) * _LightColor0.xyz) * pow(max(0,dot(halfDirection,normalDirection)),specPow);
                float3 specular = directSpecular * specularColor;
/////// Diffuse:
                NdotL = max(0.0,dot( normalDirection, lightDirection ));
                float3 indirectDiffuse = float3(0,0,0);
                float3 directDiffuse = max( 0.0, NdotL) * attenColor;
                indirectDiffuse += UNITY_LIGHTMODEL_AMBIENT.rgb; // Ambient Light
                float node_31_if_leA = step(node_24,node_33);
                float node_31_if_leB = step(node_33,node_24);
                float3 node_44 = (_SnowColour.rgb*node_6);
                float3 diffuse = (directDiffuse + indirectDiffuse) * lerp((node_31_if_leA*(_DiffuseColour.rgb*node_6))+(node_31_if_leB*node_44),node_44,node_31_if_leA*node_31_if_leB);
/// Final Color:
                float3 finalColor = diffuse + specular;
                fixed4 finalRGBA = fixed4(finalColor,1);
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
            }
            ENDCG
        }
        Pass {
            Name "ForwardAdd"
            Tags {
                "LightMode"="ForwardAdd"
            }
            Blend One One
            Cull Off
            
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDADD
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #pragma multi_compile_fwdadd_fullshadows
            #pragma multi_compile_fog
            #pragma exclude_renderers xbox360 ps3 flash d3d11_9x 
            #pragma target 3.0
            uniform float4 _LightColor0;
            uniform float _VertexOcclusion;
            uniform float4 _DiffuseColour;
            uniform float _SnowLevel;
            uniform float4 _SnowColour;
            uniform float _SpecularPower;
            uniform float4 _SnowSpecularColour;
            uniform float4 _SpecularColour;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 vertexColor : COLOR;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float4 posWorld : TEXCOORD0;
                float3 normalDir : TEXCOORD1;
                float4 vertexColor : COLOR;
                LIGHTING_COORDS(2,3)
                #ifndef LIGHTMAP_OFF
                    float4 uvLM : TEXCOORD4;
                #else
                    float3 shLight : TEXCOORD4;
                #endif
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.vertexColor = v.vertexColor;
                o.normalDir = mul(unity_ObjectToWorld, float4(v.normal,0)).xyz;
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = UnityObjectToClipPos(v.vertex);
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            fixed4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
/////// Vectors:
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 normalDirection = i.normalDir;
                
                float nSign = sign( dot( viewDirection, i.normalDir ) ); // Reverse normal if this is a backface
                i.normalDir *= nSign;
                normalDirection *= nSign;
                
                float3 lightDirection = normalize(lerp(_WorldSpaceLightPos0.xyz, _WorldSpaceLightPos0.xyz - i.posWorld.xyz,_WorldSpaceLightPos0.w));
                float3 lightColor = _LightColor0.rgb;
                float3 halfDirection = normalize(viewDirection+lightDirection);
////// Lighting:
                float attenuation = LIGHT_ATTENUATION(i);
                float3 attenColor = attenuation * _LightColor0.xyz;
///////// Gloss:
                float gloss = _SpecularPower;
                float specPow = exp2( gloss * 10.0+1.0);
////// Specular:
                float NdotL = max(0, dot( normalDirection, lightDirection ));
                float node_24 = dot(i.normalDir,float3(0,1,0));
                float node_33 = lerp(1.0,(-1.0),_SnowLevel);
                float node_48_if_leA = step(node_24,node_33);
                float node_48_if_leB = step(node_33,node_24);
                float node_6 = pow(i.vertexColor.r,_VertexOcclusion);
                float3 node_50 = (_SnowSpecularColour.rgb*node_6);
                float3 specularColor = lerp((node_48_if_leA*(_SpecularColour.rgb*node_6))+(node_48_if_leB*node_50),node_50,node_48_if_leA*node_48_if_leB);
                float3 directSpecular = attenColor * pow(max(0,dot(halfDirection,normalDirection)),specPow);
                float3 specular = directSpecular * specularColor;
/////// Diffuse:
                NdotL = max(0.0,dot( normalDirection, lightDirection ));
                float3 directDiffuse = max( 0.0, NdotL) * attenColor;
                float node_31_if_leA = step(node_24,node_33);
                float node_31_if_leB = step(node_33,node_24);
                float3 node_44 = (_SnowColour.rgb*node_6);
                float3 diffuse = directDiffuse * lerp((node_31_if_leA*(_DiffuseColour.rgb*node_6))+(node_31_if_leB*node_44),node_44,node_31_if_leA*node_31_if_leB);
/// Final Color:
                float3 finalColor = diffuse + specular;
                return fixed4(finalColor * 1,0);
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
