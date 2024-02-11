# HueControlWithUnity
 Control your Philips Hue with a Unity App

 Work In progress

 

 I am building a library to facilitate Philips Hue integration in your own application.

 For now you can
 * locate your bridge in a local network
 * create a user
 * control lights (On, Off, Fade in)
 * set light colors

 A sample scene is provided to use the different functions

 Warning : locate will freeze your ui as currently the code has a thread sleep. I plan on changing that as soon as possible.


Changes in 11 February 2024
I tested with 2022.3.19F1 LTS as requested and it works.
* WARNING : Unity decided that now http requests are prevented by default. You need to go in player settings and change allow downloads over HTTP* to **always**. That makes the requests insecure, but Hue works in http unfortunately.
* Fun fact : I could make that application work, while I could not get the official application to detect my bridge (It was not used for several years so I hope newer bridge works on the same set of API)


 Next functionalities will be :
 * Color control with a color wheel
 * get the list of your current lights
