using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using UnityEngine;
using UnityEngine.TestTools;
using static UnityEditor.Progress;

namespace Tests
{
    public class NewTestScript
    {
        public class ItemSO : ScriptableObject
        {
            public int id;
            public string itemname;
            public ItemType itemType;
            public string description;
            public List<ItemProperty> propertyList;
            public Sprite icon;
            public GameObject prefab;
        }
        public enum ItemType
        {
            Weapon,
            Consumable
        }

        [Serializable]
        public class ItemProperty
        {
            public ItemPropertyType propertytype;
            public int value;
        }

        public enum ItemPropertyType
        {
            HP,
            MP,
            Energy,
            Speed,
            Attack,
            Defence
        }
        public class BagUI : MonoBehaviour
        {
            public ItemSO itemSO;
            public static BagUI Instance { get; private set; }
            public GameObject uiGameObject;
            private GameObject content;
            public GameObject itemPrefab;
            private bool isShow = false;

            private void Awake()
            {
                if (Instance != null && Instance != this)
                {
                    Destroy(gameObject);
                }
                Instance = this;
            }

            void Start()
            {
                uiGameObject = transform.Find("UI").gameObject;
                content = transform.Find("UI/background/Scroll View/Viewport/Content").gameObject;
                Hide();
            }

            public void Update()
            {
                if (Input.GetKeyDown(KeyCode.B))
                {
                    if (isShow)
                    {
                        Hide();
                        isShow = false;
                    }
                    else
                    {
                        Show();
                        isShow = true;
                    }
                }
            }
            public void Show()
            {
                uiGameObject.SetActive(true);
            }

            public void Hide()
            {
                uiGameObject.SetActive(false);
            }

        }
        // A Test behaves as an ordinary method
        [Test]
        public void TestBagUIShowAndHide()
        {
            var gameObject = new GameObject();
            var bagUI = gameObject.AddComponent<BagUI>();

            var uiGameObject = new GameObject("UI");
            uiGameObject.transform.SetParent(gameObject.transform);
            bagUI.uiGameObject = uiGameObject;

            bagUI.Hide();
            Assert.IsFalse(bagUI.uiGameObject.activeSelf, "UI should be hidden initially.");

            bagUI.Show();
            Assert.IsTrue(bagUI.uiGameObject.activeSelf, "UI should be visible after calling Show().");

            bagUI.Hide();
            Assert.IsFalse(bagUI.uiGameObject.activeSelf, "UI should be hidden after calling Hide().");
        }

        // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
        // `yield return null;` to skip a frame.
        [UnityTest]
        public IEnumerator NewTestScriptWithEnumeratorPasses()
        {
            ShowDetails();

            yield return null;
        }
    }
}