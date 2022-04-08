using System.Collections.Generic;
using UnityEngine;

namespace TileGraph.Utilities
{
    public partial class FunctionLibrary
    {
        public class Roguelike : SubLibrary
        {
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
                const int maxIterations = 1000;
                // Division chance of 0 would result in an infinite loop
                // A very low division chance is still not a good idea either
                divisionChance = Mathf.Clamp(divisionChance, float.Epsilon, 1f);
                // Negative minWallWidth could result in indexError
                minWallWidth = Mathf.Max(minWallWidth, 0);

                List<Rect> partitions = new List<Rect>() {new Rect(0, 0, tileMap.width, tileMap.height)};
                int randomCounter = 0;

                bool divisionMade = true;
                int iteration = 0;
                while ((divisionMade || partitions.Count < minRooms) && !(partitions.Count >= maxRooms))
                {
                    iteration++;
                    if (iteration > maxIterations)
                        break;
                    divisionMade = false;
                    List<Rect> newPartitions = new List<Rect>();
                    int partitionsLeft = partitions.Count;
                    foreach (Rect partition in partitions)
                    {
                        float doDivide = (float) _FunctionLibrary.Random01(seed, randomCounter++);
                        if (doDivide > divisionChance || newPartitions.Count + partitionsLeft-- >= maxRooms)
                        {
                            newPartitions.Add(partition);
                            continue;
                        }

                        float direction = (float) _FunctionLibrary.Random01(seed, randomCounter++);
                        Rect partition0;
                        Rect partition1;
                        if (direction < 0.5f) // Split vertically
                        {
                            float leftWidth = Mathf.Max(minRoomSize, partition.width / 2);
                            partition0 = new Rect(partition.xMin, partition.yMin, leftWidth, partition.height);
                            partition1 = new Rect(partition.xMin + leftWidth, partition.yMin, partition.width - leftWidth, partition.height);
                        }
                        else // Split horizontally
                        {
                            float bottomHeight = Mathf.Max(minRoomSize, partition.height / 2);
                            partition0 = new Rect(partition.xMin, partition.yMin, partition.width, bottomHeight);
                            partition1 = new Rect(partition.xMin, partition.yMin + bottomHeight, partition.width, partition.height - bottomHeight);
                        }
                        float partition0Area = partition0.width * partition0.height;
                        float partition1Area = partition1.width * partition1.height;
                        if (partition0Area < minRoomArea || partition1Area < minRoomArea
                            || partition0.width - minWallWidth * 2 < minRoomSize || partition0.height - minWallWidth * 2 < minRoomSize
                            || partition1.width - minWallWidth * 2 < minRoomSize || partition1.height - minWallWidth * 2 < minRoomSize)
                        {
                            newPartitions.Add(partition);
                        }
                        else
                        {
                            divisionMade = true;
                            newPartitions.Add(partition0);
                            newPartitions.Add(partition1);
                        }
                    }
                    partitions = newPartitions;
                }

                uint[] cells = new uint[tileMap.width * tileMap.height];
                for (int i = 0; i < partitions.Count; i++)
                {
                    Rect rect = new Rect(partitions[i].position, partitions[i].size);

                    float xRand = _FunctionLibrary.Random01(seed, randomCounter++);
                    float yRand = _FunctionLibrary.Random01(seed, randomCounter++);
                    float xOffsetRand = _FunctionLibrary.Random01(seed, randomCounter++);
                    float yOffsetRand = _FunctionLibrary.Random01(seed, randomCounter++);

                    float localMaxWidth  = Mathf.Min(rect.width,  maxRoomSize) - minWallWidth * 2;
                    float localMaxHeight = Mathf.Min(rect.height, maxRoomSize) - minWallWidth * 2;
                    
                    float targetWidth  = Mathf.Floor(minRoomSize + xRand * (localMaxWidth - minRoomSize));
                    float targetHeight = Mathf.Floor(minRoomSize + yRand * (localMaxHeight - minRoomSize));
                    
                    float leftOffset   = Mathf.Floor(minWallWidth + rect.xMin + xOffsetRand * (rect.width  - targetWidth)  / 2);
                    float bottomOffset = Mathf.Floor(minWallWidth + rect.yMin + yOffsetRand * (rect.height - targetHeight) / 2);
                    
                    rect.xMin = leftOffset;
                    rect.yMin = bottomOffset;
                    rect.width = targetWidth;
                    rect.height = targetHeight;

                    for (int x = Mathf.FloorToInt(rect.xMin); x < Mathf.Floor(rect.xMax); x++)
                    {
                        for (int y = Mathf.FloorToInt(rect.yMin); y < Mathf.Floor(rect.yMax); y++)
                        {
                            cells[x + y * tileMap.width] = 1;
                        }
                    }

                    if (showDebugLines)
                    {
                        for (int x = Mathf.FloorToInt(partitions[i].xMin); x < partitions[i].xMax; x++)
                            cells[x + (Mathf.FloorToInt(partitions[i].yMax) - 1) * tileMap.width] = 2;

                        for (int y = Mathf.FloorToInt(partitions[i].yMin); y < partitions[i].yMax; y++)
                            cells[(Mathf.FloorToInt(partitions[i].xMax) - 1) + y * tileMap.width] = 2;
                    }
                }
                tileMap.SetCells(cells);
            }
        }
    }
}
