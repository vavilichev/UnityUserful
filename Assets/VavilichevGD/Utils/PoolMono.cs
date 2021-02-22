using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace VavilichevGD.Utils {
    public class PoolMono<T> where T : MonoBehaviour {
        
        public bool autoExpand { get; set; }
        public T prefab { get; }
        public Transform container { get; }
        
        
        protected List<T> pool;

        
        public PoolMono(T prefab, int count) {
            this.prefab = prefab;
            this.container = null;
            this.CreatePool(this.prefab, count, this.container);
        }
        
        public PoolMono(T prefab, int count, Transform container) {
            this.prefab = prefab;
            this.container = container;
            this.CreatePool(this.prefab, count, this.container);
        }
        
        

        private void CreatePool(T prefab, int count, Transform container) {
            this.pool = new List<T>();

            for (int i = 0; i < count; i++) 
                this.CreateObject(prefab, container);
        }

        private T CreateObject(T prefab, Transform container, bool isActiveByDefault = false) {
            var createdObject = Object.Instantiate(prefab, container);
            createdObject.gameObject.SetActive(isActiveByDefault);
            this.pool.Add(createdObject);
            return createdObject;
        }

        
        
        
        public bool HasFreeElement(out T element) {
            foreach (var mono in pool) {
                if (!mono.gameObject.activeInHierarchy) {
                    mono.gameObject.SetActive(true);
                    element = mono;
                    return true;
                }
            }

            element = null;
            return false;
        }

        public T GetFreeElement() {
            if (this.HasFreeElement(out var element))
                return element;
            
            if (this.autoExpand)
                return this.CreateObject(this.prefab, this.container, true);

            throw new Exception($"The pool of type {typeof(T).Name} is empty. Current elements number is: {pool.Count}");
        }

        public T[] GetFreeElements(int count) {
            var freeElements = new List<T>();
            
            foreach (var mono in this.pool) {
                if (!mono.gameObject.activeInHierarchy) {
                    freeElements.Add(mono);
                    mono.gameObject.SetActive(true);
                }
            }

            if (freeElements.Count < count) {
                if (this.autoExpand) {
                    var difference = count - freeElements.Count;
                    for (int i = 0; i < difference; i++) {
                        var createdObject = this.CreateObject(this.prefab, this.container);
                        createdObject.gameObject.SetActive(true);
                        freeElements.Add(createdObject);
                    }

                    return freeElements.ToArray();
                }    
                
                throw new Exception($"Pool of type {typeof(T).Name} doesn't have so much free elements. Only {freeElements.Count}/{count}");
            }

            return freeElements.ToArray();
        }

        public T[] GetAllElements() {
            return this.pool.ToArray();
        }

        public T[] GetAllActiveElements() {
            var activeElements = new List<T>();
            foreach (var element in this.pool) {
                if (element.gameObject.activeInHierarchy)
                    activeElements.Add(element);
            }

            return activeElements.ToArray();
        }

        public int GetFreeElementsCount() {
            var sum = 0;
            foreach (var mono in this.pool) {
                if (!mono.gameObject.activeInHierarchy)
                    sum++;
            }

            return sum;
        }

    }
}