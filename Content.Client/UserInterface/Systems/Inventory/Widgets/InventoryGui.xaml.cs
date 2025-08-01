// SPDX-FileCopyrightText: 2023 Nemanja <98561806+EmoGarbage404@users.noreply.github.com>
// SPDX-FileCopyrightText: 2025 taydeo <td12233a@gmail.com>
//
// SPDX-License-Identifier: MIT

using Robust.Client.AutoGenerated;
using Robust.Client.UserInterface.Controls;
using Robust.Client.UserInterface.XAML;

namespace Content.Client.UserInterface.Systems.Inventory.Widgets;

[GenerateTypedNameReferences]
public sealed partial class InventoryGui : UIWidget
{
    public InventoryGui()
    {
        RobustXamlLoader.Load(this);

        var inventoryUIController = UserInterfaceManager.GetUIController<InventoryUIController>();
        inventoryUIController.RegisterInventoryBarContainer(InventoryHotbar);

        LayoutContainer.SetGrowVertical(this, LayoutContainer.GrowDirection.Begin);
    }
}
