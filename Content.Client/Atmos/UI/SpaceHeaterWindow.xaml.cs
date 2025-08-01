// SPDX-FileCopyrightText: 2024 Menshin <Menshin@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 taydeo <td12233a@gmail.com>
//
// SPDX-License-Identifier: MIT

using Robust.Client.AutoGenerated;
using Robust.Client.UserInterface.Controls;
using Robust.Client.UserInterface.CustomControls;
using Robust.Client.UserInterface.XAML;
using Content.Shared.Atmos;
using Content.Shared.Atmos.Piping.Portable.Components;

namespace Content.Client.Atmos.UI;

/// <summary>
///     Client-side UI used to control a space heater.
/// </summary>
[GenerateTypedNameReferences]
public sealed partial class SpaceHeaterWindow : DefaultWindow
{
    // To account for a minimum delta temperature for atmos equalization to trigger we use a fixed step for target temperature increment/decrement
    public int TemperatureChangeDelta = 5;
    public bool Active;

    // Temperatures range bounds in Kelvin (K)
    public float MinTemp;
    public float MaxTemp;

    public RadioOptions<int> PowerLevelSelector;

    public SpaceHeaterWindow()
    {
        RobustXamlLoader.Load(this);

        // Add the Mode selector list
        foreach (var value in Enum.GetValues<SpaceHeaterMode>())
        {
            ModeSelector.AddItem(Loc.GetString($"comp-space-heater-mode-{value}"), (int)value);
        }

        // Add the Power level radio buttons
        PowerLevelSelectorHBox.AddChild(PowerLevelSelector = new RadioOptions<int>(RadioOptionsLayout.Horizontal));
        PowerLevelSelector.FirstButtonStyle = "OpenRight";
        PowerLevelSelector.LastButtonStyle = "OpenLeft";
        PowerLevelSelector.ButtonStyle = "OpenBoth";
        foreach (var value in Enum.GetValues<SpaceHeaterPowerLevel>())
        {
            PowerLevelSelector.AddItem(Loc.GetString($"comp-space-heater-ui-{value}-power-consumption"), (int)value);
        }

        // Only allow temperature increment/decrement of TemperatureChangeDelta
        Thermostat.Editable = false;
    }

    public void SetActive(bool active)
    {
        Active = active;
        ToggleStatusButton.Pressed = active;

        if (active)
        {
            ToggleStatusButton.Text = Loc.GetString("comp-space-heater-ui-status-enabled");
        }
        else
        {
            ToggleStatusButton.Text = Loc.GetString("comp-space-heater-ui-status-disabled");
        }
    }

    public void SetTemperature(float targetTemperature)
    {
        Thermostat.SetText($"{targetTemperature - Atmospherics.T0C} °C");

        IncreaseTempRange.Disabled = targetTemperature + TemperatureChangeDelta > MaxTemp;
        DecreaseTempRange.Disabled = targetTemperature - TemperatureChangeDelta < MinTemp;
    }
}

