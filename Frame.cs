using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class Frame
{
  [SerializeField] public List<int> blocks;
  [SerializeField] public List<List<int>> trigs;
  [SerializeField] public List<List<int>> types;
  [SerializeField] public List<List<List<float>>> pnts;
  [SerializeField] public List<List<float>> scals;
}