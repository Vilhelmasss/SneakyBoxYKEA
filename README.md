# SneakyBoxYKEA

# How to Run the Project
* Download the project.
* Navigate to the project folder "../SneakyBoxYKEA-main/YKEA" and launch it
* Turn on "MainMenu.scene", start the project and click "New Game" button.

# Controls
* WASD To move camera.
* Hand icon - Free select, for the object to be selectable (default state).
* Upon clicking onto an object, a bar appears with possible controls: Delete, Move, Change Color.
* Change Color changes based on three sliders, each responsible for the RGB code.
* '+' button allows you to select which primitive shape to add.
* Grid button makes it either snap or not snap to the grid.
* From main menu - "New Game" deletes the save, "Load Game" loads the existing save from json.

### Funkciniai reikalavimai:

⚪ Turi būti matomi 3 statmeni paviršiai (kambario kampas)
* STATUS: DONE
* WASD while moving the camera culls the closest walls based on the shortest distance.

⚪ Vartotojas turi galėti padėti erdvėje bent 3 skirtingus modelius. (gali būti ir primityvai)
* STATUS: DONE
* In the game scene, click '+', then select the object you want to place.

⚪ Vartotojas turi galėti pajudinti jau padėtus objektus
* STATUS: PARTIALLY DONE
* When clicked on FreeSelect (Hand icon), you may click the 4 arrows (Move icon) button and it will move the placed object.
* PARTIALLY DONE BECAUSE - needs to save object color when moving

⚪ Vartotojas turi galėti pakeisti padėtų objektų spalvą (ir pasirinkti spalvą)
* STATUS: DONE
* When clicked on FreeSelect (Hand icon), you may click the color wheel and choose the color with the sliders.

⚪ Vartotojas turi galėti ištrinti jau padėtus objektus
* STATUS: DONE
* When clicked on FreeSelect (Hand icon), you may click the trash bin icon and delete the object.

⚪ Judinant objektus, turi būti pasirinkimas juos prilipinti prie kitų objektų arba grid’o (snap)
* STATUS: PARTIALLY DONE
* When placing or moving objects, click on the Grid icon, seamless grid means it's snapping, else it's free placed.
* Partially, because it only snaps to the grid, not other game objects.

⚪ Aišku susikūrę svajonių kambarį, savo nuostabiu dizainu norime juo pasidalinti su draugais, todėl sudėtus objektus su jų parametrais reikia išsaugoti, ir vėliau atkurti. (New, save, load funkcionalumas)
* STATUS: DONE
* Room is saved when the application is quit.

### Packages Used
* Newtonsoft Json for Unity
