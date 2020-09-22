# Tank Wars
This Repository contains the Unity3D project done with C#
## _Dependencies used_
- [Unity3D](https://unity3d.com/get-unity/download)
- [Asset Store](https://assetstore.unity.com/)

### Starting Up
Download Unity Hub, and install Unity Editor (2019.3 or above). Make sure that you select Microsoft Visual Studio and Android Build Support in Add-ons.

### Install Instructions
1. Download the TankWars.apk file on your android device and install it
OR
1. Download the project folder
2. Open it in Unity Editor (2019.4.0f1 or above)
3. Connect your android device via USB cable (ensure debugging mode is on)
4. In unity File->Build Settings -> Add all scenes
5. Select your device in Run Device
6. Click Build and Run

### Description
This is a top down movement based touch controlled Android Game. The player controls the green tank and objective is to destroy all enemy tanks. It consists of 3 worlds with 5 levels each, and a new element of difficulty is added in each world. There is a final boss battle and 3 survival modes. The shop in main menu allows the player to modify the statistics of their tank randomly. Each level is to be completed within the time limit, and stars out of 3 are awarded on the basis of shot accuracy and damage taken.

### Implementation
##### Movement
The screen has an overlay canvas which consists of a health bar, timer, and two joysticks. A script is responsible for taking the input from left joystick and move the tank accordingly. I used unity NavMesh tools to bake a Navigation Mesh on the map. This creates a map for the enemy tanks to move around. I added a script to each enemy tank, which sets a random point on the map as the destination for that tank. The NavMesh tools find the shortest path to the destination and moves the enemy tank along that path. Both scripts also ensure that the front of tank is aligned with the movement direction and turning is done smoothly, so that the movement looks natural.
##### Shooting
For the player, the right joystick is used for shooting and aiming. The player can aim by pulling the joystick softly in the desired direction, and when the joystick is pulled strongly, it shoots in the aimed direction.
For enemy tanks, there are 3 types of shooting
1.  The tank first uses ray-cast shooting, which shoots out a ray (invisible to player) in the direction of player tank. If this hits the player tank, it then shoots a bullet in that direction. I have added a slight noise to this, so that the bullet is shot with a random inaccuracy, which decreases as the player reaches higher levels.
2. This is a projectile shot, which means that the tank shoots a bullet which goes over the walls and obstacles. For this, I used the Physics component of Unity Engine to write a projectile path with given destination
3. This is a heatseeker shot, which means it chases the player tank. This also uses NavMesh, and its destination is set to the player position. the bullet self destructs after 10 seconds or when it hits the player

##### Environment, Graphics and Aesthetics
Most of the visual effects were made as a particle system with suitable settings. Lightening in world 3 was simulated by adjusting the intensity of a directional light over time, according to an animation curve. I designed some 3D models using Pro-Builder and created materials for various GameObjects. I also added a bit of post processing to make the environment look even better.

##### Sound
I implemented a custom class called Audio Manager (under guidance of [Brackeys](https://www.youtube.com/watch?v=6OT43pvUyfY)) which made it a lot easier to play and pause various sound effects, like those of shooting, thunder and explosions.

##### UI
The UI elements were mostly from TextMeshPro class, which provides more flexibility, features and better appearance. I implemented a script for scene management which ensured smooth scene transitions and designed all menus and panels.



