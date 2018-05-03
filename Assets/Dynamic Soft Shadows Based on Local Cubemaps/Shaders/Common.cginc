/*
 * This confidential and proprietary software may be used only as
 * authorised by a licensing agreement from ARM Limited
 * (C) COPYRIGHT 2016 ARM Limited
 * ALL RIGHTS RESERVED
 * The entire notice above must be reproduced on all authorised
 * copies and copies may only be made to the extent permitted
 * by a licensing agreement from ARM Limited.
 */
 
#ifndef COMMON_INCLUDED
#define COMMON_INCLUDED


// Apply local correction and additionally returns in the fourth component the
// distance from the pixel to the intersection point.
float4 LocalCorrectAndLodDist(float3 origVec, float3 bboxMin, float3 bboxMax, float3 vertexPos, float3 cubemapPos)
{
    // Local-correction code
    // Find the ray intersection with box plane
    float3 invOrigVec = float3(1.0,1.0,1.0)/origVec;
    float3 FirstPlaneIntersect = (bboxMax - vertexPos) * invOrigVec;
    float3 SecondPlaneIntersect = (bboxMin - vertexPos) * invOrigVec;
    
    // Get the furthest of these intersections along the ray
    float3 FurthestPlane = max(FirstPlaneIntersect, SecondPlaneIntersect);

    // Find the closest far intersection
    float Distance = min(min(FurthestPlane.x, FurthestPlane.y), FurthestPlane.z);

    // Get the intersection position
    float3 IntersectPositionWS = vertexPos + origVec * Distance;
    // Get corrected vector
    float3 localCorrectedVec = IntersectPositionWS - cubemapPos;

    float lodDist = length(IntersectPositionWS - vertexPos);

    return float4(localCorrectedVec, lodDist);
}

#endif
