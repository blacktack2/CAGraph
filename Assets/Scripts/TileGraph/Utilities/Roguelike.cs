using System.Collections.Generic;
using UnityEngine;

namespace TileGraph.Utilities
{
    public partial class FunctionLibrary
    {
        public class Roguelike : SubLibrary
        {
            private struct RoomData
            {
                public RectInt partition;
                public RectInt? room;
                public int parent;
                public int left;
                public int right;
            }

            public Roguelike(FunctionLibrary functionLibrary) : base(functionLibrary)
            {
            }

            public void BinarySpacePartitioning(Types.TileMapUint tileMap,
                int seed = 0, float divisionChance = 0.5f,
                int minRooms = 1, int maxRooms = 10,
                int minRoomSize = 1, int maxRoomSize = 50,
                int minRoomArea = 1, int maxRoomArea = 2500,
                int minWallWidth = 0,
                bool showDebugLines = false)
            {
                const int maxIterations = 1000; // Since an infinite loop may be possible this is required

                // Division chance of 0 would result in an infinite loop
                // A very low division chance is still not a good idea either
                divisionChance = Mathf.Clamp(divisionChance, float.Epsilon, 1f);
                // Negative minWallWidth could result in indexError
                minWallWidth = Mathf.Max(minWallWidth, 0);

                List<RoomData> partitions = new List<RoomData>() {
                    new RoomData() {
                        partition=new RectInt(0, 0, tileMap.width, tileMap.height),
                        room=null,
                        parent=-1,
                        left=-1,
                        right=-1
                    }
                };
                int randomCounter = 0;

                bool divisionMade = true;
                int iteration = 0;
                while ((divisionMade || partitions.Count < minRooms) && !(partitions.Count >= maxRooms))
                {
                    iteration++;
                    if (iteration > maxIterations)
                        break;

                    divisionMade = false;
                    int partitionCount = partitions.Count; // Don't want to operate on partitions being added during the loop
                    for (int i = 0; i < partitionCount; i++)
                    {
                        RoomData partition = partitions[i];
                        if (partition.left != -1) // Only operate on leaf nodes
                            continue;

                        float doDivide = (float) _FunctionLibrary.Random01(seed, randomCounter++);
                        if (doDivide > divisionChance || partitions.Count >= maxRooms)
                            break;

                        float splitRand = _FunctionLibrary.Random01(seed, randomCounter++);
                        RectInt partition0;
                        RectInt partition1;
                        if (splitRand < 0.5f) // Split vertically
                        {
                            int leftWidth = Mathf.Max(minRoomSize, partition.partition.width / 2);
                            partition0 = new RectInt(partition.partition.xMin, partition.partition.yMin, leftWidth, partition.partition.height);
                            partition1 = new RectInt(partition.partition.xMin + leftWidth, partition.partition.yMin, partition.partition.width - leftWidth, partition.partition.height);
                        }
                        else // Split horizontally
                        {
                            int bottomHeight = Mathf.Max(minRoomSize, partition.partition.height / 2);
                            partition0 = new RectInt(partition.partition.xMin, partition.partition.yMin, partition.partition.width, bottomHeight);
                            partition1 = new RectInt(partition.partition.xMin, partition.partition.yMin + bottomHeight, partition.partition.width, partition.partition.height - bottomHeight);
                        }
                        int partition0Area = partition0.width * partition0.height;
                        int partition1Area = partition1.width * partition1.height;
                        // If partitions are within acceptable bounds add them to the list
                        if (!(partition0Area < minRoomArea || partition1Area < minRoomArea
                             || partition0.width - minWallWidth * 2 < minRoomSize || partition0.height - minWallWidth * 2 < minRoomSize
                             || partition1.width - minWallWidth * 2 < minRoomSize || partition1.height - minWallWidth * 2 < minRoomSize))
                        {
                            divisionMade = true;
                            RoomData left = new RoomData() {
                                partition=partition0,
                                room=null,
                                parent=i,
                                left=-1,
                                right=-1
                            };
                            partition.left = partitions.Count;
                            partitions.Add(left);
                            RoomData right = new RoomData() {
                                partition=partition1,
                                room=null,
                                parent=i,
                                left=-1,
                                right=-1
                            };
                            partition.right = partitions.Count;
                            partitions.Add(right);
                            partitions[i] = partition;
                        }
                    }
                }

                uint[] cells = new uint[tileMap.width * tileMap.height];

                // List<RectInt> rooms = new List<RectInt>();
                for (int i = 0; i < partitions.Count; i++)
                {
                    RoomData partition = partitions[i];
                    if (partition.left != -1) // Only operate on leaf nodes
                        continue;
                    RectInt rect = new RectInt(partition.partition.position, partition.partition.size);

                    float xRand = _FunctionLibrary.Random01(seed, randomCounter++);
                    float yRand = _FunctionLibrary.Random01(seed, randomCounter++);
                    float xOffsetRand = _FunctionLibrary.Random01(seed, randomCounter++);
                    float yOffsetRand = _FunctionLibrary.Random01(seed, randomCounter++);

                    int localMaxWidth  = Mathf.Min(rect.width,  maxRoomSize) - minWallWidth * 2;
                    int localMaxHeight = Mathf.Min(rect.height, maxRoomSize) - minWallWidth * 2;
                    
                    int targetWidth  = Mathf.FloorToInt(minRoomSize + xRand * (localMaxWidth - minRoomSize));
                    int targetHeight = Mathf.FloorToInt(minRoomSize + yRand * (localMaxHeight - minRoomSize));
                    
                    int leftOffset   = Mathf.FloorToInt(minWallWidth + rect.xMin + xOffsetRand * (rect.width  - targetWidth)  / 2);
                    int bottomOffset = Mathf.FloorToInt(minWallWidth + rect.yMin + yOffsetRand * (rect.height - targetHeight) / 2);
                    
                    rect.xMin = leftOffset;
                    rect.yMin = bottomOffset;
                    rect.width = targetWidth;
                    rect.height = targetHeight;

                    // rooms.Add(rect);
                    partition.room = rect;
                    partitions[i] = partition;
                }

                ApplyRooms(ref cells, partitions, tileMap.width);
                ApplyCorridors(ref cells, partitions, tileMap.width, seed, ref randomCounter, 0);
                if (showDebugLines)
                    ApplyDebugLines(ref cells, partitions, tileMap.width);

                tileMap.SetCells(cells);
            }

