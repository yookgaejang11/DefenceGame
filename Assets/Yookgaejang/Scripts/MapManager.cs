using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace yookgaejang
{
    [Serializable]
    public enum BlockName
    {
        Wall = 0, //0, 0, 0
        NotWalkable = 1, // 255, 255, 255
        Walkable = 2, // 153, 217, 234
        Response = 3, // 237, 28, 36
        BuildingLand = 4, // 181, 230 29
        DefenseBuilding = 5,// 255, 242, 0

        Build = 6,
        Monster = 7,
    }

    public class MapData
    {
        public BlockName blockName;
        public int x;
        public int z;

        public MapData(int x, int z)
        {
            this.x = x;
            this.z = z;
        }

        public Vector2 ToVector()
        {
            return new Vector2(x, z);
        }

        public static MapData operator +(MapData a, MapData b)//MapData를 더한다는 개념이 없는 것을 어떻게 더한다는것을 정하여 더할 수 있게 하는 것
            => new MapData(a.x + b.x, a.z + b.z); //람다식 : 내가 표현하고 있는것을 짧게 줄일려고 하는것
        
        public override bool Equals(object obj) //원레 Equals가 하는것을 덮어씌운것
        {
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else
            {
                return x == ((MapData)obj).x && z == ((MapData)obj).z;
            }
        }

        public override int GetHashCode()
        {
            return 0;
        }
    }
    public class MapManager : MonoBehaviour
    {
        public Texture2D MapInfo;
        public Color[] colorBlock;

        public int mapWidth;
        public int mapHeight;

        public GameObject[] Block;
        public int blockScale = 1;
        public Transform Map;
        public List<MapData> mapdata = new List<MapData>();
        public List<MapData> directions = new List<MapData>()//4방위
        {
            new MapData(1, 0),
            new MapData(0,1),
            new MapData(-1,0),
            new MapData(0,-1),
        };

        // Start is called before the first frame update
        void Start()
        {
            Map = GameObject.Find("MAP").transform;
            GenerateMap();   
            
        }

        // Update is called once per frame
        void Update()
        {
        
        }
        
        public bool isRoad(int x, int z, int size)
        {
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    int posX = x + i;
                    int posZ = z + j;
                    if(posX >= mapWidth || posX <= 0 || posZ >= mapHeight || posZ <= 0)
                    {
                        return false;
                    }
                    MapData temp = GetMapData(posX, posZ);
                    if(temp.blockName != BlockName.BuildingLand)
                    {
                        return false;
                    }

                }
            }
            return true;
        }

        public MapData GetMapData(int x, int z)
        {
            return mapdata.Find(data => data.x == x && data.z == z);
        }

        public void ChangeBuild(int x, int z, int size, BlockName blockName)
        {
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    int posX = x + i;
                    int posZ = z + j;
                    GetMapData(posX, posZ).blockName = blockName;
                }
            }
        }


        public void GenerateMap()
        {
            mapWidth = MapInfo.width;
            mapHeight = MapInfo.height;
            Debug.Log("mapWidth : " + mapWidth + ", mapHeight : " + mapHeight);
            Color[] pixels = MapInfo.GetPixels();
            for(int i = 0; i < mapHeight; i++)
            {
                for(int j = 0; j < mapWidth; j++)
                {
                    Color pixelColor = pixels[i * mapWidth + j];
                    MapData data = new MapData(j,i);
                    if(pixelColor == colorBlock[(int)BlockName.Wall])
                    {
                        Instantiate(Block[(int)BlockName.Wall], new Vector3(blockScale * j, 0, blockScale * i), Quaternion.identity, Map);
                        data.blockName = BlockName.Wall;
                    }
                    else if(pixelColor == colorBlock[(int)BlockName.Walkable])
                    {
                        Instantiate(Block[(int)BlockName.Walkable], new Vector3(blockScale * j, 0, blockScale * i), Quaternion.identity, Map);
                        data.blockName = BlockName.Walkable;
                    }
                    else if (pixelColor == colorBlock[(int)BlockName.NotWalkable])
                    {
                        Instantiate(Block[(int)BlockName.NotWalkable], new Vector3(blockScale * j, 0, blockScale * i), Quaternion.identity, Map);
                        data.blockName = BlockName.NotWalkable;
                    }
                    else if (pixelColor == colorBlock[(int)BlockName.Response])
                    {
                        Instantiate(Block[(int)BlockName.Response], new Vector3(blockScale * j, 0, blockScale * i), Quaternion.identity, Map);
                        data.blockName = BlockName.Response;
                    }
                    else if (pixelColor == colorBlock[(int)BlockName.BuildingLand])
                    {
                        Instantiate(Block[(int)BlockName.BuildingLand], new Vector3(blockScale * j, 0, blockScale * i), Quaternion.identity, Map);
                        data.blockName = BlockName.BuildingLand;
                    }
                    else if (pixelColor == colorBlock[(int)BlockName.DefenseBuilding])
                    {
                        Instantiate(Block[(int)BlockName.DefenseBuilding], new Vector3(blockScale * j, 0, blockScale * i), Quaternion.identity, Map);
                        data.blockName = BlockName.DefenseBuilding;
                    }
                    mapdata.Add(data);
                }
            }
        }
    }
}
