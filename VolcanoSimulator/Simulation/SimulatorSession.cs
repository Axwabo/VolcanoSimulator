﻿using VolcanoSimulator.Models;

namespace VolcanoSimulator.Simulation;

public sealed class SimulatorSession
{

    public List<LandmarkBase> Landmarks { get; } = [];

    public HashSet<EruptedMaterialBase> AllEruptedMaterials { get; } = [];

    private readonly Dictionary<object, ISimulator> _simulators = [];

    private readonly List<object> _objectsToClear = [];

    public void RefreshEruptedMaterial()
    {
        foreach (var volcano in Landmarks.OfType<Volcano>())
        {
            volcano.ClearDecayedMaterial();
            AllEruptedMaterials.UnionWith(volcano.EruptedMaterial);
        }

        AllEruptedMaterials.RemoveWhere(e => e.HasDecayed);
    }

    public void Step(TimeSpan time)
    {
        try
        {
            foreach (var (obj, simulatable) in _simulators)
            {
                simulatable.Step(time);
                if (!simulatable.Active)
                    _objectsToClear.Add(obj);
            }

            foreach (var o in _objectsToClear)
                _simulators.Remove(o);
            RefreshEruptedMaterial();
        }
        finally
        {
            _objectsToClear.Clear();
        }
    }

}
