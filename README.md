<img src="https://img.shields.io/badge/.NET%20Framework-4.x-%23989898"/> <img src="https://img.shields.io/badge/nuget-no%20ETA-red"/>
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

## Demo program
<img width="500" alt="Demo program" src="https://user-images.githubusercontent.com/5046671/197372065-bc01c4a5-7cd2-43aa-a632-3b71c8307ba3.png">

## How to use

1. Get the **BBox.Wpf** project from release page or development branch.
2. Feel free to modify based on your needs. 👌
3. Build the project. (Note: The source is targeting .NET Framework 4.7.2, but you should be able to build with any compatible framework version (>4.x).)
4. At your WPF project, add reference to the built DLL.
5. At your XAML, add namespace ```xmlns:bbox="clr-namespace:BBox.Wpf.Controls;assembly=BBox.Wpf"``` to use controls.
