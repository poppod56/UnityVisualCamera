# Unity Realtime Visual Camera

This Unity package provides real-time camera synchronization between an AR camera (mobile device) and a normal camera (PC/server), using **Telepathy** for network communication. It supports AR Foundation and includes features like recording camera movements for use in cutscenes and realistic camera animations.

## Features

- **Real-Time Camera Sync**: Syncs AR camera (mobile) with a standard camera (server) for real-time transformations.
- **Telepathy Networking**: Simple client-server setup using Telepathy for fast and reliable data transfer.
- **AR Foundation Support**: Ideal for AR projects, with real-time syncing of AR camera movements.
- **Cutscene Creation**: Record and save camera movements as animation clips, which can be used in Unity's Timeline for cutscenes.
- **Sample Scenes**: Includes separate sample scenes for both mobile (AR) and editor/server usage.

## Use Cases


- **Real-Time Camera Presentation**: Sync AR camera transformations in real time for virtual production, collaborative design, or live presentations. This feature allows you to display and present camera movements on a screen in real time.
- **Cutscene Creation**: Create realistic camera animations and record the camera’s movement for Unity’s Timeline or other animation tools.
- **Realistic Camera Movement**: Capture lifelike camera movements and save them as animation clips, useful for cinematic projects.


## Installation

1. Open your Unity project.
2. In the **Unity Package Manager**, click the `+` button and select **Add package from Git URL**.
3. Enter the following URL:

https://github.com/poppod56/UnityVisualCamera.git

4. Unity will import the package and all related files.

### Package Dependencies

The package includes the following dependencies:

```json
{
"com.unity.xr.arfoundation": "5.1.4",
"com.unity.xr.interaction.toolkit": "3.0.4",
"com.unity.render-pipelines.universal": "14.0.9"
}
```
## Setup and Usage

 ####Example VDO Setup
 https://youtu.be/5HysCVaIZvk
 
### 1. Mobile (Client) Setup

1. **AR Mobile Project Setup**:
    - Create your mobile project using the **AR Mobile** template to avoid conflicts and errors.
    - Ensure AR Foundation is installed via Unity’s **Package Manager**.
  
2. **Add the AR Camera**:
    - Open the **Mobile.scene** and add an AR camera or AR Session Origin if needed.
    - Attach the **CameraClient** script to the AR camera’s GameObject.
  
3. **Set Server IP**:
    - After setting up the AR camera, check the Unity Console log for the server's IP address (from the Editor/Server setup).
    - Assign the **serverIP** and **serverPort** in the **CameraClient** script Inspector fields.
  
4. **Build for Mobile**:
    - Build the mobile scene for your AR-supported mobile device.

### 2. Editor (Server) Setup

1. **Open the Editor Scene**:
    - Open the **Editor.scene** in Unity to act as the server for receiving the AR camera's position and rotation.
  
2. **Run the Server**:
    - Attach the **CameraServer** script to a GameObject in the Editor scene.
    - Assign the **parentTransform** and **cameraTransform** fields in the Inspector for the camera to follow AR movement.
    - Press **Play** to start the server. The local IP address will be displayed in the Unity Console and UI using the **DisplayIPAddress** script.
  
3. **Connect the Mobile Client**:
    - Ensure your mobile device is connected to the same local network.
    - Run the mobile app, and it will sync AR camera data to the server.

### 3. Recording Camera Movements

On the server side, you can record camera movements:

1. Attach the **CameraRecorder** script to the camera.
2. Use the buttons in the **Inspector** to **Start Recording** and **Stop Recording** the camera’s movement.
3. The recorded animation will be saved as an animation clip in the project’s `Assets` folder, which can be used in Unity's Timeline.

### 4. Displaying Server IP Address

To display the local IP address of the machine running the server:

1. Attach the **DisplayIPAddress** script to any GameObject in the server scene.
2. The IP address will be printed in the Unity Console and displayed in a simple UI.

## Sample Scenes

The package includes the following sample scenes to demonstrate the camera synchronization:

- **Mobile.scene**: A scene for AR devices using **AR Foundation**.
- **Editor.scene**: A scene for the server side (PC) where the AR camera data is synced and recorded.

You can import the sample scenes via Unity’s **Package Manager**:

1. Open **Package Manager**.
2. Select the **Unity Realtime Visual Camera** package.
3. Click **Import in Project** next to the sample scenes.

## Requirements

- Unity 2020.3 or later.
- **AR Foundation** (required for AR camera functionality on mobile).
- **Telepathy** (already included in the package for networking).

## Use Case Examples

1. **Real-Time Camera Sync**: Ideal for syncing AR camera movement to a traditional camera in real-time. This is useful in virtual production, collaborative design, or AR experiences.
2. **Cutscene Creation**: Record camera movements during AR experiences and save them as animation clips, which can then be used in Unity’s Timeline to create cutscenes.
3. **Realistic Camera Movement**: Use actual AR camera movements to capture realistic motion and save them as animation clips for use in cinematic projects.

## License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for more details.
