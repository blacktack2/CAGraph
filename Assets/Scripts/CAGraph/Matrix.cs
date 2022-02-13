using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Matrix
{
    [SerializeField, HideInInspector]
    private int[] _Cells;
    private Texture2D _Preview;
    public Texture2D preview {get {return _Preview;}}
    [SerializeField, HideInInspector, Min(2)]
    public int width;
    [SerializeField, HideInInspector, Min(2)]
    public int height;

    private long _ID;
    public long id {get {return _ID;}}

    public Matrix(int width, int height)
    {
        MatrixInit(width, height, DateTime.Now.Ticks);
    }

    private void MatrixInit(int width, int height, long id)
    {
        _ID = id;
        this.width = width;
        this.height = height;

        Reset();
    }

    public void Reset()
    {
        _Cells = new int[width * height];

        GeneratePreview();
    }

    public int[] GetCells()
    {
        return _Cells;
    }

    public void SetCells(int[] cells)
    {
        if (cells.Length != width * height)
            throw new FormatException(string.Format("Cannot accept matrix of different size (Expected size: {}, got {})", width * height, cells.Length));
        
        Array.Copy(cells, _Cells, _Cells.Length);
        UpdateID();
        GeneratePreview();
    }

    private void GeneratePreview()
    {
        Texture2D preview = new Texture2D(width, height);
        Color[] pixels = preview.GetPixels(0);

        for (int px = 0; px < pixels.Length; px++)
        {
            pixels[px] = _Cells[px] == 0 ? Color.black : Color.white;
        }

        preview.SetPixels(pixels);
        preview.Apply();
        float scale = Mathf.Min((float)CAEditorUtilities.previewWidth / (float)width, (float)CAEditorUtilities.previewWidth / (float)height);
        int targetWidth = (int) (width * scale);
        int targetHeight = (int) (height * scale);
        _Preview = CAEditorUtilities.ScaleTexture(preview, targetWidth, targetHeight);
    }

    public Matrix Copy()
    {
        Matrix copy = new Matrix(width, height);
        copy.SetCells(_Cells);
        return copy;
    }

    public void UpdateID()
    {
        _ID = DateTime.Now.Ticks;
    }
}
