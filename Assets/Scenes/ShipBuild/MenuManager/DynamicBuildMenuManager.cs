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
        #region Props & fields
        
        //Other GameObject managers
        public ShipBuildUIManager UIManager;

        //Prefabs needed to display menus
        public GameObject LayoutGroup;
        public GameObject MenuPrefab;
        public GameObject TogglePrefab;
        public GameObject DetailsPrefab;

        //The information to create the menus from, and the menu that was created
        private MenuData _menuDatas;
        private Menu _menu;

        //Constants
        private Color _fade = new Color(1, 1, 1, 0.3f);
        private Color _normal = new Color(1, 1, 1, 1);
        private List<GameObject> fadedObjs;

        #endregion

        #region Main methods

        void Start()
        {
            _menuDatas = new MenuDataManager().Get();
            _menu = GetMenu(LayoutGroup, _menuDatas, null);

            foreach (var toggle in _menu.MenuToggles)
                SetTreeActive(toggle.Menu, false);

            //TODO: Call this from here?
            Initialize();

            //TODO: for testing only
            var cc = _menuDatas.MenuDatas
                .First(x => x.Text == "Control Centres");
            var ss = cc.ChildMenu.MenuDatas.First(x => x.Text == "Small Ships");
            var cp = ss.ChildMenu.MenuDatas.First(x => x.Text == "Basic Cockpit");
            OnBuild((cp.ChildMenu as CommandModuleDetailsMenuData).Blueprint as CockpitModuleBlueprint);
        }

        //TODO: Where to call this from?
        public void Initialize()
        {
            //TODO: Where to get this from
            var isNew = true;

            if (isNew)
                NewShipInitialization();
            else
                InitializeShip();
        }

        void Update()
        {
            if (_menu == null) return;

            foreach (var toggle in _menu.MenuToggles)
            {

                if (!toggle.GameObject.GetComponent<Toggle>().isOn)
                    SetTreeActive(toggle.Menu, false);
                else
                    ToggleSubmenu(toggle);
            }
        }

        #endregion

        #region Helper methods

        private void InitializeShip()
        {

        }
        private void NewShipInitialization()
        {
            DisabledAllButCommand();
            //TODO: Other initialization -- grab user prefs, show/hide tut modals accordingly
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
            dv.BuildButton.onClick.AddListener(() => OnBuild(data.Blueprint));
            dv.ModuleImage.preserveAspect = true;
            dv.ModuleImage.sprite = GraphicsUtils.GetSpriteFromPath(td.Image, true);
            dv.Description.text = data.Blueprint.Description;
            //TODO: COST CHANGES
            //dv.Cost.text = data.Blueprint.Cost.ToString();
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
                    var obj = Instantiate(detailObj.DetailsPrefabQuarterWidth, detailObj.DetailsArea.transform);
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
        private void SetMenuToPlaceMode(Color c, Menu menu)
        {
            if (c == _fade)
                fadedObjs = GetMenuToPlaceMode(menu);

            foreach (var obj in fadedObjs)
            {
                var img = obj.GetComponent<Image>();
                if (img != null)
                    img.color = c;

                var text = obj.GetComponent<Text>();
                if (text != null)
                    text.color = c;

                var toggle = obj.GetComponent<Toggle>();
                if (toggle != null)
                    toggle.interactable = c == _normal;

                var scroll = obj.GetComponent<ScrollRect>();
                if (scroll != null)
                    scroll.scrollSensitivity = c == _fade ? 0 : 10;

                var button = obj.GetComponent<Button>();
                if (button != null)
                    button.interactable = c == _normal;
            }
        }
        private List<GameObject> GetChildrenMenuToPlaceMode(GameObject m)
        {
            var dv = m.GetComponent<DetailView>();

            var images = m.GetComponentsInChildren<Image>().ToList();
            var texts = m.GetComponentsInChildren<Text>().ToList();
            var scrolls = dv.GetComponentsInChildren<ScrollRect>().ToList();

            images.AddRange(dv.GetComponentsInChildren<Image>().ToList());
            texts.AddRange(dv.GetComponentsInChildren<Text>().ToList());

            var r = images.Select(x => x.gameObject).ToList();
            r.AddRange(texts.Select(x => x.gameObject));
            r.AddRange(scrolls.Select(x => x.gameObject));
            r.Add(dv.BuildButton.gameObject);
            return r;
        }
        private List<GameObject> GetMenuToPlaceMode(Menu m)
        {
            var gos = new List<GameObject>();

            if (m.MenuToggles == null)
            {
                gos.AddRange(GetChildrenMenuToPlaceMode(m.GameObject));
                return gos;
            }

            gos.Add(m.GameObject);

            foreach (var mt in m.MenuToggles)
            {
                gos.Add(mt.GameObject);
                gos.AddRange(GetMenuToPlaceMode(mt.Menu));
            }

            return gos;
        }

        #endregion

        #region Events

        private void OnBuild(ModuleBlueprint blueprint)
        {
            SetMenuToPlaceMode(_fade, _menu);
            UIManager.Build(blueprint);
        }

        public void ModulePlaced(ModuleBlueprint blueprint)
        {
            SetMenuToPlaceMode(_normal, _menu);
            //Process rules
            if (blueprint is CockpitModuleBlueprint)
                ProcessCockpitRules();
            else if (blueprint is CommandModuleBlueprint)
                ProcessCommandModuleRules();

            if (Input.GetKey(KeyCode.LeftShift))
                OnBuild(blueprint);
        }

        public void PlaceCancelled()
        {
            SetMenuToPlaceMode(_normal, _menu);
        }

        #endregion

        #region Rules

        private void ProcessCommandModuleRules()
        {
            EnableAllButCommand();

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
            EnableAllButCommand();

            var mt = _menu.MenuToggles.First(x => x.GameObject.name == "Control Centres");
            var cct = mt.GameObject.GetComponent<Toggle>();
            cct.interactable = false;
            cct.isOn = false;
        }
        private void EnableAllButCommand()
        {
            var gos = _menu.MenuToggles.Where(x => x.GameObject.name != "Control Centres").ToArray();
            for (var i = 0; i < gos.Count(); i++)
            {
                var t = gos[i].GameObject.GetComponent<Toggle>();
                t.interactable = true;
                t.isOn = i == 0;
            }
        }
        private void DisabledAllButCommand()
        {
            //3.n) First module on a new ship must be a command module
            var toggles = _menu.MenuToggles.Where(x => x.GameObject.name != "Control Centres");

            foreach (var go in toggles)
            {
                var t = go.GameObject.GetComponent<Toggle>();
                t.interactable = false;
                t.isOn = false;
            }

            var cct = _menu.MenuToggles.First(x => x.GameObject.name == "Control Centres");
            cct.GameObject.GetComponent<Toggle>().isOn = true;
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
