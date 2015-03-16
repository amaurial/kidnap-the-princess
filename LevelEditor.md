Describes how to use Gleed2D to make levels for magic world.

# Introduction #

Although the level loading is not completely implemented yet it is already fairly


# Setup #

  1. Download Gleed2D here: http://gleed2d.codeplex.com/
  1. Start it and press F9 or use Tools->Settings to bring up the settings menu.
  1. Choose a default content root folder. This should be the path to the projects content root folder. In my case it is "C:\MagicWorld\Platformer\Content".
  1. Make sure DefaultTextureOrigin mode is set to TextureCenter(should be the default anyway).

# Creating levels (Quick step guide) #

|Layer|Description|
|:----|:----------|
|Zero|Has only one Textures that is always displayed and never moved. It spans the whole screen and represents the one "most far away".|
|Background|The actual level background that is scrolled when the player moves. This layer does not collide with anything.|
|Middle|This is where all the platforms and the ground of the level go. Everything in here is included in collision checks.|
|Front|Behaves like Background but is drawn in over middle.|
|Special|This layer holds all information like starting point, exit point, level bounds and Level Properties (usable spells, collectable items ...).|
|Ingredient|Layer for Ingredients|
|Enemy|Layer for enemies|
|Blockade|Impassable objects.|
|PushPull|Objects which are influenced with push and pull - Optional Layer|
|Moveable Platform|TODO John - Optional Layer|
|Portal|Define Portals for moving objects in time and space - Optional Layer |
|Icecicle| Falling down Objects, which explodes on collision and kills hitted enemies |

  1. Start by creating layers and name them according to their ingame alias (see above).
  1. Add textures to the layers (except special layer) to create your level. Do not scale or rotate textures as it is not supported yet.
  1. Add a circle primitive to the special layer and name it **start** to add the starting point for the level.
  1. Do the same again but name the circle **exit** to set the exit point of the level.
  1. Finally place two rectangle primitives. One at the top left corner of the level and the other at the bottom right corner of the level. Name them **bottomRight** and **topLeft** according to their position.
  1. Save the file and add it to your project

Note: Please name the xml level files "levelN.xml" where N is the number of the level and put the xml files in the LevelData folder.

### General ###
You could add the custom string property named "Magic" to all switches, all types of moveable plattforms, gravity elements and push and pull elements to enable a particle effect which shows that this item has some special behavior and the player should interact with it.

