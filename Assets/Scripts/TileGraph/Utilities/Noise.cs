using UnityEngine;

namespace TileGraph.Utilities
{
    public partial class FunctionLibrary
    {
        public class Noise : SubLibrary
        {
            private static int[] p = new int[] {
                151, 160, 137, 91, 90, 15, 131, 13, 201, 95, 96, 53, 194, 233, 7, 225, 140,
                36, 103, 30, 69, 142, 8, 99, 37, 240, 21, 10, 23, 190, 6, 148, 247, 120, 234,
                75, 0, 26, 197, 62, 94, 252, 219, 203, 117, 35, 11, 32, 57, 177, 33, 88, 237,
                149, 56, 87, 174, 20, 125, 136, 171, 168, 68, 175, 74, 165, 71, 134, 139, 48,
                27, 166, 77, 146, 158, 231, 83, 111, 229, 122, 60, 211, 133, 230, 220, 105,
                92, 41, 55, 46, 245, 40, 244, 102, 143, 54, 65, 25, 63, 161, 1, 216, 80, 73,
                209, 76, 132, 187, 208, 89, 18, 169, 200, 196, 135, 130, 116, 188, 159, 86,
                164, 100, 109, 198, 173, 186, 3, 64, 52, 217, 226, 250, 124, 123, 5, 202, 38,
                147, 118, 126, 255, 82, 85, 212, 207, 206, 59, 227, 47, 16, 58, 17, 182, 189,
                28, 42, 223, 183, 170, 213, 119, 248, 152, 2, 44, 154, 163, 70, 221, 153, 101,
                155, 167, 43, 172, 9, 129, 22, 39, 253, 19, 98, 108, 110, 79, 113, 224, 232,
                178, 185, 112, 104, 218, 246, 97, 228, 251, 34, 242, 193, 238, 210, 144, 12,
                191, 179, 162, 241, 81, 51, 145, 235, 249, 14, 239, 107, 49, 192, 214, 31,
                181, 199, 106, 157, 184, 84, 204, 176, 115, 121, 50, 45, 127, 4, 150, 254,
                138, 236, 205, 93, 222, 114, 67, 29, 24, 72, 243, 141, 128, 195, 78, 66, 215,
                61, 156, 180,
                151, 160, 137, 91, 90, 15, 131, 13, 201, 95, 96, 53, 194, 233, 7, 225, 140,
                36, 103, 30, 69, 142, 8, 99, 37, 240, 21, 10, 23, 190, 6, 148, 247, 120, 234,
                75, 0, 26, 197, 62, 94, 252, 219, 203, 117, 35, 11, 32, 57, 177, 33, 88, 237,
                149, 56, 87, 174, 20, 125, 136, 171, 168, 68, 175, 74, 165, 71, 134, 139, 48,
                27, 166, 77, 146, 158, 231, 83, 111, 229, 122, 60, 211, 133, 230, 220, 105,
                92, 41, 55, 46, 245, 40, 244, 102, 143, 54, 65, 25, 63, 161, 1, 216, 80, 73,
                209, 76, 132, 187, 208, 89, 18, 169, 200, 196, 135, 130, 116, 188, 159, 86,
                164, 100, 109, 198, 173, 186, 3, 64, 52, 217, 226, 250, 124, 123, 5, 202, 38,
                147, 118, 126, 255, 82, 85, 212, 207, 206, 59, 227, 47, 16, 58, 17, 182, 189,
                28, 42, 223, 183, 170, 213, 119, 248, 152, 2, 44, 154, 163, 70, 221, 153, 101,
                155, 167, 43, 172, 9, 129, 22, 39, 253, 19, 98, 108, 110, 79, 113, 224, 232,
                178, 185, 112, 104, 218, 246, 97, 228, 251, 34, 242, 193, 238, 210, 144, 12,
                191, 179, 162, 241, 81, 51, 145, 235, 249, 14, 239, 107, 49, 192, 214, 31,
                181, 199, 106, 157, 184, 84, 204, 176, 115, 121, 50, 45, 127, 4, 150, 254,
                138, 236, 205, 93, 222, 114, 67, 29, 24, 72, 243, 141, 128, 195, 78, 66, 215,
                61, 156, 180
            };
            private const float F2 = 0.366025403f;
            private const float G2 = 0.211324865f;
            private const float F3 = 0.333333333f;
            private const float G3 = 0.166666667f;

