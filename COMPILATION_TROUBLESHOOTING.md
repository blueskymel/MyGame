# Unity Compilation Error Troubleshooting Guide

## 🚨 **Quick Fixes for Compilation Errors**

### **Step 1: Run the Automated Fix**
1. **Double-click** `fix_compilation.bat`
2. **Wait** for it to complete
3. **Open Unity** and try loading the project

### **Step 2: Unity Menu Fixes**
Once Unity opens:
1. Go to **Donkey Kong** → **Fix Compilation Errors**
2. Go to **Donkey Kong** → **Setup Complete Project**
3. Go to **Donkey Kong** → **Validate Project Structure**

### **Step 3: Manual Unity Fixes**

#### **A. Refresh Assets**
- Press **Ctrl+R** or go to **Assets** → **Refresh**

#### **B. Reimport Scripts**
- Right-click on **Assets/Scripts** folder
- Select **Reimport**

#### **C. Clear Script Cache**
- Close Unity
- Delete **Library** and **Temp** folders
- Reopen Unity

#### **D. Check Package Manager**
- Go to **Window** → **Package Manager**
- Make sure these packages are installed:
  - **2D Sprite** (1.0.0)
  - **Visual Studio Editor** (2.0.18)
  - **TextMeshPro** (3.0.6)
  - **UI** (1.0.0)

### **Step 4: Common Error Solutions**

#### **❌ "The type or namespace name could not be found"**
- **Solution**: Package Manager → Install missing packages
- **Alternative**: Edit → Project Settings → Player → Configuration → Assembly Definition References

#### **❌ "Assembly 'Assembly-CSharp' has no reference to UnityEngine.UI"**
- **Solution**: Window → Package Manager → Install **UI Toolkit** package

#### **❌ "Script 'GameManager' has the same name as built-in Unity component"**
- **Solution**: Scripts are correctly named - ignore this warning

#### **❌ "Missing script references"**
- **Solution**: Run **Donkey Kong** → **Setup Complete Project**

#### **❌ "Scene 'MainScene' could not be opened"**
- **Solution**: 
  1. Go to **Assets/Scenes/**
  2. Double-click **MainScene.unity**
  3. If missing, run **Setup Complete Project**

### **Step 5: Visual Studio 2022 Integration**

#### **If VS2022 isn't recognized:**
1. **Edit** → **Preferences** → **External Tools**
2. **External Script Editor**: Browse to VS2022:
   - Community: `C:\Program Files\Microsoft Visual Studio\2022\Community\Common7\IDE\devenv.exe`
   - Professional: `C:\Program Files\Microsoft Visual Studio\2022\Professional\Common7\IDE\devenv.exe`
   - Enterprise: `C:\Program Files\Microsoft Visual Studio\2022\Enterprise\Common7\IDE\devenv.exe`

### **Step 6: Advanced Troubleshooting**

#### **Complete Reset (if nothing else works):**
1. **Close Unity**
2. **Delete**: Library, Temp, obj, .vs folders
3. **Keep**: Assets, ProjectSettings, Packages folders
4. **Reopen** in Unity Hub
5. **Wait** for Unity to regenerate everything

#### **Script Execution Order Issues:**
1. **Edit** → **Project Settings** → **Script Execution Order**
2. **Set order**:
   - GameManager: -100
   - AudioManager: -90
   - UIManager: -80
   - LevelBuilder: -70

#### **Platform-Specific Issues:**
- **Android**: Player Settings → Android → Target API Level (33+)
- **iOS**: Player Settings → iOS → Target minimum iOS Version (12.0+)

## 📞 **Still Having Issues?**

### **Check Unity Console:**
1. **Window** → **General** → **Console**
2. **Look for** specific error messages
3. **Double-click** errors to go to problematic code

### **Common Error Types:**
- **Red errors**: Critical compilation issues
- **Yellow warnings**: Non-critical issues (can ignore)
- **Blue messages**: Informational (can ignore)

### **Project Validation:**
Run this command in Unity Console:
```csharp
UnityEditor.EditorApplication.ExecuteMenuItem("Donkey Kong/Validate Project Structure");
```

## ✅ **Success Indicators:**

- **No red errors** in Console
- **Play button** is clickable (not grayed out)
- **Scripts compile** without errors
- **MainScene.unity** opens properly

Once these are all green, your Donkey Kong Mobile project is ready to run!
