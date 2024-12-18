# Simple-Genetic-Algorithm
Implementation of genetic algorithm with visualization

Workflow:
<br/>
1) Firstly, the program creates squares with different colors.  
2) Then they are moving across the area of the window  
3) When some of them intersect each other -> program create a descendant with color generated via a genetic algorithm  
4) As a result all new squares have new colors  
5) After a while, all entities die  


Exmaples of work:<br/>
![Screenshot](Results/Screenshot-1GA.png)

![Screenshot](Results/Screenshot-2GA.png)

![Screenshot](Results/Screenshot-3GA.png)


TODO: 

 - Add unit tests
 - Replace WinForms with MAUI and use graphics for visualization.
 - Change shape to circles
 - Fix issues with ui lag between iterations / generations. It called flickering. Need to use MAUI framework for visualization.
