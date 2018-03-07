using System.Collections.Generic;
using UnityEngine;

namespace Assets.Ships
{
    public class Ship : MonoBehaviour
    {
        public GameObject ModulePrefab;

        public List<Module> Modules { get; set; }
        public double TotalWeight { get; private set; }
        public double TotalFwThrust { get; private set; }
        public double TotalReverseThrust { get; private set; }
        public double TotalCcwRotationalThrust { get; private set; }
        public double TotalCwRotationalThrust { get; private set; }
        public int TotalCost { get; private set; }
        public bool HasStrikeCraftLaunchers { get; private set; }

        public Ship()
        {
            
        }

        public Ship(List<Module> modules, double totalWeight, double totalFwThrust, double totalReverseThrust,
            double totalCcwRotationalThrust, double totalCwRotationalThrust, int totalCost,
            bool hasStrikeCraftLaunchers)
        {
            Modules = modules;
            TotalWeight = totalWeight;
            TotalFwThrust = totalFwThrust;
            TotalReverseThrust = totalReverseThrust;
            TotalCcwRotationalThrust = totalCcwRotationalThrust;
            TotalCwRotationalThrust = totalCwRotationalThrust;
            TotalCost = totalCost;
            HasStrikeCraftLaunchers = hasStrikeCraftLaunchers;
        }
    }
}
