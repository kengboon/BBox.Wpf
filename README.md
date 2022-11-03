<a href="https://github.com/kengboon/BBox.Wpf/releases"><img src="https://img.shields.io/badge/release-v1.1.0.rc-blue"/></a> <a href="https://github.com/kengboon/BBox.Wpf/blob/master/LICENSE.md"><img src="https://img.shields.io/badge/usage-personal%20%7C%20commercial-brightgreen"/></a> <img src="https://img.shields.io/badge/support-not%20provided-orange"/> <img src="https://img.shields.io/badge/.NET%20Framework-4.x-lightgray"/> <img src="https://img.shields.io/badge/nuget-no%20ETA-red"/> 
# BBox.Wpf
Movable, resizable bounding box WPF control on image canvas.

## Use cases
- Object annotations
- Image editors
- ROI markers
- and more...

## Features
<img width="120" alt="Draggable & resizable control" src="https://user-images.githubusercontent.com/5046671/197372136-e2bac9f8-e230-4557-8124-500990dd1634.png">

- Moveable & resizable (Optional based on use case)
- Auto update size and position relative to image source (not misaligned on different window size)
- Draw bbox on canvas (by cursor)

## Demo program
<img width="500" alt="Demo program" src="https://user-images.githubusercontent.com/5046671/199659771-1c4dfcc7-5c80-4744-9709-bbc8f8313977.png">

## How to use

1. Get the **BBox.Wpf** project from release page or development branch.
2. Feel free to modify based on your needs - see [license](https://github.com/kengboon/BBox.Wpf/blob/master/LICENSE.md). 👌
3. Build the project. (Note: The source is targeting .NET Framework 4.7.2, but you should be able to build with any compatible framework version >=4.x)
4. At your WPF project, add reference to the built DLL.
5. At your XAML, add namespace ```xmlns:bbox="clr-namespace:BBox.Wpf.Controls;assembly=BBox.Wpf"``` to use controls.

## If this is useful
<a href="https://ko-fi.com/woolf42"><img src="https://user-images.githubusercontent.com/5046671/197377067-ce6016ae-6368-47b6-a4eb-903eb7b0af9c.png" width="200" alt=""/></a>
