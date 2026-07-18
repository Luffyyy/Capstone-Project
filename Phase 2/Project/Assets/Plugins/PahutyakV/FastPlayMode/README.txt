Fast Play Mode is a lightweight Unity Editor extension that gives you quick and intuitive control over "Enter Play Mode Options" directly inside the Scene View.
No more digging through Project Settings — toggle "Domain Reload" and "Scene Reload" with a single click, and even start Play Mode instantly from the overlay!

Features
- Overlay UI in SceneView with buttons:
  - Toggle "Domain Reload"
  - Toggle "Scene Reload"
- Icon-based UI for clarity
- Tooltip with full status information
- Context Menu integration (`Tools → PahutyakV → Fast Play Options`)
  - Reset to Default
  - Domain Reload toggle
  - Scene Reload toggle
  - Help/Documentation

How to Use
1. Install the package.
2. Open "Scene View" — the overlay "Fast Play Mode" will appear automatically.
3. Use the buttons:
   - Domain Reload toggle
   - Scene Reload toggle
4. Or use the *context menu at the top bar:  
   `Tools → PahutyakV → Fast Play Options`

Important Notes
- Disabling "Domain Reload" makes Play Mode enter much faster, but static variables will not reset automatically.
- Disabling "Scene Reload" means your current scene will not reload on Play.  
  This is great for iteration, but can cause unexpected states.  
- Always use "Reset to Default" before testing or releasing builds.

Compatibility
- Unity 2021.3 LTS and newer