using UnityEngine;

namespace Assets.Scenes.ShipBuild.UI.Build_Menu
{
    public class Submenu : MonoBehaviour
    {
        public BuildMenuToggle[] Children;
        public GameObject ParentObj;
        public BuildMenuToggle Parent;

        void Start()
        {
            Parent = ParentObj.GetComponent<BuildMenuToggle>();
            Children = GetComponentsInChildren<BuildMenuToggle>();

            foreach (var child in Children)
                child.Parent = gameObject;
        }

        void Update()
        {
            if (Parent == null) return;

            if (!Parent.IsOn)
                foreach (var child in Children)
                    child.IsOn = Parent.IsOn;

            gameObject.SetActive(Parent.IsOn);
        }
    }
}