            public enum Algorithm { Perlin, Simplex }

            public Noise(FunctionLibrary functionLibrary) : base(functionLibrary)
            {
            }

            public void GradientNoise2D(Types.TileMapCont tileMap, Vector2? frequency = null, Vector2? offset = null,
                                        uint octaves = 1, float[] lacunarity = null, float[] persistence = null,
                                        Algorithm algorithm = Algorithm.Simplex, bool useGPU = true)
            {
                if (frequency == null)
                    frequency = new Vector2(0.1f, 0.1f);
                if (offset == null)
                    offset = Vector2.zero;
                if (lacunarity == null)
                    lacunarity = new float[] {2f, 4f, 8f, 16f, 32f, 64f, 128f, 256f, 512f, 1024f, 2048f, 4096f, 8192f, 16384f, 32768f, 65536f, 131072f, 262144f, 524288f, 1048576f};
                if (persistence == null)
                    persistence = new float[] {0.5f, 0.25f, 0.125f, 0.0625f, 0.03125f, 0.015625f, 0.0078125f, 0.00390625f, 0.001953125f, 0.0009765625f, 0.00048828125f, 0.000244140625f, 0.0001220703125f, 6.103515625e-05f, 3.0517578125e-05f, 1.52587890625e-05f, 7.62939453125e-06f, 3.814697265625e-06f, 1.9073486328125e-06f, 9.5367431640625e-07f};
                if (octaves <= 1)
                {
                    if (useGPU)
                        GradientNoise2DGPU(tileMap, (Vector2) frequency, (Vector2) offset, algorithm);
                    else
                        GradientNoise2DCPU(tileMap, (Vector2) frequency, (Vector2) offset, algorithm);
                }
                else
                {
                    if (useGPU)
                        FractalGradientNoise2DGPU(tileMap, (Vector2) frequency, (Vector2) offset, octaves, lacunarity, persistence, algorithm);
                    else
                        FractalGradientNoise2DCPU(tileMap, (Vector2) frequency, (Vector2) offset, octaves, lacunarity, persistence, algorithm);
                }
            }
            private void GradientNoise2DCPU(Types.TileMapCont tileMap, Vector2 frequency, Vector2 offset, Algorithm algorithm)
            {
                switch (algorithm)
                {
                    case Algorithm.Perlin:
                        PerlinNoise2DCPU(tileMap, frequency, offset);
                        break;
                    case Algorithm.Simplex: default:
                        SimplexNoise2DCPU(tileMap, frequency, offset);
                        break;
                }
            }
            private float Grad(int seed, float x, float y)
            {
                int h = seed & 7;
                float u = h < 4 ? x : y;
                float v = h < 4 ? y : x;
                return ((h & 1) != 0 ? u : -u) + ((h & 2) != 0 ? 2f * v : -2f * v);
            }
            private float Fade(float t)
            {
                return t * t * t * (t * (t * 6f - 15f) + 10f);
            }
            private float PerlinNoise2D(float x, float y)
            {
                int xi = Mathf.FloorToInt(x) & 255;
                int yi = Mathf.FloorToInt(y) & 255;
                float xd = x - xi;
                float yd = y - yi;
                float xc = Fade(xd);
                float yc = Fade(yd);

                int A = p[xi] + yi;
                int B = p[xi + 1] + yi;

                return Mathf.Lerp(Mathf.Lerp(Grad(p[A    ], xd    , yd    ),
                                             Grad(p[B    ], xd - 1, yd    ), xc),
                                  Mathf.Lerp(Grad(p[A + 1], xd    , yd - 1),
                                             Grad(p[B + 1], xd - 1, yd - 1), xc), yc);
            }
            private void PerlinNoise2DCPU(Types.TileMapCont tileMap, Vector2 frequency, Vector2 offset)
            {
                float[] cells = new float[tileMap.width * tileMap.height];
                for (int c = 0, x = 0, y = 0; c < cells.Length; c++, x++)
                {
                    if (x >= tileMap.width)
                    {
                        x = 0;
                        y++;
                    }
                    cells[c] = 0.5f + PerlinNoise2D(x * frequency.x + offset.x, y * frequency.y + offset.y) * 0.5f;
                }
                tileMap.SetCells(cells);
            }
            
