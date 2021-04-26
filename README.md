BEEGROUND-README
===========================
BEEGROUND is an open-source simulation tool for aggregation of swarm robots controlled by the bio-inspired algorithm BEECLUST

Author: Shiyi Wang

****
# Contents
* [Overview](#Overview)
* [Installation](#Installation)
* [Setting](#Setting)


# Overview
-----------
Bee-Ground is an open-source simulation tool based on Unity and Unity Machine Learning Agents which can be applied to the research on aggregation of swarm robots, especially the swarm robots controlled by the bio-inspired algorithm BEECLUST. MONA[1] is the modelled robot in this simulation software, however different robotic platform can be easily developed in Bee-Ground.

* • Bee-Ground is an open source, cross-platform simulation tool
* • Bee-Ground can simulate the operation of swarm robots in various complex and dynamic environments, including obstacles and multiple heat source scenarios. 
* • Bee-Ground performs multi-layer multi-scenario simulations simultaneously, and the simulation speed can reach 100 times as the real-time without loosing sampling resolution.
* • Bee-Ground provides extended possibilities for application of machine learning technics in swarm robotics.




# Installation
## Step 1. Download a Beeground version from the folder 01 Version
Download a Geeground version from the folder 01 Version, here I will use "BeeGround Ver. 1.0" as an example. Unzip the zip file in your local.

## Step 2. Download the latest Unity from the Unity official website
Click this link to the Unity officical webist [www.unity.com](https://unity.com/ "Unity"). For Student and personal using the 100k. it is free to use.
### Unity Hub
You need to download Unity Hub for first. 
### Install Unity "2019.4.25f1(LTS)"
Then click "Installs" on the left side and click "ADD" button and choose "Unity 2019.4.25f1(LTS)"
Then click "Next" and choose Microsoft Visual Studio Community 2019 (For editing code).

## Step 3. Import Beegground in Unity Start
Click "Projects" on the left side and then click "ADD" button to import BeeGround as a project.
Find "Bee-Ground 1.0" folder on your desk and double click the folder to import.  

![import dir](https://i.imgur.com/7RxZFVl.png)  
Once you import finshed. You should see "BeeGround 1.0" in your Unity Hub.

## Step 4. First View
Now click "Bee Ground 1.0" to open it with Unity.
![First View](https://i.imgur.com/FSKrgR7.png)
### Scene
You should see a part of an arena and 10 robots randomly distributed on the corner in the main view (center of the screen).

You will find a Scene Gizmo on the top right corner of the main view. Click "Y" to change to top view. Then you can hold your mouse wheel to move or zoom in/out. If you don't have the mouse wheel you can use "Hand Tool"(the icon under File menu on the top left corner.) If your Unity UI appears different as the screenshot. You can click Window-Layouts-Default to reset the layout.

### Hierachy
On the left side is the "Hierachy" which shows the components in the environment.
You can click the arrow to open the tree list.

Here are  
* Master: Virtual component for saving simulation parameters  
* Main Camera: Set the view when runing the simulation  
* Directional Light: Environment light source (just for apparance)  
* Timer: A text field which shows the simulation time and other information when runing  
* Arena: The area where agents can run  
* BeeXXX: Agents  

## Step 5. First run.
Click "Play" button(top middle) to run the simulation.
![First run](https://i.imgur.com/1ycANAZ.png)
You should see that ten robots starting moving. They are moving according to the Beeclust algorithm. The background colour of the arena shows the temperature distribution. Blue means 0 degree(minium) and red means 255 degree(maxium). The timer shows on the left side of the arena. and you can press keys to change the simulation speed. Meanwhile, you can press UP to switch the background colour on or off.

## Step 6. Logs
![Logs](https://i.imgur.com/KQOgyee.png)
There are four log files which record many informations where are saved in Asset/03 Results(Default dir). There
### dd-mm-yy_hhmmss_Parameters.txt
Record one time at the start. This file includes all the parameters of the simulation: 
* Date and Time
* Simulation:Time/Repeat count
* Arena:Length/Width
* Agents:Counts/Length/Width/Speed/Turning Speed/Sensor Range
* Initialization Postion: Range of horizonal position and vertical position

### dd-mm-yy_hhmmss_State.txt
Once an agent starts or finishes aggregation, it adds a record in this file includes
Postion/Current state(1 means stop or aggregated/ 0 means start to move)/Waiting time(Deleay)/the collision is on the cue or not

### dd-mm-yy_hhmmss_Position.txt
Record per second for each agents in the arena.

### dd-mm-yy_hhmmss_Collision.txt
Collsion counts for each agents once they have collision.


## Step 5. Click "Tools"-"BeeGround Setting to change the parameter"

# Setting
### Arena settings  
The parameters of arena dimensions can be adjusted in this part and the obstacles and temperature are generated by array files.  
### Robots settings  
The default settings of robots (here using MONA[1] as an example) including size, forward speed, turn speed, sensor range, and wait time are initialized here.  
### Simulation setting  
Simulation duration and number of iterations are defined in this step.  
