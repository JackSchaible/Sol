using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scenes.ShipBuild.UI;
using Assets.Ships;
using Assets.Utils;
using Assets.Utils.Extensions;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scenes.ShipBuild.MenuManager
{
    public class DynamicBuildMenuManager : MonoBehaviour
    {
        public ShipBuildUIManager UIManager;

        public GameObject LayoutGroup;
        public GameObject MenuPrefab;
        public GameObject TogglePrefab;
        public GameObject DetailsPrefab;

        private MenuData _menuDatas;
        private Menu _menu;

        void Start()
        {
            _menuDatas = new MenuDataManager().Get();
            _menu = GetMenu(LayoutGroup, _menuDatas, null);

            foreach (var toggle in _menu.MenuToggles)
                SetTreeActive(toggle.Menu, false);

        }

        void Update()
        {
            if (_menu == null) return;

            foreach (var toggle in _menu.MenuToggles)
                ToggleSubmenu(toggle);
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
        private Menu GetMenu(GameObject parent, MenuData md, ToggleData td)
        {
            Menu m;
            var data = md as DetailsMenuData;

            if (data == null)
                m = ConfigureMenu(parent, md);
            else
                m = ConfigureDetailView(td, data);

            return m;
        }
        private MenuToggle ConfigureToggle(ToggleData toggle, Transform content, ToggleGroup group)
        {
            var mt = new MenuToggle
            {
                GameObject = Instantiate(TogglePrefab, content)
            };

            mt.GameObject.name = toggle.Text;
            mt.Menu = GetMenu(mt.GameObject, toggle.ChildMenu, toggle);
            mt.GameObject.GetComponent<Toggle>().group = group;
            mt.GameObject.GetComponentInChildren<Image>().sprite = GraphicsUtils.GetSpriteFromPath(toggle.Image, true);
            mt.GameObject.GetComponentInChildren<Text>().text = toggle.Text;

            return mt;
        }
        private Menu ConfigureMenu(GameObject parent, MenuData md)
        {
            Menu m = new Menu { GameObject = Instantiate(MenuPrefab, LayoutGroup.transform) };

            if (parent.name == "Menus View")
                m.GameObject.name = "Top Menu";
            else
                m.GameObject.name = parent.name + " Menu";

            var content = GetContentAreaTransform(m.GameObject);
            var group = content.gameObject.GetComponent<ToggleGroup>();
            var toggles = md.MenuDatas.Select(toggle => ConfigureToggle(toggle, content, group)).ToList();

            m.MenuToggles = toggles;

            return m;
        }
        private Menu ConfigureDetailView(ToggleData td, DetailsMenuData data)
        {
            Menu dm = new Menu { GameObject = Instantiate(DetailsPrefab, LayoutGroup.transform) };

            var dv = dm.GameObject.GetComponent<DetailView>();
            dv.Name.text = td.Text;
            dv.BuildButton.onClick.AddListener(() => UIManager.Build(data.Blueprint));
            dv.ModuleImage.preserveAspect = true;
            dv.ModuleImage.sprite = GraphicsUtils.GetSpriteFromPath(td.Image, true);
            dv.Description.text = data.Blueprint.Description;
            dv.Cost.text = data.Blueprint.Cost.ToString();
            dv.Health.text = data.Blueprint.Health.ToString();
            dv.Weight.text = data.Blueprint.Weight.ToSiUnit("g");
            dv.Power.text = data.Blueprint.PowerConumption.ToSiUnit("W");
            dv.Crew.text = data.Blueprint.CrewRequirement.ToString();
            dv.Command.text = data.Blueprint.CommandRequirement.ToString();

            if (data is CommandModuleDetailsMenuData)
            {
                dv.CommandIcon.preserveAspect = true;
                dv.CommandIcon.sprite = GraphicsUtils.GetSpriteFromPath("Ships/Control Centres Icon");
                dv.Command.text = (data.Blueprint as CommandModuleBlueprint).CommandSupplied.ToString();

                if (data.Blueprint is CockpitModuleBlueprint)
                    dv.Crew.text = (data.Blueprint as CockpitModuleBlueprint).PersonnelHoused.ToString();
            }

            var height = dv.Description.preferredHeight;
            dv.Description.gameObject.transform.parent.gameObject.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);

            foreach (var d in data.DetailsFields)
            {
                var detailObj = dm.GameObject.GetComponent<DetailView>();
                var qw = d as DetailsFieldQw;

                if (qw == null)
                {
                    var obj = Instantiate(detailObj.DetailsPrefabHalfWidth, detailObj.DetailsArea.transform);
                    var hfInfo = obj.GetComponent<DetailViewHw>();

                    hfInfo.Icon1.preserveAspect = true;
                    hfInfo.Icon1.sprite = GraphicsUtils.GetSpriteFromPath(d.Icon1);
                    hfInfo.Text1.name = d.Name1;
                    hfInfo.Text1.text = d.Value1;
                    hfInfo.Icon2.preserveAspect = true;
                    hfInfo.Icon2.sprite = GraphicsUtils.GetSpriteFromPath(d.Icon2);
                    hfInfo.Text2.name = d.Name2;
                    hfInfo.Text2.text = d.Value2;
                }
                else
                {
                    var obj = Instantiate(detailObj.DetailsPrefabHalfWidth, detailObj.DetailsArea.transform);
                    var qwInfo = obj.GetComponent<DetailViewQw>();

                    qwInfo.Icon1.preserveAspect = true;
                    qwInfo.Icon1.sprite = GraphicsUtils.GetSpriteFromPath(qw.Icon1);
                    qwInfo.Text1.name = qw.Name1;
                    qwInfo.Text1.text = qw.Value1;
                    qwInfo.Icon2.preserveAspect = true;
                    qwInfo.Icon2.sprite = GraphicsUtils.GetSpriteFromPath(qw.Icon2);
                    qwInfo.Text2.name = qw.Name2;
                    qwInfo.Text2.text = qw.Value2;
                    qwInfo.Icon3.preserveAspect = true;
                    qwInfo.Icon3.sprite = GraphicsUtils.GetSpriteFromPath(qw.Icon3);
                    qwInfo.Text3.name = qw.Name3;
                    qwInfo.Text3.text = qw.Value3;
                    qwInfo.Icon4.preserveAspect = true;
                    qwInfo.Icon4.sprite = GraphicsUtils.GetSpriteFromPath(qw.Icon4);
                    qwInfo.Text4.name = qw.Name4;
                    qwInfo.Text4.text = qw.Value4;
                }
            }

            return dm;
        }
        private void SetTreeActive(Menu m, bool active)
        {
            if (m == null)
                return;

            if (m.MenuToggles != null)
                foreach (var toggle in m.MenuToggles)
                {
                    SetTreeActive(toggle.Menu, active);
                    toggle.GameObject.SetActive(active);
                }

            m.GameObject.SetActive(active);
        }
        private void ToggleSubmenu(MenuToggle t)
        {
            var active = t.GameObject.GetComponent<Toggle>().isOn;
            t.Menu.GameObject.SetActive(active);

            if (t.Menu.MenuToggles == null) return;

            foreach (var subtoggle in t.Menu.MenuToggles)
            {
                subtoggle.GameObject.SetActive(active);
                ToggleSubmenu(subtoggle);
            }
        }

        #region Rules

        public void ModulePlaced(ModuleBlueprint blueprint)
        {
            //Process rules
            if (blueprint is CockpitModuleBlueprint)
                ProcessCockpitRules();
            else if (blueprint is CommandModuleBlueprint)
                ProcessCommandModuleRules();
        }

        private void ProcessCommandModuleRules()
        {
            //3.a.ii.2) If a command module other than a cockpit module is placed, no cockpit modules may be placed.
            var pt = _menu.MenuToggles.First(x => x.GameObject.name == "Control Centres");
            var mt = pt.Menu.MenuToggles.First(x => x.GameObject.name == "Small Ships");
            var t = mt.GameObject.GetComponent<Toggle>();
            t.isOn = false;
            t.enabled = false;
        }
        private void ProcessCockpitRules()
        {
            //3.a.ii.1) If a cockpit module is placed, no other command modules may be placed.
            var mt = _menu.MenuToggles.First(x => x.GameObject.name == "Control Centres");
            var t = mt.GameObject.GetComponent<Toggle>();
            t.isOn = false;
            t.enabled = false;
        }

        #endregion

        #region Modals

        //TODO: Show these
        public void ShowCommandModulePopup(Toggle t)
        {
            if (t.isOn)
            {
                //Modal.Initialize(Modals.BuildMenu.CommandModulesModalData);
                //Modal.ShowModal();
            }
        }

        public void ShowCockpitModulePopup(Toggle t)
        {
            if (t.isOn)
            {
                //Modal.Initialize(Modals.BuildMenu.CockpitModalData);
                //Modal.ShowModal();
            }
        }

        #endregion
    }
}