            private float SimplexNoise2D(float x, float y)
            {
                float n0, n1, n2;

                float s = (x + y) * F2;
                float xs = x + s;
                float ys = y + s;
                int i = Mathf.FloorToInt(xs);
                int j = Mathf.FloorToInt(ys);

                float t = (i + j) * G2;
                float X0 = i - t;
                float Y0 = j - t;
                float x0 = x - X0;
                float y0 = y - Y0;

                int i1, j1;
                if (x0 > y0)
                {
                    i1 = 1;
                    j1 = 0;
                }
                else
                {
                    i1 = 0;
                    j1 = 1;
                }

                float x1 = x0 - i1 + G2;
                float y1 = y0 - j1 + G2;
                float x2 = x0 - 1f + 2f * G2;
                float y2 = y0 - 1f + 2f * G2;

                int ii = i & 255;
                int jj = j & 255;

                float t0 = 0.5f - x0 * x0 - y0 * y0;
                if (t0 < 0f)
                {
                    n0 = 0f;
                }
                else
                {
                    t0 *= t0;
                    n0 = t0 * t0 * Grad(p[ii + p[jj]], x0, y0);
                }

                float t1 = 0.5f - x1 * x1 - y1 * y1;
                if (t1 < 0f)
                {
                    n1 = 0f;
                }
                else
                {
                    t1 *= t1;
                    n1 = t1 * t1 * Grad(p[ii + i1 + p[jj + j1]], x1, y1);
                }

                float t2 = 0.5f - x2 * x2 - y2 * y2;
                if (t2 < 0.0)
                {
                    n2 = 0f;
                }
                else
                {
                    t2 *= t2;
                    n2 = t2 * t2 * Grad(p[ii + 1 + p[jj + 1]], x2, y2);
                }

                return 40f * (n0 + n1 + n2);
            }
            private void SimplexNoise2DCPU(Types.TileMapCont tileMap, Vector2 frequency, Vector2 offset)
            {
                float[] cells = new float[tileMap.width * tileMap.height];
                for (int c = 0, x = 0, y = 0; c < cells.Length; c++, x++)
                {
                    if (x >= tileMap.width)
                    {
                        x = 0;
                        y++;
                    }
                    cells[c] = 0.5f + SimplexNoise2D(x * frequency.x + offset.x, y * frequency.y + offset.y) * 0.5f;
                }
                tileMap.SetCells(cells);
            }
            private void GradientNoise2DGPU(Types.TileMapCont tileMap, Vector2 frequency, Vector2 offset, Algorithm algorithm)
            {
                int kernelIndex;
                switch (algorithm)
                {
                    case Algorithm.Perlin:
                        kernelIndex = (int) FunctionLibrary.FunctionKernels.PerlinNoise2D;
                        break;
                    case Algorithm.Simplex: default:
                        kernelIndex = (int) FunctionLibrary.FunctionKernels.SimplexNoise2D;
                        break;
                }

                float[] cells = new float[tileMap.width * tileMap.height];

                _FunctionLibrary._TileMapCont0Buffer.SetData(cells);
                _FunctionLibrary._TileMapCont1Buffer.SetData(cells);

                _FunctionLibrary._ComputeShader.SetInt(_ScaleXID, tileMap.width);
                _FunctionLibrary._ComputeShader.SetInt(_ScaleYID, tileMap.height);
                _FunctionLibrary._ComputeShader.SetVector(_FrequencyID, frequency);
                _FunctionLibrary._ComputeShader.SetVector(_OffsetID, offset);
                _FunctionLibrary._ComputeShader.SetBuffer(kernelIndex, _TileMapCont0ID, _FunctionLibrary._TileMapCont0Buffer);
                _FunctionLibrary._ComputeShader.SetBuffer(kernelIndex, _TileMapCont1ID, _FunctionLibrary._TileMapCont1Buffer);

                _FunctionLibrary._ComputeShader.SetBool(_BufferFlagID, false);
                
                int groupsX = Mathf.CeilToInt(tileMap.width / 8f);
                int groupsY = Mathf.CeilToInt(tileMap.height / 8f);
                _FunctionLibrary._ComputeShader.Dispatch(kernelIndex, groupsX, groupsY, 1);

                _FunctionLibrary._TileMapCont1Buffer.GetData(cells);
                tileMap.SetCells(cells);
            }
            private void FractalGradientNoise2DCPU(Types.TileMapCont tileMap, Vector2 frequency, Vector2 offset,
                                                 uint octaves, float[] lacunarity, float[] persistence, Algorithm algorithm)
            {
                switch (algorithm)
                {
                    case Algorithm.Perlin:
                        FractalPerlinNoise2DCPU(tileMap, frequency, offset, octaves, lacunarity, persistence);
                        break;
                    case Algorithm.Simplex: default:
                        FractalSimplexNoise2DCPU(tileMap, frequency, offset, octaves, lacunarity, persistence);
                        break;
                }
            }
            private void FractalPerlinNoise2DCPU(Types.TileMapCont tileMap, Vector2 frequency, Vector2 offset,
                                                 uint octaves, float[] lacunarity, float[] persistence)
            {
                float[] cells = new float[tileMap.width * tileMap.height];
                for (int c = 0, x = 0, y = 0; c < cells.Length; c++, x++)
                {
                    if (x >= tileMap.width)
                    {
                        x = 0;
                        y++;
                    }
                    float px = x * frequency.x + offset.x;
                    float py = y * frequency.y + offset.y;
                    cells[c] = PerlinNoise2D(px, py);
                    float totalMax = 1f;
                    for (int o = 0; o < octaves; o++)
                    {
                        cells[c] += PerlinNoise2D(px * lacunarity[o], py * lacunarity[o]) * persistence[o];
                        totalMax += persistence[o];
                    }
                    cells[c] = (cells[c] / totalMax) * 0.5f + 0.5f;
                }
                tileMap.SetCells(cells);
            }
            private void FractalSimplexNoise2DCPU(Types.TileMapCont tileMap, Vector2 frequency, Vector2 offset,
                                                 uint octaves, float[] lacunarity, float[] persistence)
            {
                float[] cells = new float[tileMap.width * tileMap.height];
                for (int c = 0, x = 0, y = 0; c < cells.Length; c++, x++)
                {
                    if (x >= tileMap.width)
                    {
                        x = 0;
                        y++;
                    }
                    float px = x * frequency.x + offset.x;
                    float py = y * frequency.y + offset.y;
                    cells[c] = SimplexNoise2D(px, py);
                    float totalMax = 1f;
                    for (int o = 0; o < octaves; o++)
                    {
                        cells[c] += SimplexNoise2D(px * lacunarity[o], py * lacunarity[o]) * persistence[o];
                        totalMax += persistence[o];
                    }
                    cells[c] = (cells[c] / totalMax) * 0.5f + 0.5f;
                }
                tileMap.SetCells(cells);
            }
            private void FractalGradientNoise2DGPU(Types.TileMapCont tileMap, Vector2 frequency, Vector2 offset,
                                                 uint octaves, float[] lacunarity, float[] persistence, Algorithm algorithm)
            {
                int kernelIndex;
                switch (algorithm)
                {
                    case Algorithm.Perlin:
                        kernelIndex = (int) FunctionLibrary.FunctionKernels.FractalPerlinNoise2D;
                        break;
                    case Algorithm.Simplex: default:
                        kernelIndex = (int) FunctionLibrary.FunctionKernels.FractalSimplexNoise2D;
                        break;
                }

                float[] cells = new float[tileMap.width * tileMap.height];

                _FunctionLibrary._TileMapCont0Buffer.SetData(cells);
                _FunctionLibrary._TileMapCont1Buffer.SetData(cells);
                _FunctionLibrary._LacunarityBuffer.SetData(lacunarity, 0, 0, (int) octaves);
                _FunctionLibrary._PersistenceBuffer.SetData(persistence, 0, 0, (int) octaves);

                _FunctionLibrary._ComputeShader.SetInt(_ScaleXID, tileMap.width);
                _FunctionLibrary._ComputeShader.SetInt(_ScaleYID, tileMap.height);
                _FunctionLibrary._ComputeShader.SetInt(_OctavesID, (int) Mathf.Max(2, octaves));
                _FunctionLibrary._ComputeShader.SetVector(_FrequencyID, frequency);
                _FunctionLibrary._ComputeShader.SetVector(_OffsetID, offset);
                _FunctionLibrary._ComputeShader.SetBuffer(kernelIndex, _TileMapCont0ID, _FunctionLibrary._TileMapCont0Buffer);
                _FunctionLibrary._ComputeShader.SetBuffer(kernelIndex, _TileMapCont1ID, _FunctionLibrary._TileMapCont1Buffer);
                _FunctionLibrary._ComputeShader.SetBuffer(kernelIndex, _LacunarityID, _FunctionLibrary._LacunarityBuffer);
                _FunctionLibrary._ComputeShader.SetBuffer(kernelIndex, _PersistenceID, _FunctionLibrary._PersistenceBuffer);

                _FunctionLibrary._ComputeShader.SetBool(_BufferFlagID, false);
                
                int groupsX = Mathf.CeilToInt(tileMap.width / 8f);
                int groupsY = Mathf.CeilToInt(tileMap.height / 8f);
                _FunctionLibrary._ComputeShader.Dispatch(kernelIndex, groupsX, groupsY, 1);

                _FunctionLibrary._TileMapCont1Buffer.GetData(cells);
                tileMap.SetCells(cells);
            }