            private void ApplyRooms(ref uint[] cells, List<RoomData> rooms, int tmWidth)
            {
                foreach (RoomData partition in rooms)
                {
                    if (partition.left != -1)
                        continue;

                    RectInt rect = (RectInt) partition.room;
                    for (int x = rect.xMin; x < rect.xMax; x++)
                        for (int y = rect.yMin; y < rect.yMax; y++)
                            cells[x + y * tmWidth] = 1;
                }
            }

            private RectInt ApplyCorridors(ref uint[] cells, List<RoomData> partitions, int tmWidth, int seed, ref int randomCounter, int index)
            {
                RoomData partition = partitions[index];
                RoomData left = partitions[partition.left];
                RoomData right = partitions[partition.right];

                bool leftIsLeaf = left.left == -1;
                bool rightIsLeaf = right.left == -1;

                if (leftIsLeaf && rightIsLeaf)
                {
                    ApplyCorridorBetween(ref cells, partition.partition, (RectInt) left.room, (RectInt) right.room, tmWidth, seed, 1, ref randomCounter);
                    return (RectInt) left.room;
                }
                else
                {
                    RectInt leftRoom, rightRoom;
                    if (leftIsLeaf)
                        leftRoom = (RectInt) left.room;
                    else
                        leftRoom = ApplyCorridors(ref cells, partitions, tmWidth, seed, ref randomCounter, partition.left);
                    if (rightIsLeaf)
                        rightRoom = (RectInt) right.room;
                    else
                        rightRoom = ApplyCorridors(ref cells, partitions, tmWidth, seed, ref randomCounter, partition.right);
                    ApplyCorridorBetween(ref cells, partition.partition, leftRoom, rightRoom, tmWidth, seed, 1, ref randomCounter);
                    return leftRoom;
                }
            }

