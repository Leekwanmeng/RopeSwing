Fit all Screen Size:-
-------------------------------------------

Hi developers, this package is to solve the issue of different screen size in Unity
including iOS & Android. This problem is faced when we use sprites in 2D, not in case
of using GUITextures.

If you see your GameObjects are placed in different position in different devices,
then this solution gonna save your time & lot of headaches.


How to use demo:-
------------------------------------------

1.Run Demo.scene(Demo scene is best viewed when build settings is for iOS/Android).

2.Change Screen size to 3:2Portrait(2:3)(If you dont find it, click on plus sign at bottom
  of screen-size selection window & provide width 2, height 3. Click OK).
  Now Press Play button & see how the cube is placed.

3.The default screen-size is used 3:2Portrait(2:3). Now change screen size
   to any other like 16:10Portrait or as you wish.(Portrait size is preferred)

4.You will see the cube & sphere is not placed same, rather it may be out of the screen.

5.This is the general problem we face.

6.Now come to the solution part.

7.Go to Cube GameObject. It has a script attached, name 'Resolution Fixer'. The script 
  is not active. Make it active by clicking on the box beside its name. Do the same for 
  Sphere GameObject also.

8.Now press the play button & you will see the cube & sphere is inside the screen & in exact
  same position.

9.Stop play button. Change the screen size in any other, & press play button again.
  The cube will be in same position relating to the screen size.


N.b:- 1. If you change screen size while in play mode in editor, you may see absurd behaviour.
Bcoz Every Device has its fixed screen-size & it doesn't change once game started. So this
script is written keeping that in mind.

2.It is not advisable to use Portrait & landscape both mood in same scene of the game...


How to use Package:-
------------------------------------------

Portrait Mode:-
_______________


1.Add 'ResolutionFixer' script to the object you want to fix screen size issue.
2.Note down size of the screen working best for your scene.
3.Write screen size x of best working in 'Working Xor Ratio' field in script on editor.
4.Write Screen size y of best working in 'Working Yor Ratio' field in script on editor.
5.Remember, For portrait mode, screen size of x will be smaller than screen size of y.
6.Click 'Change X' box.
7.Drag MainCamera in 'Main Camera' field in script on editor.
8.Press Play & enjoy fit screen size... :)

N.B:-If the GameObject is rotated, Undo 6th step & experiment to click on other box/boxes
to check whichone satisfies your need. In most cases, click on 'Change X Rotated' do the work.


LandScape Mode:-
________________

1.Add 'ResolutionFixer' script to the object you want to fix screen size issue.
2.Note down size of the screen working best for your scene.
3.Write screen size x of best working in 'Working Xor Ratio' field in script on editor.
4.Write Screen size y of best working in 'Working Yor Ratio' field in script on editor.
5.Remember, For Landscape mode, screen size of x will be greater than screen size of y.
6.Click 'Change X' box.
7.Drag MainCamera in 'Main Camera' field in script on editor.
8.Press Play & enjoy fit screen size... :)

N.B:-If the GameObject is rotated, Undo 6th step & experiment to click on other box/boxes
to check whichone satisfies your need. In most cases, click on 'Change X Rotated' do the work.

----------------------------------------
Feel free to Contact me:-thechildofjessus.6@gmail.com
-----------------------------------------------------------------------------------------------------------------------