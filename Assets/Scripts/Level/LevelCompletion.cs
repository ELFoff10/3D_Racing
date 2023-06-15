using System;
using UnityEngine;

public class LevelCompletion : MonoSingleton<LevelCompletion>
{
    public const string filename = "completion.dat";

    [Serializable]
    public class LevelScore
    {
        public RaceInfo RaceInfo;
        public int Score;
    }

    [SerializeField] private LevelScore[] completionData;

    private new void Awake()
    {
        base.Awake();
        Saver<LevelScore[]>.TryLoad(filename, ref completionData);
    }

    public void RelpadSavingData()
    {
        foreach (var levelScore in completionData)
        {
            levelScore.Score = 0;
        }

        Saver<LevelScore[]>.TryLoad(filename, ref completionData);
    }

    public static void SaveLevelResult(int levelScore)
    {
        if (Instance)
        {
            foreach (var item in Instance.completionData)
            {

                if (levelScore > item.Score)
                {
                    item.Score = levelScore;
                    Saver<LevelScore[]>.Save(filename, Instance.completionData);
                }

            }
        }
    }

    public int GetEpisodeScore(RaceInfo raceInfo)
    {
        foreach (var data in completionData)
        {
            if (data.RaceInfo = raceInfo)
            {
                return data.Score;
            }
        }
        return 0;
    }

    public bool TryIndex(int id, out RaceInfo race, out int score)
    {
        if (id >= 0 && id < completionData.Length)
        {
            race = completionData[id].RaceInfo;
            score = completionData[id].Score;
            return true;
        }
        race = null;
        score = 0;
        return false;
    }
}