            private void ApplyCorridorBetween(ref uint[] cells, RectInt parentArea, RectInt rectA, RectInt rectB, int tmWidth, int seed, uint corridorValue, ref int randomCounter)
            {
                float doorPosRandA = _FunctionLibrary.Random01(seed, randomCounter++);
                float doorPosRandB = _FunctionLibrary.Random01(seed, randomCounter++);

                bool rectALeftB = rectB.xMin > rectA.xMax;
                bool rectAOverB = rectA.yMin > rectB.yMax;

                if (!(rectA.xMin > rectB.xMax || rectB.xMin > rectA.xMax)) // Overlaps across x
                {
                    int bottomX, topX;
                    if (rectAOverB)
                    {
                        bottomX = rectB.xMin + Mathf.FloorToInt(doorPosRandB * rectB.width);
                        topX    = rectA.xMin + Mathf.FloorToInt(doorPosRandA * rectA.width);
                    }
                    else
                    {
                        bottomX = rectA.xMin + Mathf.FloorToInt(doorPosRandA * rectA.width);
                        topX    = rectB.xMin + Mathf.FloorToInt(doorPosRandB * rectB.width);
                    }

                    int startY, stopY;
                    if (rectAOverB) // rectA is above rectB
                    {
                        startY = Mathf.Min(rectB.yMax, rectA.yMin);
                        stopY  = Mathf.Max(rectB.yMax, rectA.yMin);
                    }
                    else // rectB is above rectA
                    {
                        startY = Mathf.Min(rectA.yMax, rectB.yMin);
                        stopY  = Mathf.Max(rectA.yMax, rectB.yMin);
                    }
                    int distance = stopY - startY;
                    int midPointY = startY + distance / 2;
                    for (int i = 0; i < distance / 2; i++)
                    {
                        int y1 = startY + i;
                        int y2 = stopY  - i - 1;
                        cells[bottomX + y1 * tmWidth] = corridorValue;
                        cells[topX    + y2 * tmWidth] = corridorValue;
                    }

                    int bendDistance = topX - bottomX;
                    int startX = Mathf.Min(bottomX, topX);
                    int stopX  = Mathf.Max(bottomX, topX);
                    for (int x = startX; x < stopX; x++)
                        cells[x + midPointY * tmWidth] = corridorValue;
                }
                else if (!(rectA.yMin > rectB.yMax || rectB.yMin > rectA.yMax)) // Overlaps across y
                {
                    int leftY, rightY;
                    if (rectALeftB)
                    {
                        leftY  = rectA.yMin + Mathf.FloorToInt(doorPosRandA * rectA.height);
                        rightY = rectB.yMin + Mathf.FloorToInt(doorPosRandB * rectB.height);
                    }
                    else
                    {
                        leftY  = rectB.yMin + Mathf.FloorToInt(doorPosRandB * rectB.height);
                        rightY = rectA.yMin + Mathf.FloorToInt(doorPosRandA * rectA.height);
                    }

                    int startX, stopX;
                    if (rectALeftB) // rectA is to the right of rectB
                    {
                        startX = Mathf.Min(rectA.xMax, rectB.xMin);
                        stopX  = Mathf.Max(rectA.xMax, rectB.xMin);
                    }
                    else // rectB is to the right of rectA
                    {
                        startX = Mathf.Min(rectB.xMax, rectA.xMin);
                        stopX  = Mathf.Max(rectB.xMax, rectA.xMin);
                    }
                    int distance = stopX - startX;
                    int midPointX = startX + distance / 2;
                    for (int i = 0; i < distance / 2; i++)
                    {
                        int x1 = startX + i;
                        int x2 = stopX  - i - 1;
                        cells[x1 + leftY  * tmWidth] = corridorValue;
                        cells[x2 + rightY * tmWidth] = corridorValue;
                    }

                    int bendDistance = rightY - leftY;
                    int startY = Mathf.Min(leftY, rightY);
                    int stopY  = Mathf.Max(leftY, rightY);
                    for (int y = startY; y < stopY; y++)
                        cells[midPointX + y * tmWidth] = corridorValue;
                }
                else // No overlap (positioned diagonally to each other)
                {
                    float dirRand = _FunctionLibrary.Random01(seed, randomCounter++);

                    int pos1X, pos1Y, pos2X, pos2Y, pos3X, pos3Y;

                    if (dirRand < 0.5) // Vertical then horizontal
                    {
                        pos1X = rectA.xMin + Mathf.FloorToInt(doorPosRandA * rectA.width);
                        pos2Y = rectB.yMin + Mathf.FloorToInt(doorPosRandB * rectB.height);
                        if (rectALeftB) // A is left of B
                        {
                            pos3X = rectB.xMin;
                            if (rectAOverB) // A is above B
                                pos1Y = rectA.yMin;
                            else // A is below B
                                pos1Y = rectA.yMax;
                        }
                        else // A is right of B
                        {
                            pos3X = rectB.xMax;
                            if (rectAOverB) // A is above B
                                pos1Y = rectA.yMin;
                            else // A is below B
                                pos1Y = rectA.yMax;
                        }
                        pos2X = pos1X;
                        pos3Y = pos2Y;

                        for (int y = Mathf.Min(pos1Y, pos2Y); y < Mathf.Max(pos1Y, pos2Y); y++)
                            cells[pos1X + y * tmWidth] = corridorValue;
                        for (int x = Mathf.Min(pos2X, pos3X); x < Mathf.Max(pos2X, pos3X); x++)
                            cells[x + pos2Y * tmWidth] = corridorValue + 1;
                    }
                    else // Horizontal then vertical
                    {
                        pos1Y = rectA.yMin + Mathf.FloorToInt(doorPosRandA * rectA.height);
                        pos2X = rectB.xMin + Mathf.FloorToInt(doorPosRandB * rectB.width);
                        if (rectALeftB) // A is left of B
                        {
                            pos1X = rectA.xMax;
                            if (rectAOverB) // A is above B
                                pos3Y = rectB.yMax;
                            else // A is below B
                                pos3Y = rectB.yMin;
                        }
                        else // A is right of B
                        {
                            pos1X = rectA.xMin;
                            if (rectAOverB) // A is above B
                                pos3Y = rectB.yMax;
                            else // A is below B
                                pos3Y = rectB.yMin;
                        }
                        pos2Y = pos1Y;
                        pos3X = pos2X;

                        for (int x = Mathf.Min(pos1X, pos2X); x < Mathf.Max(pos1X, pos2X); x++)
                            cells[x + pos1Y * tmWidth] = corridorValue;
                        for (int y = Mathf.Min(pos2Y, pos3Y); y < Mathf.Max(pos2Y, pos3Y); y++)
                            cells[pos2X + y * tmWidth] = corridorValue + 1;
                    }
                }
            }

            private void ApplyDebugLines(ref uint[] cells, List<RoomData> partitions, int tmWidth)
            {
                foreach (RoomData partition in partitions)
                {
                    for (int x = partition.partition.xMin; x < partition.partition.xMax; x++)
                        cells[x + (partition.partition.yMax - 1) * tmWidth] = 2;

                    for (int y = partition.partition.yMin; y < partition.partition.yMax; y++)
                        cells[(partition.partition.xMax - 1) + y * tmWidth] = 2;
                }
            }
        }
    }
}
