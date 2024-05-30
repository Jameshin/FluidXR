using System;
using System.Collections.Generic;
using UnityEngine;

public struct GradientKey
{
  public float t { get; set; }
  public Color Color { get; set; }
  public GradientKey(float t, Color color)
  {
    this.t = t;
    this.Color = color;
  }
}
public class Gradient
{
  List<GradientKey> _keys;
  public Gradient()
  {
    _keys = new List<GradientKey>();
  }
  public int Count => _keys.Count;
  public GradientKey this[int index]
  {
    get => _keys[index];
    set { _keys[index] = value; sortKeys(); }
  }
  public void AddKey(float t, Color color) => AddKey(new GradientKey(t, color));
  public void AddKey(GradientKey key)
  {
    _keys.Add(key);
    sortKeys();
  }
  public void InsertKey(int index, float t, Color color) => InsertKey(index, new GradientKey(t, color));
  public void InsertKey(int index, GradientKey key)
  {
    _keys.Insert(index, key);
    sortKeys();
  }
  public void RemoveKey(int index)
  {
    _keys.RemoveAt(index);
    sortKeys();
  }
  public void RemoveInRange(float min, float max)
  {
    for (int i = _keys.Count - 1; i >= 0; i--)
      if (_keys[i].t >= min && _keys[i].t <= max) _keys.RemoveAt(i);
    sortKeys();
  }
  public void Clear() => _keys.Clear();
  void sortKeys() => _keys.Sort((a, b) => a.t.CompareTo(b.t));
  (int l, int r) getNeighborKeys(float t)
  {
    var l = Count - 1;

    for (int i = 0; i <= l; i++)
    {
      if (_keys[i].t >= t)
      {
        if (i == 0) return (-1, i);
        return (i - 1, i);
      }
    }

    return (l, -1);
  }
  public Color Evaluate(float t)
  {
    if (Count == 0) return new Color(0f, 0f, 0f, 0f);

    var n = getNeighborKeys(t);

    if (n.l < 0) return _keys[n.r].Color;
    else if (n.r < 0) return _keys[n.l].Color;

    return Color.Lerp(
        _keys[n.l].Color,
        _keys[n.r].Color,
        Mathf.InverseLerp(_keys[n.l].t, _keys[n.r].t, t)
    );
  }


  public void SetColorMap(int code, ref Gradient grad)
  {
    grad.Clear();
    if (code == 0)
    {
      grad.AddKey(0.0f, new Color(0.02f, 0.3813f, 0.9981f, 1.0f));
      grad.AddKey(0.02439f, new Color(0.0200001f, 0.424268f, 0.96907f, 1.0f));
      grad.AddKey(0.04878f, new Color(0.02f, 0.467234f, 0.940033f, 1.0f));
      grad.AddKey(0.07317f, new Color(0.02f, 0.5102f, 0.911f, 1.0f));
      grad.AddKey(0.09756f, new Color(0.0200001f, 0.546401f, 0.872669f, 1.0f));
      grad.AddKey(0.12195f, new Color(0.02f, 0.5826f, 0.834333f, 1.0f));
      grad.AddKey(0.14634f, new Color(0.02f, 0.6188f, 0.796f, 1.0f));
      grad.AddKey(0.17073f, new Color(0.0200001f, 0.652535f, 0.749802f, 1.0f));
      grad.AddKey(0.19512f, new Color(0.02f, 0.686267f, 0.7036f, 1.0f));
      grad.AddKey(0.21951f, new Color(0.02f, 0.72f, 0.6574f, 1.0f));

      grad.AddKey(0.2439f, new Color(0.0200001f, 0.757035f, 0.603735f, 1.0f));
      grad.AddKey(0.26829f, new Color(0.02f, 0.794067f, 0.550066f, 1.0f));
      grad.AddKey(0.29268f, new Color(0.02f, 0.8311f, 0.4964f, 1.0f));
      grad.AddKey(0.31707f, new Color(0.0213543f, 0.864537f, 0.428558f, 1.0f));
      grad.AddKey(0.34146f, new Color(0.0233129f, 0.897999f, 0.360739f, 1.0f));
      grad.AddKey(0.36585f, new Color(0.0159761f, 0.931048f, 0.292563f, 1.0f));
      grad.AddKey(0.39024f, new Color(0.274211f, 0.952563f, 0.153568f, 1.0f));
      grad.AddKey(0.41463f, new Color(0.493355f, 0.961904f, 0.111195f, 1.0f));
      grad.AddKey(0.43902f, new Color(0.6439f, 0.9773f, 0.0469f, 1.0f));
      grad.AddKey(0.46341f, new Color(0.762402f, 0.98467f, 0.0346002f, 1.0f));

      grad.AddKey(0.4878f, new Color(0.880901f, 0.992033f, 0.0222999f, 1.0f));
      grad.AddKey(0.51219f, new Color(0.999529f, 0.999519f, 0.0134885f, 1.0f));
      grad.AddKey(0.53658f, new Color(0.999403f, 0.955036f, 0.0790666f, 1.0f));
      grad.AddKey(0.56097f, new Color(0.9994f, 0.910666f, 0.148134f, 1.0f));
      grad.AddKey(0.58536f, new Color(0.9994f, 0.8663f, 0.2172f, 1.0f));
      grad.AddKey(0.60975f, new Color(0.99927f, 0.818036f, 0.217201f, 1.0f));
      grad.AddKey(0.63414f, new Color(0.999133f, 0.769766f, 0.2172f, 1.0f));
      grad.AddKey(0.65853f, new Color(0.999f, 0.7215f, 0.2172f, 1.0f));
      grad.AddKey(0.68292f, new Color(0.999136f, 0.673436f, 0.217201f, 1.0f));
      grad.AddKey(0.70731f, new Color(0.999267f, 0.625366f, 0.2172f, 1.0f));

      grad.AddKey(0.7317f, new Color(0.9994f, 0.5773f, 0.2172f, 1.0f));
      grad.AddKey(0.75609f, new Color(0.999403f, 0.521068f, 0.217201f, 1.0f));
      grad.AddKey(0.78048f, new Color(0.9994f, 0.464833f, 0.2172f, 1.0f));
      grad.AddKey(0.80487f, new Color(0.9994f, 0.4086f, 0.2172f, 1.0f));
      grad.AddKey(0.82926f, new Color(0.99476f, 0.331773f, 0.211231f, 1.0f));
      grad.AddKey(0.85365f, new Color(0.986713f, 0.259518f, 0.190122f, 1.0f));
      grad.AddKey(0.87804f, new Color(0.991246f, 0.147994f, 0.210789f, 1.0f));
      grad.AddKey(0.90243f, new Color(0.949903f, 0.116867f, 0.252901f, 1.0f));
      grad.AddKey(0.92682f, new Color(0.9032f, 0.0784329f, 0.2918f, 1.0f));
      grad.AddKey(0.95121f, new Color(0.8565f, 0.04f, 0.3307f, 1.0f));
      grad.AddKey(0.9756f, new Color(0.798903f, 0.0433335f, 0.358434f, 1.0f));
      grad.AddKey(1.0f, new Color(0.741299f, 0.0466667f, 0.386167f, 1.0f));
    }
  }
}
