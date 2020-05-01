# Room planner
This project is written in `C#` using `WinForms`. This is simple room planner with following features : <br>
1. Adding 5 different types of elements : coffee and kitchen tables, sofa, double bed and wall. Wall is drawn using lines.
2. Each element can be selected (opasity to 50%) and rotated, using mouse wheel.
3. When size of the blueprint is changed the bitmap, on which it is drawn, also resized (but only when it grows). When it shrinks autoscroll appears.
4. Elements are displayed in ListBox on the right with its center coordinates. For line it is its first point. Elements can be selected via this ListBox.
5. Blueprint can be saved and then restored.
6. Language can be changed (English `<->` Russian) using localization mechanism.