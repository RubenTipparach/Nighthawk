# Nighthawk
A VR based environment for execution of cyber attack simulations.

## Instructions
- Install Unity Unity 2018.3.0f2 (64-bit)
- Boot up Kali with hosted script (WIP)
- Put on VR Headset
- Enter the Matrix.

## Features
- built with ezhack by @burtonyaboy in mind
- provides multiple various ways to visualize data
- the framework can evolve to accomodate different graphing algoirthms

## Version progression

### v0.4
- Added in animations for search transitions. 
- Also restructured graph and search system to make system more robust and queryable.
- Objects now have really detailed data on their relationships with other data.
![awesome animation 2](/images/awesome_animation_2.gif)

### v0.3
- got the search toolbox to work
- UI system was integrated into graph visualization pretty well
- reorganized the data a little
- using Seeded-Random unit sphere surphace for now, will figure out a good formation later
- sorted nodes by type in hirearchy
- all nodes and lines update on the fly

![awesome animation](/images/awesome_animation_1.gif)

- this is a test animation of the central connection nodes in motion
- leaf nodes follow their parent, and point away from the center for readability.

![crude drawing](/images/crude_drawing_of_results.png)
- while searching, results become eliminated, and filtered results will reorganize themselves in front of you
- this sketch is based on an idea I had at work...

### v0.2.1
- Tried out some metasploit stuff, looks like we got it working pretty easily, next step is to implement server-command version of eazy hack. Also a commit later tonight will update the Port information on the JSON generator.

- reorganized the graph structure to better represent the data, (General Purpose/Broadband Router)
	- Logic follows: make all GP nodes a child of BR nodes, GP should not connect to other GP
	- Draw green for BR-BR, and orange for BR-GP, and fan out with 30 degree increments for each GP node.
- still has some bugs, but I'm calling it a night

![V 0.2.1](/images/v.2.1.png)
- special note: I think I know why this is happening, routers are sharing GPs. I'll have to copy some to make up for that, should be good.

### v0.2
-  traditional graph rendering. Might have assigned too many connections, and added redundant ones too. Will need clean up for more usability.
![v 0.2](/images/v.2.png)

### v0.1
- envision the universe as buildings
![v 0.1](/images/v.1.png)

# Visuals inspired by this repo.
https://github.com/tedsluis/nmap
https://gojs.net/latest/index.html
http://lepo.it.da.ut.ee/~radan/3D_Graph_Exploration.pdf

## Maintenance
	- be sure to clean up the extra array in data[]...