            public void VoronoiNoise2D(Types.TileMapCont tileMap, Vector2? frequency, Vector2? offset, bool useGpu = false)
            {
                if (frequency == null)
                    frequency = new Vector2(0.1f, 0.1f);
                if (offset == null)
                    offset = Vector2.zero;
                
                if (useGpu)
                    VoronoiNoise2DGPU(tileMap, (Vector2) frequency, (Vector2) offset);
                else
                    VoronoiNoise2DCPU(tileMap, (Vector2) frequency, (Vector2) offset);
            }

            private void VoronoiNoise2DCPU(Types.TileMapCont tileMap, Vector2 frequency, Vector2 offset)
            {
                float[] cells = new float[tileMap.width * tileMap.height];
                for (int c = 0, x = 0, y = 0; c < cells.Length; c++, x++)
                {
                    if (x >= tileMap.width)
                    {
                        x = 0;
                        y++;
                    }
                    float xp = x * frequency.x + offset.x;
                    float yp = y * frequency.y + offset.y;
                    int xi = Mathf.FloorToInt(xp);
                    int yi = Mathf.FloorToInt(yp);
                    float xf = xp - xi;
                    float yf = yp - yi;

                    float minDist = 1f;
                    for (int i = -1; i <= 1; i++)
                    {
                        for (int j = -1; j <= 1; j++)
                        {
                            Vector2Int r = _FunctionLibrary.RandomPCG(new Vector2Int(xi + i, yi + j));

                            float px = ((float) r.x / 2147483648f);
                            px -= Mathf.Floor(px);
                            float py = ((float) r.y / 2147483648f);
                            py -= Mathf.Floor(py);

                            float dx = i + px - xf;
                            float dy = j + py - yf;

                            float dist = Mathf.Sqrt(dx * dx + dy * dy);

                            minDist = Mathf.Min(minDist, dist);
                        }
                    }

                    cells[c] = minDist;
                }
                tileMap.SetCells(cells);
            }

