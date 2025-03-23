#ifndef _TRIPLANARLOD_
#define _TRIPLANARLOD_

void TriplanarLOD_float(UnityTexture2D Texture, float3 Position, float3 Normal, float Tile, float Blend, out float4 Out)
{
    //float4 _SampleTexture2DLOD_RGBA = SAMPLE_TEXTURE2D_LOD(Texture, Texture, UV, LOD);
 
    float3 Node_UV = Position * Tile;
    float3 Node_Blend = pow(abs(Normal), Blend);
    Node_Blend /= dot(Node_Blend, 1.0);
    float4 Node_X = SAMPLE_TEXTURE2D_LOD(Texture, Texture.samplerstate, Node_UV.zy, 0);
    float4 Node_Y = SAMPLE_TEXTURE2D_LOD(Texture, Texture.samplerstate, Node_UV.xz, 0);
    float4 Node_Z = SAMPLE_TEXTURE2D_LOD(Texture, Texture.samplerstate, Node_UV.xy, 0);
    Out = Node_X * Node_Blend.x + Node_Y * Node_Blend.y + Node_Z * Node_Blend.z;
}

void TriplanarLOD_half(UnityTexture2D Texture, float3 Position, float3 Normal, float Tile, float Blend, out float4 Out)
{
    //float4 _SampleTexture2DLOD_RGBA = SAMPLE_TEXTURE2D_LOD(Texture, Texture, UV, LOD);
 
    float3 Node_UV = Position * Tile;
    float3 Node_Blend = pow(abs(Normal), Blend);
    Node_Blend /= dot(Node_Blend, 1.0);
    float4 Node_X = SAMPLE_TEXTURE2D_LOD(Texture, Texture.samplerstate, Node_UV.zy, 0);
    float4 Node_Y = SAMPLE_TEXTURE2D_LOD(Texture, Texture.samplerstate, Node_UV.xz, 0);
    float4 Node_Z = SAMPLE_TEXTURE2D_LOD(Texture, Texture.samplerstate, Node_UV.xy, 0);
    Out = Node_X * Node_Blend.x + Node_Y * Node_Blend.y + Node_Z * Node_Blend.z;
}

#endif