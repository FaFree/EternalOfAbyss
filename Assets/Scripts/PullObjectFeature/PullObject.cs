using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Scripts.PullObjectFeature
{
    public class PullObject
    {
        private bool isAutoExpand;
        private Transform container;

        private GameObject prefab;
        private List<GameObject> pool;

        public PullObject(GameObject prefab, int count, bool isAutoExpand = true)
        {
            this.prefab = prefab;
            this.isAutoExpand = isAutoExpand;

            pool = new List<GameObject>();
            
            this.CreatePool(count);
        }

        public PullObject(GameObject prefab, Transform container, int count, bool isAutoExpand = true)
        {
            this.prefab = prefab;
            this.container = container;
            this.isAutoExpand = isAutoExpand;

            pool = new List<GameObject>();
            
            this.CreatePool(count);
        }

        private void CreatePool(int count)
        {
            for (var i = 0; i < count; i++)
                this.CreateObject();
        }

        private GameObject CreateObject(bool isActiveByDefault = false)
        {
            var createdObject = Object.Instantiate(this.prefab, this.container);
            createdObject.gameObject.SetActive(isActiveByDefault);
            this.pool.Add(createdObject);
            return createdObject;
        }

        public bool HasFreeElement(out GameObject element)
        {
            foreach (var obj in this.pool)
            {
                if (!obj.activeInHierarchy)
                {
                    element = obj;
                    obj.SetActive(true);
                    return true;
                }
            }

            element = null;
            return false;
        }

        public GameObject GetFreeElement()
        {
            if (this.HasFreeElement(out var element))
                return element;

            if (this.isAutoExpand)
                return this.CreateObject(true);

            throw new Exception("There is no free elements in pool");
        }
        
    }
}