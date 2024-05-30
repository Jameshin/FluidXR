using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

public class ParticleFrameRenderer : MonoBehaviour
{
    [SerializeField] private FrameDatabase frameDatabase;
  private Gradient gradient;
  private float scalarMin = 1.0f;
  private float scalarMax = -1.0f;
  public Material ParticleMaterial;
  public GameObject ParticleParent;
    public float scale = 3.0f;
  private ParticleSystem.Particle particleTemplate;
  private List<List<ParticleSystem.Particle[]>> particleBlockFrames;
  private GameObject[] blockGameObjects;
  private int maxNumberOfVertices = 60000;
  private int currentFrameIndex = -1;
  private bool ready = false;

  async void Start()
  {
    gradient = new Gradient();
    gradient.SetColorMap(0, ref gradient);

    particleTemplate = new ParticleSystem.Particle
    {
      startLifetime = 10000000,
      remainingLifetime = 1,
      startSize = 0.03f,
      startColor = Color.red
    };

    particleBlockFrames = await LoadFramesAsync();

    blockGameObjects = new GameObject[2];
    for (var i = 0; i < blockGameObjects.Length; ++i)
    {
      blockGameObjects[i] = new GameObject($"block{i}");
      blockGameObjects[i].transform.SetParent(ParticleParent.transform, false);
      var particleSystem = blockGameObjects[i].AddComponent<ParticleSystem>();
      var ts = particleSystem.textureSheetAnimation;
      ts.enabled = true;
      ts.numTilesX = 8;
      ts.numTilesY = 8;
      ts.animation = ParticleSystemAnimationType.WholeSheet;
      ts.frameOverTime = new ParticleSystem.MinMaxCurve(0, 1);

      var particleRenderer = blockGameObjects[i].GetComponent<ParticleSystemRenderer>();
      particleRenderer.material = ParticleMaterial;
    }

    ready = true;
  }

  private void Update()
  {
    if (ready)
    {
      var nextFrameIndex = (currentFrameIndex + 1) % particleBlockFrames.Count;
      FlipToFrame(nextFrameIndex);
      currentFrameIndex = nextFrameIndex;
    }
  }

  private void FlipToFrame(int index)
  {
    var blocks = particleBlockFrames[index];

    for (var i = 0; i < blockGameObjects.Length; ++i)
    {
      var particleSystem = blockGameObjects[i].GetComponent<ParticleSystem>();
      var mainParticleSystem = particleSystem.main;
      mainParticleSystem.maxParticles = Math.Min(maxNumberOfVertices, blocks[0].Length);
      particleSystem.SetParticles(blocks[i], blocks[i].Length);
    }
  }

  private async Task<List<List<ParticleSystem.Particle[]>>> LoadFramesAsync()
  {
    var particleBlockFrames = new List<List<ParticleSystem.Particle[]>>();
        var frames = frameDatabase.Frames;
    foreach (var frame in frames)
    {
      var particleSet = new List<ParticleSystem.Particle[]>();
      foreach (var block in frame.blocks)
      {
        var pnts = frame.trigs[block].pnts.Select((pnt) => new Vector3(pnt.pnts[0]*3.8f, pnt.pnts[1]*3.8f, pnt.pnts[2]*3.8f)).ToList();
        var scalars = frame.trigs[block].scals;

        scalars.ToList().ForEach((s) =>
        {
            if (s < scalarMin)
            {
                scalarMin = s;
            }

            if (s > scalarMax)
            {
                scalarMax = s;
            }
        });

        var particles = new ParticleSystem.Particle[pnts.Count];
        for (var j = 0; j < pnts.Count; ++j)
        {
          particleTemplate.position = pnts[j];
          var val = Rescale(scalarMin, scalarMax, 0, 1, scalars[j]);
          var c = gradient.Evaluate(val);
          particleTemplate.startColor = c;
          particles[j] = particleTemplate;
        }
        particleSet.Add(particles);

        //Debug.Log($"{i}th frame, {block}th block, {pnts.Count} particles, {scalars.Count} scalars");
      }
      particleBlockFrames.Add(particleSet);
      await Task.Delay(1);
    }


        //var framesDatabase = ScriptableObject.CreateInstance<FrameDatabase>();

        //List<FrameWrapper> frameWrappers = new List<FrameWrapper>();
        //foreach (var frame in frames)
        //{
        //    var frameWrapper = new FrameWrapper();
        //    frameWrapper.blocks = frame.blocks;
        //    frameWrapper.trigs = new List<TrigsListWrapper>();
        //    foreach (var block in frame.blocks)
        //    {
        //        frameWrapper.trigs.Add(new TrigsListWrapper());
        //        frameWrapper.trigs[block].trigs = frame.trigs[block];
        //        frameWrapper.trigs[block].types = frame.types[block];
        //        frameWrapper.trigs[block].pnts = new List<PointsListWrapper>();
        //        for (var i =0; i < frame.pnts[block].Count; ++i)
        //        {
        //            frameWrapper.trigs[block].pnts.Add(new PointsListWrapper());
        //            frameWrapper.trigs[block].pnts[i].pnts = frame.pnts[block][i];
        //        }
        //        frameWrapper.trigs[block].scals = frame.scals[block];
        //    }
        //    frameWrappers.Add(frameWrapper);
        //}

        //framesDatabase.Frames = frameWrappers;

        //AssetDatabase.CreateAsset(framesDatabase, "Assets/Frames.asset");
        //AssetDatabase.SaveAssets();
    return particleBlockFrames;
  }

  private async Task<List<ParticleSystem.Particle[]>> GenerateFrameParticles(Frame frame)
  {
    var particleSet = new List<ParticleSystem.Particle[]>();
    foreach (var block in frame.blocks)
    {
      var pnts = frame.pnts[block].Select((pnt) => new Vector3(pnt[0] * 5.0f, pnt[1] * 5.0f, pnt[2] * 5.0f)).ToList();
      var scalars = frame.scals[block];
      var particles = new ParticleSystem.Particle[pnts.Count];
      for (var i = 0; i < pnts.Count; ++i)
      {
        particleTemplate.position = pnts[i];
        var val = Rescale(scalarMin, scalarMax, 0, 1, scalars[i]);
        var c = gradient.Evaluate(val);
        particleTemplate.startColor = c;
        particles[i] = particleTemplate;
      }
      particleSet.Add(particles);
      await Task.Delay(1);
    }
    return particleSet;
  }

  private float Rescale(float oldmin, float oldmax, float newmin, float newmax, float val)
  {
    var fromAbs = val - oldmin;
    var fromMaxAbs = oldmax - oldmin;
    var normal = fromAbs / fromMaxAbs;
    var toMaxAbs = newmax - newmin;
    var toAbs = toMaxAbs * normal;
    var to = toAbs + newmin;
    return to;
  }
}