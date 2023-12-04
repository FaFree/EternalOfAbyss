using System.Collections.Generic;
using ECS.Scripts.Events;
using Scellecs.Morpeh;
using Scripts;
using Scripts.BoostFeature;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace BuilderFeature
{
    public class BuildWindowStart : MonoBehaviour
    {
        private const string ICON_KEY = "Assets/Addressables/ui/BuildItem.prefab";
        
        [SerializeField] private RectTransform iconRoot;

        private List<BuildItemView> buildItemsViews;

        private GameObject buildObj;

        private BuildItemView currentBuildItemView;

        private GameObject currentGhostObject;

        private Event<BuildEndedEvent> buildEndedEvent;

        private List<Vector3> buildingPositions;

        public bool isDragging;

        private void Start()
        {
            this.buildEndedEvent = World.Default.GetEvent<BuildEndedEvent>();

            this.buildingPositions = new List<Vector3>();
            
            this.buildItemsViews = new List<BuildItemView>();
            
            var boosts = WorldModels.Default.Get<BoostsModel>().boosts;

            var prefab = Addressables.LoadAssetAsync<GameObject>(ICON_KEY).WaitForCompletion();

            foreach (var boost in boosts)
            {
                if (boost.isBuilding)
                {
                    var buildItemView = this.FirstOrDefaultItemView(boost.category);

                    if (buildItemView != default)
                    {
                        buildItemView.Add(boost);
                        buildItemView.onClick += OnClick;
                    }
                    else
                    {
                        var obj = Instantiate(prefab, iconRoot);

                        buildItemView = obj.GetComponent<BuildItemView>();
                        
                        buildItemView.Add(boost);
                        
                        this.buildItemsViews.Add(buildItemView);
                    }
                }
            }
            
            this.EndCheck();
        }

        private void OnClick(BuildItemView buildItemView)
        {
            if (this.isDragging)
                return;
            
            buildItemView.TakeItemCount();
            
            this.isDragging = true;
            this.currentBuildItemView = buildItemView;
            
            var prefab = Addressables.LoadAssetAsync<GameObject>(buildItemView.ghostObjectKey).WaitForCompletion();

            this.currentGhostObject = Instantiate(prefab);
        }

        private BuildItemView FirstOrDefaultItemView(Categories category)
        {
            foreach (var itemView in this.buildItemsViews)
            {
                if (itemView.category == category)
                    return itemView;
            }

            return default;
        }

        private void Update()
        {
            if (!this.isDragging)
                return;
            
            Vector3 cursorScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 35f);
            Vector3 cursorPosition = WorldModels.Default.Get<Camera>().ScreenToWorldPoint(cursorScreenPoint);

            this.currentGhostObject.transform.position = cursorPosition;
            
            if (Input.GetMouseButtonUp(0))
                this.MouseUp();
        }

        private void MouseUp()
        {
            if (!this.isDragging)
                return;
            
            this.isDragging = false;
            Destroy(this.currentGhostObject);

            Ray ray = WorldModels.Default.Get<Camera>().ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
                
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject.CompareTag(this.currentBuildItemView.buildingTag) && this.CheckPosition(hit.point))
                {
                    var prefab = Addressables.LoadAssetAsync<GameObject>(this.currentBuildItemView.objectKey).WaitForCompletion();
                        
                    var go = Instantiate(prefab, hit.point, Quaternion.identity);
                    
                    this.buildingPositions.Add(go.transform.position);

                    if (this.currentBuildItemView.itemsCount == 0)
                    {
                        this.currentBuildItemView.gameObject.SetActive(false);
                        
                        this.buildItemsViews.Remove(this.currentBuildItemView);
                        
                        this.EndCheck();
                    }
                }
                else
                {
                    this.currentBuildItemView.AddItemCount();
                }
            }
            else
            {
                this.currentBuildItemView.AddItemCount();
            }
        }

        private bool CheckPosition(Vector3 position)
        {
            foreach (var buildingPosition in this.buildingPositions)
            {
                if (Vector3.SqrMagnitude(buildingPosition - position) < 1.5f)
                {
                    return false;
                }
            }

            return true;
        }

        private void EndCheck()
        {
            if (this.buildItemsViews.Count == 0)
            {
                this.gameObject.SetActive(false);
                
                Time.timeScale = 1f;
                
                this.buildEndedEvent.NextFrame(new BuildEndedEvent());
            }
        }
    }
}