            private void VoronoiNoise2DGPU(Types.TileMapCont tileMap, Vector2 frequency, Vector2 offset)
            {
                const int kernelIndex = (int) FunctionLibrary.FunctionKernels.VoronoiNoise2D;

                float[] cells = new float[tileMap.width * tileMap.height];

                _FunctionLibrary._TileMapCont0Buffer.SetData(cells);
                _FunctionLibrary._TileMapCont1Buffer.SetData(cells);

                _FunctionLibrary._ComputeShader.SetInt(_ScaleXID, tileMap.width);
                _FunctionLibrary._ComputeShader.SetInt(_ScaleYID, tileMap.height);
                _FunctionLibrary._ComputeShader.SetVector(_FrequencyID, frequency);
                _FunctionLibrary._ComputeShader.SetVector(_OffsetID, offset);
                _FunctionLibrary._ComputeShader.SetBuffer(kernelIndex, _TileMapCont0ID, _FunctionLibrary._TileMapCont0Buffer);
                _FunctionLibrary._ComputeShader.SetBuffer(kernelIndex, _TileMapCont1ID, _FunctionLibrary._TileMapCont1Buffer);

                _FunctionLibrary._ComputeShader.SetBool(_BufferFlagID, false);
                
                int groupsX = Mathf.CeilToInt(tileMap.width / 8f);
                int groupsY = Mathf.CeilToInt(tileMap.height / 8f);
                _FunctionLibrary._ComputeShader.Dispatch(kernelIndex, groupsX, groupsY, 1);

                _FunctionLibrary._TileMapCont1Buffer.GetData(cells);
                tileMap.SetCells(cells);
            }
        }
    }
}
