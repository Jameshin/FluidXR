using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[Serializable]
public class PointsListWrapper
{
    [SerializeField] public List<float> pnts;
}

[Serializable]
public class TrigsListWrapper
{
    [SerializeField] public List<int> trigs;
    [SerializeField] public List<int> types;
    [SerializeField] public List<PointsListWrapper> pnts;
    [SerializeField] public List<float> scals;
}

[Serializable]
public class FrameWrapper
{
    [SerializeField] public List<int> blocks;
    [SerializeField] public List<TrigsListWrapper> trigs;
}

[CreateAssetMenu(fileName = "FrameDatabase", menuName = "ScriptableObjects/FrameDatabase", order = 1)]
public class FrameDatabase : ScriptableObject
{
    [SerializeField] private List<FrameWrapper> frames;

    public List<FrameWrapper> Frames { get => frames; set => frames = value; }

    // 파일 범위에 따른 데이터 로드
    public void LoadDataFromJsonFiles(int startFile, int endFile)
    {
        for (int i = startFile; i <= endFile; i++)
        {
            string filename = $"{i}Gamma";
            Frame data = FrameFileLoader.Parse(filename);
            var convertedData = ConvertFrameToFrameWrapper(data);
            frames.Add(convertedData);
        }
    }

    private FrameWrapper ConvertFrameToFrameWrapper(Frame frameData)
    {
        FrameWrapper frameWrapper = new FrameWrapper();
        frameWrapper.blocks = frameData.blocks;


        frameWrapper.trigs = new List<TrigsListWrapper>();
        for (int i = 0; i < frameData.trigs.Count; i++)
        {
            TrigsListWrapper trigsWrapper = new TrigsListWrapper();
            trigsWrapper.trigs = frameData.trigs[i];
            trigsWrapper.types = frameData.types[i];

            trigsWrapper.pnts = new List<PointsListWrapper>();
            foreach (var pntList in frameData.pnts[i])
            {
                PointsListWrapper pointsListWrapper = new PointsListWrapper();
                pointsListWrapper.pnts = pntList;
                trigsWrapper.pnts.Add(pointsListWrapper);
            }

            trigsWrapper.scals = frameData.scals[i];
            frameWrapper.trigs.Add(trigsWrapper);
        }

        return frameWrapper;
    }
}
