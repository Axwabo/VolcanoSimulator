﻿using VolcanoSimulator.Models;
using VolcanoSimulator.Rendering.Gui;
using VolcanoSimulator.Simulation;

namespace VolcanoSimulator.Rendering;

public sealed class SimulatorRenderer
{

    private Coordinates _viewLocation = new(-Console.WindowHeight / 2, -Console.WindowWidth / 2);

    public SimulatorSession Session { get; }

    public SimulatorInput Input { get; }

    private readonly RendererTable _renderers = [];

    public ViewportRect Viewport => new(Console.WindowWidth, Console.WindowHeight, _viewLocation.Longitude, _viewLocation.Latitude);

    public GuiBase? CurrentGui { get; set; }

    public LandmarkBase? SelectedLandmark { get; set; }

    public SimulatorRenderer(SimulatorSession session)
    {
        Session = session;
        Input = new SimulatorInput(this);
    }

    public void RedrawAll()
    {
        Console.Clear();
        Draw();
    }

    private void Clear()
    {
        var viewport = Viewport;
        var center = viewport.Size / 2;
        Erase.SelectionIndicator(center);
        if (SelectedLandmark != null)
        {
            SelectedLandmark.ClearInfo();
            SelectedLandmark = null;
        }

        Session.Landmarks.ClearAll(_renderers, viewport);
    }

    private void Draw()
    {
        var viewport = Viewport;
        var center = viewport.Size / 2;
        if (CurrentGui == null && Session.Landmarks.DrawAllAndTryGetSelected(_renderers, viewport, center, out var landmark))
        {
            Render.SelectionIndicator(center);
            landmark.DrawInfo();
            SelectedLandmark = landmark;
        }
        else
        {
            Render.Cursor = center;
            Console.Write('+');
            CurrentGui?.Draw();
        }
    }

    public void Move(Coordinates delta)
    {
        Clear();
        _viewLocation += delta;
        Draw();
    }

    public void ShowGui(GuiBase? gui)
    {
        var current = CurrentGui;
        CurrentGui = gui;
        if (current != null)
        {
            RedrawAll();
            return;
        }

        SelectedLandmark?.ClearInfo();
        gui?.Draw();
    }

}
