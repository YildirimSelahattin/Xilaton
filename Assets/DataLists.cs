using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DataLists
{
    [System.Serializable]
    public class GeneralDataStructure
    {
        public string[] gridValueIndexes;
        public int[] starSpotIndexes;
        public string[] wordsCanBeFoundArray;
        public int gridHeight;
        public int gridWidth;

    }

    public GeneralDataStructure[] deckArray;


    /*
    {"table":[{"name":"broketable","price":0,"power":0,"isbuyed":0},{"name":"table1","price":1,"power":0,"isbuyed":0},{"name":"table2","price":2,"power":0,"isbuyed":0},{"name":"table3","price":3,"power":0,"isbuyed":0},{"name":"table4","price":4,"power":0,"isbuyed":0}],"chair":[{"name":"brokechair","price":4,"power":0,"isbuyed":0},{"name":"chair1","price":1,"power":0,"isbuyed":0},{"name":"chair2","price":2,"power":0,"isbuyed":0},{"name":"chair3","price":3,"power":0,"isbuyed":0},{"name":"chair4","price":4,"power":0,"isbuyed":0}],"sofa":[{"name":"brokesofa","price":4,"power":0,"isbuyed":0},{"name":"sofa1","price":1,"power":0,"isbuyed":0},{"name":"sofa2","price":2,"power":0,"isbuyed":0},{"name":"sofa3","price":3,"power":0,"isbuyed":0},{"name":"sofa4","price":4,"power":0,"isbuyed":0}],"mirror":[{"name":"brokemirror","price":4,"power":0,"isbuyed":0},{"name":"mirror1","price":1,"power":0,"isbuyed":0},{"name":"mirror2","price":2,"power":0,"isbuyed":0},{"name":"mirror3","price":3,"power":0,"isbuyed":0},{"name":"mirror4","price":4,"power":0,"isbuyed":0}],"window":[{"name":"brokewindow","price":0,"power":0,"isbuyed":0},{"name":"window1","price":1,"power":0,"isbuyed":0},{"name":"window2","price":2,"power":0,"isbuyed":0},{"name":"window3","price":3,"power":0,"isbuyed":0},{"name":"window4","price":4,"power":0,"isbuyed":0}],"flower":[{"name":"brokeflower","price":4,"power":0,"isbuyed":0},{"name":"flower1","price":1,"power":0,"isbuyed":0},{"name":"flower2","price":2,"power":0,"isbuyed":0},{"name":"flower3","price":3,"power":0,"isbuyed":0},{"name":"flower4","price":4,"power":0,"isbuyed":0}],"wall":[{"name":"brokewall","price":4,"power":0,"isbuyed":0},{"name":"wall1","price":1,"power":0,"isbuyed":0},{"name":"wall2","price":2,"power":0,"isbuyed":0},{"name":"wall3","price":3,"power":0,"isbuyed":0},{"name":"wall4","price":4,"power":0,"isbuyed":0}],"komodin":[{"name":"brokekomodin","price":4,"power":0,"isbuyed":0},{"name":"komodin1","price":1,"power":0,"isbuyed":0},{"name":"komodin2","price":2,"power":0,"isbuyed":0},{"name":"komodin3","price":3,"power":0,"isbuyed":0},{"name":"komodin4","price":4,"power":0,"isbuyed":0}],"platform":[{"name":"brokeplatform","price":4,"power":0,"isbuyed":0},{"name":"platform1","price":1,"power":0,"isbuyed":0},{"name":"platform2","price":2,"power":0,"isbuyed":0},{"name":"platform3","price":3,"power":0,"isbuyed":0},{"name":"platform4","price":4,"power":0,"isbuyed":0}],"picture":[{"name":"brokepicture","price":4,"power":0,"isbuyed":0},{"name":"picture1","price":1,"power":0,"isbuyed":0},{"name":"picture2","price":2,"power":0,"isbuyed":0},{"name":"picture3","price":3,"power":0,"isbuyed":0},{"name":"picture4","price":4,"power":0,"isbuyed":0}],"tabure":[{"name":"broketabure","price":4,"power":0,"isbuyed":0},{"name":"tabure1","price":1,"power":0,"isbuyed":0},{"name":"tabure2","price":2,"power":0,"isbuyed":0},{"name":"tabure3","price":3,"power":0,"isbuyed":0},{"name":"tabure4","price":4,"power":0,"isbuyed":0}],"floor":[{"name":"brokefloor","price":4,"power":0,"isbuyed":0},{"name":"floor1","price":1,"power":0,"isbuyed":0},{"name":"floor2","price":2,"power":0,"isbuyed":0},{"name":"floor3","price":3,"power":0,"isbuyed":0},{"name":"floor4","price":4,"power":0,"isbuyed":0}],"room":{"currentRoomIndexes":[0,0,0,0,0,0,0,0,0,0,0],"generalThemeIndex":1,"nextUpgradeIndex":0},"stackedChangeParentIndexes":[]}
    */
}
