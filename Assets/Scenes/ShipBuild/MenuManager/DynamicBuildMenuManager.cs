using System.Collections.Generic;
using Assets.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scenes.ShipBuild.MenuManager
{
    public class DynamicBuildMenuManager : MonoBehaviour
    {
        public GameObject LayoutGroup;
        public GameObject MenuPrefab;
        public GameObject TogglePrefab;

        private MenuData _menuDatas;
        private Menu _menu;

        private GameObject test;

        private int _menuDepth;
        private Vector3 _lastMenuPos;

        void Start()
        {
            _menuDatas = new MenuDataManager().Get();

            _menu = GetMenu(LayoutGroup, _menuDatas);
        }

        void Update()
        {
        }

        private Transform GetContentAreaTransform(GameObject o)
        {
            var scrollView = o.transform.GetChild(0);
            if (scrollView == null) return null;

            var viewport = scrollView.transform.GetChild(0);
            if (viewport == null) return null;

            var content = viewport.transform.GetChild(0);
            if (content == null) return null;

            return content.gameObject.transform;
        }

        private Menu GetMenu(GameObject parent, MenuData md)
        {
            if (md is DetailsMenuData)
            {
                //Do some other shit

                return null;
            }

            Menu m = new Menu { GameObject = Instantiate(MenuPrefab, LayoutGroup.transform) };

            if (parent.name == "Canvas")
                m.GameObject.name = "Top Menu";
            else
                m.GameObject.name = parent.name + " Menu";

            var toggles = new List<MenuToggle>();
            var content = GetContentAreaTransform(m.GameObject);
            var group = content.gameObject.GetComponent<ToggleGroup>();

            foreach (var toggle in md.MenuDatas)
            {
                //TODO: might need to futz with y positioning?
                toggles.Add(ConfigureToggle(toggle, content, group));
            }

            m.MenuToggles = toggles;

            return m;
        }

        private MenuToggle ConfigureToggle(ToggleData toggle, Transform content, ToggleGroup group)
        {
            var mt = new MenuToggle
            {
                GameObject = Instantiate(TogglePrefab)
            };
            _menuDepth++;
            mt.GameObject.name = toggle.Text;
            mt.Menu = GetMenu(mt.GameObject, toggle.ChildMenu);
            mt.GameObject.transform.parent = content;
            mt.GameObject.GetComponent<Toggle>().group = group;
            mt.GameObject.GetComponentInChildren<Image>().sprite = GraphicsUtils.GetSpriteFromPath(toggle.Image);
            mt.GameObject.GetComponentInChildren<Text>().text = toggle.Text;

            return mt;
        }
    }
}
