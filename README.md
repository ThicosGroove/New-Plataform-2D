# New-Plataform-2D


<p align="center"> <img src="Gifs for Git/gameplay.gif" width="500"/>        

### My first 2D plataform. This is a project from a [Dunki Code][1] course. It's a begginer course that i took after finish the [Unity learn pathway][2], so i implemented some features way further the original intented. Every line of code was improved as well as gameplay and UI.
Currently avaliable on [itch.io][3]. Working on mobile version 😊.  
  
## Gameplay
  
  As a plataform 2D the mechanics are simple: You move left and right, jump, attack and roll. With Unity's new input system you can play on keyboard and mouse or a controller. You can get the controls scheme on the main menu and on pause menu.
  
  <p align="center"> <img src="Gifs for Git/controls.gif" width="500"/>   
  
  
## Enemies
    
  There are 3 types of enemies: Slime, Bat and Blob. They share an abstract class, so they all behave more or less the same way. But they differ in health and movement.
    
## UI
    
  The player and each enemy has a health bar. The pause button and menu options are simple, but works great with well design game state code. 
    
   <p align="center"> <img src="Gifs for Git/healthBar.gif" width="350"/> <img src="Gifs for Git/buttons.gif" width="350"/> 
  
## Save
  
  The game saves the progress automatically when the player reaches a new level. The player will always start from the begin of the level.     
      <p align="center"> <img src="Gifs for Git/save.gif" width="500"/> 
  
[1]: https://cursos.dankicode.com/unity  
[2]: https://learn.unity.com/pathway/junior-programmer  
[3]: https://thicosgroove.itch.io/     
