using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.EventSystems;

namespace yookgaejang
{
    public class ControlManager : MonoBehaviour
    {
        public enum TowerName
        {
            NormalTower
        }
        public class MouseRect
        {
            public int left;
            public int top;
            public int right;
            public int bottom;

            public string MouseInfo()
            {
                return "left : " + left + ", top : " + top + ", right : " + right + ", bottom " + bottom;
            }
        }

        public class CameraPos
        {
            public float left = 5;
            public float right = 25;
            public float top = 20;
            public float boottom = 0;
        }
        public float camMoveSpeed = 1.0f;

        public MouseRect mouseRect = new MouseRect();
        public int moveInterval = 20;
        CameraPos cameraPos = new CameraPos();
        public float cameraSpeed = 1.0f;
        Camera mainCam;
        public MapManager mapManager;
        public int buildingSize = 0;
        public GameObject currentTower;
        public TowerName towerName;
        public int ErrorNum = -1;
        public GameObject normalTower;
        // Start is called before the first frame update
        void Start()
        {

            
            Cursor.lockState = CursorLockMode.Confined;
            mapManager = this.GetComponent<MapManager>();
            mainCam = GameObject.Find("Main Camera").GetComponent<Camera>();

            //마우스가 이동할 수 있는 범위 지정
            mouseRect.left = moveInterval;
            mouseRect.bottom = moveInterval;
            mouseRect.right = Screen.width - 1 - moveInterval;
            mouseRect.top = Screen.height - 1 - moveInterval;
        }

        // Update is called once per frame
        void Update()
        {
            MouseControl();
            CreateDefenceTower();
        }

        public void MouseControl()
        {
            //마우스 가두기
            if (Input.GetKeyDown(KeyCode.L))
            {
                Cursor.lockState = CursorLockMode.Confined ;
            }

            //마우스 가두기 풀기
            if (Input.GetKeyDown(KeyCode.K))
            {
                Cursor.lockState = CursorLockMode.None;
            }

            Vector2  mousePos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            //마우스가 화면 왼쪽으로 이동
            if(mouseRect.left > mousePos.x)
            {
                if(cameraPos.left < mainCam.transform.position.x)
                {
                    mainCam.transform.position -= new Vector3(1.0f, 0f,0) * camMoveSpeed * Time.deltaTime;
                }
            }

            if(mouseRect.right < mousePos.x)
            {
                if(cameraPos.right > mainCam.transform.position.x)
                {
                    mainCam.transform.position += new  Vector3(1.0f, 0, 0) * camMoveSpeed *Time.deltaTime;
                }
            }

            if (mouseRect.top < mousePos.y)
            {
                if (cameraPos.right > mainCam.transform.position.y)
                {
                    mainCam.transform.position += new Vector3(0f, 0, 1.0f) * camMoveSpeed *Time.deltaTime;
                }
            }

            if (mouseRect.bottom > mousePos.y)
            {
                if (cameraPos.right > mainCam.transform.position.y)
                {
                    mainCam.transform.position -= new Vector3(0f, 0, 1.0f) * camMoveSpeed * Time.deltaTime;
                }
            }
        }
        public void CreateDefenceTower()
        {
            int posX = -1;
            int posZ = 01;
            if(currentTower != null )
            {
                if(buildingSize > 0)
                {
                    Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
                    if(Physics.Raycast(ray, out RaycastHit raycastHit))
                    {
                        posX = Mathf.FloorToInt(raycastHit.point.x);//마우스 타일식 처리
                        posZ = Mathf.FloorToInt(raycastHit.point.z);
                        bool isBuilding = mapManager.isRoad(posX, posZ, buildingSize);
                        if(isBuilding)
                        {
                            currentTower.SetActive(true);
                            currentTower.transform.position = new Vector3(posX, 0.2f, posZ);
                            ErrorNum = -1;
                        }
                        else
                        {
                            currentTower.SetActive(false);
                            ErrorNum = 0;
                        }
                    }
                    else
                    {
                        ErrorNum = 0;
                    }
                }
            }

            if (!EventSystem.current.IsPointerOverGameObject())
            {
                if(Input.GetMouseButton(0))
                {
                    if(currentTower != null)
                    {
                        if(ErrorNum != -1)
                        {

                        }
                        else
                        {
                            mapManager.ChangeBuild((int)currentTower.transform.position.x, (int)currentTower.transform.position.z, buildingSize, BlockName.Build);
                            currentTower = null;
                        }
                    }
                }
            }
        }

        public void CreateTower()
        {
            currentTower = Instantiate(normalTower);
        }

        public void BtnBuild()
        {
            CreateTower();
            buildingSize = 1;
        }
    }
}
