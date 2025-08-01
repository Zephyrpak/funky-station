-- SPDX-FileCopyrightText: 2024 Ed <96445749+TheShuEd@users.noreply.github.com>
-- SPDX-FileCopyrightText: 2024 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
-- SPDX-FileCopyrightText: 2025 Tay <td12233a@gmail.com>
-- SPDX-FileCopyrightText: 2025 pa.pecherskij <pa.pecherskij@interfax.ru>
-- SPDX-FileCopyrightText: 2025 taydeo <td12233a@gmail.com>
--
-- SPDX-License-Identifier: MIT

-- Displacement Map Visualizer
--
-- This script will create a little preview window that will test a displacement map.
--
-- TODO: Handling of sizes != 127 doesn't work properly and rounds differently from the real shader. Ah well.

local scale = 4

-- This script requires UI
if not app.isUIAvailable then
    return
end

local getOffsetPixel = function(x, y, image, rect)
    local posX = x - rect.x
    local posY = y - rect.y

    if posX < 0 or posX >= image.width or posY < 0 or posY >= image.height then
        return image.spec.transparentColor
    end

    return image:getPixel(posX, posY)
end

local pixelValueToColor = function(sprite, value)
    return Color(value)
end

local applyDisplacementMap = function(width, height, size, displacement, displacementRect, target, targetRect)
    -- print(Color(displacement:getPixel(17, 15)).red)
    local image = target:clone()
    image:resize(width, height)
    image:clear()

    for x = 0, width - 1 do
        for y = 0, height - 1 do
            local value = getOffsetPixel(x, y, displacement, displacementRect)
            local color = pixelValueToColor(sprite, value)

            if color.alpha ~= 0 then
                local offset_x = (color.red - 128) / 127 * size
                local offset_y = (color.green - 128) / 127 * size

                local colorValue = getOffsetPixel(x + offset_x, y + offset_y, target, targetRect)
                image:drawPixel(x, y, colorValue)
            end
        end
    end

    return image
end

local dialog = nil

local sprite = app.editor.sprite
local spriteChanged = sprite.events:on("change",
    function(ev)
        dialog:repaint()
    end)

local layers = {}
for i,layer in ipairs(sprite.layers) do
    table.insert(layers, 1, layer.name)
end

local findLayer = function(sprite, name)
    for i, layer in ipairs(sprite.layers) do
        if layer.name == name then
            return layer
        end
    end

    return nil
end

local applyOffset = function(dx, dy)
    local cel = app.cel
    local image = cel.image:clone()
    local sprite = app.editor.sprite
    local selection = sprite.selection
    
    for x = selection.bounds.x, selection.bounds.x + selection.bounds.width - 1 do
        for y = selection.bounds.y, selection.bounds.y + selection.bounds.height - 1 do
            local xImg = x - cel.position.x
            local yImg = y - cel.position.y
            if xImg >= 0 and xImg < image.width and yImg >= 0 and yImg < image.height then
                local pixelValue = image:getPixel(xImg, yImg)
                local color = Color(pixelValue)

                -- Offset R and G channel
                color.red = math.min(255, math.max(0, color.red + dx))
                color.green = math.min(255, math.max(0, color.green + dy))

                image:drawPixel(xImg, yImg, app.pixelColor.rgba(color.red, color.green, color.blue, color.alpha))
            end
        end
    end
    
    cel.image = image
    dialog:repaint()
end

dialog = Dialog{
    title = "Displacement map preview",
    onclose = function(ev)
        sprite.events:off(spriteChanged)
    end}

dialog:canvas{
    id = "canvas",
    width = sprite.width * scale,
    height = sprite.height * scale,
    onpaint = function(ev)
        local context = ev.context

        local layerDisplacement = findLayer(sprite, dialog.data["displacement-select"])
        local layerTarget = findLayer(sprite, dialog.data["reference-select"])
        local layerBackground = findLayer(sprite, dialog.data["background-select"])
        -- print(layerDisplacement.name)
        -- print(layerTarget.name)

        local celDisplacement = layerDisplacement:cel(1)
        local celTarget = layerTarget:cel(1)
        local celBackground = layerBackground:cel(1)

        -- Draw background
        context:drawImage(
            -- srcImage
            celBackground.image,
            -- srcPos
            0, 0,
            -- srcSize
            celBackground.image.width, celBackground.image.height,
            -- dstPos
            celBackground.position.x * scale, celBackground.position.y * scale,
            -- dstSize
            celBackground.image.width * scale, celBackground.image.height * scale)

        -- Apply displacement map and draw
        local image = applyDisplacementMap(
            sprite.width, sprite.height,
            dialog.data["size"],
            celDisplacement.image, celDisplacement.bounds,
            celTarget.image, celTarget.bounds)

        context:drawImage(
            -- srcImage
            image,
            -- srcPos
            0, 0,
            -- srcSize
            image.width, image.height,
            -- dstPos
            0, 0,
            -- dstSize
            image.width * scale, image.height * scale)
    end
}

dialog:combobox{
    id = "displacement-select",
    label = "displacement layer",
    options = layers,
    onchange = function(ev)
        dialog:repaint()
    end
}

dialog:combobox{
    id = "reference-select",
    label = "reference layer",
    options = layers,
    onchange = function(ev)
        dialog:repaint()
    end
}

dialog:combobox{
    id = "background-select",
    label = "background layer",
    options = layers,
    onchange = function(ev)
        dialog:repaint()
    end
}

dialog:slider{
    id = "size",
    label = "displacement size",
    min = 127, --We dont support non 127 atm
    max = 127,
    value = 127,
    onchange = function(ev)
        dialog:repaint()
    end
}

dialog:button{
    id = "moveDown",
    text = "Down",
    onclick = function(ev)
        applyOffset(0, -1)
    end
}

dialog:button{
    id = "moveUp",
    text = "Up",
    onclick = function(ev)
        applyOffset(0, 1)
    end
}

dialog:button{
    id = "moveLeft",
    text = "Left",
    onclick = function(ev)
        applyOffset(1, 0)
    end
}

dialog:button{
    id = "moveRight",
    text = "Right",
    onclick = function(ev)
        applyOffset(-1, 0)
    end
}

dialog:show{wait = false}