### Push Pull ###
Pushable and pullable objects have two optional parameters (custom properties:
  1. enable\_gravity - this is a boolean value, set it to true or false
  1. collision\_type - this is a (type: free text) value with possible content of impassable,passable,platform

default values are enable\_gravity=false and collision\_type=impassable

# Special layer #

As mention above the special layer holds information instead of textures.
What was not covered before is the placement of the collectibles.
Collectibles are added through circle primitives, but instead of naming them you have to add a custom property to them (John please elaborate!).


## Instructions/Ingame messages ##

An instruction can be placed in the following way:

  * Add a rectangle to the special layer.
  * Add a custom property to the rectangle by the name of Instruction.
  * Assign its value whatever you want the message to be.

How it works:
The instruction is displayed as soon as the player crosses its x coordinate. The length the instruction is displayed relates to the length of the message. ATM there is no automatic /n implemented so format your messages yourself or adjust the code.

## Portal ##
A Portal moves the colliding object to a destination point.
You specify destination by adding new custom property named "Destination" with type item and choose a item from the select dialog (same way selecting a path for bats/platforms), the position of this item is the destination. Default the portal only reacts on player collision. If you want reaction with ShadowCreature add custom property free text with name "Type" and write freetext "ShadowCreature", write free text "PushPull" for pushable and pullable objects.

## Ingredients ##
Ingredients must be placed on the Ingredients layer.

## Icecicle ##
But every texture you wand on this layer, which should behave like a falling down icecicle, no need to use a icecicle texture.


## setting the level properties ##
Put a primitive object on the special layer with the name **LevelProperties**.

### add usable spells ###
Add one custom property (type: free text) to the **LevelProperties**-Item. If you name the property:
  * warm
  * cold
  * push
  * pull
  * electric
  * nogravity
  * matter
it means that this spell is available for this level. Use one custom property for each available spell. No value is needed for the property.

### min item to collect to finish the level ###
To set the minimum amount of collectable items the player needs to collect to finish the level add a custom property to the LevelProperties with the name **min item** (type: free text). Set the value to the number of items the player has to collect to finish the level.

### set max time for level ###
Add a custom property to the "LevelProperties" with the name **max time** (type: free text). Set the value to the time (in seconds) the player has to finish the level.

## Enemies ##
For every Enemy on the Enemy Layer you need to add the custom property type free text "EnemyType" and the value depending on prefered Enemy

  * ShadowCreature
  * Bat
  * Bullet

### Bullet ###
Bullet defines an object(texture is free) which itself is not dangerous but shoots dangerous bullets/fireballs in defined direction. To define the shooting diretion and velocity you need to add a custom property of type item named "Aim" and reference it to a cycle primitive.
The primitive sets the shooting direction and the distance between the Bullet/Vulcano defines the velocity. More far away gives higher velocity.
Moreover you could define if the bullets should be influenced by gravity (see gravity elements).
You could define the shooting interval in milliseconds with the custom property type free text and name "DelayTime".
Default for enable\_gravity ist true and for delay milliseconds 2000.

### Set path for Enemy ###
Add Primitive with type path to the level editor. Set the waypoint in the Leveleditor with left mouse click press 2 if you are done (or middlemouse click).
Add a custom property "Path "to the enemy which you want to add the path. Link the PathItem with the custom property in the enemy.


# switches and switchable objects #
Put all switches and switchable objects on special layer.
For Switches add one custom property (type: free text) "switch" with a unique id.
For the switchable item add a custom property (type: free text) "switchable" with the id of the switch.

> ## Switches ##
| Switches Property Name | activation | comment |
|:-----------------------|:-----------|:--------|
| switch\_weight| by created matter / player / enemy |  |
| switch\_electricitySwitch | by electricity | optional property: switchTime |
| switch\_torch\_off | by fire |  |
| switch\_torch\_on | by ice |  |
| switch\_destroy | by fire |  |

> ## optional ##
Some switches can use extra propertys. Just add to the switchable item (type: free text).
> > ### switchTime ###
Use the switchTime property to set a time when the switch resets to off.
The value must be a double value.

## switchable objects ##

### door ###
Add the name of prefered door as custom propery (type: free text)
Use the switchabel interface like described above.

Doors:

  1. updown\_door: a door which moves up if open, the start status could be set by a custom property of type boolean named: "open", default is closed

  1. openclose\_door: Depending on the texture path which must include the word "open" or "closed" the open/close status is detected. You must have a open and closed texture, with the only difference in path of "open" and closed

### moveable Platforms ###
#### switchable moveable Platform ####
For the switchable moveable Platforms item add a custom property (type: free text) "switchable" with the id of the switch to an existing moveable platform (see Moveable Platforms). The Platfrom will go to the rndpoint if activated and stops there. The next time it is activated it goes to the other endpoint.

#### auto moveable ####
Use all stepts like in the switchable moveable Platforms. Add a custom property (type: free text) "autoPlatform". If the platform is activated it will start moving. If it is deactivated it will stop.
Optional you can add a custom property (type: boolean) "isActivated". If isActivated is false the Platform will NOT start moving from the begining.


### gravity elements ###
For Switches add one custom property (type: free text) "gravity\_element". This element could be used as a switchable item, but it is not necessary.
It has some optional custom properties:
  1. enable\_gravity - this is a boolean value, set it to true or false
  1. enable\_collision - this is a boolean value, set it to true or false
  1. collision\_type - this is a (type: free text) value with possible content of impassable,passable,platform

Info: If you set enable\_gravity to false it will fall through the ground after activation, but maybe you want this behavior only for an action effect. Enable gravity is only the start value for this object it could be influenced by connected switch.

### Platforms ###
### Moveable Platforms ###
Add Primitive with type path to the level editor in the special layer. Set the waypoint in the Leveleditor with left mouse click press 2 if you are done (or middlemouse click).
Add the platform texture which you want to use to the Layer "Moveable Platform"

Add a custom property "Path" from type Item to the platform which you want to add the path. Link the PathItem with the custom property in the platform.

Add a custom property Spell from type bool to the platform to define if the platform should be automatically move along the waypoints or it should be move using push and pull spell.

Pushable platforms can just be defines between 2 waypoint and it can just move along the X or Y Axis. (Not Diagonaly)
Moveable platforms can move diagonaly and can have several waypoints.