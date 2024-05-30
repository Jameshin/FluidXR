using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class FrameFileLoader
{
  public static Frame Parse(string filename)
  {
    var assetPath = Application.streamingAssetsPath + $"/SimulationData/{filename}.json";
    using (StreamReader file = File.OpenText(assetPath))
    {
      JsonSerializer serializer = new JsonSerializer();
      Frame gamma = (Frame)serializer.Deserialize(file, typeof(Frame));
      return gamma;
    }
  }
}
