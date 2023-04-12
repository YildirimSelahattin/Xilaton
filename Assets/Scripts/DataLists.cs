using System.Collections.Generic;

[System.Serializable]
public class DataLists
{
    [System.Serializable]
    public class GeneralDataStructure
    {
        public string themeName;
        public string[] gridValueIndexes;
        public List<int> starSpotIndexes;
        public string[] wordsCanBeFoundArray;
        public int startPoint;
        public int stopPoint;
        public int gridHeight;
        public int gridWidth;
    }

    public GeneralDataStructure[] deckArray;
}
