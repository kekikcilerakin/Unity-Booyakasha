# Character Creation System

This system allows creating and customizing characters with different skin colors.

## Architecture

The system follows a separation of concerns pattern:

1. **CharacterCreationManager**: Handles character data and appearance
   - Manages skin color application to character
   - Stores temporary selections until saved
   - Provides methods for saving character

2. **CharacterCreationUI**: Handles UI interactions
   - Manages color selection buttons
   - Provides visual feedback for selection
   - Communicates with CharacterCreationManager

3. **CharacterSO**: Stores character data
   - Holds the saved skin color index

4. **SkinColorData**: Stores available skin colors
   - Defines all available skin color options

## Setup Instructions

### 1. Create the SkinColorData asset:
- Right-click in the Project window
- Select Create > Character Creation > Skin Color Data
- Name it "DefaultSkinColors"

### 2. Set up Character Creation Scene:
1. Add the CharacterCreationManager to a GameObject:
   - Create empty GameObject named "CharacterCreationManager"
   - Add the CharacterCreationManager component
   - Assign references:
     - Character Data: your CharacterSO asset
     - Skin Color Data: the DefaultSkinColors asset
     - Skin Renderer: drag character mesh renderer here

2. Set up UI elements:
   - Create skin color buttons in your UI (one for each skin color)
   - Make sure each button has an Image component
   - Organize buttons in a layout (e.g., Horizontal Layout Group) 
   - Create a Save Button

3. Add the CharacterCreationUI to a GameObject:
   - Create empty GameObject named "CharacterCreationUI" (or add to existing Canvas)
   - Add the CharacterCreationUI component
   - Assign references:
     - Character Manager: reference to the CharacterCreationManager
     - Skin Color Buttons: assign your UI buttons for skin colors
     - Save Button: assign your save button

4. Set up your character model in the scene

### Usage:
1. In play mode, the system will automatically:
   - Set the color of each button based on the SkinColorData
   - Add click handlers to each button
   - Scale up the currently selected color button for visual feedback
2. Click on a color button to change the character's skin color
   - This updates the character's appearance temporarily
   - Changes are not saved to the character data yet
3. Click "Save Character" to commit the changes to the character data

## How it Works:
1. The user clicks a skin color button in the UI
2. The CharacterCreationUI tells the CharacterCreationManager which color was selected
3. The CharacterCreationManager:
   - Stores the selection temporarily
   - Applies the skin color to the character model
   - Notifies the UI of the change
4. The CharacterCreationUI updates the visual state of the buttons
5. When the user clicks Save:
   - The CharacterCreationManager commits all temporary selections to the CharacterSO

## Extending the System:
To add more creation options:
1. Add new properties to CharacterSO (hair style, eye color, etc.)
2. Create additional data ScriptableObjects as needed
3. Extend the CharacterCreationManager to handle temporary selections for the new options
4. Extend the CharacterCreationUI to provide controls for the new options 