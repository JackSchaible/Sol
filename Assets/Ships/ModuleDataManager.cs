using Assets.Data;
using Assets.Ships.Weapons;

namespace Assets.Ships
{
    public class ModuleDataManager
    {
        private IDataManager<ModuleStats[]> _dataManager;

        public ModuleDataManager()
        {
            _dataManager = new XmlDataManager<ModuleStats[]>();
        }

        public ModuleStats[] Get()
        {
            var result = _dataManager.Load();
            if (result == null)
            {
                result = Generate();
                _dataManager.Save(result);
            }

            return result;
        }

        private ModuleStats[] Generate()
        {
            return new ModuleStats[]
            {
                new WeaponStats("Light Machine Gun", "Projectile", "LMG - Build", 10, 0, 200, 15, 6000, new WeaponDamage(1, 0, 0), 700, 0, 200, 15000),
            };
        }
    }
}
